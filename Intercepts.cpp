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
MultiPointer(_strlen, 0, 0, 0x006B8DE4, 0x006C8E5C);
MultiPointer(fnEcho, 0, 0, 0x005e3178, 0x005E6A1C);

MultiPointer(ptrClientVehicleSpawn, 0, 0, 0, 0x0046D69B);
MultiPointer(ptrVehicleAnimationTime, 0, 0, 0, 0x004A40C4);
MultiPointer(ptrVehicleSpeedCoeff, 0, 0, 0, 0x004A18A4);
MultiPointer(ptrWorldBoundaryMin, 0, 0, 0, 0x004A0B9C);
MultiPointer(ptrWorldBoundaryMax, 0, 0, 0, 0x004A0BA0);

MultiPointer(ptrMouseLeftClick, 0, 0, 0, 0x005C6738);
MultiPointer(ptrMouseRightClick, 0, 0, 0, 0x005C6898);

using namespace std;
using namespace Console;

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
	u32 dummy, dummy2, dummy3, dummy4, dummy5;
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

	char* partName;
	float partDamage;
	float partDamageMax;
	void assignDamageVars()
	{
		char partNameAssign[127];
		//Assign part name to array
		strcpy(partNameAssign, "$Nova::damageStatus[$damStatArr++, name] = '");
		strcat(partNameAssign, partName);
		strcat(partNameAssign, "';");
		Console::eval(partNameAssign);
		free(partNameAssign);

		//Calculate the damge percent
		int damageStatPercent = partDamage / partDamageMax * 100;

		//Assign damge percentile to array
		strcpy(partNameAssign, "$Nova::damageStatus[$damStatArr, damage] = '");
		strcat(partNameAssign, tostring(damageStatPercent));
		strcat(partNameAssign, "';");
		Console::eval(partNameAssign);
		free(partNameAssign);

		//Assign damge max percentile to array
		//strcpy(partNameAssign, "$Nova::damageStatus[$damStatArr, damageMax] = '");
		//strcat(partNameAssign, tostring(partDamageMaxPercentile));
		//strcat(partNameAssign, "';");
		//Console::eval(partNameAssign);
		//free(partNameAssign);
	}
	//NAKED void statusPartCalc2() {
	//	__asm {
	//		//push eax
	//		//mov partName, eax
	//		//pop eax
	//
	//		fld     dword ptr[ebx + 0x1C]
	//		fdiv    dword ptr[ebx + 0x18]
	//		fstp	[esp + 0x1C - 0x18]
	//
	//		//push eax
	//		//mov eax, dword ptr[ebx + 0x1C]
	//		//mov partDamagePercentile, eax
	//		//pop eax
	//
	//		mov     edx, [ebx + 0x3C]
	//		test    edx, edx
	//		jz      __jz
	//		fld     dword ptr[edx + 4]
	//		fdiv    dword ptr[edx + 8]
	//		fstp	[esp + 0x1C - 0x14]
	//		mov     ecx, [ebx + 0x3C]
	//		add     esp, 0xFFFFFFF8
	//		fld     dword ptr[ecx + 8]
	//		fstp	[esp + 0x24 - 0x24]
	//		mov     edx, [ebx + 0x3C]
	//		add     esp, 0xFFFFFFF8
	//		fld     dword ptr[edx + 4]
	//		fstp	[esp + 0x2C - 0x2C]
	//		fld		[esp + 0x2C - 0x14]
	//		fmul    fltMaxHealth
	//		add     esp, 0xFFFFFFF8
	//		fstp	[esp + 0x34 - 0x34]
	//		add     esp, 0xFFFFFFF8
	//		fld     dword ptr[ebx + 0x18]
	//		fstp	[esp + 0x3C - 0x3C]
	//		add     esp, 0x0FFFFFFF8
	//		fld     dword ptr[ebx + 0x1C]
	//		fstp	[esp + 44h - 0x44]
	//		fld     fltMaxHealth
	//		fmul	[esp + 0x44 - 0x18]
	//		add     esp, 0xFFFFFFF8
	//		fstp	[esp + 0x4C - 0x4C]
	//		push    eax
	//		push    aS41f30f30fArmo
	//		push    ebp
	//		call    fnEcho
	//		add     esp, 0x3C
	//		call assignDamageVars
	//		jmp ptrStatusPartDamageCalcResume
	//		__jz:
	//		jmp ptrStatusInternalPartDamageCalc
	//
	//	}
	//}

	//void clearDamageStatArrCounter()
	//{
	//	Console::eval("deleteVariables('$damStatArr*'); ");
	//}
	//
	//MultiPointer(fnDumpDamage, 0, 0, 0, 0x004662F8);
	//MultiPointer(fnDumpDamageResume, 0, 0, 0, 0x00466308);
	//CodePatch dumpdamageintercept = { fnDumpDamage, "", "\xE9_DDI", 5, false };
	//NAKED void dumpDamageIntercept() {
	//	__asm {
	//		push	ebx
	//		push	esi
	//		push	edi
	//		push	ebp
	//		add		esp, 0xFFFFFFF4
	//		mov		ebp, edx
	//		mov		edi, eax
	//		mov		[esp + 0x1C - 0x1C], ecx
	//		xor		esi, esi
	//
	//		//Wipe the damStatArr global variable
	//		call clearDamageStatArrCounter
	//		jmp fnDumpDamageResume
	//	}
	//}

	//MultiPointer(ptrStatusPartDamageCalc, 0, 0, 0, 0x0046634A);
	//MultiPointer(ptrStatusPartDamageCalcResume, 0, 0, 0, 0x00466350);
	//MultiPointer(ptrStatusPart, 0, 0, 0, 0x006E8AAC);
	//MultiPointer(ptrStatusInternalPartDamageCalc, 0, 0, 0, 0x004663C0);
	//CodePatch statuspartcalc = { ptrStatusPartDamageCalc, "", "\xE9SPDC", 5, false };
	//NAKED void statusPartCalc() {
	//	__asm {
	//		push eax
	//		mov partName, eax // Part name
	//		pop eax
	//		fld dword ptr [ebx + 0x1C]
	//		push eax
	//		mov eax, dword ptr [ebx + 0x1C] //Current hit points (float)
	//		mov partDamage, eax
	//		pop eax
	//		fdiv dword ptr[ebx + 0x18]
	//		push eax
	//		mov eax, dword ptr [ebx + 0x18] //Max hit points (float)
	//		mov partDamageMax, eax
	//		pop eax
	//
	//		//push eax
	//		//mov eax, [NovaGetDamageStatus]
	//		//push eax
	//		//call Console::eval
	//		//add esp, 0x8
	//		call assignDamageVars
	//		mov eax, [esp + 0x1C + 0x1C]
	//
	//		jmp[ptrStatusPartDamageCalcResume]
	//	}
	//}

	//BuiltInFunction("Nova::getDamageStatus", _novaecho)
	//{
	//	//Console::echo("%s (%3.0f / %3.0f)", partName, partHealth, partMaxHealth);
	//	if (strlen(partName))
	//	{
	//		char partNameAssign[127];
	//		char partHealthAssign[127];
	//		float partCurrentHealth = (partHealth / partMaxHealth) * 100;
	//
	//		//Assign part name to array
	//		strcpy(partNameAssign, "$damStat[$damStatArr++, name] = '");
	//		strcat(partNameAssign, partName);
	//		strcat(partNameAssign, "';");
	//		Console::eval(partNameAssign);
	//		free(partNameAssign);
	//
	//		//Assign part health to array
	//		strcpy(partHealthAssign, "$damStat[$damStatArr, health] = '");
	//		strcat(partHealthAssign, tostring(partCurrentHealth));
	//		strcat(partHealthAssign, "';");
	//		Console::eval(partHealthAssign);
	//		//Console::echo(partNameFormatted);
	//		free(partHealthAssign);
	//	}
	//	//free(arrayIndex);
	//	//Console::echo(partName);
	//	//Console::echo(tostring(partHealth));
	//	//Console::echo(tostring(partMaxHealth));
	//	return 0;
	//}

	BuiltInFunction("Nova::getRenderDevices", _getRenderDevices)
	{
		if (strlen(renderDevice))
		{
			std::string device_full = renderDevice;
			if (device_full.find("OpenGL") != -1) { Console::setVariable("Nova::OpenGL::Info", device_full.c_str()); }
			std::size_t trim_pos = device_full.find(":");
			std::string device = device_full.erase(trim_pos, device_full.length());//Trim off the resolution mode list
			if (device.find("Software") != -1) { Console::setVariable("Nova::SoftwareDevice", device.c_str());}
			else if (device.find("Glide") != -1) { Console::setVariable("Nova::GlideDevice", device.c_str());}
			else if (device.find("OpenGL") != -1){Console::setVariable("Nova::OpenGLDevice", device.c_str());}
		}
		return "false";
	}


	float gamecursor_x;
	float gamecursor_y;
	void CursorRestraints()
	{
		Console::execFunction(0, "Nova::containCursor");
		Console::execFunction(0, "Nova::onCursorMove");
	}

	MultiPointer(ptrMouseMove, 0, 0, 0x005C26A3, 0x005C5EBF);
	MultiPointer(ptrMouseMoveResume, 0, 0, 0, 0x005C5ED0);
	CodePatch getcursor = { ptrMouseMove, "", "\xE9GCPS", 5, false };
	BuiltInVariable("Nova::cursorLocX", float, cursor_x, 0);
	BuiltInVariable("Nova::cursorLocY", float, cursor_y, 0);
	NAKED void GetCursor() {
		__asm {
			mov al, [edi + 0x1F8]
			push eax
			mov eax, dword ptr[edi + 0x1FC]
			mov cursor_x, eax
			pop eax

			push eax
			mov eax, dword ptr[edi + 0x200]
			mov cursor_y, eax
			pop eax
			call CursorRestraints

			jmp[ptrMouseMoveResume]
		}
	}

	BuiltInFunction("Nova::onCursorMove", _novaoncursormove) { return 0; }

	float cursorX = 320;
	float cursorY = 240;
	void setCursorCoords(float x, float y)
	{
		cursorX = x;
		cursorY = y;
	}

	MultiPointer(ptrSetCursorPos, 0, 0, 0x005C2790, 0x005C5FAC);
	MultiPointer(ptrSetCursorPosResume, 0, 0, 0x005C27A4, 0x005C5FC0);
	CodePatch setcursorloc = { ptrSetCursorPos, "", "\xE9SCLC", 5, false };
	NAKED void SetCursorLoc() {
		__asm {
			cmp cursorX, 0
			jnz adjust_X
			mov edx, [edi + 0x1FC]
			mov [esp + 0x8C - 0x7C], edx
			jmp continue_Y

			adjust_X:
				mov edx, cursorX
				mov [esp + 0x8C - 0x7C], edx
				mov cursorX, 0
			    jmp continue_Y
			continue_Y :
				cmp cursorY, 0
				jnz adjust_Y
				mov ecx, [edi + 0x200]
				mov[esp + 0x8C - 0x78], ecx
				jmp[ptrSetCursorPosResume]
			adjust_y:
				mov ecx, cursorY
				mov[esp + 0x8C - 0x78], ecx
				mov cursorY, 0
				jmp[ptrSetCursorPosResume]

		}
	}

	BuiltInFunction("Nova::setCursorLoc", _novasetcursorloc)
	{ 
		if (argc == 2)
		{
			setCursorCoords(atof(argv[0]), atof(argv[1]));
			setcursorloc.DoctorRelative((u32)SetCursorLoc, 1).Apply(true);
		}
		return 0;
	}

	u32 lastKeyDown = 0;
	void DirectInputToLabel()
	{
		char* chr;
		if (lastKeyDown == 0x01) { chr = "escape";}
		if (lastKeyDown == 0x02) { chr = "1";}
		if (lastKeyDown == 0x03) { chr = "2";}
		if (lastKeyDown == 0x04) { chr = "3";}
		if (lastKeyDown == 0x05) { chr = "4";}
		if (lastKeyDown == 0x06) { chr = "5";}
		if (lastKeyDown == 0x07) { chr = "6";}
		if (lastKeyDown == 0x08) { chr = "7";}
		if (lastKeyDown == 0x09) { chr = "8";}
		if (lastKeyDown == 0x0A) { chr = "9";}
		if (lastKeyDown == 0x0B) { chr = "0";}
		if (lastKeyDown == 0x0C) { chr = "-";}
		if (lastKeyDown == 0x0D) { chr = "=";}
		if (lastKeyDown == 0x0E) { chr = "backspace";}
		if (lastKeyDown == 0x0F) { chr = "tab";}
		if (lastKeyDown == 0x10) { chr = "q";}
		if (lastKeyDown == 0x11) { chr = "w";}
		if (lastKeyDown == 0x12) { chr = "e";}
		if (lastKeyDown == 0x13) { chr = "r";}
		if (lastKeyDown == 0x14) { chr = "t";}
		if (lastKeyDown == 0x15) { chr = "y";}
		if (lastKeyDown == 0x16) { chr = "u";}
		if (lastKeyDown == 0x17) { chr = "i";}
		if (lastKeyDown == 0x18) { chr = "o";}
		if (lastKeyDown == 0x19) { chr = "p";}
		if (lastKeyDown == 0x1A) { chr = "[";}
		if (lastKeyDown == 0x1B) { chr = "]";}
		if (lastKeyDown == 0x1C) { chr = "enter";}
		if (lastKeyDown == 0x1D) { chr = "lcontrol";}
		if (lastKeyDown == 0x1E) { chr = "a";}
		if (lastKeyDown == 0x1F) { chr = "s";}
		if (lastKeyDown == 0x20) { chr = "d";}
		if (lastKeyDown == 0x21) { chr = "f";}
		if (lastKeyDown == 0x22) { chr = "g";}
		if (lastKeyDown == 0x23) { chr = "h";}
		if (lastKeyDown == 0x24) { chr = "j";}
		if (lastKeyDown == 0x25) { chr = "k";}
		if (lastKeyDown == 0x26) { chr = "l";}
		if (lastKeyDown == 0x27) { chr = ";";}
		if (lastKeyDown == 0x28) { chr = "'";}
		if (lastKeyDown == 0x29) { chr = "`";}
		if (lastKeyDown == 0x2A) { chr = "lshift";}
		if (lastKeyDown == 0x2B) { chr = "backslash";}
		if (lastKeyDown == 0x2C) { chr = "z";}
		if (lastKeyDown == 0x2D) { chr = "x";}
		if (lastKeyDown == 0x2E) { chr = "c";}
		if (lastKeyDown == 0x2F) { chr = "v";}
		if (lastKeyDown == 0x30) { chr = "b";}
		if (lastKeyDown == 0x31) { chr = "n";}
		if (lastKeyDown == 0x32) { chr = "m";}
		if (lastKeyDown == 0x33) { chr = ",";}
		if (lastKeyDown == 0x34) { chr = ".";}
		if (lastKeyDown == 0x35) { chr = "slash";}
		if (lastKeyDown == 0x36) { chr = "rshift";}
		if (lastKeyDown == 0x37) { chr = "*";}
		if (lastKeyDown == 0x38) { chr = "lmenu";}
		if (lastKeyDown == 0x39) { chr = "space";}
		if (lastKeyDown == 0x3A) { chr = "capslock";}
		if (lastKeyDown == 0x3B) { chr = "f1";}
		if (lastKeyDown == 0x3C) { chr = "f2";}
		if (lastKeyDown == 0x3D) { chr = "f3";}
		if (lastKeyDown == 0x3E) { chr = "f4";}
		if (lastKeyDown == 0x3F) { chr = "f5";}
		if (lastKeyDown == 0x40) { chr = "f6";}
		if (lastKeyDown == 0x41) { chr = "f7";}
		if (lastKeyDown == 0x42) { chr = "f8";}
		if (lastKeyDown == 0x43) { chr = "f9";}
		if (lastKeyDown == 0x44) { chr = "f10";}
		if (lastKeyDown == 0x45) { chr = "numlock";}
		if (lastKeyDown == 0x46) { chr = "scroll";}
		if (lastKeyDown == 0x47) { chr = "numpad7";}
		if (lastKeyDown == 0x48) { chr = "numpad8";}
		if (lastKeyDown == 0x49) { chr = "numpad9";}
		if (lastKeyDown == 0x4A) { chr = "numpad-";}
		if (lastKeyDown == 0x4B) { chr = "numpad4";}
		if (lastKeyDown == 0x4C) { chr = "numpad5";}
		if (lastKeyDown == 0x4D) { chr = "numpad6";}
		if (lastKeyDown == 0x4E) { chr = "numpad+";}
		if (lastKeyDown == 0x4F) { chr = "numpad1";}
		if (lastKeyDown == 0x50) { chr = "numpad2";}
		if (lastKeyDown == 0x51) { chr = "numpad3";}
		if (lastKeyDown == 0x52) { chr = "numpad0";}
		if (lastKeyDown == 0x53) { chr = "numpad.";}
		if (lastKeyDown == 0x56) { chr = "rt102key";}
		if (lastKeyDown == 0x57) { chr = "f11";}
		if (lastKeyDown == 0x58) { chr = "f12";}
		if (lastKeyDown == 0x64) { chr = "f13";}
		if (lastKeyDown == 0x65) { chr = "f14";}
		if (lastKeyDown == 0x66) { chr = "f15";}
		if (lastKeyDown == 0x70) { chr = "kana";}
		if (lastKeyDown == 0x73) { chr = "abntc1";}
		if (lastKeyDown == 0x79) { chr = "convert";}
		if (lastKeyDown == 0x7B) { chr = "nonconvert";}
		if (lastKeyDown == 0x7D) { chr = "yen";}
		if (lastKeyDown == 0x7E) { chr = "abntc2";}
		if (lastKeyDown == 0x8D) { chr = "numpad=";}
		if (lastKeyDown == 0x90) { chr = "prevtrack";}
		if (lastKeyDown == 0x91) { chr = "@";}
		if (lastKeyDown == 0x92) { chr = ":";}
		if (lastKeyDown == 0x93) { chr = "_";}
		if (lastKeyDown == 0x94) { chr = "kanji";}
		if (lastKeyDown == 0x95) { chr = "stop";}
		if (lastKeyDown == 0x96) { chr = "ax";}
		if (lastKeyDown == 0x97) { chr = "unlabeled";}
		if (lastKeyDown == 0x99) { chr = "nexttrack";}
		if (lastKeyDown == 0x9C) { chr = "numpadenter";}
		if (lastKeyDown == 0x9D) { chr = "rcontrol";}
		if (lastKeyDown == 0xA0) { chr = "mute";}
		if (lastKeyDown == 0xA1) { chr = "calculator";}
		if (lastKeyDown == 0xA2) { chr = "playpause";}
		if (lastKeyDown == 0xA4) { chr = "mediastop";}
		if (lastKeyDown == 0xAE) { chr = "volumedown";}
		if (lastKeyDown == 0xB0) { chr = "volumeup";}
		if (lastKeyDown == 0xB2) { chr = "webhome";}
		if (lastKeyDown == 0xB3) { chr = "numpadcomma";}
		if (lastKeyDown == 0xB5) { chr = "numpaddivide";}
		if (lastKeyDown == 0xB7) { chr = "sysrq";}
		if (lastKeyDown == 0xB8) { chr = "rmenu";}
		if (lastKeyDown == 0xC5) { chr = "pause";}
		if (lastKeyDown == 0xC7) { chr = "home";}
		if (lastKeyDown == 0xC8) { chr = "up";}
		if (lastKeyDown == 0xC9) { chr = "pageup";}
		if (lastKeyDown == 0xCB) { chr = "left";}
		if (lastKeyDown == 0xCD) { chr = "right";}
		if (lastKeyDown == 0xCF) { chr = "end";}
		if (lastKeyDown == 0xD0) { chr = "down";}
		if (lastKeyDown == 0xD1) { chr = "pagedown";}
		if (lastKeyDown == 0xD2) { chr = "insert";}
		if (lastKeyDown == 0xD3) { chr = "delete";}
		if (lastKeyDown == 0xDB) { chr = "lwin";}
		if (lastKeyDown == 0xDC) { chr = "rwin";}
		if (lastKeyDown == 0xDD) { chr = "apps";}
		if (lastKeyDown == 0xDE) { chr = "power";}
		if (lastKeyDown == 0xDF) { chr = "sleep";}
		if (lastKeyDown == 0xE3) { chr = "wake";}
		if (lastKeyDown == 0xE5) { chr = "websearch";}
		if (lastKeyDown == 0xE6) { chr = "webfavorites";}
		if (lastKeyDown == 0xE7) { chr = "webrefresh";}
		if (lastKeyDown == 0xE8) { chr = "webstop";}
		if (lastKeyDown == 0xE9) { chr = "webforward";}
		if (lastKeyDown == 0xEA) { chr = "webback";}
		if (lastKeyDown == 0xEB) { chr = "computer";}
		if (lastKeyDown == 0xEC) { chr = "mail";}
		if (lastKeyDown == 0xED) { chr = "mediaselect";}

		Console::setVariable("nova::lastKeyDown", chr);
		if (Console::functionExists("Canvas::onKeyDown"))
		{
			Console::eval("Canvas::onKeyDown($nova::lastKeyDown);");
		}
	}

	MultiPointer(ptrCanvasOnKeyDown, 0, 0, 0, 0x005C81B4);
	MultiPointer(ptrCanvasOnKeyDownResume, 0, 0, 0, 0x005C81C3);
	MultiPointer(loc_5C81D2, 0, 0, 0, 0x005C81D2);
	MultiPointer(loc_5C8210, 0, 0, 0, 0x005C8210);
	CodePatch canvaskeyintercept = { ptrCanvasOnKeyDown, "", "\xE9GCKD", 5, false };
	NAKED void canvasKeyIntercept() {
		__asm {
			xor ecx, ecx
			mov cl, [edx + 0x2C]
			mov lastKeyDown, ecx

			mov dummy, ecx
			mov dummy2, eax
			mov dummy3, edx
			mov dummy4, ebx
			call DirectInputToLabel
			mov ecx, dummy
			mov eax, dummy2
			mov edx, dummy3
			mov ebx, dummy4
			cmp ecx, 0xCB
			jg __jg
			jz __jz
			jmp ptrCanvasOnKeyDownResume

			__jg:
			jmp loc_5C81D2

			__jz:
			jmp loc_5C8210
		}
	}

	//BuiltInFunction("Nova::gameCursorPos", _novacursorposition)
	//{
	//	if (argc != 1)
	//	{
	//		Console::echo("%s( x/y );", self);
	//		return 0;
	//	}
	//	string axis = argv[0];
	//	if (axis.compare("x") == 0 || axis.compare("X") == 0)
	//	{
	//		return tostring(gamecursor_x);
	//	}
	//	else if (axis.compare("y") == 0 || axis.compare("Y") == 0)
	//	{
	//		return tostring(gamecursor_y);
	//	}
	//	return 0;
	//}

	//BuiltInFunction("Nova::onLoadVehicle", _novaonloadvehicle){return "true";}
	//MultiPointer(ptrGuiVehControllerLoadVehicle, 0, 0, 0, 0x004F4ECE);
	//MultiPointer(ptrTextWrap, 0, 0, 0, 0x004F3B20);
	//MultiPointer(ptrVehicleViewStatusDisp, 0, 0, 0, 0x004F4770);
	//static const char* NovaLoadVehicle = "Nova::onLoadVehicle();";
	//char* clientVehicle = "NONE";
	//CodePatch guivehcontrollerloadvehicle = { ptrGuiVehControllerLoadVehicle, "", "\xE9VEHL", 5, false };
	//NAKED void GuiVehControllerLoadVehicle() {
	//	__asm {
	//		mov edx, edi
	//		mov clientVehicle, edx
	//		mov eax, ebx
	//		call ptrTextWrap
	//		mov eax, ebx
	//		call ptrVehicleViewStatusDisp
	//		add esp, 0x100
	//
	//		//Call our function
	//		push eax
	//		mov eax, [NovaLoadVehicle]
	//		push eax
	//		call Console::eval
	//		add esp, 0x8
	//
	//		pop ebp
	//		pop edi
	//		pop esi
	//		pop ebx
	//		retn
	//	}
	//}

	void fnVehicleFileLookup()
	{
		Console::eval("Nova::onVehicleFileLookup();");
	}

	BuiltInFunction("Nova::onVehicleFileLookup", _novaonVehicleFileLookup) { return "true"; }
	MultiPointer(ptrGuiVehControllerFileLookup, 0, 0, 0x004F2B40, 0x004F4FD8);
	MultiPointer(ptrGuiVehControllerLoadFileNameResume, 0, 0, 0x004F2B49, 0x004F4FE1);
	static const char* NovaLoadVehicle = "Nova::onVehicleFileLookup();";
	char* clientVehicleFile = "NONE";
	CodePatch vehiclefilelookup = { ptrGuiVehControllerFileLookup, "", "\xE9VFLU", 5, false };
	NAKED void VehicleFileLookup() {
		__asm {
			push ebx
			mov ebx, edx
			mov clientVehicleFile, ebx
			call fnVehicleFileLookup
			push ebx
			call _strlen
			jmp ptrGuiVehControllerLoadFileNameResume
		}
	}

	char* getLoadedVehicle()
	{
		return clientVehicleFile;
	}

	BuiltInFunction("Nova::getLoadedVehicleFileName", _novagetloadedvehiclename)
	{
		return clientVehicleFile;
	}


	MultiPointer(ptrPlayerMessageEveryone, 0, 0, 0x0055ACD7, 0x0055DB13);
	MultiPointer(ptrPlayerMessageEveryoneResume, 0, 0, 0x0055AD3D, 0x0055DB79);
