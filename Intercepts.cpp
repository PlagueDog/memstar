#include "Fear.h"
#include "Console.h"
#include "Windows.h"
#include "Patch.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <sstream>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <iterator>
#include <filesystem>
#include <dirent.h>
#include "VersionSnoop.h"
#include <bitset>
MultiPointer(ptrConsoleBuffer, 0, 0, 0, 0x00722FA4);
MultiPointer(ptrConsoleEval, 0, 0, 0, 0x005E6DF0);
MultiPointer(ptrConsoleEval2, 0, 0, 0x005E387C, 0x005E7120);
MultiPointer(_sprintf, 0, 0, 0, 0x006CD8B4);


namespace Intercepts {
//GuiLoad
	MultiPointer(ptrGuiOpened, 0, 0, 0x005C52B2, 0x005C8B2A);
	MultiPointer(ptrGuiOpenedRetn, 0, 0, 0x005C52B8, 0x005C8B30);
	static const char* s_OnOpen = "%s::onOpen";
	char* currentGui;
	CodePatch guiopened = { ptrGuiOpened, "", "\xE9GOPD", 5, false };
	NAKED void guiOpened() {
		__asm {
			push eax
			mov currentGui, eax
			push s_OnOpen
			jmp[ptrGuiOpenedRetn]
		}
	}

	BuiltInFunction("Nova::getGui", _getGui)
	{
		if (strlen(currentGui))
		{
			return currentGui;
		}
		return "false";
	}

	MultiPointer(ptrGuiOpen, 0, 0, 0x005C52CD, 0x005C8B45);
	MultiPointer(ptrGuiOpenRetn, 0, 0, 0x005C52D9, 0x005C8B51);
	static const char* s_NovaOnOpen = "Nova::guiOpen(Nova::getGui());";
	CodePatch guiopen = { ptrGuiOpen, "", "\xE9GUIO", 5, false };
	NAKED void guiOpen() {
		__asm {
			lea edx, [esp + 0x320 + 0x110]
			call[ptrConsoleEval2]
			push eax
			mov eax, [s_NovaOnOpen]
			push eax
			call Console::eval
			add esp, 0x8
			jmp[ptrGuiOpenRetn]
		}
	}

//GuiClose
	MultiPointer(ptrGuiClosed, 0, 0, 0x005C522C, 0x005C8AA4);
	MultiPointer(ptrGuiClosedRetn, 0, 0, 0x005C5232, 0x005C8AAA);
	static const char* s_OnClose = "%s::onClose";
	char* lastGui;
	CodePatch guiclosed = { ptrGuiClosed, "", "\xE9GCSD", 5, false };
	NAKED void guiClosed() {
		__asm {
			push ecx
			mov lastGui, ecx
			push s_OnClose
			jmp [ptrGuiClosedRetn]
		}
	}

	BuiltInFunction("Nova::getLastGUI", _getLastGUI)
	{
		if (strlen(lastGui))
		{
			return lastGui;
		}
		return "false";
	}

	MultiPointer(ptrGuiClose, 0, 0, 0x005C5247, 0x005C8ABF);
	MultiPointer(ptrGuiCloseRetn, 0, 0, 0x005C5253, 0x005C8ACB);
	static const char* s_NovaOnClose = "Nova::guiClose(Nova::getLastGUI());";
	CodePatch guiclose = { ptrGuiClose, "", "\xE9GCLS", 5, false };
	NAKED void guiClose() {
		__asm {
			lea edx, [esp + 0x320 + 0x110]
			call [ptrConsoleEval2]
			push eax
			mov eax, [s_NovaOnClose]
			push eax
			call Console::eval
			add esp, 0x8
			jmp [ptrGuiCloseRetn]
		}
	}

	//Game cursor event function calls
	MultiPointer(ptrOnSim3DMouseEvent, 0, 0, 0, 0x005A1846);
	MultiPointer(ptrSim3DMouseProcessEvent, 0, 0, 0, 0x005A188C);
	MultiPointer(ptrSim3DMouseEvent_RETN, 0, 0, 0, 0x005A184B);
	static const char* OnSim3DMouseEvent_func = "Nova::onInputEvent();";
	CodePatch onsim3Dmouseevent = { ptrOnSim3DMouseEvent, "", "\xE9VMSE", 5, false };
	NAKED void OnSim3DMouseEvent() {
		__asm {
			call [ptrSim3DMouseProcessEvent]
			push eax
			mov eax, [OnSim3DMouseEvent_func]
			push eax
			call Console::eval
			add esp, 0x8
			jmp [ptrSim3DMouseEvent_RETN]
		}
	}

	struct Init 
	{
		Init() 
		{

		//////////////////////////////////////////////////
		// Gui onOpen & onClose patches
		/////////////////////////////////////////////////
			//Nova::getGui();
			guiopened.DoctorRelative((u32)guiOpened, 1).Apply(true);

			//Nova::guiOpen();
			guiopen.DoctorRelative((u32)guiOpen, 1).Apply(true);

			//Nova::getLastGui();
			guiclosed.DoctorRelative((u32)guiClosed, 1).Apply(true);

			//Nova::guiClose();
			guiclose.DoctorRelative((u32)guiClose, 1).Apply(true);

			//Input Event Calls
			//onsim3Dmouseevent.DoctorRelative((u32)OnSim3DMouseEvent, 1).Apply(true);

		}
	} init;
}