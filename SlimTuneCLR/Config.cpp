/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#include "stdafx.h"
#include "Config.h"

bool ProfilerConfig::Load()
{
	const int kBufferSize = 1024;
	wchar_t buffer[kBufferSize];
	if(FAILED(GetEnvironmentVariable(L"SLIMTUNE_CONFIG", buffer, kBufferSize)))
		return false;

	return true;
}