MultiPointer(ptrPlayerMessageEveryoneReturn, 0, 0, 0, 0x0055DB79);
static const char* messageEventfn = "Nova::push::onPlayerMessage();";
static const char* msg_sprint = "%s %s->%s: %s";
//static const char* msg_sprint = "";
CodePatch playeronmessage = { ptrPlayerMessageEveryone, "", "\xE9PONM", 5, false };
char* playerMessage;
char* playerName;
char* messageRecipient;

//This function calls player::onMessage with the playerName and their message as args
//BuiltInFunction("Nova::push::onPlayerMessage", _novaplayermessage)
void onPlayerMessage()
{
	Console::echo("MSG: %s-> (%s): %s", playerName, messageRecipient, playerMessage);
	char playerOnMessage_evalString[127];
	strcpy(playerOnMessage_evalString, "player::onMessage(\"");
	strcat(playerOnMessage_evalString, playerName);
	strcat(playerOnMessage_evalString, "\",\"");
	strcat(playerOnMessage_evalString, playerMessage);
	strcat(playerOnMessage_evalString, "\");");
	//Final string -> player::onMessage("%playerName","%playerMessage");
	eval(playerOnMessage_evalString);
	free(playerOnMessage_evalString);
	//return "true";
}

NAKED void playerOnMessage() {
	__asm {
		mov [dummy], 0
		add ebx, 0x75
		push ebx
		mov playerMessage, ebx
		push edi
		mov messageRecipient, edi
		push ebp
		mov playerName, ebp
		push esi

		push[msg_sprint]
		
		mov eax, [ptrConsoleBuffer]
		push eax
		call printf //Just throw the message sprint to printf. We must handle this input else the subroutine will crash.
		add esp, 0x18
		mov dummy, eax
		call onPlayerMessage
		mov eax, dummy
		jmp[ptrPlayerMessageEveryoneResume]
	}
}

