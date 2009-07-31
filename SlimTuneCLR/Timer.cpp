/*
* Copyright (c) 2007-2009 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
#include "stdafx.h"

static __int64 TimerFrequency = 0;
static bool CycleTiming = false;

void InitializeTimer(bool allowCycleTiming)
{
	//look up the current OS to figure out if cycle timing is available
	OSVERSIONINFO version = {0};
	version.dwOSVersionInfoSize = sizeof(version);
	GetVersionEx(&version);

	if(allowCycleTiming && version.dwMajorVersion >= 6)
	{
		CycleTiming = true;
		TimerFrequency = 1;
		return;
	}

	LARGE_INTEGER freq;
	QueryPerformanceFrequency(&freq);
	TimerFrequency = freq.QuadPart;
}

void QueryTimerFreq(unsigned __int64& freq)
{
	freq = TimerFrequency;
}

void QueryTimer(unsigned __int64& counter)
{
	if(CycleTiming)
	{
		QueryThreadCycleTime(GetCurrentThread(), &counter);
	}
	else
	{
		LARGE_INTEGER countStruct;
		QueryPerformanceCounter(&countStruct);
		counter = countStruct.QuadPart;
	}
}
