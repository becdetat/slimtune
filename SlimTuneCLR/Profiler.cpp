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
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#include "stdafx.h"
#include "Profiler.h"
#include "NativeHooks.h"
#include "ThreadState.h"
#include "Timer.h"
#include "SigFormat.h"

// global reference to the profiler object (ie this) used by the static functions
CProfiler* g_ProfilerCallback = NULL;

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

CProfiler::CProfiler()
: m_server(NULL)
{
#ifdef DEBUG
	__debugbreak();
#endif

	InitializeCriticalSectionAndSpinCount(&m_lock, 200);
	m_functions.reserve(4096);
	
	FunctionInfo* invalid = new FunctionInfo(0);
	invalid->Name = L"$INVALID$";
	m_functions.push_back(invalid);
}

CProfiler::~CProfiler()
{
	DeleteCriticalSection(&m_lock);
}

HRESULT CProfiler::FinalConstruct()
{
	return S_OK;
}

void CProfiler::FinalRelease()
{
}

STDMETHODIMP CProfiler::Initialize(IUnknown *pICorProfilerInfoUnk)
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

	//set up unmanaged configuration
	SymSetOptions(SYMOPT_UNDNAME | SYMOPT_DEFERRED_LOADS);
	SymInitialize(GetCurrentProcess(), NULL, TRUE);

	m_mode = PM_Sampling;

	hr = SetInitialEventMask();
    assert(SUCCEEDED(hr));

	//CONFIG: Choose profiling mode? (sampling vs instrumentation)
	hr = m_ProfilerInfo2->SetFunctionIDMapper(StaticFunctionMapper);
	assert(SUCCEEDED(hr));

	hr = m_ProfilerInfo2->SetEnterLeaveFunctionHooks2(FunctionEnterNaked, FunctionLeaveNaked, FunctionTailcallNaked);
	assert(SUCCEEDED(hr));

	//CONFIG: port?
	//CONFIG: Server type?
	m_active = false;
	m_server.reset(IProfilerServer::CreateSocketServer(*this, 3000));
	m_server->SetCallbacks(boost::bind(&CProfiler::OnConnect, this), boost::bind(&CProfiler::OnDisconnect, this));
	m_server->Start();

	//initialize high performance timing
	InitializeTimer();
	QueryTimerFreq(m_timerFreq);
	unsigned __int64 time;
	QueryTimer(time);
	srand((unsigned int) time);

	// set up our global access pointer
	g_ProfilerCallback = this;

	//CONFIG: Wait for connection?
	m_server->WaitForConnection();
	//kick off the IO thread
	IoThreadFunc threadFunc(*m_server);
	m_ioThread.reset(new boost::thread(threadFunc));

    return S_OK;
}

STDMETHODIMP CProfiler::Shutdown()
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

void CProfiler::OnConnect()
{
#if 0
	LONG eventMask = m_currentEventMask;
	//eventMask |= COR_PRF_MONITOR_OBJECT_ALLOCATED;
	eventMask |= COR_PRF_MONITOR_THREADS;
	InterlockedExchange(&m_currentEventMask, eventMask);
	HRESULT hr = m_ProfilerInfo->SetEventMask((DWORD) eventMask);
	assert(SUCCEEDED(hr));
#endif

	if(m_mode == PM_Sampling)
	{
		timeBeginPeriod(1);
		StartSampleTimer(200);
	}

	m_active = true;
}

void CProfiler::OnDisconnect()
{
#if 0
	//Nobody is listening for profiling info anymore, so stop monitoring this stuff
	LONG eventMask = m_currentEventMask;
	//eventMask &= ~COR_PRF_MONITOR_OBJECT_ALLOCATED;
	eventMask &= ~COR_PRF_MONITOR_THREADS;
	InterlockedExchange(&m_currentEventMask, eventMask);
	HRESULT hr = m_ProfilerInfo->SetEventMask((DWORD) eventMask);
	assert(SUCCEEDED(hr));
	/*HRESULT hr = SetInitialEventMask();
	assert(SUCCEEDED(hr));*/
#endif

	m_active = false;
}

