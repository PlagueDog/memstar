#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <string>
#include "conversionFunctions.h"

using namespace std;
using namespace Console;
using namespace conversionFunctions;

namespace ExtendedVariables
{
	u32 dummy, dummy2, dummy3, dummy4, dummy5;
	MultiPointer(ptrFloatTOInt, 0, 0, 0, 0x00693F08);
	MultiPointer(_tan, 0, 0, 0, 0x006D0D34);
	MultiPointer(_atan2, 0, 0, 0, 0x006D0404);
	//Big endian to little endian
	string _BEtoLE(string& str)
	{
		string it;
		std::reverse(str.begin(), str.end());
		for (auto it = str.begin(); it != str.end(); it += 2) {
			std::swap(it[0], it[1]);
		}
		return it;
	}

	//MultiPointer(ptrsprintf, 0, 0, 0, 0x006CD8B4);

	MultiPointer(ptrgetkph, 0, 0, 0x0051A3CB, 0x0051C868);
	MultiPointer(ptrKPHstrResume, 0, 0, 0x0051A3D1, 0x0051C86E);
	CodePatch codepatch_getkph = { ptrgetkph, "", "\xE9GKPH", 5, false };
	char* kphValue = 0;
	static const char* kphFormat = "%0.0fKph";
	BuiltInVariable("vehicle::KPH", float, vehicleKPH, 0);
	NAKED void getKPH() {
		__asm {
			push [kphFormat]
			push eax
			mov vehicleKPH, eax;
			jmp [ptrKPHstrResume]
		}
	}

	BuiltInFunction("Nova::getKPH", _getKPH)
	{
		if (strlen(kphValue))
		{
			return kphValue;
		}
		return 0;
	}

	MultiPointer(ptrgetradarrange, 0, 0, 0, 0x0051C318);
	MultiPointer(ptrRadarRangeResume, 0, 0, 0, 0x0051C31E);
	CodePatch codepatch_getradarrange = { ptrgetradarrange, "", "\xE9GRDR", 5, false };
	float vehicleRadarRange = 0;
	NAKED void getRadarRange() {
		__asm {
			fld dword ptr[ebx + 0x298]
			fstp[esp + 0x1B4 - 0x1B4]
			mov ecx, [esp + 0x1B4 - 0x1B4]
			mov vehicleRadarRange, ecx
			mov eax, [ebx + 0x1CC]
			jmp[ptrRadarRangeResume]
		}
	}

	MultiPointer(ptrgetradar, 0, 0, 0, 0x0051C309);
	MultiPointer(ptrRadarResume, 0, 0, 0, 0x0051C310);
	CodePatch codepatch_getradar = { ptrgetradar, "", "\xE9GRAD", 5, false };
	static const char* radarFormat = "%s %0.0fm";
	char* vehicleRadarMode = 0;
	NAKED void getRadar() {
		__asm {
			push ecx
			mov vehicleRadarMode, ecx
			push radarFormat
			push eax
			jmp[ptrRadarResume]
		}
	}

	void variableSet_HudElements()
	{
		Console::setVariable("vehicle::radarMode", vehicleRadarMode);
		Console::setVariable("vehicle::radarRange", tostring(vehicleRadarRange));
	}

	MultiPointer(ptrHudElements, 0, 0, 0, 0x0051C9D4);
	MultiPointer(ptrHudElementsEND, 0, 0, 0, 0x0051C9DA);
	CodePatch assignhudproperties = { ptrHudElements, "", "\xE9_AHP", 5, false };
	NAKED void assignHudProperties() {
		__asm {
			call variableSet_HudElements
			pop     edi
			pop     esi
			pop     ebx
			mov     esp, ebp
			pop     ebp
			jmp[ptrHudElementsEND]
		}
	}

	//BuiltInFunction("getRadar", _getRadar)
	//{
	//	if (strlen(vehicleRadarMode))
	//	{
	//		Console::echo(vehicleRadarMode);
	//		Console::echo(tostring(vehicleRadarRange));
	//	}
	//	return 0;
	//}

	MultiPointer(ptrgetenergy, 0, 0, 0, 0x0051B06F);
	MultiPointer(ptrEnergyResume, 0, 0, 0, 0x0051B075);
	CodePatch codepatch_getenergy = { ptrgetenergy, "", "\xE9GENG", 5, false };
	BuiltInVariable("vehicle::energyLevel", float, vehicleEnergy, 0);
	NAKED void getEnergy() {
		__asm {
			mov esi, [eax + 4]
			call dword ptr [esi + 0x24]
			mov ecx, [ebp + 8]
			mov vehicleEnergy, ecx
			xor ecx, ecx
			jmp [ptrEnergyResume]
		}
	}

