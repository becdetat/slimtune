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

#include "StdAfx.h"
#include "FunctionInfo.h"

CFunctionInfo::CFunctionInfo(FunctionID functionID, const char* name)
{
	m_functionID = functionID;
	m_name = strdup(name);
	m_callCount = 0;
}

CFunctionInfo::~CFunctionInfo()
{
	// free the allocated name
	free(m_name);
}

char* CFunctionInfo::GetName()
{
	return m_name;
}

FunctionID CFunctionInfo::GetFunctionID()
{
	return m_functionID;
}

long CFunctionInfo::GetCallCount()
{
	return m_callCount;
}

void CFunctionInfo::IncrementCallCount()
{
	m_callCount++;
}

