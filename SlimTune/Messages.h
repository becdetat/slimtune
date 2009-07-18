/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef MESSAGES_H
#define MESSAGES_H
#pragma once

class IProfilerServer;

enum MessageId
{
	MID_MapFunction = 0x01,

	MID_EnterFunction = 0x10,
	MID_LeaveFunction,
	MID_TailCall,

	MID_ObjectAllocated = 0x20,
	MID_CollectionStarted,
	MID_CollectionFinished,

	MID_TransitionOut = 0x30,		//From managed to unmanaged (out of the runtime)
	MID_TransitionIn,				//From unmanaged to managed (back into the runtime)

	MID_CreateThread = 0x40,
	MID_DestroyThread,
	MID_NameThread,

	MID_Sample = 0x50,

	MID_BeginEvent = 0xf0,
	MID_EndEvent
};

enum ClientRequest
{
	CR_GetFunctionInfo = 0x01,
	CR_GetClassInfo,
	CR_GetThreadInfo,
};

namespace Messages
{
	struct MapFunction
	{
		static const int MaxNameSize = 1024;

		unsigned int FunctionId;
		wchar_t SymbolName[MaxNameSize];

		void Write(IProfilerServer& server, size_t nameCount);
	};

	//Used for enter, leave, tailcall
	struct FunctionELT
	{
		unsigned int ThreadId;
		unsigned int FunctionId;
		unsigned __int64 TimeStamp;

		void Write(IProfilerServer& server, MessageId id);
	};

	struct ObjectAllocated
	{
		ClassID ClassId;
		size_t Size;
	};

	//Also used for DestroyThread
	struct CreateThread
	{
		unsigned int ThreadId;

		void Write(IProfilerServer& server, MessageId id);
	};

	struct NameThread
	{
		static const int MaxNameSize = 256;

		unsigned int ThreadId;
		wchar_t Name[MaxNameSize];

		void Write(IProfilerServer& server, size_t nameCount);
	};

	struct Sample
	{
		unsigned int ThreadId;
		std::vector<unsigned int> Functions;

		void Write(IProfilerServer& server);
	};
}

namespace Requests
{
	struct GetFunctionInfo
	{
		unsigned int FunctionId;
	};

	struct GetClassInfo
	{
		unsigned int ClassId;
	};

	struct GetThreadInfo
	{
		unsigned int ThreadId;
	};
}

#endif