const char* campaignFace;
void assignFaceBmpVar()
{
	Console::setVariable("engine::campaignFace", campaignFace);
	execFunction(0, "ProfileGUI::onSelectFace");
}

//Get the face or logo bmp on the initial button press
static const u32 sub_4DADE8 = 0x4DADE8;
MultiPointer(ptrGuiButtonInitialFace, 0, 0, 0, 0x0053503A);
MultiPointer(ptrGuiButtonInitialFaceResume, 0, 0, 0, 0x00535041);
CodePatch guibuttoninitialface = { ptrGuiButtonInitialFace, "", "\xE9SBFI", 5, false };
NAKED void guiButtonInitialFace() {
	__asm {
		mov     eax, esi
		call    sub_4DADE8
		mov		campaignFace, eax
		call	assignFaceBmpVar
		mov		eax, campaignFace
		jmp		ptrGuiButtonInitialFaceResume
	}
}

MultiPointer(ptrGuiButtonSelectFace, 0, 0, 0, 0x0053512D);
MultiPointer(ptrGuiButtonSelectFaceResume, 0, 0, 0, 0x00535134);
CodePatch guibuttonselectface = { ptrGuiButtonSelectFace, "", "\xE9SSFS", 5, false };
NAKED void guiButtonSelectFace() {
	__asm {
		mov     eax, ebx
		call    sub_4DADE8
		mov		campaignFace, eax
		call	assignFaceBmpVar
		mov		eax, campaignFace
		jmp		ptrGuiButtonSelectFaceResume
	}
}

