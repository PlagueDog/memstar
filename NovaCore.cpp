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

	struct Init {
		Init() {

			if (VersionSnoop::GetVersion() == VERSION::v001004) {
				clientinitredirect.DoctorRelative((u32)ClientInitRedirect_1004r, 1).Apply(true);
			}
			if (VersionSnoop::GetVersion() == VERSION::v001003) {
				clientinitredirect.DoctorRelative((u32)ClientInitRedirect_1003r, 1).Apply(true);
			}
		}
	} init;
}