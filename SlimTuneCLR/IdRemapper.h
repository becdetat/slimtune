/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/

#ifndef IDREMAPPER_H
#define IDREMAPPER_H
#pragma once

class IdRemapper
{
public:
	IdRemapper() : m_nextIndex(1)
	{ }

	unsigned int Alloc() { return m_nextIndex++; }
	
	unsigned int& operator[] (UINT_PTR value) { return m_remap[value]; }

private:
	std::tr1::unordered_map<UINT_PTR, unsigned int> m_remap;
	unsigned int m_nextIndex;
};

#endif
