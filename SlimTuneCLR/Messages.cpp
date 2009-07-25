#include "stdafx.h"
/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#include "Messages.h"
#include "IProfilerServer.h"

namespace Messages
{
	typedef char MessageIdStorageType;

	void MapFunction::Write(IProfilerServer& server, size_t nameCount, size_t signatureCount)
	{
		char* const buffer = (char*) _alloca(32 + sizeof(wchar_t) * (nameCount + signatureCount));
		char* bufPtr = buffer;

		*bufPtr++ = MID_MapFunction;
		bufPtr = Write7BitEncodedInt(bufPtr, this->FunctionId);
		bufPtr = Write7BitEncodedInt(bufPtr, this->IsNative);
		bufPtr = WriteString(bufPtr, this->SymbolName, nameCount);
		bufPtr = WriteString(bufPtr, this->Signature, signatureCount);

		server.Write(buffer, bufPtr - buffer);
	}

	void FunctionELT::Write(IProfilerServer& server, MessageId id)
	{
		//comfortably big enough
		char buffer[32];
		char* bufPtr = buffer;

		*bufPtr++ = (char) id;
		bufPtr = Write7BitEncodedInt(bufPtr, ThreadId);
		bufPtr = Write7BitEncodedInt(bufPtr, FunctionId);
		bufPtr = Write7BitEncodedInt64(bufPtr, TimeStamp);

		server.Write(buffer, bufPtr - buffer);
	}

	void CreateThread::Write(IProfilerServer& server, MessageId id)
	{
		char buffer[16];
		char* bufPtr = buffer;

		*bufPtr++ = (char) id;
		bufPtr = Write7BitEncodedInt(bufPtr, ThreadId);

		server.Write(buffer, bufPtr - buffer);
	}

	void NameThread::Write(IProfilerServer& server, size_t nameCount)
	{
		char* const buffer = (char*) _alloca(16 + sizeof(wchar_t) * nameCount);
		char* bufPtr = buffer;

		*bufPtr++ = MID_NameThread;
		bufPtr = Write7BitEncodedInt(bufPtr, ThreadId);
		bufPtr = WriteString(bufPtr, this->Name, nameCount);

		server.Write(buffer, bufPtr - buffer);
	}

	void Sample::Write(IProfilerServer& server)
	{
		char* const buffer = (char*) _alloca(24 + 10 * Functions.size());
		char* bufPtr = buffer;

		*bufPtr++ = MID_Sample;
		bufPtr = Write7BitEncodedInt(bufPtr, ThreadId);
		bufPtr = Write7BitEncodedInt(bufPtr, (unsigned int) Functions.size());
		for(size_t i = 0; i < Functions.size(); ++i)
		{
			bufPtr = Write7BitEncodedInt(bufPtr, Functions[i]);
		}

		server.Write(buffer, bufPtr - buffer);
	}
}