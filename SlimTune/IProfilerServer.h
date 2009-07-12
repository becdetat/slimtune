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

#ifndef IPROFILERSERVER_H
#define IPROFILERSERVER_H
#pragma once

class CProfiler;
typedef boost::function<void()> ServerCallback;

//TODO: Separate protocol from communication layer
class IProfilerServer
{
public:
	static IProfilerServer* CreateSocketServer(CProfiler& profiler, unsigned short port);

	virtual void Start() = 0;
	virtual void Run() = 0;
	virtual void Stop() = 0;

	virtual void WaitForConnection() = 0;
	virtual bool Connected() = 0;
	virtual void SetCallbacks(ServerCallback onConnect, ServerCallback onDisconnect) = 0;

	virtual void Write(const void* data, size_t sizeBytes) = 0;
};

char* Write7BitEncodedInt(char* buffer, unsigned int value);
char* Write7BitEncodedInt64(char* buffer, unsigned __int64 value);
char* WriteString(char* buffer, const wchar_t* string, size_t count);

#endif
