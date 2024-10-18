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
MultiPointer(fnCockpitShake, 0, 0, 0, 0x0046BE30);
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
	//CodePatch statuspartcalc = { ptrStatusPartDamageCalc, "", "\xE9SPDC", 5, false };
	//char* partName;
	//float partHealth;
	//float partMaxHealth;
	//static const char* NovaGetDamageStatus = "Nova::getDamageStatus();";
	//NAKED void statusPartCalc() {
	//	__asm {
	//		push eax
	//		mov partName, eax // Part name
	//		pop eax
	//		fld dword ptr [ebx + 0x1C]
	//		push eax
	//		mov eax, dword ptr [ebx + 0x1C] //Current hit points (float)
	//		mov partHealth, eax
	//		pop eax
	//		fdiv dword ptr[ebx + 0x18]
	//		push eax
	//		mov eax, dword ptr [ebx + 0x18] //Max hit points (float)
	//		mov partMaxHealth, eax
	//		pop eax
	//
	//		push eax
	//		mov eax, [NovaGetDamageStatus]
	//		push eax
	//		call Console::eval
	//		add esp, 0x8
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
	void SetCursorLocationVars()
	{
		Console::setVariable("Nova::cursorLocX", tostring(gamecursor_x));
		Console::setVariable("Nova::cursorLocY", tostring(gamecursor_y));
		Console::execFunction(0, "Nova::containCursor");
		Console::execFunction(0, "Nova::onCursorMove");
	}

	MultiPointer(ptrMouseMove, 0, 0, 0x005C26A3, 0x005C5EBF);
	MultiPointer(ptrMouseMoveResume, 0, 0, 0x005C26A9, 0x005C5EC5);
	CodePatch getcursor = { ptrMouseMove, "", "\xE9GCPS", 5, false };
	//static const char* NovaOnMouseMove = "Nova::containCursor();Nova::onCursorMove();";
	NAKED void GetCursor() {
		__asm {
			mov al, [edi + 0x1F8]
			push eax
			mov eax, dword ptr[edi + 0x1FC]
			//jmp[ptrFloatTOInt]
			mov gamecursor_x, eax
			pop eax

			push eax
			mov eax, dword ptr[edi + 0x200]
			//jmp[ptrFloatTOInt]
			mov gamecursor_y, eax
			pop eax
			call SetCursorLocationVars

			//push eax
			//mov eax, [NovaOnMouseMove]
			//push eax
			//call Console::eval
			//add esp, 0x8

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
	static const char* messageEventfn = "Nova::push::onPlayerMessage();";
	static const char* msg_sprint = "%s %s->%s: %s";
	CodePatch playeronmessage = { ptrPlayerMessageEveryone, "", "\xE9PONM", 5, false };
	char* playerMessage;
	char* playerName;
	char* messageRecipient;
	NAKED void playerOnMessage() {
		__asm {
			add ebx, 0x75
			push ebx
			mov playerMessage, ebx
			push edi
			mov messageRecipient, edi
			push ebp
			mov playerName, ebp
			push esi
			
			push [msg_sprint]
			
			mov eax, [ptrConsoleBuffer]
			push eax
			call Console::echo
			//call fnEcho
			add esp, 0x18

			push eax
			mov eax, [messageEventfn]
			push eax
			call Console::eval
			add esp, 0x8

			jmp [ptrPlayerMessageEveryoneResume]
		}
	}

	//This function calls player::onMessage with the playerName and their message as args
	BuiltInFunction("Nova::push::onPlayerMessage", _novaplayermessage)
	{
		Console::echo("MSG: %s-> (%s): %s", playerName, messageRecipient, playerMessage);
		char playerOnMessage_evalString[127];
		strcpy(playerOnMessage_evalString, "player::onMessage(\"");
		strcat(playerOnMessage_evalString, playerName);
		strcat(playerOnMessage_evalString,  "\",\"");
		strcat(playerOnMessage_evalString, playerMessage);
		strcat(playerOnMessage_evalString, "\");");
		//Final string -> player::onMessage("%playerName","%playerMessage");
		eval(playerOnMessage_evalString);
		free(playerOnMessage_evalString);
		return "true";
	}

	//Create a empty player::onMessage(); initially
	BuiltInFunction("player::onMessage", _playeronmessage){return 0;}

	BuiltInFunction("Nova::CursorPatch", _novacursorpatch)
	{ 
		getcursor.DoctorRelative((u32)GetCursor, 1).Apply(true);
		return "true";
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

		}
	} init;
}