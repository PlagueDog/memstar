/*
Copyright (c) 2017,2020 Artem Boldariev <artem@boldariev.com>
Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files(the "Software"),
to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions :
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
*/

#include <windows.h>

#include <stdlib.h>
#include <stdio.h>

/*
A safer replacement for the obsolete IsBadReadPtr() and IsBadWritePtr() WinAPI functions
on top of VirtualQuery() which respects Windows guard pages. It does not use SEH
and is designed to be compatible with the above-mentioned functions.
The calls to the IsBadReadPtr() and IsBadWritePtr() can be replaced with the calls to
the IsBadMemPtr() as follows:
- IsBadReadPtr(...)  => IsBadMemPtr(FALSE, ...)
- IsBadWritePtr(...) => IsBadMemPtr(TRUE, ...)
*/
BOOL IsBadMemPtr(BOOL write, void* ptr, size_t size)
	{
		MEMORY_BASIC_INFORMATION mbi;
		BOOL ok;
		DWORD mask;
		BYTE* p = (BYTE*)ptr;
		BYTE* maxp = p + size;
		BYTE* regend = NULL;

		if (size == 0)
		{
			return FALSE;
		}

		if (p == NULL)
		{
			return TRUE;
		}

		if (write == FALSE)
		{
			mask = PAGE_READONLY | PAGE_READWRITE | PAGE_WRITECOPY | PAGE_EXECUTE_READ | PAGE_EXECUTE_READWRITE | PAGE_EXECUTE_WRITECOPY;
		}
		else
		{
			mask = PAGE_READWRITE | PAGE_WRITECOPY | PAGE_EXECUTE_READWRITE | PAGE_EXECUTE_WRITECOPY;
		}

		do
		{
			if (p == ptr || p == regend)
			{
				if (VirtualQuery((LPCVOID)p, &mbi, sizeof(mbi)) == 0)
				{
					return TRUE;
				}
				else
				{
					regend = ((BYTE*)mbi.BaseAddress + mbi.RegionSize);
				}
			}

			ok = (mbi.Protect & mask) != 0;

			if (mbi.Protect & (PAGE_GUARD | PAGE_NOACCESS))
			{
				ok = FALSE;
			}

			if (!ok)
			{
				return TRUE;
			}

			if (maxp <= regend) /* the whole address range is inside the current memory region */
			{
				return FALSE;
			}
			else if (maxp > regend) /* this region is a part of (or overlaps with) the address range we are checking */
			{
				p = regend; /* lets move to the next memory region */
			}
		} while (p < maxp);

		return FALSE;
	}