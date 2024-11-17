#include "Console.h"
#include "Patch.h"
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
#include <cmath>
#include <VersionHelpers.h>
#include <WinBase.h>

using namespace std;
using namespace Fear;
namespace NovaCore
{

	std::string getEnvVar(const char* var)
	{
		const DWORD size = GetEnvironmentVariable(var, nullptr, 0);
		std::string value(size, 0);
		if (!value.empty())
		{
			GetEnvironmentVariable(var, &value.front(), size);
			value.pop_back();
		}
		return value;
	}

	BuiltInFunction("Nova::getCompatLayer", _novagetcompatlayer)
	{
		char* compatLayer = const_cast<char*>(getEnvVar("__COMPAT_LAYER").c_str());
		return compatLayer;
	}

	BuiltInFunction("Nova::IsWindows10OrGreater", _novaiswindows10orgreater)
	{
		if (IsWindows10OrGreater())
		{
			return "true";
		}
		else
		{
			return 0;
		}
	}
	void executeNova()
	{
		Console::eval("IDSTR_MISSING_FILE_TITLE = 00131400,\"Missing File\";");
		Console::eval("IDSTR_MISSING_FILE_ERROR = 00131401,\"Unable to find '%s'.\\nIt is required for use with mem.dll\";");
		Console::eval("checkForFile(\"Nova.vol\", \"mods\\\\replacements\\\\NovaAssets.zip\");");
		Console::eval("newObject(cDel,ESCSDelegate,false,LOOPBACK,0);");
		Console::eval("newobject(NovaVol, simVolume, \"Nova.vol\");");
		Console::eval("if($cargv1 != \"-s\"){newObject(simCanvas,SimGui::Canvas,Starsiege,640,480,true,1);}");
		Console::eval("exec(\"Nova_Start.cs\");");
	}

	HWND getHWND() {
		MultiPointer(ptrHWND, 0, 0, 0x00705C5C, 0x007160CC);
		uintptr_t HWND_PTR = ptrHWND;
		int GAME_HWND = *reinterpret_cast<int*>(HWND_PTR);
		HWND SS_HWND = reinterpret_cast<HWND>(GAME_HWND);
		return SS_HWND;
	}

