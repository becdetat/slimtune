#include "stdafx.h"
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

#include "Messages.h"
#include "IProfilerServer.h"

namespace Messages
{
	typedef char MessageIdStorageType;

	void MapFunction::Write(IProfilerServer& server, size_t nameCount, size_t classCount)
	{
		char* const buffer = (char*) _alloca(6 + sizeof(wchar_t) * (nameCount + classCount));
		char* bufPtr = buffer;

		*bufPtr++ = MID_MapFunction;
		bufPtr = Write7BitEncodedInt(bufPtr, this->FunctionId);
		bufPtr = WriteString(bufPtr, this->Name, nameCount);
		bufPtr = WriteString(bufPtr, this->Class, classCount);

		server.Write(buffer, bufPtr - buffer);
	}

	void FunctionELT::Write(IProfilerServer& server, MessageId id)
	{
		//comfortably big enough
		char buffer[32];
		char* bufPtr = buffer;

		*bufPtr++ = (char) id;
		bufPtr = Write7BitEncodedInt64(bufPtr, ThreadId);
		bufPtr = Write7BitEncodedInt(bufPtr, FunctionId);
		bufPtr = Write7BitEncodedInt64(bufPtr, TimeStamp);

		server.Write(buffer, bufPtr - buffer);
	}

	void CreateThread::Write(IProfilerServer& server, MessageId id)
	{
		char buffer[16];
		char* bufPtr = buffer;

		*bufPtr++ = (char) id;
		bufPtr = Write7BitEncodedInt64(bufPtr, ThreadId);

		server.Write(buffer, bufPtr - buffer);
	}

	void NameThread::Write(IProfilerServer& server, size_t nameCount)
	{
		char* const buffer = (char*) _alloca(16 + sizeof(wchar_t) * nameCount);
		char* bufPtr = buffer;

		*bufPtr++ = MID_NameThread;
		bufPtr = Write7BitEncodedInt64(bufPtr, ThreadId);
		bufPtr = WriteString(bufPtr, this->Name, nameCount);

		server.Write(buffer, bufPtr - buffer);
	}

	void Sample::Write(IProfilerServer& server)
	{
		char* const buffer = (char*) _alloca(24 + 10 * Functions.size());
		char* bufPtr = buffer;

		*bufPtr++ = MID_Sample;
		bufPtr = Write7BitEncodedInt64(bufPtr, ThreadId);
		bufPtr = Write7BitEncodedInt(bufPtr, (unsigned int) Functions.size());
		for(size_t i = 0; i < Functions.size(); ++i)
		{
			bufPtr = Write7BitEncodedInt(bufPtr, Functions[i]);
		}

		server.Write(buffer, bufPtr - buffer);
	}
}