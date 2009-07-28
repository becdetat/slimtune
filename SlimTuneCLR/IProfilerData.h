/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef IPROFILERDATA_H
#define IPROFILERDATA_H
#pragma once

#include "Messages.h"

struct ModuleInfo
{
	const unsigned int Id;
	std::wstring Name;

	ModuleInfo(unsigned int id)
		: Id(id)
	{
	}
};

struct ClassInfo
{
	const unsigned int Id;
	std::wstring Name;

	ClassInfo(unsigned int id)
		: Id(id)
	{
	}
};

struct FunctionInfo
{
	const unsigned int Id;
	unsigned int ClassId;
	std::wstring Name;
	std::wstring Signature;
	int IsNative;

	FunctionInfo(unsigned int id)
		: Id(id)
	{
	}
};

struct ThreadInfo
{
	ThreadID ThreadId;
	DWORD SystemId;
	wchar_t Name[Messages::NameThread::MaxNameSize];
	bool Destroyed;
};

struct IProfilerData
{
	virtual const FunctionInfo* GetFunction(unsigned int id) const = 0;
};

#endif