	MultiPointer(ptrgetshield, 0, 0, 0, 0x0051E6DA);
	MultiPointer(ptrShieldResume, 0, 0, 0, 0x0051E6E0);
	CodePatch codepatch_getshield = { ptrgetshield, "", "\xE9GSHD", 5, false };
	BuiltInVariable("vehicle::shieldStr", float, vehicleShields, 0);
	float mul_100 = 100;
	NAKED void getShield() {
		__asm {
			mov [ebp - 0x44], edx
			mov edx, [ebp + 0x10]
			fld [ebp + 8]
			fmul mul_100
			fstp [esp + 0x144 - 0x148 + 4]
			mov ecx, [esp + 0x144 - 0x148 + 4]
			mov vehicleShields, ecx
			xor ecx, ecx
			jmp[ptrShieldResume]
		}
	}

	BuiltInVariable("vehicle::shieldFocStrFront", float, vehicleShieldsFront, 0);
	BuiltInVariable("vehicle::shieldFocStrRear", float, vehicleShieldsRear, 0);
	void calcForwardShieldStrength()
	{
		float calculatedFront = 100 - vehicleShieldsRear;
		if (calculatedFront == -50) //This should only happen if the player does not have a shield modulator
		{
			Console::setVariable("vehicle::shieldFocusStrengthFront", "50");
		}
		Console::setVariable("vehicle::shieldFocusStrengthFront", tostring(calculatedFront));
	}

	MultiPointer(ptrgetshieldmod, 0, 0, 0, 0x0051EF52);
	MultiPointer(ptrShieldModResume, 0, 0, 0, 0x0051EF58);
	CodePatch codepatch_getshieldforward = { ptrgetshieldmod, "", "\xE9GSHM", 5, false };
	float fld_100 = 100;
	float fld_49 = 49;
	float fsubr_50 = 50;
	NAKED void getShieldForward() {
		__asm {
			fld fld_49
			fmul [ebp + 0xC]
			fsubr fsubr_50
			fstp [esp + 0x84 - 0x84]
			mov ecx, [esp + 0x84 - 0x84]
			mov vehicleShieldsRear, ecx
			xor ecx, ecx
			call calcForwardShieldStrength
			jmp[ptrShieldModResume]
		}
	}

	BuiltInVariable("vehicle::shieldModRotation", float, vehicleShieldsModRotation, 0);
	MultiPointer(ptrgetshieldmodrotation, 0, 0, 0, 0x0051EA19);
	MultiPointer(ptrShieldModRotationResume, 0, 0, 0, 0x0051EA26);
	CodePatch codepatch_getshieldmodrotation = { ptrgetshieldmodrotation, "", "\xE9GSMR", 5, false };
	float mul_180 = 180;
	float div_pi = 3.14159;
	NAKED void getShieldModRotation() {
		__asm {
			fld [ebp + 8]
			fmul mul_180
			fdiv div_pi
			fstp [esp + 0x34 - 0x34]
			mov ecx, [esp + 0x34 - 0x34]
			mov vehicleShieldsModRotation, ecx
			xor ecx, ecx
			mov [ebp - 4], eax
			mov eax, [ebp - 4]
			mov byte ptr [eax + 0x39C], 0

			jmp[ptrShieldModRotationResume]
		}
	}

	//BuiltInFunction("Nova::getEnergy", _novagetEnergy)
	//{
	//	if ((energyValue * 100) > 99.8)
	//	{
	//		return "100";
	//	}
	//	if (energyValue)
	//	{
	//		return tostring(energyValue * 100);
	//	}
	//	return 0;
	//}

	MultiPointer(ptrgetthrottle, 0, 0, 0, 0x0051B318);
	MultiPointer(ptrthrottleResume, 0, 0, 0, 0x0051B31E);
	CodePatch codepatch_getthrottle = { ptrgetthrottle, "", "\xE9GENG", 5, false };
	BuiltInVariable("vehicle::throttle", float, vehicleThrottle, 0);
	NAKED void getThrottle() {
		__asm {
			mov ecx, [ebp + 8]
			mov vehicleThrottle, ecx
			pop		ecx
			pop     edi
			pop     esi
			pop     ebx
			mov     esp, ebp
			pop     ebp
			jmp[ptrthrottleResume]
		}
	}

	void setVehicleTeamVarYellow()	{ Console::setVariable("vehicle::team", "Yellow"); }
	void setVehicleTeamVarBlue()	{ Console::setVariable("vehicle::team", "Blue"); }
	void setVehicleTeamVarRed()		{ Console::setVariable("vehicle::team", "Red"); }
	void setVehicleTeamVarPurple()	{ Console::setVariable("vehicle::team", "Purple"); }

