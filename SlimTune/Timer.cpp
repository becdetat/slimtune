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
