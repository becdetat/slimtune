/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef TIMER_H
#define TIMER_H
#pragma once

void InitializeTimer();
void QueryTimerFreq(unsigned __int64& freq);
void QueryTimer(unsigned __int64& counter);

#endif
