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

#ifndef RINGBUFFER_H
#define RINGBUFFER_H
#pragma once

class RingBuffer
{
public:
	typedef boost::scoped_array<char> BufferPtr;

private:
	BufferPtr m_bufferRoot;
	const LONG m_size;
	//char* m_start;
	//this is typed to match Interlocked functions
	volatile LONG m_cursor;

public:
	RingBuffer(LONG size);
	~RingBuffer();

	const char* GetBufferRoot() const { return m_bufferRoot.get(); }

	char* Alloc(LONG size);
	void Free(LONG size);
};

#endif
