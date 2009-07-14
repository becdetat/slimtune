#include "stdafx.h"
/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

static __int64 TimerFrequency = 0;
static HANDLE ProcessHandle;

void InitializeTimer()
{
	/*HKEY hKey;
	DWORD dwSpeed;

	if(RegOpenKeyEx(HKEY_LOCAL_MACHINE, L"HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0\\",
		0, KEY_QUERY_VALUE,&hKey) != ERROR_SUCCESS)
	{
		return;
	}

	DWORD dwLen = 4;
	if(RegQueryValueEx(hKey, L"~MHz", NULL, NULL, (LPBYTE) &dwSpeed, &dwLen) != ERROR_SUCCESS)
	{
		RegCloseKey(hKey);
		return;
	}

	RegCloseKey(hKey);
    TimerFrequency = dwSpeed;*/
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
	//counter = __rdtsc();
	//QueryThreadCycleTime(GetCurrentThread(), &counter);
	LARGE_INTEGER countStruct;
	QueryPerformanceCounter(&countStruct);
	counter = countStruct.QuadPart;
}
