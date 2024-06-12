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

char* despace(char* input)
{
	int i, j;
	char* output = input;
	for (i = 0, j = 0; i < strlen(input); i++, j++)
	{
		if (input[i] != ' ')
			output[j] = input[i];
		else
			j--;
	}
	output[j] = 0;
	return output;
}

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

	//Simgui Responder event calls
	//These cursor events only fire on the games emulated cursor
	MultiPointer(ptrOnGameCursorLeftDown, 0, 0, 0, 0x005CB814);
	MultiPointer(ptrOnGameCursorRightDown, 0, 0, 0, 0x005CB844);
	MultiPointer(ptrOnGameCursorRightUp, 0, 0, 0, 0x005CB8A4);
	MultiPointer(ptrOnGameCursorRepeat, 0, 0, 0, 0x005CB814);
	MultiPointer(ptrOnGameCursorMove, 0, 0, 0, 0x005CB844);
	MultiPointer(ptrOnKeyDown, 0, 0, 0, 0x005CB8D4);
	MultiPointer(ptrOnKeyUp, 0, 0, 0, 0x005CB8C4);

	//MultiPointer(ptrSim3DMouseProcessEvent, 0, 0, 0, 0x005A188C);
	//MultiPointer(ptrSim3DMouseEvent_RETN, 0, 0, 0, 0x005A184B);
	//static const char* OnSim3DMouseEvent_func = "Nova::onInputEvent();";
	//CodePatch onsim3Dmouseevent = { ptrOnSim3DMouseEvent, "", "\xE9VMSE", 5, false };
	//NAKED void OnSim3DMouseEvent() {
	//	__asm {
	//		call [ptrSim3DMouseProcessEvent]
	//		push eax
	//		mov eax, [OnSim3DMouseEvent_func]
	//		push eax
	//		call Console::eval
	//		add esp, 0x8
	//		jmp [ptrSim3DMouseEvent_RETN]
	//	}
	//}

	//ListDevices
	MultiPointer(ptrListDevices, 0, 0, 0x005A8EE2, 0x005AC6FE);
	MultiPointer(ptrListDevicesRetn, 0, 0, 0x005A8EE8, 0x005AC704);
	char* renderDevice;
	static const char* _s = "%s";
	static const char* NovaGetRenderDevice = "Nova::getRenderDevices();";
	CodePatch listdevices = { ptrListDevices, "", "\xE9LSTD", 5, false };
	NAKED void ListDevices() {
		__asm {
			mov renderDevice, eax
			push eax
			mov eax, [NovaGetRenderDevice]
			push eax
			call Console::eval
			add esp, 0x8
			lea eax, [ebp - 0x5E4]
			push eax
			push _s
			jmp[ptrListDevicesRetn]
		}
	}

	MultiPointer(ptrStatusPartDamageCalc, 0, 0, 0, 0x0046634A);
	MultiPointer(ptrStatusPartDamageCalcResume, 0, 0, 0, 0x00466350);
	MultiPointer(ptrStatusPart, 0, 0, 0, 0x006E8AAC);
	CodePatch statuspartcalc = { ptrStatusPartDamageCalc, "", "\xE9SPDC", 5, false };
	char* partName;
	float partHealth;
	float partMaxHealth;
	static const char* NovaGetDamageStatus = "Nova::getDamageStatus();";
	NAKED void statusPartCalc() {
		__asm {
			push eax
			mov partName, eax // Part name
			pop eax
			fld dword ptr [ebx + 0x1C]
			push eax
			mov eax, dword ptr [ebx + 0x1C] //Current hit points (float)
			mov partHealth, eax
			pop eax
			fdiv dword ptr[ebx + 0x18]
			push eax
			mov eax, dword ptr [ebx + 0x18] //Max hit points (float)
			mov partMaxHealth, eax
			pop eax

			push eax
			mov eax, [NovaGetDamageStatus]
			push eax
			call Console::eval
			add esp, 0x8
			mov eax, [esp + 0x1C + 0x1C]

			jmp[ptrStatusPartDamageCalcResume]
		}
	}

	BuiltInFunction("Nova::getDamageStatus", _novaecho)
	{
		//Console::echo("%s (%3.0f / %3.0f)", partName, partHealth, partMaxHealth);
		if (strlen(partName))
		{
			char partNameAssign[127];
			char partHealthAssign[127];
			float partCurrentHealth = (partHealth / partMaxHealth) * 100;
			//char* arrayIndex;
			//
			//strcat("Stat[", tostring(partArrayIndex));
			//strcat(partNameFormatted, "]");

			//Assign part name to array
			strcpy(partNameAssign, "$damStat[$damStatArr++, name] = '");
			strcat(partNameAssign, partName);
			strcat(partNameAssign, "';");
			Console::eval(partNameAssign);
			free(partNameAssign);

			//Assign part health to array
			strcpy(partHealthAssign, "$damStat[$damStatArr, health] = '");
			strcat(partHealthAssign, tostring(partCurrentHealth));
			strcat(partHealthAssign, "';");
			Console::eval(partHealthAssign);
			//Console::echo(partNameFormatted);
			free(partHealthAssign);
		}
		//free(arrayIndex);
		//Console::echo(partName);
		//Console::echo(tostring(partHealth));
		//Console::echo(tostring(partMaxHealth));
		return 0;
	}

	BuiltInFunction("Nova::getRenderDevices", _getRenderDevices)
	{
		if (strlen(renderDevice))
		{
			std::string device_full = renderDevice;
			std::size_t trim_pos = device_full.find(":");
			std::string device = device_full.erase(trim_pos, device_full.length());//Trim off the resolution mode list
			if (device.find("Software") != -1) { Console::setVariable("Nova::SoftwareDevice", device.c_str());}
			if (device.find("Glide") != -1) { Console::setVariable("Nova::GlideDevice", device.c_str());}
			if (device.find("OpenGL") != -1){Console::setVariable("Nova::OpenGLDevice", device.c_str());}
		}
		return "false";
	}

	struct Init 
	{
		Init() 
		{
			if (std::filesystem::exists("Nova.vol"))
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

				//ListDevices();
				listdevices.DoctorRelative((u32)ListDevices, 1).Apply(true);

				//dumpDamage Patches
				statuspartcalc.DoctorRelative((u32)statusPartCalc, 1).Apply(true);

				//Input Event Calls
				//onsim3Dmouseevent.DoctorRelative((u32)OnSim3DMouseEvent, 1).Apply(true);
			}

		}
	} init;
}