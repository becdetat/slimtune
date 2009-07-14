/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef SOCKETSERVER_H
#define SOCKETSERVER_H
#pragma once

#include <boost/asio.hpp>

#include "IProfilerServer.h"
#include "Messages.h"
#include "RingBuffer.h"

typedef boost::shared_ptr<class TcpConnection> TcpConnectionPtr;

class SocketServer : public IProfilerServer
{
public:
	SocketServer(CProfiler& profiler, unsigned short port = 200);
	~SocketServer();

	CProfiler& Profiler() { return m_profiler; }

	void Start();
	void Run();
	void Stop();

	void WaitForConnection();
	bool Connected() { return m_connections.size() > 0; }
	void SetCallbacks(ServerCallback onConnect, ServerCallback onDisconnect);

	void Write(const void* data, size_t sizeBytes);

private:
	friend class TcpConnection;

	CProfiler& m_profiler;

	boost::asio::io_service m_io;
	boost::asio::ip::tcp::acceptor m_acceptor;

	std::vector<TcpConnectionPtr> m_connections;
	CRITICAL_SECTION m_writeLock;
	ServerCallback m_onConnect;
	ServerCallback m_onDisconnect;

	//Allocate a 4 MB send buffer
	//CONFIG: buffer sizes?
	static const size_t SendBufferSize = 4 * 1024 * 1024;
	static const size_t FlushSize = 32 * 1024;
	RingBuffer m_sendBuffer;
	//protected by a lock, so volatile is no longer necessary
	/*volatile*/ const char* m_writeStart;
	volatile LONG m_writeLength;

	void Accept(TcpConnectionPtr conn, const boost::system::error_code& error);
	void HandleWrite(TcpConnectionPtr source, const boost::system::error_code&, size_t);
	void Flush(int oldLength, const char* bufferPos);
};

#endif