//Create a empty player::onMessage(); initially
BuiltInFunction("player::onMessage", _playeronmessage) { return 0; }

//BuiltInFunction("Nova::CursorPatch", _novacursorpatch)
//{
//	getcursor.DoctorRelative((u32)GetCursor, 1).Apply(true);
//	return "true";
//}

void overrideFadeEvent()
{
	std::string gui = currentGui;
	if (gui.compare("playgui") != 0 || gui.compare("playGui") != 0)
	{
		Console::eval("schedule(\"ffEvent(0, 0, 0, 0);\");\",0);");
	}
}

	MultiPointer(ptrFadeEvent, 0, 0, 0, 0x0045DD28);
	CodePatch fadeeventpatch = { ptrFadeEvent, "\x8B\xD8", "\xEB\x45", 2, false };

	MultiPointer(ptrFadeEventPacket, 0, 0, 0, 0x00691D58);
	MultiPointer(ptrFadeEventPacketHandle, 0, 0, 0, 0x00691D6D);
	MultiPointer(ptrFadeEventPacketBad, 0, 0, 0, 0x00691D5E);
	MultiPointer(ptrFadeEventPacketDrop, 0, 0, 0, 0x00691F50);
	CodePatch fadeeventpacketpatch = { ptrFadeEventPacket, "", "\xE9_FPP", 5, false };
	CodePatch fadeeventpacketpatchback = { ptrFadeEventPacket, "", "\x8B\xF8\x85\xFF\x75\x0F", 6, false };

	void enableFadeEvents()
	{
		fadeeventpacketpatchback.Apply(true);
		fadeeventpatch.Apply(false);
	}

	MultiPointer(ptrWaitroomCloseToPlayGui, 0, 0, 0, 0x0054112E);
	MultiPointer(sub5C6C54, 0, 0, 0, 0x005C6C54);
	MultiPointer(ptrCloseWaitroomGui, 0, 0, 0, 0x00541136);
	CodePatch waitroomclosetoplaygui = { ptrWaitroomCloseToPlayGui, "", "\xE9_FPP", 5, false };
	//Intercept the call to join the map to re-enable server side fade events (Disabled fade events breaks player spawn packets)
	NAKED void WaitroomCloseToPlayGUI() {
		__asm {
			call enableFadeEvents
			mov eax, [esi + 0x58]
			call sub5C6C54
			jmp ptrCloseWaitroomGui
		}
	}

	NAKED void fadeEventPacketPatch() {
		__asm {
			mov edi, eax
			cmp[edi], 0
			je __je
			test edi, edi
			jnz __jnz
			jmp ptrFadeEventPacketBad
			__je :
			jmp ptrFadeEventPacketDrop
				__jnz :
			jmp ptrFadeEventPacketHandle
		}
	}

	//Used to drop fade event packets from the server
	//BuiltInFunction("Nova::fadeEvents", _novafadeevents)
	//{
	//	if (argc != 1)
	//	{
	//		return 0;
	//	}
	//
	//	std::string boolean = argv[0];
	//	if (boolean.compare("0") == 0 || boolean.compare("false") == 0 || boolean.compare("False") == 0)
	//	{
	//		fadeeventpacketpatch.DoctorRelative((u32)fadeEventPacketPatch, 1).Apply(true);
	//		fadeeventpatch.Apply(true);
	//	}
	//	else if (boolean.compare("1") == 0 || boolean.compare("true") == 0 || boolean.compare("True") == 0)
	//	{
	//		fadeeventpacketpatchback.Apply(true);
	//		fadeeventpatch.Apply(false);
	//		Console::eval("ffevent(0,0,0,0);");
	//	}
	//	return "true";
	//}

	//Allow ffevent without PlayerPacketStream
	MultiPointer(ptrffevent_packetstreamcheck, 0, 0, 0, 0x004B796D);
	CodePatch ffeventpatch0 = { ptrffevent_packetstreamcheck, "", "\xEB", 1, false };

	MultiPointer(ptrffevent_err1, 0, 0, 0, 0x004B7A57);
	//CodePatch ffeventpatch1 = { ptrffevent_err1, "", "\x90\x90\x90\x90\x90\x90", 6, false };
	CodePatch ffeventpatch1 = { ptrffevent_err1, "", "\xE9_FE1", 5, false };
	MultiPointer(ptrffevent_err1_resume, 0, 0, 0, 0x004B7A5D);
	NAKED void ffEventPatch1() {
		__asm {
			cmp edx , 0
			je __je
			fadd dword ptr[edx + 0x8C]
			jmp ptrffevent_err1_resume
			__je:
			jmp ptrffevent_err1_resume
		}
	}

	MultiPointer(ptrffevent_err2, 0, 0, 0, 0x004B7AC5);
	//CodePatch ffeventpatch2 = { ptrffevent_err2, "", "\x90\x90\x90\x90\x90\x90", 6, false };
	CodePatch ffeventpatch2 = { ptrffevent_err2, "", "\xE9_FE2", 5, false };
	MultiPointer(ptrffevent_err2_resume, 0, 0, 0, 0x004B7ACB);
	NAKED void ffEventPatch2() {
		__asm {
			cmp ecx, 0
			je __je
			fadd dword ptr[ecx + 0x8C]
			jmp ptrffevent_err2_resume
			__je:
			jmp ptrffevent_err2_resume
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

				//ListDevices();
				listdevices.DoctorRelative((u32)ListDevices, 1).Apply(true);

				//dumpDamage Patches
				//statuspartcalc.DoctorRelative((u32)statusPartCalc, 1).Apply(true);

				//Input Event Calls
				//onsim3Dmouseevent.DoctorRelative((u32)OnSim3DMouseEvent, 1).Apply(true);

				//GuiVehCustController Vehicle Select Call
				//guivehcontrollerloadvehicle.DoctorRelative((u32)GuiVehControllerLoadVehicle, 1).Apply(true);
				vehiclefilelookup.DoctorRelative((u32)VehicleFileLookup, 1).Apply(true);

				//player::onMessage
				playeronmessage.DoctorRelative((u32)playerOnMessage, 1).Apply(true);
				
				//OpenGL DEV
				//openglmodeextend.DoctorRelative((u32)OpenGLModeExtend, 1).Apply(true);

				//dumpDamage / damage
				//dumpdamageintercept.DoctorRelative((u32)dumpDamageIntercept, 1).Apply(true);
				//statuspartcalc.DoctorRelative((u32)statusPartCalc, 1).Apply(true);

				guibuttoninitialface.DoctorRelative((u32)guiButtonInitialFace, 1).Apply(true);
				guibuttonselectface.DoctorRelative((u32)guiButtonSelectFace, 1).Apply(true);

				//Disable fade event packets to prevent scriptGL fading as well
				//waitroomclosetoplaygui.DoctorRelative((u32)WaitroomCloseToPlayGUI, 1).Apply(true);
				//fadeeventpacketpatch.DoctorRelative((u32)fadeEventPacketPatch, 1).Apply(true);
				//fadeeventpatch.Apply(true);

				//Allow ffevent to be used without a playerpacketstream
				ffeventpatch0.Apply(true);
				ffeventpatch1.DoctorRelative((u32)ffEventPatch1, 1).Apply(true);
				ffeventpatch2.DoctorRelative((u32)ffEventPatch2, 1).Apply(true);

				//Gets cursor coordinates
				getcursor.DoctorRelative((u32)GetCursor, 1).Apply(true);

				//Capture key presses to a variable
				canvaskeyintercept.DoctorRelative((u32)canvasKeyIntercept, 1).Apply(true);
		}
	} init;
}