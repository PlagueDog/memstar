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

using namespace std;
using namespace Fear;

namespace NovaCore
{
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
		ShowWindow(getHWND(), SW_RESTORE);
		return 0;
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