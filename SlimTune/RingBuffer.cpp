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
	LONG offset = InterlockedExchangeAdd(&m_cursor, (LONG) size);
	//this is inverted since the latter check will almost always pass
	if(offset + size > m_size && offset <= m_size)
	{
		//we pushed the cursor past the end of the buffer, move it back to the beginning
		InterlockedExchange(&m_cursor, size);
		offset = 0;
	}
	else if(offset >= m_size)
	{
		//allocation is past the end and we didn't do it
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
