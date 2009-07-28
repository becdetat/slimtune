/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#include "stdafx.h"
#include "Config.h"

ProfilerConfig::ProfilerConfig()
{
	Mode = PM_Sampling;
	ListenPort = 3000;
	WaitForConnection = false;

	TrackMemory = true;
	AllowInlining = false;

	InstrumentSmallFunctions = false;

	SampleInterval = 3;
	SampleUnmanaged = false;
}

template<typename T>
T Parse(const wchar_t* str, T defaultValue)
{
	try
	{
		return boost::lexical_cast<T>(str);
	}
	catch(const std::bad_cast&)
	{
		return defaultValue;
	}
}

template<typename T>
void ParseRef(const wchar_t* str, T& value)
{
	value = Parse<T>(str, value);
}

void ParseVar(ProfilerConfig& config, wchar_t* var)
{
	wchar_t* equals = wcsstr(var, L"=");
	if(equals == NULL)
		return;

	*equals = 0;
	wchar_t* valueStr = equals + 1;
	//hard coded parse for recognized variables
	if(_wcsicmp(var, L"mode") == 0)
		config.Mode = (ProfilerMode) Parse<int>(valueStr, PM_Disabled);
	else if(_wcsicmp(var, L"port") == 0)
		ParseRef(valueStr, config.ListenPort);
	else if(_wcsicmp(var, L"wait") == 0)
		ParseRef(valueStr, config.WaitForConnection);
}

bool ProfilerConfig::LoadEnv()
{
	const int kBufferSize = 2048;
	wchar_t buffer[kBufferSize];
	size_t length = 0;
	length = GetEnvironmentVariable(L"SLIMTUNE_CONFIG", buffer, kBufferSize);
	if(length == 0 || length > kBufferSize)
		return false;

	wchar_t* beginVar = buffer;
	wchar_t* endVar = NULL;
	while(*beginVar)
	{
		endVar = wcsstr(beginVar, L";");
		if(endVar == NULL)
			break;
		
		*endVar = 0;
		ParseVar(*this, beginVar);
		beginVar = endVar + 1;
	}

	return true;
}
