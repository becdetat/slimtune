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
Copyright (c) 2009  Promit Roy

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.

You should have received a copy of the GNU Library General Public
License along with this library; if not, write to the
Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
Boston, MA  02110-1301, USA.
*/

#include "stdafx.h"
#include "Profiler.h"
#include "NativeHooks.h"
#include "ThreadState.h"
#include "Timer.h"

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
	InitializeCriticalSectionAndSpinCount(&m_lock, 200);
	m_functions.reserve(4096);
	
	FunctionInfo* invalid = new FunctionInfo(0);
	invalid->Class = L"$INVALID$";
	invalid->Name = L"$INVALID$";
	m_functions.push_back(invalid);
}

CProfiler::~CProfiler()
{
	DeleteCriticalSection(&m_lock);
}

HRESULT CProfiler::FinalConstruct()
{
#ifdef DEBUG
	__debugbreak();
#endif

	return S_OK;
}

void CProfiler::FinalRelease()
{
}

STDMETHODIMP CProfiler::Initialize(IUnknown *pICorProfilerInfoUnk)
{
	//multiple active profilers are not a supported configuration, if that's even possible
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
	m_server.reset(IProfilerServer::CreateSocketServer(*this, 200));
	m_server->SetCallbacks(boost::bind(&CProfiler::OnConnect, this), boost::bind(&CProfiler::OnDisconnect, this));
	m_server->Start();
	//CONFIG: Wait for connection?
	m_server->WaitForConnection();
	//kick off the IO thread
	IoThreadFunc threadFunc(*m_server);
	m_ioThread.reset(new boost::thread(threadFunc));

	//initialize high performance timing
	InitializeTimer();
	QueryTimerFreq(m_timerFreq);
	unsigned __int64 time;
	QueryTimer(time);
	srand((unsigned int) time);

	// set up our global access pointer
	g_ProfilerCallback = this;

	if(m_mode == PM_Sampling)
	{
		timeBeginPeriod(1);
		StartSampleTimer();
	}

    return S_OK;
}

STDMETHODIMP CProfiler::Shutdown()
{
	//force everything else to finish
	EnterLock localLock(&m_lock);

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

		//get the method name and class
		ULONG nameLength = Messages::MapFunction::MaxNameSize;
		ULONG classLength = Messages::MapFunction::MaxClassSize;
		HRESULT hr = GetFullMethodName(functionID, mapFunction.Name, nameLength, mapFunction.Class, classLength); 
		assert(SUCCEEDED(hr));
		//send the map message
		mapFunction.Write(*m_server, nameLength - 1, classLength - 1);

		info->Name = std::wstring(mapFunction.Name, nameLength);
		info->Class = std::wstring(mapFunction.Class, classLength);
	}
	else
	{
		info = m_functions[newId];
	}

	return (UINT_PTR) info;
}

void CProfiler::StartSampleTimer()
{
	//CONFIG: Timing resolution
	BOOL result = CreateTimerQueueTimer(&m_sampleTimer, NULL, &CProfiler::OnTimerGlobal, this, 1, 0, WT_EXECUTEDEFAULT);
	assert(result);
}

void CProfiler::StopSampleTimer()
{
	BOOL result = DeleteTimerQueueTimer(NULL, m_sampleTimer, NULL);
	assert(result);
}

HRESULT CProfiler::GetFullMethodName(FunctionID functionID, LPWSTR wszFunction, ULONG& maxFunctionLength, LPWSTR wszClass, ULONG& maxClassLength)
{
	IMetaDataImport* pIMetaDataImport = 0;
	HRESULT hr = S_OK;
	mdToken funcToken = 0;

	// get the token for the function which we will use to get its name
	hr = m_ProfilerInfo->GetTokenAndMetaDataFromFunction(functionID, IID_IMetaDataImport, (LPUNKNOWN *) &pIMetaDataImport, &funcToken);
	if(SUCCEEDED(hr))
	{
		mdTypeDef classTypeDef;

		// retrieve the function properties based on the token
		hr = pIMetaDataImport->GetMethodProps(funcToken, &classTypeDef, wszFunction, maxFunctionLength, &maxFunctionLength, 0, 0, 0, 0, 0);
		if (SUCCEEDED(hr))
		{
			// get the function name
			hr = pIMetaDataImport->GetTypeDefProps(classTypeDef, wszClass, maxClassLength, &maxClassLength, 0, 0);
		}
		// release our reference to the metadata
		pIMetaDataImport->Release();
	}

	return hr;
}

