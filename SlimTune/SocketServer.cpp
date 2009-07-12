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

#include <iostream>

#include "SocketServer.h"

using boost::asio::ip::tcp;

typedef boost::shared_ptr<class TcpConnection> TcpConnectionPtr;

class TcpConnection : public boost::enable_shared_from_this<TcpConnection>
{
private:
	SocketServer& m_server;
	boost::asio::ip::tcp::socket m_socket;
	CRITICAL_SECTION m_writeLock;

	static const int ReceiveBufferSize = 128;
	boost::array<int, ReceiveBufferSize> m_recvBuffer;

	void HandleWrite(const boost::system::error_code&, size_t);
	bool ContinueRead(const boost::system::error_code& err, size_t);

	TcpConnection(SocketServer& server, boost::asio::io_service& io);

public:
	~TcpConnection();

	static TcpConnectionPtr Create(SocketServer& server, boost::asio::io_service& io)
	{
		return TcpConnectionPtr(new TcpConnection(server, io));
	}

	boost::asio::ip::tcp::socket& GetSocket()
	{
		return m_socket;
	}

	bool IsOpen() const { return m_socket.is_open(); }
	void Close() { m_socket.close(); }

	void Start();
	void Write(const void* data, size_t size);
};

TcpConnection::TcpConnection(SocketServer& server, boost::asio::io_service& io)
: m_server(server),
m_socket(io)
{
	InitializeCriticalSection(&m_writeLock);
	//the use of 100 here is entirely arbitrary
	SetCriticalSectionSpinCount(&m_writeLock, 100);
}

TcpConnection::~TcpConnection()
{
	DeleteCriticalSection(&m_writeLock);
}

void TcpConnection::Start()
{
	std::string message = "ClearProf v0.1\r\n";
	/*boost::asio::async_write(m_socket, boost::asio::buffer(message),
		boost::bind(&TcpConnection::HandleWrite, shared_from_this(),
		boost::asio::placeholders::error,
		boost::asio::placeholders::bytes_transferred));*/
	//Send(message);

	/*EnterLock writeLock(&m_writeLock);

	boost::asio::async_read(m_socket, boost::asio::buffer(m_recvBuffer, ReceiveBufferSize / 2;),
		boost::bind(&TcpConnection::ContinueRead, shared_from_this(),
		boost::asio::placeholders::error,
		boost::asio::placeholders::bytes_transferred),
		boost::function<void(const boost::system::error_code& err, size_t)>()
		);*/
}

void TcpConnection::Write(const void* data, size_t size)
{
	EnterLock writeLock(&m_writeLock);

	boost::asio::async_write(m_socket, boost::asio::buffer(data, size),
		boost::bind(&SocketServer::HandleWrite, &m_server,
		shared_from_this(),
		boost::asio::placeholders::error,
		boost::asio::placeholders::bytes_transferred));
}

bool TcpConnection::ContinueRead(const boost::system::error_code&, size_t bytesRead)
{
	/*if(bytesRead < 1)
		return false; //can this even happen?

	EnterLock writeLock(&m_writeLock);

	char* bufPtr = m_recvBuffer;
	while(bytesRead > 0)
	{
		--bytesRead;
		switch(*bufPtr++)
		{
		case CR_GetFunctionMapping:
			Requests::GetFunctionMapping* gfmReq = (Requests::GetFunctionMapping*) bufPtr;

		}
	}*/
	return true;
}

void TcpConnection::HandleWrite(const boost::system::error_code& err, size_t)
{

}

SocketServer::SocketServer(CProfiler& profiler, unsigned short port)
: m_profiler(profiler),
m_io(),
m_acceptor(m_io, tcp::endpoint(tcp::v4(), port)),
m_onConnect(NULL),
m_onDisconnect(NULL),
m_sendBuffer(SendBufferSize),
m_writeStart(m_sendBuffer.GetBufferRoot()),
m_writeLength(0)
{
	InitializeCriticalSection(&m_writeLock);
}

SocketServer::~SocketServer()
{
	DeleteCriticalSection(&m_writeLock);
}

