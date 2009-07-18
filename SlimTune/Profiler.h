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

#pragma once

#include "IProfilerServer.h"
#include "IdRemapper.h"
#include "Messages.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

#define ASSERT_HR(x) _ASSERT(SUCCEEDED(x))
#define NAME_BUFFER_SIZE 1024

struct ProfilerConfig
{
	bool AllowSampling;
	bool ProfileGC;
	bool ProfileTransitions;

	bool Instrument;
	bool ProfileUnmanaged;
};

struct FunctionInfo
{
	static const LONG Multiplier = 17;

	const unsigned int Id;
	std::wstring Name;
	std::wstring Class;

	//statistical analysis
	//CONTROL: Clear stats
	volatile LONG HitCount;
	volatile LONG NextLevel;
	volatile LONG Divider;	//the real divider value is 2^n

	FunctionInfo(unsigned int id)
		: Id(id),
		HitCount(0),
		NextLevel(Multiplier),
		Divider(0)
	{
	}
};

struct ThreadInfo
{
	ThreadID ThreadId;
	DWORD SystemId;
	wchar_t Name[Messages::NameThread::MaxNameSize];
	bool Destroyed;
};

enum ProfilerMode
{
	PM_Disabled = 0,

	PM_Sampling = 0x01,
	PM_Tracing = 0x02,

	PM_Hybrid = PM_Sampling | PM_Tracing,
};

// CProfiler
class ATL_NO_VTABLE CProfiler :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CProfiler, &CLSID_Profiler>,
	public CCorProfilerCallbackImpl
{
public:
	CProfiler();
	virtual ~CProfiler();

	DECLARE_REGISTRY_RESOURCEID(IDR_PROFILER)
	BEGIN_COM_MAP(CProfiler)
		COM_INTERFACE_ENTRY(ICorProfilerCallback)
		COM_INTERFACE_ENTRY(ICorProfilerCallback2)
	END_COM_MAP()
	DECLARE_PROTECT_FINAL_CONSTRUCT()

	// overridden implementations of FinalConstruct and FinalRelease
	HRESULT FinalConstruct();
	void FinalRelease();

	bool IsActive() const { return m_active; }
	ProfilerMode GetMode() const { return m_mode; }

    // STARTUP/SHUTDOWN EVENTS
    STDMETHOD(Initialize)(IUnknown *pICorProfilerInfoUnk);
    STDMETHOD(Shutdown)();

	// callback functions
	void Enter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
	void Leave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *argumentRange);
	void Tailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo);

	// mapping functions
	static UINT_PTR _stdcall StaticFunctionMapper(FunctionID functionId, BOOL *pbHookFunction);
	UINT_PTR MapFunction(FunctionID);

	STDMETHOD(ObjectAllocated)(ObjectID objectId, ClassID classId);

	STDMETHOD(ThreadCreated)(ThreadID threadId);
	STDMETHOD(ThreadDestroyed)(ThreadID threadId);
	STDMETHOD(ThreadNameChanged)(ThreadID threadId, ULONG nameLen, WCHAR name[]);
	STDMETHOD(ThreadAssignedToOSThread)(ThreadID managedThreadId, DWORD osThreadId);

private:
	HRESULT GetFullMethodName(FunctionID functionID, LPWSTR wszFunction, ULONG& maxFunctionLength, LPWSTR wszClass, ULONG& maxClassLength);
	HRESULT SetInitialEventMask();

	//COM Interface pointers
	CComQIPtr<ICorProfilerInfo> m_ProfilerInfo;
	CComQIPtr<ICorProfilerInfo2> m_ProfilerInfo2;
	volatile LONG m_currentEventMask;
	ProfilerMode m_mode;

	boost::scoped_ptr<IProfilerServer> m_server;
	boost::scoped_ptr<boost::thread> m_ioThread;
	volatile bool m_active;

	unsigned __int64 m_timerFreq;

	//Mainly used as a data compression trick for the communication layer
	IdRemapper m_threadRemapper;
	IdRemapper m_functionRemapper;
	IdRemapper m_classRemapper;

	CRITICAL_SECTION m_lock;
	std::vector<FunctionInfo*> m_functions;
	typedef std::tr1::unordered_map<UINT_PTR, ThreadInfo> ThreadMap;
	ThreadMap m_threads;

	HANDLE m_sampleTimer;

	void OnConnect();
	void OnDisconnect();

	static void CALLBACK OnTimerGlobal(LPVOID lpParameter, BOOLEAN TimerOrWaitFired);
	void OnTimer();
	void StartSampleTimer();
	void StopSampleTimer();

	struct WalkData
	{
		CProfiler* profiler;
		std::vector<unsigned int>* functions;
	};

	static HRESULT CALLBACK StackWalkGlobal(FunctionID funcId, UINT_PTR ip, COR_PRF_FRAME_INFO frameInfo, ULONG32 contextSize, BYTE context[], void *clientData);
};

OBJECT_ENTRY_AUTO(__uuidof(Profiler), CProfiler)