void CProfiler::Enter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
	FunctionInfo* info = (FunctionInfo*) clientData;

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
	FunctionInfo* info = (FunctionInfo*) clientData;

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
	FunctionInfo* info = (FunctionInfo*) clientData;

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
	if(funcId == 0)
		return S_OK;

	WalkData* data = (WalkData*) clientData;
	FunctionInfo* info = (FunctionInfo*) data->profiler->MapFunction(funcId);
	unsigned int id = info->Id;
	data->functions->push_back(id);
	return S_OK;
}

void CProfiler::OnTimer()
{
	//This code is basically lifted out of the core of NProf and cleaned up, although it's basically
	//just a literal implementation of the algorithm described in this article:
	//http://msdn.microsoft.com/en-us/library/bb264782.aspx

	EnterLock localLock(&m_lock);

	for(ThreadMap::iterator it = m_threads.begin(); it != m_threads.end(); ++it)
	{
		Messages::Sample sample;
		sample.ThreadId = it->first;

		DWORD threadId = it->second.SystemId;
		HANDLE threadHandle = OpenThread(THREAD_SUSPEND_RESUME | THREAD_QUERY_INFORMATION | THREAD_GET_CONTEXT, false, threadId);
		if(threadHandle != NULL)
		{
			SuspendThread(threadHandle);
			std::vector<unsigned int>* functions = &sample.Functions;
			functions->reserve(32);
			ThreadID id = it->first;

			CONTEXT context;
			context.ContextFlags = CONTEXT_FULL;
			GetThreadContext(threadHandle, &context);
			FunctionID newID;
			HRESULT result = m_ProfilerInfo->GetFunctionFromIP((BYTE*) context.Eip, &newID);

			bool found = false;
			if(result != S_OK || newID != 0)
			{
				STACKFRAME64 stackFrame = {0};
				stackFrame.AddrPC.Offset = context.Eip;
				stackFrame.AddrPC.Mode = AddrModeFlat;
				stackFrame.AddrFrame.Offset = context.Ebp;
				stackFrame.AddrFrame.Mode = AddrModeFlat;
				stackFrame.AddrStack.Offset = context.Esp;
				stackFrame.AddrStack.Mode = AddrModeFlat;
				while(true)
				{
					if(!StackWalk64(IMAGE_FILE_MACHINE_I386, GetCurrentProcess(), threadHandle, &stackFrame,
						NULL, NULL, SymFunctionTableAccess64, SymGetModuleBase64, NULL))
					{
						break;
					}

					if (stackFrame.AddrPC.Offset == stackFrame.AddrReturn.Offset)
					{
						break;
					}

					FunctionID id;
					HRESULT result = m_ProfilerInfo->GetFunctionFromIP((BYTE*) stackFrame.AddrPC.Offset, &id);
					if(result == S_OK && id != 0)
					{
						memset(&context, 0, sizeof(context));
						context.Eip = stackFrame.AddrPC.Offset;
						context.Ebp = stackFrame.AddrFrame.Offset;
						context.Esp = stackFrame.AddrStack.Offset;
						found = true;
						break;
					}
				}
			}
			else 
			{
				found = true;
			}

			if(found)
			{
				WalkData data = { this, functions };
				m_ProfilerInfo2->DoStackSnapshot(id, StackWalkGlobal, COR_PRF_SNAPSHOT_DEFAULT,
					&data, (BYTE*) &context, sizeof(context));

				sample.Write(*m_server);
			}

			ResumeThread(threadHandle);
		}
	}

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

	Messages::CreateThread msg = { threadId };
	msg.Write(*m_server, MID_CreateThread);

	return S_OK;
}

HRESULT CProfiler::ThreadDestroyed(ThreadID threadId)
{
	//wrap the lock so that it isn't held for the whole time
	{
		EnterLock lock(&m_lock);
		m_threads[threadId].Destroyed = true;
	}

	Messages::CreateThread msg = { threadId };
	msg.Write(*m_server, MID_DestroyThread);

	return S_OK;
}

HRESULT CProfiler::ThreadNameChanged(ThreadID threadId, ULONG nameLen, WCHAR name[])
{
	Messages::NameThread msg;
	msg.ThreadId = threadId;
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
