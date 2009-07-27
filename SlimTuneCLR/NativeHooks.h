/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef NATIVEHOOKS_H
#define NATIVEHOOKS_H

class ClrProfiler;

extern ClrProfiler* g_ProfilerCallback;

void FunctionEnterNaked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
void FunctionLeaveNaked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
void FunctionTailcallNaked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func);

#endif
