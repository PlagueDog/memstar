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
#include <cassert>
#include <cmath>

MultiPointer(ptrConsole, 0, 0, 0, 0x00722FA4);

using namespace std;
using namespace Fear;
namespace NovaCore
{
	BuiltInVariable("pref::novaNotifyText", bool, prefnovanotifytext, true);
	//Big endian to little endian
	string BEtoLE(string& str)
	{
		string it;
		std::reverse(str.begin(), str.end());
		for (auto it = str.begin(); it != str.end(); it += 2) {
			std::swap(it[0], it[1]);
		}
		return it;
	}

	bool is_number(const std::string& s)
	{
		std::string::const_iterator it = s.begin();
		while (it != s.end() && std::isdigit(*it)) ++it;
		return !s.empty() && it == s.end();
	}

	char* hexToASCII3(char* input)
	{
		// initialize the ASCII code string as empty.
		char buffer[MAX_PATH];
		strcpy(buffer, "00");
		strcat(buffer, input);
		string hex = buffer;
		string ascii = "";
		for (size_t i = 0; i < hex.length(); i += 2)
		{
			// extract two characters from hex string
			string part = hex.substr(i, 2);

			// change it into base 16 and
			// typecast as the character
			char ch = stoul(part, nullptr, 16);

			// add this char to final ASCII string
			ascii += part;
		}
		char* ascii_cstr = const_cast<char*>(ascii.c_str());
		//char* ascii_cstr_ = const_cast<char*>(ascii_cstr);
		free(buffer);
		return ascii_cstr;
	}

	string hexToASCII(string input)
	{
		int length = input.length();
		string result;
		for (int i = 0; i < length; i += 2)
		{
			string byte = input.substr(i, 2);
			char chr = (char)(int)strtol(byte.c_str(), NULL, 16);
			result.push_back(chr);
		}
		char* ascii = const_cast<char*>(result.c_str());
		return ascii;
	}

	string hexToASCII2(std::string hex) {
		stringstream ss;
		for (size_t i = 0; i < hex.length(); i += 2) {
			unsigned char byte = stoi(hex.substr(i, 2), nullptr, 16);
			ss << byte;
		}
		return ss.str();
	}

	char* hex2char(char* hexString = "00")
	{
		const std::string hex = hexString;
		std::basic_string<uint8_t> bytes;
		for (size_t i = 0; i < hex.length(); i += 2)
		{
			uint16_t byte;
			std::string nextbyte = hex.substr(i, 2);
			std::istringstream(nextbyte) >> std::hex >> byte;
			bytes.push_back(static_cast<uint8_t>(byte));
		}
		std::string result(begin(bytes), end(bytes));
		//Escaped hex strings in local variables dont parse correctly with CodePatch so they are passed pre-parsed
		char* rawHexString = const_cast<char*>(result.c_str());
		return rawHexString;
	}

	char* int2hex(int input = 0, int endian = 0)
	{
		char hex_string[MAX_PATH];
		char bit[2] = "0";
		sprintf(hex_string, "%X", input);
		if (strlen(hex_string) % 2 != 0)
		{
			if (endian == 1)
			{
				std::string final_input = strcat(bit, hex_string);
				const char* result = BEtoLE(final_input).c_str();
				char* output = const_cast<char*>(result);
				return(output);
			}
		}
		return hex_string;
	}

	char* flt2hex(float input = 0.00, int type = 0)
	{
		const unsigned char* pf = reinterpret_cast<const unsigned char*>(&input);

		char hexString[MAX_PATH];
		if (type == 0)
		{
			//Reverse
			strcpy(hexString, int2hex(pf[3], 1));
			strcat(hexString, int2hex(pf[2], 1));
			strcat(hexString, int2hex(pf[1], 1));
			strcat(hexString, int2hex(pf[0], 1));
		}
		else
		{
			strcpy(hexString, int2hex(pf[0], 1));
			strcat(hexString, int2hex(pf[1], 1));
			strcat(hexString, int2hex(pf[2], 1));
			strcat(hexString, int2hex(pf[3], 1));
		}
		return hexString;
	}

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

