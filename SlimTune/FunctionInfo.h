/*****************************************************************************
 * DotNetProfiler
 * 
 * Copyright (c) 2006 Scott Hackett
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from the
 * use of this software. Permission to use, copy, modify, distribute and sell
 * this software for any purpose is hereby granted without fee, provided that
 * the above copyright notice appear in all copies and that both that
 * copyright notice and this permission notice appear in supporting
 * documentation.
 * 
 * Scott Hackett (code@scotthackett.com)
 *****************************************************************************/

#pragma once

#include "CorProfilerCallbackImpl.h"

class CFunctionInfo
{
public:
	CFunctionInfo(FunctionID functionID, const char* name);
	virtual ~CFunctionInfo();

	char* GetName();
	FunctionID GetFunctionID();
	long GetCallCount();
	void IncrementCallCount();

private:
	FunctionID m_functionID;
	char* m_name;
	long m_callCount;

};