	//Sets the game window priority to the bottom
	BuiltInFunction("Nova::sendWindowToBack", _novasendWindowToBack)
	{
		SetWindowPos(getHWND(), HWND_BOTTOM, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
		//ShowWindow(getHWND(), SW_RESTORE);
		return 0;
	}

	BuiltInFunction("Nova::sendWindowToFront", _novasendWindowToFront)
	{
		//SetWindowPos(getHWND(), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
		SetWindowPos(getHWND(), HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
		//ShowWindow(getHWND(), SW_RESTORE);
		return 0;
	}


	//We now just use the software grid renderer
	MultiPointer(ptr_TerrainRenderOGLCheck, 0, 0, 0, 0x00583C05);
	//BuiltInFunction("Nova::terrainFix", _novaterrainfix)
	//{
	//	if (argc != 1)
	//	{
	//		Console::echo("%s(bool);", self);
	//		return 0;
	//	}
	//	std::string arg1 = argv[0];
	//	if (arg1.compare("true") == 0 || arg1.compare("1") == 0)
	//	{
	//		CodePatch patchOGL_to_Software_terrain_render = { ptr_TerrainRenderOGLCheck,"","\xEB",1,false };
	//		patchOGL_to_Software_terrain_render.Apply(true);
	//	}
	//	else
	//	{
	//		CodePatch patchOGL_to_Software_terrain_render = { ptr_TerrainRenderOGLCheck,"","\x74",1,false };
	//		patchOGL_to_Software_terrain_render.Apply(true);
	//	}
	//	return "true";
	//}

	bool is_number(const std::string& s)
	{
		std::string::const_iterator it = s.begin();
		while (it != s.end() && std::isdigit(*it)) ++it;
		return !s.empty() && it == s.end();
	}

	BuiltInFunction("cos", _cos)
	{
		if (argc != 1)
		{
			Console::echo("%s( radians );", self);
		}
		return tostring(cos(atof(argv[0])));
	}

	BuiltInFunction("sin", _sin)
	{
		if (argc != 1)
		{
			Console::echo("%s( radians );", self);
		}
		return tostring(sin(atof(argv[0])));
	}

	BuiltInFunction("tan", _tan)
	{
		if (argc != 1)
		{
			Console::echo("%s( radians );", self);
		}
		return tostring(tan(atof(argv[0])));
	}

	BuiltInFunction("round", _round)
	{
		if (argc != 1)
		{
			Console::echo("%s( float );", self);
		}
		return tostring(round(atof(argv[0])));
	}

	BuiltInFunction("fmod", _fmod)
	{
		if (argc != 2)
		{
			Console::echo("%s( float, float );", self);
		}
		return tostring(fmod(atof(argv[0]),atof(argv[1])));
	}

	BuiltInFunction("trunc", _trunc)
	{
		if (argc != 1)
		{
			Console::echo("%s( float );", self);
		}
		return tostring(trunc(atof(argv[0])));
	}

	MultiPointer(ptrExecuteConsole, 0, 0, 0x00402050, 0x00402057);
	MultiPointer(ptrExecuteConsolePop, 0, 0, 0x00402067, 0x00402075);
	MultiPointer(ptrConstructorByte, 0, 0, 0, 0x006D9058);
	CodePatch clientinitredirect = { ptrExecuteConsole, "", "\xE9GINR", 5, false };
	NAKED void ClientInitRedirect_1004r() {
		__asm {
			mov ptrConstructorByte, 1
			call executeNova
			jmp ptrExecuteConsolePop
		}
	}

	NAKED void ClientInitRedirect_1003r() {
		__asm {
			call executeNova
			jmp ptrExecuteConsolePop
		}
	}

	MultiPointer(ptrConsoleOutput, 0, 0, 0, 0x005E6AD7);

	MultiPointer(ptrSwitchToWindowed, 0, 0, 0x00578313, 0x0057B51B);
	CodePatch disableWindowed = { ptrSwitchToWindowed, "", "\xEB", 1, false };
	BuiltInFunction("Nova::disableWindowed", _novadisabledwindowed)
	{
		std::string var = Console::getVariable("pref::GWC::SIM_FS_MODE");
		if (var.compare("Upscaled") == 0)
		{
			disableWindowed.Apply(true);
		}
	}


	MultiPointer(ptrbadweapon1, 0, 0, 0x00499977, 0x0049BC83);
	MultiPointer(ptrbadweapon2, 0, 0, 0x0049998B, 0x0049BC97);
	MultiPointer(ptrbadweapon3, 0, 0, 0x004999B1, 0x0049BCBD);
	MultiPointer(ptrbadweapon4, 0, 0, 0x00499AF7, 0x0049BE03);
	MultiPointer(ptrbadweapon5, 0, 0, 0x0049679A, 0x004989EE);
	MultiPointer(ptrbadweapon6, 0, 0, 0x00499B3A, 0x0049BE46);
	CodePatch badweapon1 = { ptrbadweapon1,"","\x90\x90\x90\x90\x90\x90",6,false };
	CodePatch badweapon2 = { ptrbadweapon2,"","\x90\x90\x90\x90\x90\x90\x90",7,false };
	CodePatch badweapon3 = { ptrbadweapon3,"","\x90\x90\x90\x90\x90\x90",6,false };
	CodePatch badweapon4 = { ptrbadweapon4,"","\x90\x90\x90\x90\x90\x90\x90",7,false };
	CodePatch badweapon5 = { ptrbadweapon5,"","\x90\x90\x90\x90\x90\x90",6,false };
	CodePatch badweapon6 = { ptrbadweapon6,"","\x90\x90\x90\x90\x90\x90\x90",7,false };

	CodePatch goodweapon1 = { ptrbadweapon1,"","\x8A\x81\xAC\x00\x00\x00",6,false };
	CodePatch goodweapon2 = { ptrbadweapon2,"","\x0F\xBE\x81\xD7\x00\x00\x00",7,false };
	CodePatch goodweapon3 = { ptrbadweapon3,"","\x8B\x8A\x34\x01\x00\x00",6,false };
	CodePatch goodweapon4 = { ptrbadweapon4,"","\x0F\xBE\x86\xD4\x00\x00\x00",7,false };
	CodePatch goodweapon5 = { ptrbadweapon5,"","\x8B\x8A\xA4\x00\x00\x00",6,false };
	CodePatch goodweapon6 = { ptrbadweapon6,"","\x0F\xBE\x88\xD4\x00\x00\x00",7,false };
	void PatchBadWeaponCreation()
	{
		badweapon1.Apply(true);
		badweapon2.Apply(true);
		badweapon3.Apply(true);
		badweapon4.Apply(true);
		badweapon5.Apply(true);
		badweapon6.Apply(true);
	}

	void RestoreGoodWeaponCreation()
	{
		goodweapon1.Apply(true);
		goodweapon2.Apply(true);
		goodweapon3.Apply(true);
		goodweapon4.Apply(true);
		goodweapon5.Apply(true);
		goodweapon6.Apply(true);
	}
	///
	/// Bad Weapon Handling
	///
	MultiPointer(ptrClientBadWeapon, 0, 0, 0x00499920, 0x0049BC2C);
	MultiPointer(ptrClientBadWeaponResume, 0, 0, 0x00499926, 0x0049BC32);

	MultiPointer(ptrClientEndWeapon, 0, 0, 0x0049712D, 0x00499381);
	MultiPointer(ptrClientEndWeaponResume, 0, 0, 0x00497133, 0x00499387);
	CodePatch weaponinit = {ptrClientBadWeapon,"","\xE9WIN1",5,false};
	CodePatch weaponinitend = {ptrClientEndWeapon,"","\xE9WIN2",5,false};
	
	NAKED void WeaponInit() {
		__asm {
			cmp ecx, NULL
			je __je
			mov eax, [ecx + 0x130]
			jmp ptrClientBadWeaponResume
			__je:
				call PatchBadWeaponCreation
				jmp ptrClientBadWeaponResume
		}
	}

	NAKED void WeaponInitEnd() {
		__asm {
			cmp eax, NULL
			je __je
			mov eax, [eax + 0x0A0]
			jmp ptrClientEndWeaponResume
			__je :
				call RestoreGoodWeaponCreation
				jmp ptrClientEndWeaponResume
		}
	}
	///
	///
	///


	///
	/// Handle modded client vehicles that the server does not have
	///
	MultiPointer(ptrClientVehicleCreate, 0, 0, 0x004694C1, 0x0046AEE5);
	MultiPointer(ptrClientVehicleCreateResume, 0, 0, 0x004694D6, 0x0046AEFA);
	MultiPointer(ptrClientVehicleCreatePop, 0, 0, 0x00469608, 0x0046B02C);
	CodePatch clientvehiclecreate = { ptrClientVehicleCreate,"","\xE9_CVR",5,false };
	NAKED void ClientVehicleCreate() {
		__asm {
			cmp eax, NULL
			je __je
			fld dword ptr[eax + 0x58]
			fadd dword ptr[ebx + 0x414]
			fstp dword ptr[ebx + 0x414]
			mov eax, [ebx + 0x200]
			jmp ptrClientVehicleCreateResume
			__je:
			jmp ptrClientVehicleCreatePop
		}
	}

	MultiPointer(ptrClientVehicleDrop, 0, 0, 0x00479D72, 0x0047A4FA);
	MultiPointer(ptrClientVehicleDropResume, 0, 0, 0x00479D81, 0x0047A509);
	MultiPointer(ptrClientVehicleDropPop, 0, 0, 0x00479E95, 0x0047A61D);
	CodePatch clientvehicledrop = { ptrClientVehicleDrop,"","\xE9_CVD",5,false };
	float vehicleMassMathFloat1 = 0.050000001;
	float vehicleMassMathFloat2 = 10.0;
	NAKED void ClientVehicleDrop() {
		__asm {
			cmp eax, NULL
			je __je
			fld dword ptr[eax + 0x5C]
			fadd vehicleMassMathFloat1
			fmul vehicleMassMathFloat2
			jmp ptrClientVehicleDropResume
			__je:
			jmp ptrClientVehicleDropPop
		}
	}

	MultiPointer(ptrClientBadVehicle, 0, 0, 0x004F193D, 0x004F3DD5);
	MultiPointer(ptrClientBadVehicleResume, 0, 0, 0x004F1943, 0x004F3DDB);
	MultiPointer(ptrClientBadVehiclePop, 0, 0, 0x004F197E, 0x004F3E16);
	CodePatch clientbadvehicle = { ptrClientBadVehicle,"","\xE9_CBV",5,false };
	NAKED void ClientBadVehicle() {
		__asm {
			cmp eax, NULL
			je __je
			mov dl, [eax + 0x17C]
			jmp ptrClientBadVehicleResume
			__je :
			jmp ptrClientBadVehiclePop
		}
	}

	///
	/// Disable number of vehicles/components checks
	///
	MultiPointer(ptrNumWeaponsCheck, 0, 0, 0x0045E008, 0x0045F500);
	MultiPointer(ptrNumVehiclesCheck, 0, 0, 0x0045DFD2, 0x0045F4CA);
	MultiPointer(ptrNumComponentsCheck, 0, 0, 0x0045E03E, 0x0045F536);
	CodePatch numberweaponscheck = { ptrNumWeaponsCheck,"","\xEB",1,false };
	CodePatch numbervehiclescheck = { ptrNumVehiclesCheck,"","\xEB",1,false };
	CodePatch numbercomponentscheck = { ptrNumComponentsCheck,"","\xEB",1,false };

	struct Init {
		Init() {

			if (VersionSnoop::GetVersion() == VERSION::v001004) {
				clientinitredirect.DoctorRelative((u32)ClientInitRedirect_1004r, 1).Apply(true);
			}
			if (VersionSnoop::GetVersion() == VERSION::v001003) {
				clientinitredirect.DoctorRelative((u32)ClientInitRedirect_1003r, 1).Apply(true);
			}
			weaponinit.DoctorRelative((u32)WeaponInit, 1).Apply(true);
			weaponinitend.DoctorRelative((u32)WeaponInitEnd, 1).Apply(true);
			clientvehiclecreate.DoctorRelative((u32)ClientVehicleCreate, 1).Apply(true);
			clientvehicledrop.DoctorRelative((u32)ClientVehicleDrop, 1).Apply(true);
			clientbadvehicle.DoctorRelative((u32)ClientBadVehicle, 1).Apply(true);
			numberweaponscheck.Apply(true);
			numbervehiclescheck.Apply(true);
			numbercomponentscheck.Apply(true);
		}
	} init;
}