	MultiPointer(ptrSetHudTeamColorYellow, 0, 0, 0, 0x0051BEDC);
	MultiPointer(ptrSetHudTeamColorBlue, 0, 0, 0, 0x0051BEE6);
	MultiPointer(ptrSetHudTeamColorRed, 0, 0, 0, 0x0051BEF0);
	MultiPointer(ptrSetHudTeamColorPurple, 0, 0, 0, 0x0051BEFA);
	MultiPointer(ptrSetHudTeamColorRESUME, 0, 0, 0, 0x0051BF0C);
	CodePatch codepatch_getteamyellow = { ptrSetHudTeamColorYellow, "", "\xE9GTYE", 5, false };
	CodePatch codepatch_getteamblue = { ptrSetHudTeamColorBlue, "", "\xE9GTBL", 5, false };
	CodePatch codepatch_getteamred = { ptrSetHudTeamColorRed, "", "\xE9GTRE", 5, false };
	CodePatch codepatch_getteampurple = { ptrSetHudTeamColorPurple, "", "\xE9GTRE", 5, false };
	NAKED void getTeamYellow() {
		__asm {
			mov [esp + 0x24 - 0x1C] , 0xF0
			call setVehicleTeamVarYellow
			jmp[ptrSetHudTeamColorRESUME]
		}
	}
	NAKED void getTeamBlue() {
		__asm {
			mov [esp + 0x24 - 0x1C] , 0xF1
			call setVehicleTeamVarBlue
			jmp[ptrSetHudTeamColorRESUME]
		}
	}
	NAKED void getTeamRed() {
		__asm {
			mov [esp + 0x24 - 0x1C] , 0xF2
			call setVehicleTeamVarRed
			jmp[ptrSetHudTeamColorRESUME]
		}
	}
	NAKED void getTeamPurple() {
		__asm {
			mov[esp + 0x24 - 0x1C] , 0xF3
			call setVehicleTeamVarPurple
			jmp[ptrSetHudTeamColorRESUME]
		}
	}
	//Add Object type pointer addresses to the clientTree object info syntax
	MultiPointer(ptrClientTreeLineSyntax_namedObject, 0, 0, 0, 0x005884AE);
	MultiPointer(ptrClientTreeLineSyntax_namedObject_RETN, 0, 0, 0, 0x005884B4);
	CodePatch ctreepointers = { ptrClientTreeLineSyntax_namedObject, "", "\xE9PTRO", 5, false };
	static const char* cTreeNamedObjectPointer = "%4i, \"%s\", %s";
	NAKED void cTreePointers() {
		__asm {
			push edx
			push cTreeNamedObjectPointer
			jmp[ptrClientTreeLineSyntax_namedObject_RETN]
		}
	}

	MultiPointer(ptrClientTreeLineSyntax_namelessObject, 0, 0, 0, 0x005884E9);
	MultiPointer(ptrClientTreeLineSyntax_namelessObject_RETN, 0, 0, 0, 0x005884EF);
	CodePatch ctreepointers_nameless = { ptrClientTreeLineSyntax_namelessObject, "", "\xE9PNLS", 5, false };
	//static const char* cTreeNamedObjectPointer_nameless = "%4i,\"\", %s  (%X)";
	static const char* cTreeNamedObjectPointer_nameless = "%4i, %s";
	NAKED void cTreePointers_nameless() {
		__asm {
			push eax
			push cTreeNamedObjectPointer_nameless
			jmp[ptrClientTreeLineSyntax_namelessObject_RETN]
		}
	}

	MultiPointer(ptrClientTreeWin, 0, 0, 0, 0x0057CA8F);
	MultiPointer(ptrClientTreeWin_RETN, 0, 0, 0, 0x0057CA95);
	CodePatch clienttreewin = { ptrClientTreeWin, "", "\xE9TWIN", 5, false };
	//static const char* cTreeNamedObjectPointer_nameless = "%4i,\"\", %s  (%X)";
	//static const char* cTreeNamedObjectPointer_nameless = "%4i, %s";
	NAKED void ClientTreeWin() {
		__asm {
			push 0
			push 1
			push WS_POPUP
			jmp [ptrClientTreeWin_RETN]
		}
	}

	MultiPointer(ptrCanvasMouseSensitivity, 0, 0, 0, 0x005C56FB);
	MultiPointer(ptrCanvasMouseSensitivity_RETN, 0, 0, 0, 0x005C5705);
	CodePatch canvasmousepatch = { ptrCanvasMouseSensitivity, "", "\xE9MSPT", 5, false };

