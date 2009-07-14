/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef UTILITIES_H
#define UTILITIES_H
#pragma once

struct EnterLock
{
	EnterLock(LPCRITICAL_SECTION lock)
		: m_lock(lock)
	{
		EnterCriticalSection(m_lock);
	}

	~EnterLock()
	{
		LeaveCriticalSection(m_lock);
	}

	LPCRITICAL_SECTION m_lock;
};

#endif
