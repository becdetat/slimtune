/*****************************************************************************
 * DotNetProfiler
 * 
 * Copyright (c) 2006 Scott Hackett
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from the
 * use of this software. Permission to use, copy, modify, distribute and sell
 * this software for any purpose is hereby granted without fee, provided that
 * the above copyright notice appear in all copies and that both that
 * copyright notice and this permission notice appear in supporting
 * documentation.
 * 
 * Scott Hackett (code@scotthackett.com)
 *****************************************************************************/
/*
* Copyright (c) 2007-2009 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
#include "stdafx.h"
#include "Profiler.h"
#include "NativeHooks.h"
#include "Timer.h"
#include "SigFormat.h"

// global reference to the profiler object (ie this) used by the static functions
ClrProfiler* g_ProfilerCallback = NULL;

#define CHECK_HR(hr) if(FAILED(hr)) return (hr);

struct IoThreadFunc
{
	IoThreadFunc(IProfilerServer& server) : m_server(server)
	{ }

	void operator()()
	{
		m_server.Run();
	}

private:
	IProfilerServer& m_server;
};

unsigned int ClassIdFromTypeDefAndModule(mdTypeDef classDef, ModuleID module)
{
	unsigned short classDefLow = classDef & 0xffff;
	unsigned short moduleLow = module & 0xffff;
	unsigned int fullClassId = (moduleLow << 16) | (classDefLow);

	return fullClassId;
}

ClrProfiler::ClrProfiler()
: m_server(NULL),
m_suspended(0),
m_instCount(0)
{
#ifdef DEBUG
	__debugbreak();
#endif

	InitializeCriticalSectionAndSpinCount(&m_lock, 200);
	m_modules.reserve(16);
	m_classes.reserve(512);
	m_functions.reserve(4096);
	
	const wchar_t* invalidName = L"$INVALID$";

	FunctionInfo* invalidFunc = new FunctionInfo(0, 0);
	invalidFunc->Name = invalidName;
	m_functions.push_back(invalidFunc);

	ClassInfo* invalidClass = new ClassInfo(0, 0);
	invalidClass->Name = invalidName;
	m_classes.push_back(invalidClass);

	ModuleInfo* invalidMod = new ModuleInfo(0);
	invalidMod->Name = invalidName;
	m_modules.push_back(invalidMod);
}

ClrProfiler::~ClrProfiler()
{
	DeleteCriticalSection(&m_lock);
}

HRESULT ClrProfiler::FinalConstruct()
{
	return S_OK;
}

void ClrProfiler::FinalRelease()
{
}

STDMETHODIMP ClrProfiler::Initialize(IUnknown *pICorProfilerInfoUnk)
{
	//multiple active profilers are not a supported configuration
	//this is possible with .NET 4.0
	if(g_ProfilerCallback != NULL)
		return E_FAIL;

	//Get the COM interfaces
    HRESULT hr = pICorProfilerInfoUnk->QueryInterface(IID_ICorProfilerInfo, (LPVOID*) &m_ProfilerInfo);
    if (FAILED(hr))
        return E_FAIL;

    hr = pICorProfilerInfoUnk->QueryInterface(IID_ICorProfilerInfo2, (LPVOID*) &m_ProfilerInfo2);
    if (FAILED(hr))
	{
		//we've decided not to support .NET before 2.0.
		return E_FAIL;
	}

	//TODO: Query for ICorProfilerInfo3

	m_config.LoadEnv();

	//set up basic profiler info
	hr = SetInitialEventMask();
    assert(SUCCEEDED(hr));

	hr = m_ProfilerInfo2->SetFunctionIDMapper(StaticFunctionMapper);
	assert(SUCCEEDED(hr));

	hr = m_ProfilerInfo2->SetEnterLeaveFunctionHooks2(FunctionEnterNaked, FunctionLeaveNaked, FunctionTailcallNaked);
	assert(SUCCEEDED(hr));

	//set up unmanaged configuration
	SymSetOptions(SYMOPT_UNDNAME | SYMOPT_DEFERRED_LOADS);
	SymInitialize(GetCurrentProcess(), NULL, TRUE);

	//CONFIG: Server type?
	m_active = false;
	m_server.reset(IProfilerServer::CreateSocketServer(*this, m_config.ListenPort));
	m_server->SetCallbacks(boost::bind(&ClrProfiler::OnConnect, this), boost::bind(&ClrProfiler::OnDisconnect, this));
	m_server->Start();

	//initialize high performance timing
	InitializeTimer(m_config.CycleTiming);
	QueryTimerFreq(m_timerFreq);
	//unsigned __int64 time;
	//QueryTimer(time);
	//srand((unsigned int) time);

	// set up our global access pointer
	g_ProfilerCallback = this;

	if(m_config.WaitForConnection)
		m_server->WaitForConnection();

	//kick off the IO thread
	IoThreadFunc threadFunc(*m_server);
	m_ioThread.reset(new boost::thread(threadFunc));

    return S_OK;
}

STDMETHODIMP ClrProfiler::Shutdown()
{
	//force everything else to finish
	EnterLock localLock(&m_lock);

	m_active = false;
	StopSampleTimer();
	timeEndPeriod(1);

	//take down the IO system
	m_server->Stop();

	g_ProfilerCallback = NULL;
    return S_OK;
}

void ClrProfiler::OnConnect()
{
	//Hybrid will pass both of these conditionals
	if(m_config.Mode & PM_Sampling)
	{
		timeBeginPeriod(1);
		StartSampleTimer(200);
	}
	
	if(m_config.Mode & PM_Tracing)
	{
		m_ProfilerInfo->SetEventMask(m_eventMask | COR_PRF_MONITOR_ENTERLEAVE);
	}

	m_active = true;

	if(m_config.SuspendOnConnection)
		SuspendAll();
}

void ClrProfiler::OnDisconnect()
{
	if(m_config.Mode & PM_Tracing)
	{
		m_ProfilerInfo->SetEventMask(m_eventMask);
	}

	m_active = false;
}

HRESULT ClrProfiler::SetInitialEventMask()
{
	//COR_PRF_MONITOR_NONE	= 0,
	//COR_PRF_MONITOR_FUNCTION_UNLOADS	= 0x1,
	//COR_PRF_MONITOR_CLASS_LOADS	= 0x2,
	//COR_PRF_MONITOR_MODULE_LOADS	= 0x4,
	//COR_PRF_MONITOR_ASSEMBLY_LOADS	= 0x8,
	//COR_PRF_MONITOR_APPDOMAIN_LOADS	= 0x10,
	//COR_PRF_MONITOR_JIT_COMPILATION	= 0x20,
	//COR_PRF_MONITOR_EXCEPTIONS	= 0x40,
	//COR_PRF_MONITOR_GC	= 0x80,
	//COR_PRF_MONITOR_OBJECT_ALLOCATED	= 0x100,
	//COR_PRF_MONITOR_THREADS	= 0x200,
	//COR_PRF_MONITOR_REMOTING	= 0x400,
	//COR_PRF_MONITOR_CODE_TRANSITIONS	= 0x800,
	//COR_PRF_MONITOR_ENTERLEAVE	= 0x1000,
	//COR_PRF_MONITOR_CCW	= 0x2000,
	//COR_PRF_MONITOR_REMOTING_COOKIE	= 0x4000 | COR_PRF_MONITOR_REMOTING,
	//COR_PRF_MONITOR_REMOTING_ASYNC	= 0x8000 | COR_PRF_MONITOR_REMOTING,
	//COR_PRF_MONITOR_SUSPENDS	= 0x10000,
	//COR_PRF_MONITOR_CACHE_SEARCHES	= 0x20000,
	//COR_PRF_MONITOR_CLR_EXCEPTIONS	= 0x1000000,
	//COR_PRF_MONITOR_ALL	= 0x107ffff,
	//COR_PRF_ENABLE_REJIT	= 0x40000,
	//COR_PRF_ENABLE_INPROC_DEBUGGING	= 0x80000,
	//COR_PRF_ENABLE_JIT_MAPS	= 0x100000,
	//COR_PRF_DISABLE_INLINING	= 0x200000,
	//COR_PRF_DISABLE_OPTIMIZATIONS	= 0x400000,
	//COR_PRF_ENABLE_OBJECT_ALLOCATED	= 0x800000,
	// New in VS2005
	//	COR_PRF_ENABLE_FUNCTION_ARGS	= 0x2000000,
	//	COR_PRF_ENABLE_FUNCTION_RETVAL	= 0x4000000,
	//  COR_PRF_ENABLE_FRAME_INFO	= 0x8000000,
	//  COR_PRF_ENABLE_STACK_SNAPSHOT	= 0x10000000,
	//  COR_PRF_USE_PROFILE_IMAGES	= 0x20000000,
	// End New in VS2005
	//COR_PRF_ALL	= 0x3fffffff,
	//COR_PRF_MONITOR_IMMUTABLE	= COR_PRF_MONITOR_CODE_TRANSITIONS | COR_PRF_MONITOR_REMOTING | COR_PRF_MONITOR_REMOTING_COOKIE | COR_PRF_MONITOR_REMOTING_ASYNC | COR_PRF_MONITOR_GC | COR_PRF_ENABLE_REJIT | COR_PRF_ENABLE_INPROC_DEBUGGING | COR_PRF_ENABLE_JIT_MAPS | COR_PRF_DISABLE_OPTIMIZATIONS | COR_PRF_DISABLE_INLINING | COR_PRF_ENABLE_OBJECT_ALLOCATED | COR_PRF_ENABLE_FUNCTION_ARGS | COR_PRF_ENABLE_FUNCTION_RETVAL | COR_PRF_ENABLE_FRAME_INFO | COR_PRF_ENABLE_STACK_SNAPSHOT | COR_PRF_USE_PROFILE_IMAGES

	if(m_config.Mode == PM_Disabled)
		return m_ProfilerInfo->SetEventMask(COR_PRF_MONITOR_NONE);

	//Definitely need config for flags, although GC and ObjectAllocated might be one control.

	//initialize with the required flags 
	DWORD eventMask = 0;
	eventMask |= COR_PRF_USE_PROFILE_IMAGES;
	//We want to know what threads are active
	eventMask |= COR_PRF_MONITOR_THREADS;
	//Need this to be able to map modules properly
	eventMask |= COR_PRF_MONITOR_MODULE_LOADS;
	//Inlining can result in rather confusing traces
	if(!m_config.AllowInlining)
		eventMask |= COR_PRF_DISABLE_INLINING;

	if(m_config.TrackMemory)
	{
		//I'm not sure we should track these at all -- CLR Profiler is way better at this than us
		eventMask |= COR_PRF_MONITOR_GC;
	}

	//enabling stack snapshots causes instrumentation to switch to slow mode sadly
	if(m_config.Mode & PM_Sampling)
	{
		eventMask |= COR_PRF_ENABLE_STACK_SNAPSHOT;
	}
	
	if(m_config.Mode & PM_Hybrid)
	{
		//We will add MONITOR_ENTERLEAVE when a frontend connects, not right now
		eventMask |= COR_PRF_MONITOR_CODE_TRANSITIONS;
	}

	m_eventMask = eventMask;
	return m_ProfilerInfo->SetEventMask(m_eventMask);
}

const FunctionInfo* ClrProfiler::GetFunction(unsigned int id)
{
	EnterLock localLock(&m_lock);

	if(id >= m_functions.size())
		return NULL;
	
	FunctionInfo* info = m_functions[id];
	if(info->Name.size() == 0 && !m_suspended)
	{
		//we're not currently suspended, try to map this function
		if(info->IsNative)
			MapUnmanaged(info->NativeId);
		else
			MapFunction(info->NativeId, false);
	}

	return info;
}

const ClassInfo* ClrProfiler::GetClass(unsigned int id)
{
	EnterLock localLock(&m_lock);

	if(id >= m_classes.size())
		return NULL;

	ClassInfo* info = m_classes[id];
	if(info->Name.size() == 0 && !m_suspended)
	{
		MapClass(info->NativeId, NULL);
	}

	return m_classes[id];
}

void ClrProfiler::SetInstrument(unsigned int id, bool enable)
{
	EnterLock localLock(&m_lock);

	if(id >= m_functions.size())
		return;

	m_functions[id]->TriggerInstrumentation = enable;
}

// this function is called by the CLR when a function has been mapped to an ID
UINT_PTR ClrProfiler::StaticFunctionMapper(FunctionID functionID, BOOL *pbHookFunction)
{
	UINT_PTR retVal = functionID;
	ClrProfiler* profiler = g_ProfilerCallback;
	if(profiler == NULL)
		return functionID;

	// make sure the global reference to our profiler is valid.  Forward this
	// call to our profiler object
    retVal = profiler->MapFunction(functionID, true);

	if(profiler->GetMode() & PM_Tracing)
	{
		*pbHookFunction = TRUE;

		if(!profiler->m_config.InstrumentSmallFunctions)
		{
			ModuleID moduleId;
			mdToken token;
			ULONG methodSize;

			HRESULT hr = profiler->m_ProfilerInfo->GetFunctionInfo(functionID, NULL, &moduleId, &token);
			if(FAILED(hr))
				return retVal;

			hr = profiler->m_ProfilerInfo->GetILFunctionBody(moduleId, token, NULL, &methodSize);
			if(FAILED(hr))
				return retVal;

			//if(methodSize < 64)
			//	*pbHookFunction = FALSE;
		}
	}
	else
	{
		*pbHookFunction = FALSE;
	}

	return retVal;
}

unsigned int ClrProfiler::MapModule(ModuleID moduleId)
{
	return 0;
}

unsigned int ClrProfiler::MapClass(mdTypeDef classDef, IMetaDataImport* metadata)
{
	assert(classDef != NULL);

	EnterLock localLock(&m_lock);

	//a TypeDef is only unique within its module, so we'll combine the module and class
	//if metadata is NULL, assume classDef is already fully combined
	UINT_PTR fullClassId = (UINT_PTR) classDef;
	ModuleInfo* moduleInfo = NULL;
	if(metadata != NULL)
	{
		//Get the MVID first
		GUID mvid;
		HRESULT hr = metadata->GetScopeProps(NULL, 0, NULL, &mvid);
		if(FAILED(hr))
			return 0;

		//look up the ModuleInfo using the MVID key
		moduleInfo = m_moduleLookup[mvid];
		if(moduleInfo == NULL)
			return 0;

		//put together a unique class identifier
		fullClassId = ClassIdFromTypeDefAndModule(classDef, moduleInfo->Id);
	}
	else
	{
		//decode the classDef to get the module id
		unsigned int moduleId = (fullClassId & 0xffff0000) >> 16;
		moduleInfo = m_modules[moduleId];
	}

	//Look up the ClassInfo, or create a new one
	ClassInfo* info;
	unsigned int& newId = m_classRemapper[fullClassId];
	if(newId == 0)
	{
		newId = m_classRemapper.Alloc();
		info = new ClassInfo(newId, fullClassId);
		m_classes.push_back(info);
	}
	else
	{
		info = m_classes[newId];
	}

	//Get the class name if we don't have it
	//This is unsafe if we're currently suspended
	if(info->Name.size() == 0 && !m_suspended)
	{
		//get a metadata if we don't have one
		CComPtr<IMetaDataImport> freshMetadata;
		if(metadata == NULL)
		{
			HRESULT hr = m_ProfilerInfo->GetModuleMetaData(moduleInfo->NativeId, ofRead, IID_IMetaDataImport, (IUnknown**) &freshMetadata);
			if(FAILED(hr))
				return newId;
			metadata = freshMetadata;
		}

		//Get the actual class name
		ULONG classLength = Messages::MapClass::MaxNameSize;
		wchar_t className[Messages::MapClass::MaxNameSize];
		HRESULT hr = metadata->GetTypeDefProps(classDef, className, classLength, &classLength, 0, 0);
		if(FAILED(hr))
			return newId;

		if(hr == S_FALSE)
		{
			//This happens with a few types, like <Module> and <CrtImplementationDetails>
			info->Name = L"Unknown";
		}
		else
		{
			info->Name = std::wstring(&className[0], &className[classLength - 1]);
			assert(wcslen(info->Name.c_str()) == info->Name.size());
		}
	}

	return newId;
}

UINT_PTR ClrProfiler::MapFunction(FunctionID functionID, bool deferNameLookup)
{
	EnterLock localLock(&m_lock);

	//Look up the FunctionInfo or create a new one
	FunctionInfo* info;
	unsigned int& newId = m_functionRemapper[functionID];
	if(newId == 0)
	{
		newId = m_functionRemapper.Alloc();
		info = new FunctionInfo(newId, functionID);
		info->IsNative = FALSE;
		m_functions.push_back(info);
	}
	else
	{
		info = m_functions[newId];
	}

	//Get the name and signature if we don't have it
	//this is unsafe if we're currently suspended
	if(!deferNameLookup && info->Name.size() == 0 && !m_suspended)
	{
		Messages::MapFunction mapFunction;
		mapFunction.FunctionId = info->Id;
		mapFunction.IsNative = FALSE;

		//get the method name and class
		ULONG nameLength = Messages::MapFunction::MaxNameSize;
		ULONG signatureLength = Messages::MapFunction::MaxSignatureSize;
		HRESULT hr = GetMethodInfo(functionID, mapFunction.Name, nameLength, mapFunction.ClassId, mapFunction.Signature, signatureLength);
		if(FAILED(hr))
		{
			//Unable to look up the name; not entirely sure why this can happen but it can.
			//We just write some placeholders and continue.
			wchar_t placeholder[] = L"$Unknown$";
			nameLength = sizeof(placeholder) / sizeof(wchar_t) - 1;
			wcscpy_s(mapFunction.Name, Messages::MapFunction::MaxNameSize, placeholder);
			mapFunction.Signature[0] = 0;
			signatureLength = 0;
			mapFunction.ClassId = 0;
		}

		//TODO: Should we send the function mapping or not?

		info->Name = std::wstring(&mapFunction.Name[0], &mapFunction.Name[nameLength - 1]);
		info->ClassId = mapFunction.ClassId;
		if(signatureLength > 0)
			info->Signature = std::wstring(&mapFunction.Signature[0], &mapFunction.Signature[signatureLength - 1]);
	}

	return (UINT_PTR) info;
}

unsigned int ClrProfiler::MapUnmanaged(UINT_PTR address)
{
	EnterLock localLock(&m_lock);

	//copied from http://msdn.microsoft.com/en-us/library/ms680578%28VS.85%29.aspx
	ULONG64 buffer[(sizeof(SYMBOL_INFO) +
		MAX_SYM_NAME * sizeof(TCHAR) +
		sizeof(ULONG64) - 1) /
		sizeof(ULONG64)];
	PSYMBOL_INFO pSymbol = (PSYMBOL_INFO) buffer;
	pSymbol->SizeOfStruct = sizeof(SYMBOL_INFO);
	pSymbol->MaxNameLen = MAX_SYM_NAME;

	DWORD64 displacement;
	BOOL symResult = SymFromAddr(GetCurrentProcess(), address, &displacement, pSymbol);
	if(!symResult)
		return 0;

	address -= (DWORD) displacement;
	unsigned int& id = m_functionRemapper[address];
	if(id == 0)
	{
		id = m_functionRemapper.Alloc();
		FunctionInfo* info = new FunctionInfo(id, address);
		m_functions.push_back(info);

		Messages::MapFunction mapFunction = {0};
		mapFunction.FunctionId = id;
		mapFunction.IsNative = TRUE;
		mapFunction.Signature[0] = 0;

		wcsncpy_s(mapFunction.Name, Messages::MapFunction::MaxNameSize, pSymbol->Name, _TRUNCATE);

		//send the map message
		mapFunction.Write(*m_server, pSymbol->NameLen, 0);
		info->Name = std::wstring(mapFunction.Name, pSymbol->NameLen);
		info->IsNative = TRUE;
		info->ClassId = 0;
	}

	return id;
}

//The returned lengths INCLUDE the null terminator on the string
//They are buffer sizes, not string lengths
HRESULT ClrProfiler::GetMethodInfo(FunctionID functionID, LPWSTR functionName, ULONG& maxFunctionLength,
									 unsigned int& classId, LPWSTR signature, ULONG& maxSignatureLength)
{
	HRESULT hr = S_OK;
	mdToken funcToken = 0;
	const ULONG originalMaxFunctionLength = maxFunctionLength;

	CComPtr<IMetaDataImport> metaData;
	CComPtr<IMetaDataImport2> metaData2;

	hr = m_ProfilerInfo->GetTokenAndMetaDataFromFunction(functionID, IID_IMetaDataImport, (IUnknown**) &metaData, &funcToken);
	CHECK_HR(hr);
	if((funcToken & 0x2b000000) == 0x2b000000)
		__debugbreak();

	hr = metaData->QueryInterface(IID_IMetaDataImport2, (void**) &metaData2);
	CHECK_HR(hr);

	//Get the method name
	mdTypeDef classTypeDef;
	DWORD methodAttribs = 0;
	PCCOR_SIGNATURE sigBlob = NULL;
	ULONG sigBlobSize = 0;
	hr = metaData->GetMethodProps(funcToken, &classTypeDef, functionName, maxFunctionLength,
		&maxFunctionLength, &methodAttribs, &sigBlob, &sigBlobSize, 0, 0);
	CHECK_HR(hr);

	//Map the class, and prepend the class name to the method name
	classId = MapClass(classTypeDef, metaData2);
	if(classId != 0)
	{
		ClassInfo* classInfo = m_classes[classId];

#pragma warning(push)
#pragma warning(disable:4996)
		if(classInfo->Name.size() > 0)
		{
			std::copy(functionName, functionName + maxFunctionLength, functionName + classInfo->Name.size() + 1);
			std::copy(classInfo->Name.begin(), classInfo->Name.end(), functionName);
			functionName[classInfo->Name.size()] = L'.';
			maxFunctionLength += classInfo->Name.size() + 1;

			assert(wcslen(functionName) + 1 == maxFunctionLength);
		}
#pragma warning(pop)
	}

	//Find any generic parameters and fill them into the name
	HCORENUM hEnum = 0;
	mdGenericParam genericParams[32] = {0};
	ULONG genericParamCount = 32;
	//returns S_FALSE if there are no generic params
	hr = metaData2->EnumGenericParams(&hEnum, funcToken, genericParams, 32, &genericParamCount);
	if(FAILED(hr))
		return hr;
	if(genericParamCount > 0)
	{
		wchar_t* end = functionName + maxFunctionLength - 1;
		*end++ = L'<';
		*end = 0;

		wchar_t genericParamName[512];
		ULONG genericNameLength = 512;
		for(ULONG g = 0; g < genericParamCount; ++g)
		{
			hr = metaData2->GetGenericParamProps(genericParams[g], NULL, NULL, NULL, NULL, genericParamName, genericNameLength, &genericNameLength);
			if(FAILED(hr))
				return hr;

			wcscpy_s(end, originalMaxFunctionLength - (end - functionName), genericParamName);
			end += genericNameLength - 1;
		}
		*end++ = L'>';
		*end = 0;
		maxFunctionLength = end - functionName + 1;
		assert(wcslen(functionName) == maxFunctionLength - 1);
	}

	//Set up the signature parser
	signature[0] = 0;
	SigFormat formatter(signature, maxSignatureLength, funcToken, metaData, metaData2);
	formatter.Parse((sig_byte*) sigBlob, sigBlobSize);
	maxSignatureLength = formatter.GetLength() + 1;
	assert(wcslen(signature) + 1 == maxSignatureLength);

	return S_OK;
}

bool ClrProfiler::SuspendAll()
{
	LONG oldValue = InterlockedExchangeAdd(&m_suspended, 1);
	if(oldValue < 0)
	{
		//something got seriously screwed up
		return false;
	}

	EnterLock localLock(&m_lock);

	for(ThreadMap::iterator it = m_threads.begin(); it != m_threads.end(); ++it)
	{
		if(it->second.Destroyed)
			continue;

		DWORD threadId = it->second.SystemId;
		HANDLE hThread = OpenThread(THREAD_SUSPEND_RESUME | THREAD_QUERY_INFORMATION | THREAD_GET_CONTEXT, false, threadId);
		if(hThread == NULL)
		{
			//Couldn't access the thread for whatever reason
			continue;
		}

		SuspendThread(hThread);
		CloseHandle(hThread);
	}

	return true;
}

bool ClrProfiler::ResumeAll()
{
	LONG oldValue = InterlockedExchangeAdd(&m_suspended, -1);
	if(oldValue < 1)
	{
		//totally bogus call
		InterlockedExchangeAdd(&m_suspended, 1);
		return false;
	}

	EnterLock localLock(&m_lock);

	for(ThreadMap::iterator it = m_threads.begin(); it != m_threads.end(); ++it)
	{
		if(it->second.Destroyed)
			continue;

		DWORD threadId = it->second.SystemId;
		HANDLE hThread = OpenThread(THREAD_SUSPEND_RESUME | THREAD_QUERY_INFORMATION | THREAD_GET_CONTEXT, false, threadId);
		if(hThread == NULL)
		{
			//Couldn't access the thread for whatever reason
			continue;
		}

		ResumeThread(hThread);
		CloseHandle(hThread);
	}

	return true;
}

void ClrProfiler::StartSampleTimer(DWORD duration)
{
	if(duration == 0)
		duration = m_config.SampleInterval;

	BOOL result = CreateTimerQueueTimer(&m_sampleTimer, NULL, &ClrProfiler::OnTimerGlobal, this, duration, 0, WT_EXECUTEDEFAULT);
	assert(result);
}

void ClrProfiler::StopSampleTimer()
{
	BOOL result = DeleteTimerQueueTimer(NULL, m_sampleTimer, NULL);
	assert(result == TRUE);
}

void ClrProfiler::Enter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
	FunctionInfo* info = reinterpret_cast<FunctionInfo*>(clientData);

	ThreadID thread;
	m_ProfilerInfo->GetCurrentThreadID(&thread);

	//look up the thread context
	ContextList::iterator it = m_threadContexts.find(thread);
	if(it == m_threadContexts.end())
	{
		//we can't do anything at all without a context!
		assert(it != m_threadContexts.end());
		return;
	}

	ThreadContext& context = it->second;

	if(m_config.Mode == PM_Hybrid)
	{
		//Hybrid mode is enabled, check the context to see if we're instrumenting
		if(info->TriggerInstrumentation)
		{
			//this is a trigger function, we'll need to increment the count
			++context.InstCount;
		}
		else if(context.InstCount == 0)
		{
			//instrumentation isn't currently active
			return;
		}
	}

	context.ShadowStack.push_back(info->Id);

	Messages::FunctionELT enterMsg;
	enterMsg.ThreadId = thread;
	enterMsg.FunctionId = info->Id;
	QueryTimer(enterMsg.TimeStamp);

	//We are in danger of colliding with our sampler from here onwards
	InterlockedIncrement(&m_instCount);
	//write the message
	enterMsg.Write(*m_server, MID_EnterFunction);
	//Safe again
	InterlockedDecrement(&m_instCount);
}

void ClrProfiler::LeaveImpl(FunctionID functionId, FunctionInfo* info, MessageId message)
{
	ThreadID thread;
	m_ProfilerInfo->GetCurrentThreadID(&thread);

	//look up the thread context
	ContextList::iterator it = m_threadContexts.find(thread);
	if(it == m_threadContexts.end())
	{
		//we can't do anything at all without a context!
		assert(it != m_threadContexts.end());
		return;
	}

	ThreadContext& context = it->second;


	if(m_config.Mode == PM_Hybrid)
	{
		//Hybrid mode is enabled, check the context to see if we're instrumenting
		if(context.InstCount == 0)
		{
			//instrumentation isn't currently active
			return;
		}

		if(info->TriggerInstrumentation && context.InstCount > 0)
		{
			//this is a trigger function, we'll need to decrement the count
			--context.InstCount;
		}
	}

	context.ShadowStack.pop_back();

	Messages::FunctionELT leaveMsg;
	leaveMsg.ThreadId = thread;
	leaveMsg.FunctionId = info->Id;
	QueryTimer(leaveMsg.TimeStamp);

	//We are in danger of colliding with our sampler from here onwards
	InterlockedIncrement(&m_instCount);
	//write the message
	leaveMsg.Write(*m_server, message);
	//Safe again
	InterlockedDecrement(&m_instCount);
}

void ClrProfiler::Leave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *argumentRange)
{
	LeaveImpl(functionID, reinterpret_cast<FunctionInfo*>(clientData), MID_LeaveFunction);
}

void ClrProfiler::Tailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo)
{
	LeaveImpl(functionID, reinterpret_cast<FunctionInfo*>(clientData), MID_TailCall);
}

void ClrProfiler::OnTimerGlobal(LPVOID lpParameter, BOOLEAN TimerOrWaitFired)
{
	ClrProfiler* profiler = static_cast<ClrProfiler*>(lpParameter);
	profiler->OnTimer();
}

HRESULT ClrProfiler::StackWalkGlobal(FunctionID funcId, UINT_PTR ip, COR_PRF_FRAME_INFO frameInfo, ULONG32 contextSize, BYTE context[], void *clientData)
{
	//TODO: We should perform a native stack walk when this happens
	if(funcId == 0)
		return S_OK;

	WalkData* data = static_cast<WalkData*>(clientData);
	FunctionInfo* info = reinterpret_cast<FunctionInfo*>(data->profiler->MapFunction(funcId, true));
	data->functions->push_back(info->Id);
	return S_OK;
}

//This is used to block StackWalk64 from trying to go to disk for symbols, which can cause deadlocks
void* CALLBACK FunctionTableAccess(HANDLE hProcess, DWORD64 AddrBase)
{
	return NULL;
}

void ClrProfiler::OnTimer()
{
	EnterLock localLock(&m_lock);

	if(!m_active)
		return;

	if(m_suspended || !SuspendAll())
	{
		//epic fail, try again later
		StartSampleTimer(20);
		return;
	}

	if(m_instCount > 0)
	{
		//We're inside the instrumenting bits of the profiler, try again later
		ResumeAll();
		StartSampleTimer(10);
		return;
	}

	for(ThreadMap::iterator it = m_threads.begin(); it != m_threads.end(); ++it)
	{
		if(it->second.Destroyed)
			continue;

		Messages::Sample sample;
		sample.ThreadId = m_threadRemapper[it->first];

		DWORD threadId = it->second.SystemId;
		HANDLE hThread = OpenThread(THREAD_SUSPEND_RESUME | THREAD_QUERY_INFORMATION | THREAD_GET_CONTEXT, false, threadId);
		if(hThread == NULL)
		{
			//Couldn't access the thread for whatever reason
			continue;
		}

		std::vector<unsigned int>* functions = &sample.Functions;
		functions->reserve(32);

		//Attempt an initial stackwalk with what we've got
		WalkData dataFirst = { this, functions };
		HRESULT walkResult = m_ProfilerInfo2->DoStackSnapshot(it->first, StackWalkGlobal, COR_PRF_SNAPSHOT_DEFAULT,
				&dataFirst, NULL, 0);
		if(SUCCEEDED(walkResult) && functions->size() > 0)
		{
			//it worked, let's move on
			sample.Write(*m_server);
			continue;
		}

		if(walkResult == CORPROF_E_STACKSNAPSHOT_UNSAFE)
		{
			//severe deadlock risk, cancel walk
			continue;
		}

		//Initial failed, time for the complicated version
		//See http://msdn.microsoft.com/en-us/library/bb264782.aspx
		if(functions->size() != 0)
			functions->clear();

		CONTEXT context;
		context.ContextFlags = CONTEXT_FULL;
		GetThreadContext(hThread, &context);
		FunctionID funcId;
		HRESULT funcResult = m_ProfilerInfo->GetFunctionFromIP((BYTE*) context.Eip, &funcId);

		bool inManagedCode = SUCCEEDED(funcResult) && funcId != 0;
		if(!inManagedCode)
		{
			HANDLE hProcess = GetCurrentProcess();

			if(m_config.SampleUnmanaged)
			{
				unsigned int id = MapUnmanaged(context.Eip);
				if(id != 0)
					functions->push_back(id);
			}

			//Stack walk to find the managed stack
			STACKFRAME64 stackFrame = {0};
			stackFrame.AddrPC.Offset = context.Eip;
			stackFrame.AddrPC.Mode = AddrModeFlat;
			stackFrame.AddrFrame.Offset = context.Ebp;
			stackFrame.AddrFrame.Mode = AddrModeFlat;
			stackFrame.AddrStack.Offset = context.Esp;
			stackFrame.AddrStack.Mode = AddrModeFlat;

#ifdef X64
			DWORD machineType = IMAGE_FILE_MACHINE_AMD64;
#else
			DWORD machineType = IMAGE_FILE_MACHINE_I386;
#endif

			while(StackWalk64(machineType, hProcess, hThread, &stackFrame,
				NULL, NULL, FunctionTableAccess, SymGetModuleBase64, NULL))
			{
				if (stackFrame.AddrPC.Offset == stackFrame.AddrReturn.Offset)
					break;

				funcResult = m_ProfilerInfo->GetFunctionFromIP((BYTE*) stackFrame.AddrPC.Offset, &funcId);
				if(SUCCEEDED(funcResult) && funcId != 0)
				{
					//we found our managed stack
					memset(&context, 0, sizeof(context));
					context.Eip = stackFrame.AddrPC.Offset;
					context.Ebp = stackFrame.AddrFrame.Offset;
					context.Esp = stackFrame.AddrStack.Offset;
					inManagedCode = true;
					break;
				}
				else
				{
					//still an unmanaged function
					if(m_config.SampleUnmanaged)
					{
						unsigned int id = MapUnmanaged(context.Eip);
						if(id != 0)
							functions->push_back(id);
					}
				}
			}
		}

		if(inManagedCode)
		{
			WalkData data = { this, functions };
			HRESULT snapshotResult = m_ProfilerInfo2->DoStackSnapshot(it->first, StackWalkGlobal, COR_PRF_SNAPSHOT_DEFAULT,
				&data, (BYTE*) &context, sizeof(context));
			if(FAILED(snapshotResult))
				functions->clear();
		}

		if(functions->size() > 0)
			sample.Write(*m_server);
	}

	ResumeAll();
	if(m_active)
		StartSampleTimer(0);
}

HRESULT ClrProfiler::ObjectAllocated(ObjectID objectId, ClassID classId)
{
	if(!m_server->Connected())
		return S_OK;

	ULONG size = 0;
	m_ProfilerInfo->GetObjectSize(objectId, &size);
	//TODO: Send a message

	return S_OK;
}

HRESULT ClrProfiler::ThreadCreated(ThreadID threadId)
{
	//Map the new thread
	unsigned int& id = m_threadRemapper[threadId];
	id = m_threadRemapper.Alloc();

	//create a context for the thread
	std::pair<ContextList::iterator, bool> contextPair = m_threadContexts.insert(threadId, ThreadContext());
	ThreadInfo info(id, threadId, &contextPair.first->second);
	{
		EnterLock lock(&m_lock);
		m_threads[threadId] = info;
	}

	Messages::CreateThread msg = { id };
	msg.Write(*m_server, MID_CreateThread);

	return S_OK;
}

HRESULT ClrProfiler::ThreadDestroyed(ThreadID threadId)
{
	//taking the lock prevents this from intersecting with the stack walk
	EnterLock lock(&m_lock);

	//it's possible we've never actually seen the thread before and have to map it
	ThreadInfo& info = m_threads[threadId];
	unsigned int& id = m_threadRemapper[threadId];
	if(id == 0)
	{
		id = m_threadRemapper.Alloc();
		info.Id = id;
		info.NativeId = threadId;
	}

	info.Destroyed = true;

	//destroy the context
	ContextList::iterator it = m_threadContexts.remove(threadId);
	//deletion is known to be safe at this point
	delete it;

	Messages::CreateThread msg = { id };
	msg.Write(*m_server, MID_DestroyThread);

	return S_OK;
}

HRESULT ClrProfiler::ThreadNameChanged(ThreadID threadId, ULONG nameLen, WCHAR name[])
{
	//it's possible we've never actually seen the thread before and have to map it
	ThreadInfo& info = m_threads[threadId];
	unsigned int& id = m_threadRemapper[threadId];
	if(id == 0)
	{
		id = m_threadRemapper.Alloc();
		//create a context for the thread
		std::pair<ContextList::iterator, bool> contextPair = m_threadContexts.insert(threadId, ThreadContext());

		//fill in the thread info
		info.Id = id;
		info.NativeId = threadId;
		info.Context = &contextPair.first->second;
		info.Destroyed = false;
	}

	info.Name = std::wstring(&name[0], &name[nameLen]);
	assert(wcslen(info.Name.c_str()) == info.Name.size());

	Messages::NameThread msg;
	msg.ThreadId = id;
	wcsncpy_s(msg.Name, Messages::NameThread::MaxNameSize, name, nameLen);
	msg.Write(*m_server, nameLen);

	return S_OK;
}

HRESULT ClrProfiler::ThreadAssignedToOSThread(ThreadID managedThreadId, DWORD osThreadId)
{
	EnterLock lock(&m_lock);
	m_threads[managedThreadId].SystemId = osThreadId;
	return S_OK;
}

HRESULT ClrProfiler::ModuleLoadFinished(ModuleID moduleId, HRESULT hrStatus)
{
	if(FAILED(hrStatus))
		return S_OK;

	EnterLock localLock(&m_lock);

	CComPtr<IMetaDataImport> metadata;
	HRESULT hr = m_ProfilerInfo2->GetModuleMetaData(moduleId, ofRead, IID_IMetaDataImport, (IUnknown**) &metadata);
	CHECK_HR(hr);
	if(metadata == NULL)
		return S_OK;

	GUID mvid;
	hr = metadata->GetScopeProps(NULL, 0, NULL, &mvid);
	CHECK_HR(hr);

	//add an entry for this module to the remapper and in the modules list
	unsigned int& newId = m_moduleRemapper[moduleId];
	newId = m_moduleRemapper.Alloc();
	ModuleInfo* info = new ModuleInfo(newId);
	info->NativeId = moduleId;
	m_modules.push_back(info);

	m_moduleLookup[mvid] = info;
	return S_OK;
}