HRESULT CProfiler::SetInitialEventMask()
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

	//CONFIG: event masks?
	//Definitely need config for ALL of these flags, although GC and ObjectAllocated might be one control.

	//initialize with the immutable flags 
	DWORD eventMask = 0;
	//Monitoring code transitions is probably only useful in instrumented mode
	eventMask |= COR_PRF_MONITOR_CODE_TRANSITIONS;
	//We want to know what threads are active
	eventMask |= COR_PRF_MONITOR_THREADS;
	//I'm not sure we should track these at all -- CLR Profiler is way better at this than us
	eventMask |= COR_PRF_MONITOR_GC | COR_PRF_ENABLE_OBJECT_ALLOCATED;
	//enabling stack snapshots causes instrumentation to switch to slow mode, so it should only be set for sampling mode
	if(m_mode == PM_Sampling)
	{
		eventMask |= COR_PRF_ENABLE_STACK_SNAPSHOT;
	}
	else if(m_mode == PM_Tracing)
	{
		eventMask |= COR_PRF_MONITOR_ENTERLEAVE;
	}

	m_currentEventMask = eventMask;
	return m_ProfilerInfo->SetEventMask(m_currentEventMask);
}

// this function is called by the CLR when a function has been mapped to an ID
UINT_PTR CProfiler::StaticFunctionMapper(FunctionID functionID, BOOL *pbHookFunction)
{
	// make sure the global reference to our profiler is valid.  Forward this
	// call to our profiler object
	UINT_PTR retVal = functionID;
    if (g_ProfilerCallback != NULL)
        retVal = g_ProfilerCallback->MapFunction(functionID);

	//CONFIG: Instrumentation enabled?
	*pbHookFunction = TRUE;
	return retVal;
}

UINT_PTR CProfiler::MapFunction(FunctionID functionID)
{
	EnterLock localLock(&m_lock);

	FunctionInfo* info;
	unsigned int& newId = m_functionRemapper[functionID];
	if(newId == 0)
	{
		newId = m_functionRemapper.Alloc();
		info = new FunctionInfo(newId);
		m_functions.push_back(info);

		Messages::MapFunction mapFunction;
		mapFunction.FunctionId = newId;
		mapFunction.IsNative = FALSE;

		//get the method name and class
		const int kMaxSize = 512;
		const int kMaxSignature = Messages::MapFunction::MaxSignatureSize;
		wchar_t name[kMaxSize];
		wchar_t className[kMaxSize];
		wchar_t signature[kMaxSignature];
		ULONG nameLength = kMaxSize;
		ULONG classLength = kMaxSize;
		ULONG signatureLength = kMaxSignature;
		HRESULT hr = GetFullMethodName(functionID, name, nameLength, className, classLength, signature, signatureLength);
		if(SUCCEEDED(hr))
		{
			wcscpy_s(mapFunction.SymbolName, Messages::MapFunction::MaxNameSize, className);
			mapFunction.SymbolName[classLength - 1] = L'.';
			mapFunction.SymbolName[classLength] = 0;
			wcscat_s(mapFunction.SymbolName, Messages::MapFunction::MaxNameSize, name);
			nameLength += classLength - 1;
			assert(wcslen(mapFunction.SymbolName) == nameLength);
			wcscpy_s(mapFunction.Signature, Messages::MapFunction::MaxSignatureSize, signature);
		}
		else
		{
			//Unable to look up the name; not entirely sure why this can happen but it can.
			//We just write some placeholders and continue.
			wchar_t placeholder[] = L"$Unknown$";
			nameLength = sizeof(placeholder) / sizeof(wchar_t) - 1;
			wcscpy_s(mapFunction.SymbolName, Messages::MapFunction::MaxNameSize, placeholder);
			signature[0] = 0;
			signatureLength = 0;
		}

		//send the map message
		mapFunction.Write(*m_server, nameLength, signatureLength);

		info->Name = std::wstring(mapFunction.SymbolName, nameLength);
		info->Signature = std::wstring(mapFunction.Signature, signatureLength);
		info->IsNative = FALSE;
	}
	else
	{
		info = m_functions[newId];
	}

	return (UINT_PTR) info;
}

unsigned int CProfiler::MapUnmanaged(UINT_PTR address)
{
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
		FunctionInfo* info = new FunctionInfo(id);
		m_functions.push_back(info);

		Messages::MapFunction mapFunction = {0};
		mapFunction.FunctionId = id;
		mapFunction.IsNative = TRUE;
		mapFunction.Signature[0] = 0;

		wcsncpy_s(mapFunction.SymbolName, Messages::MapFunction::MaxNameSize, pSymbol->Name, _TRUNCATE);

		//send the map message
		mapFunction.Write(*m_server, pSymbol->NameLen, 0);
		info->Name = std::wstring(mapFunction.SymbolName, pSymbol->NameLen);
		info->IsNative = TRUE;
	}

	return id;
}

#define CHECK_HR(hr) if(FAILED(hr)) return (hr);