void SocketServer::Start()
{
	TcpConnectionPtr conn = TcpConnection::Create(*this, m_acceptor.io_service());
	m_acceptor.async_accept(conn->GetSocket(),
		boost::bind(&SocketServer::Accept, this, conn, boost::asio::placeholders::error));
}

void SocketServer::Run()
{
	m_io.run();
}

void SocketServer::Stop()
{
	int oldLength = InterlockedExchange(&m_writeLength, 0);
	char* bufferPos = m_sendBuffer.Alloc(0);
	Flush(oldLength, bufferPos);

	m_io.stop();

	for(size_t i = 0; i < m_connections.size(); ++i)
	{
		if(m_connections[i]->IsOpen())
			m_connections[i]->Close();
	}
}

void SocketServer::Accept(TcpConnectionPtr conn, const boost::system::error_code& error)
{
	if(!error)
	{
		EnterLock localLock(&m_writeLock);

		conn->Start();
		m_connections.push_back(conn);

		if(m_connections.size() == 1 && m_onConnect)
			m_onConnect();

		Start();
	}
}

void SocketServer::HandleWrite(TcpConnectionPtr source, const boost::system::error_code& error, size_t)
{
	if(error)
	{
		std::string errorMessage = error.message();

		//we can remove this connection from our list
		EnterLock localLock(&m_writeLock);

		std::vector<TcpConnectionPtr>::iterator deadConn = std::find(m_connections.begin(), m_connections.end(), source);
		//this callback will trigger multiple times when multiple writes are queued up,
		//so we can't be sure the connection exists to erase
		if(deadConn != m_connections.end())
		{
			m_connections.erase(deadConn);

			if(m_connections.size() == 0 && m_onDisconnect)
			{
				m_onDisconnect();
			}
		}
	}
}

void SocketServer::WaitForConnection()
{
	m_io.run_one();
}

void SocketServer::SetCallbacks(ServerCallback onConnect, ServerCallback onDisconnect)
{
	m_onConnect = onConnect;
	m_onDisconnect = onDisconnect;
}

void SocketServer::Write(const void* data, size_t sizeBytes)
{
	assert(sizeBytes < FlushSize); //this function will break otherwise

	char* bufferPos = m_sendBuffer.Alloc((LONG) sizeBytes);
	if(bufferPos == NULL)
		return;

	memcpy(bufferPos, data, sizeBytes);

	//Decide whether or not to flush
	bool flush = false;

#ifdef LOCKLESS
	LONG oldLength = InterlockedExchangeAdd(&m_writeLength, (LONG) sizeBytes);
	assert(oldLength < 2 * FlushSize);

	if(oldLength + sizeBytes >= FlushSize && oldLength < FlushSize)
	{
		//we're the ones who pushed the write length over the limit
		flush = true;
	}
	else if(bufferPos == m_sendBuffer.GetBufferRoot())
	{
		//this means the ring buffer has rolled over and it's our fault
		flush = true;
	}
#else
	LONG oldLength = m_writeLength;
	m_writeLength += sizeBytes;

	if(m_writeLength > FlushSize || bufferPos == m_sendBuffer.GetBufferRoot())
		flush = true;
#endif

	if(flush)
	{
		Flush(oldLength, bufferPos);
	}
}

void SocketServer::Flush(int oldLength, const char* bufferPos)
{
	if(oldLength == 0)
		return;

	EnterLock localLock(&m_writeLock);

#ifdef LOCKLESS
	//we do NOT flush the current write
	InterlockedExchangeAdd(&m_writeLength, -oldLength);
	//Since the introduction of the lock here, we don't need to do this interlocked
	//const char* writeStart = (char*) InterlockedExchange((LONG*) &m_writeStart, (LONG) bufferPos);
#else
	m_writeLength -= oldLength;
#endif

	for(size_t i = 0; i < m_connections.size(); ++i)
	{
		m_connections[i]->Write(m_writeStart, oldLength);
	}
	m_writeStart = bufferPos;
}
