/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef THREADSTATE_H
#define THREADSTATE_H
#pragma once

struct ThreadState
{
	typedef std::vector<FunctionID> CallStackType;

	ThreadID Id;
	CallStackType CallStack;
	bool InNativeCode;
};

#endif