HRESULT CProfiler::GetFullMethodName(FunctionID functionID, LPWSTR functionName, ULONG& maxFunctionLength,
									 LPWSTR className, ULONG& maxClassLength, LPWSTR signature, ULONG& maxSignatureLength)
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

	//Get the owning class
	hr = metaData->GetTypeDefProps(classTypeDef, className, maxClassLength, &maxClassLength, 0, 0);
	CHECK_HR(hr);

	/*ClassID args[64];
	ULONG32 argCount = 64;
	hr = m_ProfilerInfo2->GetFunctionInfo2(functionID, NULL, NULL, NULL, &funcToken, argCount, &argCount, args);
	CHECK_HR(hr);

	ULONG tempSigLength = 0;
	ClassID typeArgs[64];
	ULONG32 typeArgCount = 64;
	for(ULONG32 c = 0; c < argCount; ++c)
	{
		HCORENUM hEnum;
		mdMethodSpec specs[32];
		ULONG numSpecs = 32;
		hr = metaData2->EnumMethodSpecs(&hEnum, funcToken, specs, numSpecs, &numSpecs);
		CHECK_HR(hr);

		mdTypeDef token;
		hr = m_ProfilerInfo2->GetClassIDInfo2(args[c], NULL, &token, NULL, 64, &typeArgCount, typeArgs);
		CHECK_HR(hr);

		ULONG argLength = 0;
		hr = metaData->GetTypeDefProps(token, signature + tempSigLength, maxSignatureLength - tempSigLength, &argLength, 0, 0);
		CHECK_HR(hr);
	}

	maxSignatureLength = tempSigLength;*/

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
	maxSignatureLength = formatter.GetLength();

	return S_OK;
}

void CProfiler::StartSampleTimer(DWORD duration)
{
	//CONFIG: Timing resolution?
	BOOL result = CreateTimerQueueTimer(&m_sampleTimer, NULL, &CProfiler::OnTimerGlobal, this, duration, 0, WT_EXECUTEDEFAULT);
	assert(result);
}

void CProfiler::StopSampleTimer()
{
	BOOL result = DeleteTimerQueueTimer(NULL, m_sampleTimer, NULL);
	assert(result == TRUE);
}

void CProfiler::Enter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
	FunctionInfo* info = reinterpret_cast<FunctionInfo*>(clientData);

	ThreadID thread;
	m_ProfilerInfo->GetCurrentThreadID(&thread);

	Messages::FunctionELT enterMsg;
	enterMsg.ThreadId = thread;
	enterMsg.FunctionId = clientData;

#if 0
	//CONFIG: statistical mode?
	LONG hitCount = InterlockedIncrement(&info->HitCount);
	LONG nextLevel = info->NextLevel;
	//there is a race condition here, where hitCount reaches nextLevel * FunctionInfo::Multiplier
	//BEFORE we manage to update it. This is really, really unlikely with the current multiplier of 17.
	if(hitCount == nextLevel)
	{
		InterlockedExchange(&info->NextLevel, nextLevel * FunctionInfo::Multiplier);
		InterlockedIncrement(&info->Divider);
	}

	LONG divider = info->Divider;
	divider = std::max(divider - 2, (LONG) 0);
	if(hitCount % (1 << divider) == 0)
	{
		QueryTimer(enterMsg.TimeStamp);
	}
#else
	QueryTimer(enterMsg.TimeStamp);
#endif

	enterMsg.Write(*m_server, MID_EnterFunction);
}

void CProfiler::Leave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *argumentRange)
{
	FunctionInfo* info = reinterpret_cast<FunctionInfo*>(clientData);

	ThreadID thread;
	m_ProfilerInfo->GetCurrentThreadID(&thread);

	Messages::FunctionELT leaveMsg;
	leaveMsg.ThreadId = thread;
	leaveMsg.FunctionId = clientData;
	QueryTimer(leaveMsg.TimeStamp);
	leaveMsg.Write(*m_server, MID_LeaveFunction);
}

void CProfiler::Tailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo)
{
	FunctionInfo* info = reinterpret_cast<FunctionInfo*>(clientData);

	ThreadID thread;
	m_ProfilerInfo->GetCurrentThreadID(&thread);

	Messages::FunctionELT tailCallMsg;
	tailCallMsg.ThreadId = thread;
	tailCallMsg.FunctionId = clientData;
	QueryTimer(tailCallMsg.TimeStamp);
	tailCallMsg.Write(*m_server, MID_TailCall);
}

void CProfiler::OnTimerGlobal(LPVOID lpParameter, BOOLEAN TimerOrWaitFired)
{
	CProfiler* profiler = static_cast<CProfiler*>(lpParameter);
	profiler->OnTimer();
}