	BuiltInFunction("Nova::IsWindowsXP", _novaiswindowsxp)
	{
    DWORD dwVersion = 0; 
    DWORD dwMajorVersion = 0;
    DWORD dwMinorVersion = 0; 
    DWORD dwBuild = 0;

    dwVersion = GetVersion();
 
    // Get the Windows version.

    dwMajorVersion = (DWORD)(LOBYTE(LOWORD(dwVersion)));
    dwMinorVersion = (DWORD)(HIBYTE(LOWORD(dwVersion)));

    // Get the build number.

    if (dwVersion < 0x80000000)              
        dwBuild = (DWORD)(HIWORD(dwVersion));

		std::string majorVersion = tostring(dwMajorVersion);
		if (majorVersion.compare("5") == 0)
		{
			return "true";
		}
		else
		{
			return 0;
		}
	}

	MultiPointer(ptrRVectorAllocSize1, 0, 0, 0, 0x0056F0FB);
	MultiPointer(ptrRVectorAllocSize2, 0, 0, 0, 0x0056F161);
	CodePatch RVectorAllocSize1 = { ptrRVectorAllocSize1, "", "\xFF\xFF\xFF\xFF", 4, false };
	CodePatch RVectorAllocSize2 = { ptrRVectorAllocSize2, "", "\xFF\xFF\xFF\xFF", 4, false };
	MultiPointer(ptrBitmapDatabasePad, 0, 0, 0, 0x006679C5);
	CodePatch SoftwareMode_BitmapDatabasePadSize = { ptrBitmapDatabasePad, "", "\xFA\x00\x00", 3, false };
	MultiPointer(ptrBitmapDataSize, 0, 0, 0, 0x006679D6);
	CodePatch SoftwareMode_BitmapDataSize = { ptrBitmapDataSize, "", "\x7D\x00\x00", 3, false };

