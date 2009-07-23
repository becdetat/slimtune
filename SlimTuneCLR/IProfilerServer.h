/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
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
