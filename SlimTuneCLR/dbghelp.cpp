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
#include "dbghelp.h"

static HMODULE DbgHelpHandle;

SymInitializeFunc SymInitializePtr;
SymFromAddrFunc SymFromAddrPtr;
StackWalk64Func StackWalk64Ptr;
SymGetModuleBase64Func SymGetModuleBase64Ptr;
SymFunctionTableAccess64Func SymFunctionTableAccess64Ptr;
SymSetOptionsFunc SymSetOptionsPtr;

BOOL SymInitializeLocal()
{
#define EXT_MAX_PATH 2048
#define MAX_DRIVE 8

	//Get the directory for the CLR plugin, which also contains dbghelp
	wchar_t path[EXT_MAX_PATH];
	DWORD result = GetModuleFileName(GetModuleHandle(L"SlimTuneCLR"), path, EXT_MAX_PATH);
	if(result == 0)
		return FALSE;

	wchar_t drive[MAX_DRIVE];
	wchar_t dir[EXT_MAX_PATH];
	_wsplitpath_s(path, drive, MAX_DRIVE, dir, EXT_MAX_PATH, NULL, 0, NULL, 0);
	//put together the path to dbghelp
	_wmakepath_s(path, EXT_MAX_PATH, drive, dir, L"dbghelp.dll", NULL);
	
	//verify that the dll exists
	DWORD fileAttr = GetFileAttributes(path);
	if(fileAttr == 0xffffffff)
		return FALSE;

	//load it up and get the functions
	DbgHelpHandle = LoadLibrary(path);
	if(DbgHelpHandle == NULL)
		return FALSE;

	//get function pointers
	SymInitializePtr = (SymInitializeFunc) GetProcAddress(DbgHelpHandle, "SymInitializeW");
	SymFromAddrPtr = (SymFromAddrFunc) GetProcAddress(DbgHelpHandle, "SymFromAddrW");
	StackWalk64Ptr = (StackWalk64Func) GetProcAddress(DbgHelpHandle, "StackWalk64");
	SymGetModuleBase64Ptr = (SymGetModuleBase64Func) GetProcAddress(DbgHelpHandle, "SymGetModuleBase64");
	SymFunctionTableAccess64Ptr = (SymFunctionTableAccess64Func) GetProcAddress(DbgHelpHandle, "SymFunctionTableAccess64");
	SymSetOptionsPtr = (SymSetOptionsFunc) GetProcAddress(DbgHelpHandle, "SymSetOptions");

	if(SymInitializePtr == NULL ||
		SymFromAddrPtr == NULL ||
		StackWalk64Ptr == NULL ||
		SymGetModuleBase64Ptr == NULL ||
		SymFunctionTableAccess64Ptr == NULL ||
		SymSetOptionsPtr == NULL)
	{
		FreeLibrary(DbgHelpHandle);
		return FALSE;
	}

	return TRUE;
}
