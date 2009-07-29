#include "stdafx.h"
/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#include "RingBuffer.h"
#include <cassert>

RingBuffer::RingBuffer(LONG size)
: m_bufferRoot(new char[size]),
m_size(size),
m_cursor(0)
{
}

RingBuffer::~RingBuffer()
{
}

char* RingBuffer::Alloc(LONG size)
{
	assert(size >= 0);
	assert(size < m_size);

#ifdef LOCKLESS
	//Move the buffer pointer up and get the old value, which we need
	LONG offset = InterlockedExchangeAdd(&m_cursor, (LONG) size);
	//this is inverted since the latter check will almost always pass
	if(offset + size > m_size && offset <= m_size)
	{
		//we pushed the cursor past the end of the buffer, so we move it back to the beginning
		//(known because old offset is inside, new offset is outside)
		InterlockedExchange(&m_cursor, size);
		offset = 0;
	}
	else if(offset >= m_size)
	{
		//allocation is past the end and we didn't do it (both old and new offset are outside)
		//we will spin trying to allocate until it's corrected
		//I may regret this, it kind of seems like a recipe for priority inversion
		//At least it's a race condition, so it should be very rare
		do
		{
			Sleep(1);
			offset = InterlockedExchangeAdd(&m_cursor, (LONG) size);
		} while(offset > m_size);
	}
#else
	//Simplified implementation for testing purposes only!
	LONG offset = m_cursor;
	m_cursor += size;
	if(m_cursor > m_size)
	{
		m_cursor = size;
		offset = 0;
	}
#endif

	return m_bufferRoot.get() + offset;
}

void RingBuffer::Free(LONG size)
{
	//we no longer track the start of the buffer, so this function has nothing to do.
	//this is a (premature?) performance optimization.

	/*ptrdiff_t startPos = m_start - m_bufferRoot.get();
	size_t remainingSize = m_size - startPos;
	if(remainingSize > size)
	{
		m_start += size;
		return;
	}

	size -= remainingSize;
	m_start = m_bufferRoot.get() + size;*/
}
