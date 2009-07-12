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
		static const int MaxNameSize = 256;
		static const int MaxClassSize = 256;

		unsigned int FunctionId;
		wchar_t Name[MaxNameSize];
		wchar_t Class[MaxClassSize];

		void Write(IProfilerServer& server, size_t nameCount, size_t classCount);
	};

	//Used for enter, leave, tailcall
	struct FunctionELT
	{
		unsigned __int64 ThreadId;
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
		unsigned __int64 ThreadId;

		void Write(IProfilerServer& server, MessageId id);
	};

	struct NameThread
	{
		static const int MaxNameSize = 256;

		unsigned __int64 ThreadId;
		wchar_t Name[MaxNameSize];

		void Write(IProfilerServer& server, size_t nameCount);
	};

	struct Sample
	{
		unsigned __int64 ThreadId;
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
