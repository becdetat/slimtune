/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#include "stdafx.h"
#include "IProfilerServer.h"
#include "SocketServer.h"
#include "IProfilerData.h"

IProfilerServer* IProfilerServer::CreateSocketServer(IProfilerData& profiler, unsigned short port)
{
	return new SocketServer(profiler, port);
}

char* Write7BitEncodedInt(char* buffer, unsigned int value)
{
	while(value >= 128)
	{
		*buffer++ = value | 0x80;
		value >>= 7;
	}
	*buffer++ = value;

	return buffer;
}

char* Write7BitEncodedInt64(char* buffer, unsigned __int64 value)
{
	while(value >= 128)
	{
		*buffer++ = (char) (value | 0x80);
		value >>= 7;
	}
	*buffer++ = (char) value;

	return buffer;
}

char* WriteString(char* buffer, const wchar_t* string, size_t count)
{
	size_t size = count * sizeof(wchar_t);
	buffer = Write7BitEncodedInt(buffer, (unsigned int) size);
	memcpy(buffer, string, size);
	return buffer + size;
}

char* Read7BitEncodedInt(char* buffer, unsigned int& value)
{
	int byteval;
	int shift = 0;

	value = 0;
	while(((byteval = *buffer++) & 0x80) != 0)
	{
		value |= ((byteval & 0x7F) << shift);
		shift += 7;
	}

	return buffer;
}
