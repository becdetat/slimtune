/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
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
