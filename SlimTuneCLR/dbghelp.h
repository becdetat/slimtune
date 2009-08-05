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
#ifndef DBGHELP_H
#define DBGHELP_H
#pragma once

typedef BOOL (WINAPI *SymInitializeFunc)(HANDLE hProcess, PCTSTR UserSearchPath, BOOL fInvadeProcess);
typedef BOOL (WINAPI *SymFromAddrFunc)(HANDLE hProcess, DWORD64 Address, PDWORD64 Displacement, PSYMBOL_INFO Symbol);
typedef BOOL (WINAPI *StackWalk64Func)(
  DWORD MachineType,
  HANDLE hProcess,
  HANDLE hThread,
  LPSTACKFRAME64 StackFrame,
  PVOID ContextRecord,
  PREAD_PROCESS_MEMORY_ROUTINE64 ReadMemoryRoutine,
  PFUNCTION_TABLE_ACCESS_ROUTINE64 FunctionTableAccessRoutine,
  PGET_MODULE_BASE_ROUTINE64 GetModuleBaseRoutine,
  PTRANSLATE_ADDRESS_ROUTINE64 TranslateAddress
);
typedef DWORD64 (WINAPI *SymGetModuleBase64Func)(HANDLE hProcess, DWORD64 dwAddr);
typedef DWORD (WINAPI *SymSetOptionsFunc)(DWORD SymOptions);

extern SymInitializeFunc SymInitializePtr;
extern SymFromAddrFunc SymFromAddrPtr;
extern StackWalk64Func StackWalk64Ptr;
extern SymGetModuleBase64Func SymGetModuleBase64Ptr;
extern SymSetOptionsFunc SymSetOptionsPtr;

BOOL SymInitializeLocal();


#endif