	int ptrMouseSensitivity;
	NAKED void CanvasMousePatch() {
		__asm {
			mov dword ptr [ebx + 0x25C], 0x3F800000
			push ebx
			lea ebx, [ebx + 0x25C]
			mov dword ptr ptrMouseSensitivity, ebx
			pop ebx
			jmp [ptrCanvasMouseSensitivity_RETN]
		}
	}

	BuiltInVariable("pref::cursorSensitivity", float, prefcursorsensitivity, 1);
	BuiltInFunction("Nova::setCursorSensitivity", _novasetmousesensitivity) {
		if (argc != 1)
		{
			Console::echo("%s( int or flt );", self);
			return 0;
		}
		float scale = stof(argv[0]);
		char* flt2hex_c = flt2hex(scale, 1);
		string buffer = hexToASCII2(flt2hex_c);
		char* hex2char_c = const_cast<char*>(buffer.c_str());

		CodePatch mouseSens = { ptrMouseSensitivity,"",hex2char_c,4,false }; mouseSens.Apply(true);
		//Console::setVariable("Opengl::scaleGUI", scale_c);
		return "true";
	}

	BuiltInVariable("$vehicle::reticleXPosition", float, clientaimreticle_X, 0);
	BuiltInVariable("$vehicle::reticleYPosition", float, clientaimreticle_Y, 0);
	void processReticleVars()
	{
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		int width = screen.x;
		int height = screen.y;
		clientaimreticle_X = floor((width / 2) - (width * (clientaimreticle_X * 0.5)));
		clientaimreticle_Y = floor((height / 2) - (height * (clientaimreticle_Y * 0.5)));
	}

	//Formula to calculate the actual vertical offset - (getwindowsize(height)/2)-(getwindowsize(height)*($vehicle::aimReticleY*0.5))
	MultiPointer(ptrAimReticleVerticalPosition, 0, 0, 0, 0x0051FF9E);
	MultiPointer(ptrAimReticleVerticalPositionResume, 0, 0, 0, 0x0051FFA4);
	CodePatch aimreticlevertical = { ptrAimReticleVerticalPosition, "", "\xE9_ARV", 5, false };
	NAKED void aimReticleVertical() {
		__asm {
			//mov dummy, 0
			//mov dummy, ecx
			mov ecx, [esp + 0x50 - 0x44] // X
			mov clientaimreticle_X, ecx
			mov ecx, [esp + 0x50 - 0x2C] // Y
			mov clientaimreticle_Y, ecx
			call processReticleVars
			//mov ecx, dummy
			mov ecx, [ebx + 0x1A8]

			jmp[ptrAimReticleVerticalPositionResume]
		}
	}

	struct Init {
		Init() {

			//Vehicle vars
			//codepatch_getkph.DoctorRelative((u32)getKPH, 1).Apply(true);
			//codepatch_getradar.DoctorRelative((u32)getRadar, 1).Apply(true);
			//codepatch_getradarrange.DoctorRelative((u32)getRadarRange, 1).Apply(true);
			//assignhudproperties.DoctorRelative((u32)assignHudProperties, 1).Apply(true);
			//codepatch_getshield.DoctorRelative((u32)getShield, 1).Apply(true);
			//codepatch_getshieldforward.DoctorRelative((u32)getShieldForward, 1).Apply(true);
			//codepatch_getshieldmodrotation.DoctorRelative((u32)getShieldModRotation, 1).Apply(true);
			//codepatch_getenergy.DoctorRelative((u32)getEnergy, 1).Apply(true);
			//codepatch_getthrottle.DoctorRelative((u32)getThrottle, 1).Apply(true);
			//codepatch_getteamyellow.DoctorRelative((u32)getTeamYellow, 1).Apply(true);
			//codepatch_getteamblue.DoctorRelative((u32)getTeamBlue, 1).Apply(true);
			//codepatch_getteamred.DoctorRelative((u32)getTeamRed, 1).Apply(true);
			//codepatch_getteampurple.DoctorRelative((u32)getTeamPurple, 1).Apply(true);
			//
			//ctreepointers.DoctorRelative((u32)cTreePointers, 1).Apply(true);
			//ctreepointers_nameless.DoctorRelative((u32)cTreePointers_nameless, 1).Apply(true);
			//clienttreewin.DoctorRelative((u32)ClientTreeWin, 1).Apply(true);
			canvasmousepatch.DoctorRelative((u32)CanvasMousePatch, 1).Apply(true);

			//dev aim-retical positions
			//aimreticlevertical.DoctorRelative((u32)aimReticleVertical, 1).Apply(true);
		}
	}init;
};