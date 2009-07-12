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
#include "IProfilerServer.h"
#include "SocketServer.h"

IProfilerServer* IProfilerServer::CreateSocketServer(CProfiler& profiler, unsigned short port)
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

