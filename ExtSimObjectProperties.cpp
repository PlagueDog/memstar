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
	MultiPointer(ptrFloatTOInt, 0, 0, 0, 0x00693F08);
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
	char* kphValue;
	static const char* kphFormat = "%0.0fKph";
	NAKED void getKPH() {
		__asm {
			push [kphFormat]
			push eax
			mov kphValue, eax;
			jmp [ptrKPHstrResume]
		}
	}

	BuiltInFunction("getKPH", _getKPH)
	{
		if (strlen(kphValue))
		{
			return kphValue;
		}
		return 0;
	}

	//MultiPointer(ptrgetradar, 0, 0, 0, 0x0051C315);
	//MultiPointer(ptrRadarResume, 0, 0, 0, 0x0051C31E);
	//CodePatch codepatch_getradar = { ptrgetradar, "", "\xE9GRAD", 5, false };
	//char* radarValue;
	//NAKED void getRadar() {
	//	__asm {
	//		mov radarValue, ecx
	//		add esp, 0x14
	//		mov eax, [ebx + 0x1CC]
	//		jmp[ptrRadarResume]
	//	}
	//}

	//BuiltInFunction("getRadar", _getRadar)
	//{
	//	if (strlen(radarValue))
	//	{
	//		Console::echo(radarValue);
	//	}
	//	return 0;
	//}

	MultiPointer(ptrgetenergy, 0, 0, 0, 0x0051B127);
	MultiPointer(ptrEnergyResume, 0, 0, 0, 0x0051B12E);
	CodePatch codepatch_getenergy = { ptrgetenergy, "", "\xE9GENG", 5, false };
	int *energyValue;
	NAKED void getEnergy() {
		__asm {
			lea edx, [ebp - 0x130 ]
			push edx
			mov dword ptr energyValue, edx
			jmp [ptrEnergyResume]
		}
	}

	//WORK IN PROGRESS (I WANT TO SCREAM)
	MultiPointer(ptrEnergy, 0, 0, 0, 0x00750EF0);
	BuiltInFunction("getEnergy", _getEnergy)
	{
	//int ptr1_data = *reinterpret_cast<int*>(&energyValue_hex);
	//char* ptr1_string = reinterpret_cast<char*>(*&energyValue_hex);
		if (energyValue)
		{
			echo("%s", energyValue);
			return 0;
		}
		return 0;
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

	u32 ptrMouseSensitivity;
	NAKED void CanvasMousePatch() {
		__asm {
			mov dword ptr [ebx + 0x25C], 0x3F800000
			push ebx
			//mov ecx, [ebx + 0x25C]
			mov ebx, [ebx + 0x25C]
			mov dword ptr ptrMouseSensitivity, ebx
			pop ebx
			jmp [ptrCanvasMouseSensitivity_RETN]
		}
	}

	//WORK IN PROGRESS
	//BuiltInFunction("Nova::setMouseSensitivity", _Nova_setMouseSensitivity) {
		//float sens = atof(argv[0]);
		//char* sens_c = flt2hex(sens, 1);
		//char* sens_hString = hex2char(sens_c);
		//char* ptr = reinterpret_cast<char*>(ptrMouseSensitivity);
		//Console::echo(ptr);
		//CodePatch GUIscalePatch = { ptrMouseSensitivity,"",sens_hString,4,false }; GUIscalePatch.Apply(true);
		//return 0;
	//}

	struct Init {
		Init() {
			codepatch_getkph.DoctorRelative((u32)getKPH, 1).Apply(true);
			//codepatch_getradar.DoctorRelative((u32)getRadar, 1).Apply(true);
			//codepatch_getenergy.DoctorRelative((u32)getEnergy, 1).Apply(true);
			ctreepointers.DoctorRelative((u32)cTreePointers, 1).Apply(true);
			ctreepointers_nameless.DoctorRelative((u32)cTreePointers_nameless, 1).Apply(true);
			clienttreewin.DoctorRelative((u32)ClientTreeWin, 1).Apply(true);
			//canvasmousepatch.DoctorRelative((u32)CanvasMousePatch, 1).Apply(true);
		}
	}init;
};