	void executeNova()
	{
		Console::eval("IDSTR_MISSING_FILE_TITLE = 00131400,\"Missing File\";");
		Console::eval("IDSTR_MISSING_FILE_ERROR = 00131401,\"Unable to find '%s'.\\nIt is required for use with mem.dll\";");
		Console::eval("if($cargv1 != \"-s\"){checkForFile(\"Nova.vol\", \"mods\\\\replacements\\\\NovaAssets.zip\", \"mods\\\\ScriptGL\\\\NovaScriptGLassets.zip\");}");
		Console::eval("newObject(cDel,ESCSDelegate,false,LOOPBACK,0);");
		Console::eval("newobject(NovaVol, simVolume, \"Nova.vol\");");
		Console::eval("exec(\"Nova_Core.cs\");");
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

	BuiltInFunction("pow", _pow)
	{
		if (argc != 2)
		{
			Console::echo("%s( input, power );", self);
		}
		return tostring(pow(stod(argv[0]), stod(argv[1])));
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

	BuiltInFunction("isFunction", _novaisfunction)
	{
		if (argc != 1 || argv[0] == NULL)
		{
			Console::echo("%s( functionName );", self);
			return false;
		}
		if (Console::functionExists(argv[0]))
		{
			return "true";
		}
		return false;
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

	///
	/// Class Type Modifications
	///
	/// Simgui::Slider
	MultiPointer(ptrSimguiSliderColor, 0, 0, 0x005DC003, 0x005DF8A7);
	CodePatch simguislidercolor = { ptrSimguiSliderColor,"","\xFF",1,false };

	MultiPointer(ptrSimguiSlider_MinMax, 0, 0, 0x005DBB50, 0x005DF3F4);
	MultiPointer(ptrSimguiSlider_Resume, 0, 0, 0x005DBB62, 0x005DF406);
	CodePatch simguisliderminmax = { ptrSimguiSlider_MinMax,"","\xE9SLMM",5,false };
	float slider_min = 0;
	float slider_max = 1;
	NAKED void SimGuiSliderMinMax() {
		__asm {
			mov edx, slider_min
			mov [ebx + 0x1BC], edx
			mov edx, slider_max
			mov dword ptr[ebx + 0x1C0], edx
			jmp ptrSimguiSlider_Resume
		}
	}

	void setSimGuiSliderMinMax(float min, float max)
	{
		slider_min = min;
		slider_max = max;
		simguisliderminmax.DoctorRelative((u32)SimGuiSliderMinMax, 1).Apply(true);
	}

	BuiltInFunction("SimGui::Slider::SetMinMax", _simguislidersetminmax)
	{
		if(argc != 2)
		{
			Console::echo("%s( min, max );", self);
			return 0;
		}
		if (!is_number(argv[0]) || !is_number(argv[1]))
		{
			Console::echo("%s: Numeric inputs only", self);
			return 0;
		}
		if (argv[0] >= argv[1])
		{
			Console::echo("%s: Min cannot be greater than Max", self);
			return 0;
		}
		setSimGuiSliderMinMax(stof(argv[0]), stof(argv[1]));
	}

	/// Simgui::   Default font tags
	MultiPointer(ptrSimguiDefaultTextFont, 0, 0, 0x005CF677, 0x005D2F1B);
	MultiPointer(ptrSimguiDefaultTextHLFont, 0, 0, 0x005CF681, 0x005D2F25);
	MultiPointer(ptrSimguiDefaultTextDFont, 0, 0, 0x005CF68B, 0x005D2F2F);
	MultiPointer(ptrSimguiDefaultTextString, 0, 0, 0x005CF695, 0x005D2F39);
	CodePatch defaultStringTag = { ptrSimguiDefaultTextString,"","\x00\x00\x00\x00",4,false };

	//Skip the version hand shake with the server.
	//IT IS WHAT BROKE RECORDING IN 1.004r//
	CodePatch _1004VersionHandshake = { 0x00460E26,"\x7C","\xEB",1,false };
	CodePatch _1004JoinVersionHandshake = { 0x0045FEBE,"\x8B\x4C\x24\x04","\xEB\x2F\x90\x90",4,false };
	BuiltInFunction("Nova::toggleRecordingFix", _simguitogglerecordingfix)
	{
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			std::string var = Console::getVariable("pref::fixRecording");
			if (var.compare("0") == 0 || var.compare("true") == 0 || var.compare("True") == 0)
			{
				_1004VersionHandshake.Apply(false);
				_1004JoinVersionHandshake.Apply(false);
			}
			else
			{
				_1004VersionHandshake.Apply(true);
				_1004JoinVersionHandshake.Apply(true);
			}
			return "true";
		}
	}

	BuiltInFunction("SimGui::setFontTags", _simguisetfonttags)
	{
		if (argc != 3)
		{
			Console::echo("%s( font_tag );", self);
			return 0;
		}
		string font = argv[0];
		string buffer0 = hexToASCII2(int2hex(atoi(argv[0]), 1));
		char* result0 = const_cast<char*>(buffer0.c_str());
		CodePatch SimguiSimpleTextFont = { ptrSimguiDefaultTextFont,"",result0,4,false };
		SimguiSimpleTextFont.Apply(true);

		string hlfont = argv[1];
		string buffer1 = hexToASCII2(int2hex(atoi(argv[1]), 1));
		char* result1 = const_cast<char*>(buffer1.c_str());
		CodePatch SimguiSimpleTextHLFont = { ptrSimguiDefaultTextHLFont,"",result1,4,false };
		SimguiSimpleTextHLFont.Apply(true);

		string dfont = argv[2];
		string buffer2 = hexToASCII2(int2hex(atoi(argv[2]), 1));
		char* result2 = const_cast<char*>(buffer2.c_str());
		CodePatch SimguiSimpleTextDFont = { ptrSimguiDefaultTextDFont,"",result2,4,false };
		SimguiSimpleTextDFont.Apply(true);

		return "true";
	}


	BuiltInVariable("pref::disableMasterServerMOTD", bool, prefdisablemasterservermotd, 0);
	CodePatch messageboxpatch = { 0x00420792,"\x0F\x8F\x85\x00\x00\x00", "\x90\x90\x90\x90\x90\x90",6,false };
	BuiltInFunction("Nova::disableMasterServerMOTD", _simguidisablemasterservermotd)
	{
		std::string var = argv[0];
		if (var.compare("1") == 0 || var.compare("true") == 0 || var.compare("True") == 0)
		{
			messageboxpatch.Apply(true);
		}
		else
		{
			messageboxpatch.Apply(false);
		}
		return "true";
	}

	MultiPointer(ptrHudDLGChatboxWidth, 0, 0, 0, 0x0052CB5E);
	CodePatch huddlgchatboxwidth = { ptrHudDLGChatboxWidth,"","\x76\x02",2,false };

	void SupportEnd1003() {
		int choice = MessageBoxW(NULL, L"Nova no longer supports v1.003r. Use the v1.004r executable instead.", L"v1.003r Support", MB_OKCANCEL | MB_ICONSTOP | MB_SETFOREGROUND);
		if (choice) {
			exit(EXIT_SUCCESS);
		}
	}

	BuiltInVariable("nova::bayesLoadedAlready", bool, novabayesloadedalready, 0);
	MultiPointer(ptrUnk_Bayes, 0, 0, 0, 0x00445E8A);
	MultiPointer(ptrUnk_BayesResume, 0, 0, 0, 0x00445E92);
	MultiPointer(ptrInitBayesEdit, 0, 0, 0, 0x00445EC4);
	CodePatch bayesInitPatch = { 0x00445FD3,"","\x90\x90\x90\x90\x90\x90",6,false };
	CodePatch manualbayesedit = { ptrUnk_Bayes,"","\xE9MBAYE",5,false };
	int bayesEditLoadBool = 0;
	int bayesEditLoadedOnce = 0;
	NAKED void manualBayesEdit() {
		__asm {
			cmp bayesEditLoadBool, 1
			je __loadBaysEdit
			sub ecx, 0x401
			jz __loadBaysEdit
			jmp ptrUnk_BayesResume

			__loadBaysEdit:
			mov bayesEditLoadBool, 0
			call ptrInitBayesEdit
			mov novabayesloadedalready, 1
			retn
		}
	}

	BuiltInFunction("Nova::loadBayesEditor", _novaloadbayeseditor)
	{
		bayesEditLoadBool = 1;
		Console::eval("newObject(BayesInitObject, Turret, 1);");
		Console::eval("schedule('deleteObject(BayesInitObject);',0.1);");
		return "true";
	}

	struct Init {
		Init() {

			if (VersionSnoop::GetVersion() == VERSION::v001004) {
				clientinitredirect.DoctorRelative((u32)ClientInitRedirect_1004r, 1).Apply(true);
			}
			if (VersionSnoop::GetVersion() == VERSION::v001003) {
				SupportEnd1003();
			}
			weaponinit.DoctorRelative((u32)WeaponInit, 1).Apply(true);
			weaponinitend.DoctorRelative((u32)WeaponInitEnd, 1).Apply(true);
			clientvehiclecreate.DoctorRelative((u32)ClientVehicleCreate, 1).Apply(true);
			clientvehicledrop.DoctorRelative((u32)ClientVehicleDrop, 1).Apply(true);
			clientbadvehicle.DoctorRelative((u32)ClientBadVehicle, 1).Apply(true);
			numberweaponscheck.Apply(true);
			numbervehiclescheck.Apply(true);
			numbercomponentscheck.Apply(true);

			//Simgui patches
			simguislidercolor.Apply(true);
			//defaultStringTag.Apply(true);

			//Expand the width of the chat input box
			huddlgchatboxwidth.Apply(true);
			//Message Box Creation
			//messageboxpatch.DoctorRelative((u32)messageBoxPatch, 1).Apply(true);

			//RVectorRead/Write size
			//RVectorAllocSize1.Apply(true);
			//RVectorAllocSize2.Apply(true);
			SoftwareMode_BitmapDatabasePadSize.Apply(true);
			SoftwareMode_BitmapDataSize.Apply(true);

			//Manual triggering of the Bayesian Network Editor (AI)
			manualbayesedit.DoctorRelative((u32)manualBayesEdit, 1).Apply(true);
			bayesInitPatch.Apply(true);
		}
	} init;
}