HRESULT CProfiler::StackWalkGlobal(FunctionID funcId, UINT_PTR ip, COR_PRF_FRAME_INFO frameInfo, ULONG32 contextSize, BYTE context[], void *clientData)
{
	//TODO: We should perform a native stack walk when this happens
	if(funcId == 0)
		return S_OK;

	WalkData* data = static_cast<WalkData*>(clientData);
	FunctionInfo* info = reinterpret_cast<FunctionInfo*>(data->profiler->MapFunction(funcId));
	data->functions->push_back(info->Id);
	return S_OK;
}

//This is used to block StackWalk64 from trying to go to disk for symbols, which can cause deadlocks
void* CALLBACK FunctionTableAccess(HANDLE hProcess, DWORD64 AddrBase)
{
	return NULL;
}

void CProfiler::OnTimer()
{
	//See http://msdn.microsoft.com/en-us/library/bb264782.aspx

	EnterLock localLock(&m_lock);

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
			//Couldn't access the thread for whatever reason, just start new timing and bail
			StartSampleTimer();
			return;
		}

		SuspendThread(hThread);

		std::vector<unsigned int>* functions = &sample.Functions;
		functions->reserve(32);

		CONTEXT context;
		context.ContextFlags = CONTEXT_FULL;
		GetThreadContext(hThread, &context);
		FunctionID funcId;
		HRESULT funcResult = m_ProfilerInfo->GetFunctionFromIP((BYTE*) context.Eip, &funcId);

		bool inManagedCode = SUCCEEDED(funcResult) && funcId != 0;
		if(!inManagedCode)
		{
			HANDLE hProcess = GetCurrentProcess();

			//unsigned int id = MapUnmanaged(context.Eip);
			//if(id != 0)
			//	functions->push_back(id);

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
					//CONFIG: Include native functions?
					//unsigned int id = MapUnmanaged(context.Eip);
					//if(id != 0)
					//	functions->push_back(id);
				}
			}
		}

		if(inManagedCode)
		{
			WalkData data = { this, functions };
			/*HRESULT snapshotResult =*/ m_ProfilerInfo2->DoStackSnapshot(it->first, StackWalkGlobal, COR_PRF_SNAPSHOT_DEFAULT,
				&data, (BYTE*) &context, sizeof(context));
		}

		if(functions->size() > 0)
			sample.Write(*m_server);

		ResumeThread(hThread);
	}

	if(m_active)
		StartSampleTimer();
}

HRESULT CProfiler::ObjectAllocated(ObjectID objectId, ClassID classId)
{
	if(!m_server->Connected())
		return S_OK;

	ULONG size = 0;
	m_ProfilerInfo->GetObjectSize(objectId, &size);
	//TODO: Send a message

	return S_OK;
}

HRESULT CProfiler::ThreadCreated(ThreadID threadId)
{
	ThreadInfo info;
	info.ThreadId = threadId;
	info.Name[0] = 0;
	info.Destroyed = false;

	//wrap the lock so that it isn't held for the whole time
	{
		EnterLock lock(&m_lock);
		m_threads[threadId] = info;
	}

	unsigned int& id = m_threadRemapper[threadId];
	if(id == 0)
		id = m_threadRemapper.Alloc();

	Messages::CreateThread msg = { id };
	msg.Write(*m_server, MID_CreateThread);

	return S_OK;
}

HRESULT CProfiler::ThreadDestroyed(ThreadID threadId)
{
	//wrap the lock so that it isn't held for the whole time
	//taking the lock also prevents this from intersecting with the stack walk
	{
		EnterLock lock(&m_lock);
		m_threads[threadId].Destroyed = true;
	}

	unsigned int& id = m_threadRemapper[threadId];
	if(id == 0)
		id = m_threadRemapper.Alloc();

	Messages::CreateThread msg = { id };
	msg.Write(*m_server, MID_DestroyThread);

	return S_OK;
}

HRESULT CProfiler::ThreadNameChanged(ThreadID threadId, ULONG nameLen, WCHAR name[])
{
	unsigned int& id = m_threadRemapper[threadId];
	if(id == 0)
		id = m_threadRemapper.Alloc();

	Messages::NameThread msg;
	msg.ThreadId = id;
	wcsncpy_s(msg.Name, Messages::NameThread::MaxNameSize, name, nameLen);

	msg.Write(*m_server, nameLen);

	return S_OK;
}

HRESULT CProfiler::ThreadAssignedToOSThread(ThreadID managedThreadId, DWORD osThreadId)
{
	EnterLock lock(&m_lock);
	m_threads[managedThreadId].SystemId = osThreadId;
	return S_OK;
}
