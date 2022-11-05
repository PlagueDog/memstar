#include "Fear.h"
#include "Filesystem.h"
#include "Console.h"
#include "Windows.h"
#include "Patch.h"
#include <stdlib.h>
//#include <cstdint>
#include <stdio.h>
#include "Strings.h"
//#include "shlwapi.h"
#include <sstream>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <dirent.h>
#include <sys/types.h>

using namespace std;
using namespace Fear;


string BEtoLE(string& str)
{
	string it;
	std::reverse(str.begin(), str.end());
	for (auto it = str.begin(); it != str.end(); it += 2) {
		std::swap(it[0], it[1]);
	}
	return it;
}

string htoa(string hex)
{
	// initialize the ASCII code string as empty.
	string ascii = "";
	for (size_t i = 0; i < hex.length(); i += 2)
	{
		// extract two characters from hex string
		string part = hex.substr(i, 2);

		// change it into base 16 and
		// typecast as the character
		char ch = stoul(part, nullptr, 16);

		// add this char to final ASCII string
		ascii += ch;
	}
	return ascii;
}

char* hex2char(char* hexString = "00")
{
	std::string hex = hexString;
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

namespace ModloaderMain {

	void toggleWindowedOpenGL(HWND hwnd, int x, int y, UINT keyFlags)
	{
		WINDOWPLACEMENT g_wpPrev = { sizeof(g_wpPrev) };
		DWORD dwStyle = GetWindowLong(hwnd, GWL_STYLE);
		if (dwStyle & WS_OVERLAPPEDWINDOW) {
			MONITORINFO mi = { sizeof(mi) };
			if (GetWindowPlacement(hwnd, &g_wpPrev) &&
				GetMonitorInfo(MonitorFromWindow(hwnd,
					MONITOR_DEFAULTTOPRIMARY), &mi)) {
				SetWindowLong(hwnd, GWL_STYLE,
					dwStyle & ~WS_OVERLAPPEDWINDOW);
				SetWindowPos(hwnd, HWND_TOP,
					mi.rcMonitor.left, mi.rcMonitor.top,
					mi.rcMonitor.right - mi.rcMonitor.left,
					mi.rcMonitor.bottom - mi.rcMonitor.top,
					SWP_NOOWNERZORDER | SWP_FRAMECHANGED);
			}
		}
		else {
			//SetWindowLong(hwnd, GWL_STYLE,
			//	dwStyle | WS_OVERLAPPEDWINDOW);
			//SetWindowPlacement(hwnd, &g_wpPrev);
			//SetWindowPos(hwnd, NULL, 0, 0, 0, 0,
			//	SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER |
			//	SWP_NOOWNERZORDER | SWP_FRAMECHANGED);
			Console::eval("OpenGL::winKeyOut();");
		}
	}

	//NYI
	//BuiltInFunction("setRadar", _sra)
	//{
		//if (argc < 1)
		//{
		//	Console::echo("setRadar( <Perspective|Scale|DiagonalOffs|HorizontalOffs|SensModeHorizOff|SensModeVertOff|AccelVertOffs|AccelHorizOffs>, int/float);");
		//	return 0;
		//}
		//std::string float_arg = argv[0];
		////If there is no float arg reset to the default value
		//float f_arg = atof(argv[1]);
		//char* f_arg_hex = flt2hex(f_arg,1);
		//char* f_arg_hString = hex2char(f_arg_hex);
		//
		//	if (float_arg.compare("Perspective") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F058,"","\x00\x00\x96\x43",4,false }; genericCodePatch0.Apply(true); return 0; }
		//else if (float_arg.compare("Perspective") == 0 && strlen(argv[1])) {CodePatch genericCodePatch0 = { 0x0074F058,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("Scale") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F05C,"","\x42\x00\x00\x00",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if(float_arg.compare("Scale") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F05C,"",i_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("DiagonalOffs") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F060,"","\x00\x00\x00\xCE",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if (float_arg.compare("DiagonalOffs") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F060,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("HorizontalOffs") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F064,"","\xE0\xFF\xFF\xFF",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if (float_arg.compare("HorizontalOffs") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F064,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("SensModeHorizOff") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F068,"","\x00\x00\x00\x5B",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if (float_arg.compare("SensModeHorizOff") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F068,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("SensModeVertOff") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F06C,"","\xF6\xFF\xFF\xFF",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if (float_arg.compare("SensModeVertOff") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F06C,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("AccelVertOffs") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F074,"","\x00\x00\x00\x1C",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if (float_arg.compare("AccelVertOffs") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F074,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}

		//if (float_arg.compare("AccelHorizOffs") == 0 && !strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F078,"","\x00\x00\x00\x23",4,false }; genericCodePatch0.Apply(true); return 0;}
		//else if (float_arg.compare("AccelHorizOffs") == 0 && strlen(argv[1])) { CodePatch genericCodePatch0 = { 0x0074F078,"",f_arg_hString,4,false }; genericCodePatch0.Apply(true); return 0;}
		//return "true";
	//}

	//BuiltInFunction("setWeaponDisplay::FontTag", _srp)
	//{
	//	if (argc == 0)
	//	{
	//		CodePatch genericCodePatch0 = { 0x00524356,"","\x47\x4A\x02",3,false }; genericCodePatch0.Apply(true);
	//		return "true";
	//	}
	//	float f_arg = atof(argv[0]);
	//	char* f_arg_hex = flt2hex(f_arg, 1);
	//	char* f_arg_hString = hex2char(f_arg_hex);
	//	CodePatch genericCodePatch0 = { 0x00524356,"",f_arg_hString,3,false }; genericCodePatch0.Apply(true);
	//	return "true";
	//}

	//BuiltInFunction("setRadar::Scale", _srs)
	//{
	//	if (argc == 0)
	//	{
	//		CodePatch genericCodePatch0 = { 0x00524356,"","\x42\x00\x00\x00",4,false }; genericCodePatch0.Apply(true);
	//		return "true";
	//	}
	//	string f_arg_hString = htoa(int2hex(stoi(argv[0])));
	//	//BEtoLE(f_arg_hString);
	//	const char* f_arg_cstr = f_arg_hString.c_str();
	//	char* f_arg_cstrr = const_cast<char*>(f_arg_cstr);
	//	CodePatch genericCodePatch0 = { 0x00524356,"",f_arg_cstrr,strlen(f_arg_cstrr),false }; genericCodePatch0.Apply(true);
	//	return "true";
	//}

	BuiltInFunction("getDirectory", _gd)
	{
		std::string path = argv[0];
		if (path.find(":") != -1)
		{
			Console::echo("Cannot escape the Starsiege directory.");
			return 0;
		}

		struct dirent* entry;
		DIR* dir = opendir(argv[0]);

		if (dir == NULL) {
			return 0;
		}

		std::stringstream fileListing;
		int counter = 0;
		while ((entry = readdir(dir)) != NULL) {
			counter++;
			char varEval[MAX_PATH];
			strcpy(varEval, "directoryFile");
			strcat(varEval, tostring(counter));
			Console::setVariable(varEval, entry->d_name);
			free(varEval);
		}
		closedir(dir);
		//const std::string tmp = fileListing.str();
		//const char* cstr = tmp.c_str();

		return 0;
	}

	BuiltInFunction("OpenGL::toggleWindowedFullscreen", _togl) {
		toggleWindowedOpenGL(FindWindowA(NULL, "Starsiege"), NULL, NULL, NULL);
		return "true";
	}

	BuiltInFunction("disableWindowBorder", _dwd) {
		LONG lStyle = GetWindowLong(FindWindowA(NULL, "Starsiege"), GWL_STYLE);
		lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
		SetWindowLong(FindWindowA(NULL, "Starsiege"), GWL_STYLE, lStyle);
		return "true";
	}

	BuiltInFunction("enableWindowBorder", _ewb) {
		LONG lStyle = GetWindowLong(FindWindowA(NULL, "Starsiege"), GWL_STYLE);
		lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
		SetWindowLong(FindWindowA(NULL, "Starsiege"), GWL_STYLE, lStyle | WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_SYSMENU);
		return "true";
	}

	BuiltInFunction("setWindowPos", _swp) {
		if (argc != 4)
		{
			Console::echo("%s( int_xPosition, int_yPosition, int_windowWidth, int_windowHeight );");
			return 0;
		}
		SetWindowPos(FindWindowA(NULL, "Starsiege"), HWND_TOPMOST, atoi(argv[0]), atoi(argv[1]), atoi(argv[2]), atoi(argv[3]), NULL);
		return "True";
	}

	BuiltInFunction("getCursorPosition", _gcp) {
		POINT p;
		GetCursorPos(&p);
		int x = p.x;
		int y = p.y;
		if (atoi(argv[0]) == 1)
		{
			return tostring(x);
		}
		else
		{
			return tostring(y);
		}
		return 0;
	}

	BuiltInFunction("getWindowPosition", _gwp) {
		RECT window;
		GetWindowRect(FindWindowA(NULL, "Starsiege"), &window);
		int x = window.left;
		int y = window.top;
		if (atoi(argv[0]) == 1)
		{
			return tostring(x);
		}
		else
		{
			return tostring(y);
		}
		return 0;
	}

	BuiltInFunction("getWindowSize", _gws) {
		RECT window;
		GetWindowRect(FindWindowA(NULL, "Starsiege"), &window);
		int width = window.right - window.left;
		int height = window.bottom - window.top;
		if (atoi(argv[0]) == 1)
		{
			return tostring(width);
		}
		else
		{
			return tostring(height);
		}
		return 0;
	}

	BuiltInFunction("int2hex", _i2h) {
		if (!strlen(argv[0]) || !atoi(argv[0]) > 0)
		{
			Console::echo("%s( integer endianness[0|1] );");
			return 0;
		}
		if (argc == 1)
		{
			return int2hex(stoi(argv[0]), 0);
		}
		if (stoi(argv[1]) == 1)
		{
			return int2hex(stoi(argv[0]), stoi(argv[1]));
		}
		return 0;
	}

	BuiltInFunction("flt2hex", _f2h) {
		if (argc != 2 || !atof(argv[0]) > 0)
		{
			Console::echo("%s( float, endianness[0|1] );");
			return 0;
		}
		if (argc == 1)
		{
			return flt2hex(stof(argv[0]), 0);
		}
		if (stoi(argv[1]) == 1)
		{
			return flt2hex(stof(argv[0]), stoi(argv[1]));
		}
		return 0;
	}

	BuiltInFunction("OpenGL::shiftGUI", _oglshgui) {
		if (argc != 1)
		{
			Console::echo("%s( int/flt ); //Additive values shift the GUI to the right and vice versa. Default: -1");
		}
		char* flt2hex_c = flt2hex(atof(argv[0]), 1);
		//Convert hex string to raw hex
		char* hex2char_c = hex2char(flt2hex_c);
		MultiPointer(ptrOGLshift, 0, 0, 0x0063D9AC, 0x0064C905);
		CodePatch genericCodePatch = { ptrOGLshift,"",hex2char_c,4,false }; genericCodePatch.Apply(true);
		return "true";
	}

	BuiltInFunction("OpenGL::offsetGUI", _oglogui) {
		if (argc != 1)
		{
			Console::echo("%s( int/flt ); //Offsets the GUI vertically. Default: 0.5");
		}
		char* flt2hex_c = flt2hex(atof(argv[0]), 1);
		//Convert hex string to raw hex
		char* hex2char_c = hex2char(flt2hex_c);
		MultiPointer(ptrOGLoffset, 0, 0, 0x0063DAFC, 0x0064CA54);
		CodePatch genericCodePatch = { ptrOGLoffset,"",hex2char_c,4,false }; genericCodePatch.Apply(true);
		return "true";
	}

	BuiltInFunction("OpenGL::scaleGUI", _oglsgui) {
		if (argc != 1)
		{
			Console::echo("%s( int/flt ); //Changes the internal GUI rendering scale. Default: 2");
		}
		float scale = atof(argv[0]);
		char* scale_c = flt2hex(scale, 1);
		char* scale_hString = hex2char(scale_c);
		MultiPointer(ptrOGLscale, 0, 0, 0x0063DAF8, 0x0064CA50);
		CodePatch GUIscalePatch = { ptrOGLscale,"",scale_hString,4,false }; GUIscalePatch.Apply(true);
		//Console::setVariable("Opengl::scaleGUI", scale_c);
		return "true";
	}

	BuiltInFunction("setCursorPos", _scp) {
		if (argc != 2)
		{
			Console::echo("%s( x, y); ");
			return "false";
		}
		Console::eval("winmouse();");
		SetCursorPos(atoi(argv[0]), atoi(argv[1]));
		Console::eval("schedule('guimouse();',0.025);");
		return "true";
	}

	//Opens the defaultPrefs.cs file the system configured text editor
	BuiltInFunction("modloader::editDefaultPrefs", _mlEditDefaultPrefs) {

		//Used to check if this function is implemented without executing the rest of the function
		if (argc >= 1)
		{
			return "true";
			exit;
		}
		ShellExecuteA(0, "edit", "defaultPrefs.cs", NULL, NULL, SW_SHOWDEFAULT);
		return "true";
	}

	//Opens the mods directory
	BuiltInFunction("modloader::openDir", _mlOpenDir) {
		if (argc >= 1)
		{
			return "true";
			exit;
		}
		ShellExecute(0, "explore", ".\\mods", NULL, NULL, SW_SHOWNORMAL);
		return "true";
	}

	//Test function to run a subroutine
	//BuiltInFunction("subroutine", _hc) {
	//	typedef int (*FunctionType)(int);
	//	//FunctionType hardcallf = (FunctionType)0x005C45CC; //0x004458A4
	//	FunctionType hardcallf = (FunctionType)0x00579950; //0x004458A4
	//	//char* arg = const_cast<char*>(argv[0]);
	//	hardcallf(atoi(argv[0]));
	//	return "true";
	//}
	// 
	 //CodePatch TEST_modloader_customTerrain_noDisconnect = {
		// 0x0057F331,
		 //"",
		 //"\x90\x90"
		 //2,
		 //false
	 //};



	////////////////////////////////////////////////////////
	// RENDERING
	////////////////////////////////////////////////////////
	//Sets the maximum size of the windowed game canvas to 1280x1024. Game does not render beyond 1280x1024 in windowed mode.
	//Allows Glide to run at 1280x1024 in windowed mode.
	//"\xC7\x02\x00\x05\x00\x00\xC7\x42\x04\x00\x04",

	MultiPointer(ptrWinMaxWinSize, 0, 0, 0x005740FF, 0x00577307);
	CodePatch canvasWindowMaxWindowedSize_patch = {
		ptrWinMaxWinSize,
		"",
		"\xC7\x02\x00\x0A\x00\x00\xC7\x42\x04\x0A\x05",
		11,
		false
	};

	MultiPointer(ptrWinMaxIntRendSizeWidth, 0, 0, 0x006487A6, 0x006584BE);
	CodePatch canvasWindowMaxInteralRenderSizeWidth_patch = {
		ptrWinMaxIntRendSizeWidth,
		"",
		"\x3D\x00\x0F\x00\x00\x7E\x06\xC7\x06\x00\x0F",
		11,
		false
	};
	MultiPointer(ptrWinMaxIntRendSizeHeight, 0, 0, 0x006487C5, 0x006584DD);
	CodePatch canvasWindowMaxInteralRenderSizeHeight_patch = {
		ptrWinMaxIntRendSizeHeight,
		"",
		"\x81\xF9\x00\x0F\x00\x00\x7E\x07\xC7\x46\x04\x00\x0F",
		13,
		false
	};
	//CodePatch gotosimres_patch = {
	//	0x006DE419,
	//	"",
	//	"\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00",
	//	13,
	//	false
	//};

	//Increase the maximum visible distance to 256000 up from 10000
	MultiPointer(ptrMaxVisDist, 0, 0, 0x006D7D43, 0x006E7F03);
	CodePatch terrainMaxVisDistance_patch = {
		ptrMaxVisDist,
		"",
		"\x4A",
		1,
		false
	};
	////////////////////////////////////////////////////////
	// GAMEPLAY
	////////////////////////////////////////////////////////
	//Increases the weapon shot cap to 12 up from 3
	MultiPointer(ptrMaxWeapShots, 0, 0, 0x00415134, 0x00417372);
	CodePatch weaponShotCap_patch = {
		ptrMaxWeapShots,
		"",
		//Dont increase the number of fires simply skip the check entirely
		//"\x83\xF9\x0C",
		"\x90\x90\x90\xEB",
		4,
		false
	};

	//For some reason Dynamix programmed the hostGUI delegate to execute datload.cs
	//Loading this twice causes issues with overlapping IDs
	MultiPointer(ptrHostDatLoad, 0, 0, 0, 0x00707471);
	CodePatch datLoad_patch = {
		ptrHostDatLoad,
		"",
		"\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00",
		19,
		false
	};
	//Allow sethudmapviewoffset(); to be used clientside
	//CodePatch clientHudviewMapOffset_patch = {
	//	0x005E6A28,
	//	"",
	//	"\x90\x90",
	//	2,
	//	false
	//};

	BuiltInFunction("modloader::patchMapview", _mlpatchMapView) {

		MultiPointer(ptrMapViewFidelity, 0, 0, 0x005155DC, 0x00517A78);
		CodePatch OverviewMapFidelity = { //Smooths the elevation rings on the "satellite map"
			ptrMapViewFidelity,
			"",
			"\x00\x00\xC0\x42\x00\x00\xB0\x3E",
			8,
			false
		};
		OverviewMapFidelity.Apply(true);
		return "true";
	}

	BuiltInFunction("fov", _fov) {
		if (argc != 1)
		{
			Console::echo("%s( 50 - 175 );", self);
			return "false";
		}
		if (atoi(argv[0]) < 50 && atoi(argv[0]) > 175)
		{
			Console::echo("%s( 50 - 175 );", self);
			return "false";
		}
		//Convert the int input to radians
		float radial = (atoi(argv[0]) * 3.14159265359 / 180) / 2;
		//Convert float to hex string
		char* flt2hex_c = flt2hex(radial, 1);
		//Convert hex string to raw hex
		char* hex2char_c = hex2char(flt2hex_c);

		//Console::echo(rawHexString);
		MultiPointer(ptrInitFov, 0, 0, 0x004689EC, 0x0046A416);
		CodePatch initialFovPatch = { ptrInitFov,"",hex2char_c,4,false }; initialFovPatch.Apply(true);
		MultiPointer(ptrZoomFov, 0, 0, 0x0046D823, 0x0046F349);
		CodePatch postZoomFovPatch = { ptrZoomFov,"",hex2char_c,4,false }; postZoomFovPatch.Apply(true);
		Console::setVariable("client::fov", argv[0]);
		Console::eval("export(\"client::*\", \"playerPrefs.cs\");");

		return "true";
	}

	BuiltInFunction("modloader::patchEvents", _mlpatchPlayerEvents) {
		MultiPointer(ptrPlayerAddEvent1, 0, 0, 0x006CBAE9, 0x006DBC01);
		CodePatch PlayerEvent1 = { ptrPlayerAddEvent1,"","MLplyr::onAdd",13,false }; PlayerEvent1.Apply(true);
		MultiPointer(ptrPlayerAddEvent2, 0, 0, 0x006CBAFA, 0x006DBC12);
		CodePatch PlayerEvent2 = { ptrPlayerAddEvent2,"","MLplyr::onAdd",13,false }; PlayerEvent2.Apply(true);
		MultiPointer(ptrPlayerRemoveEvent1, 0, 0, 0x006CBB26, 0x006DBC3E);
		CodePatch PlayerEvent3 = { ptrPlayerRemoveEvent1,"","MLplyr::onRemove",16,false }; PlayerEvent3.Apply(true);
		MultiPointer(ptrPlayerRemoveEvent2, 0, 0, 0x006CBB3A, 0x006DBC52);
		CodePatch PlayerEvent4 = { ptrPlayerRemoveEvent2,"","MLplyr::onRemove",16,false }; PlayerEvent4.Apply(true);
		MultiPointer(ptrVehAddEvent, 0, 0, 0x006D97DC, 0x006E9A22);
		CodePatch VehicleOnAdd = { ptrVehAddEvent,"","MLveh::onAdd\x00\x00",16,false }; VehicleOnAdd.Apply(true);
		return "true";
	}

	BuiltInFunction("quitGame", _quitGame) {
		exit(0);
		return "true";
	}

	//Get the executable path and file name, close the current session, open new session
	//BuiltInFunction("newSession", _newSession) {
	//
		//char szEXEPath[MAX_PATH];
		//char actualpath[MAX_PATH] ;
		//GetModuleFileName(NULL, szEXEPath, MAX_PATH);
		//for (int j = 0; szEXEPath[j] != 0; j++)
		//{
		//	actualpath[j] = szEXEPath[j];
		//}
		//Execute the exact executable
		//ShellExecuteA(0, NULL, actualpath, NULL, NULL, SW_SHOWDEFAULT);
		//Console::eval("function newSession(){}");
		//return "true";
	//}

	////////////////////////////////////////////////////////
	// NETWORKING
	////////////////////////////////////////////////////////
	 //Packet frame is how often the client sends move information to the server.
	 //Increase the packetFrame cap from 14 to 1024
	MultiPointer(ptrRemEvalBufferSen, 0, 0, 0x005A0330, 0x005A3B4C);
	CodePatch remoteEvalBufferSize_S_patch = {
		ptrRemEvalBufferSen,
		"",
		"\x81\xFE\x00\x10",
		4,
		false
	};

	MultiPointer(ptrRemEvalBufferRec, 0, 0, 0x005A00FE, 0x005A391A);
	CodePatch remoteEvalBufferSize_R_patch = {
		ptrRemEvalBufferRec,
		"",
		"\x3D\x00\x10",
		3,
		false
	};

	// Don't disconnect when server has a volume we don't have
	MultiPointer(ptrMissServVol, 0, 0, 0x0059875B, 0x0059BF77);
	CodePatch ignoreMissingServerVolume_patch = {
		ptrMissServVol,
		"",
		"\x90\x90\xEB\x18",
		4,
		false
	};

	// Ignore invalid packets. Make the client handle them instead of fleeing the server
	MultiPointer(ptrInvPacket1, 0, 0, 0x00682FF2, 0x00692EE2);
	CodePatch invalidPackets1_patch = {
		ptrInvPacket1,
		"",
		"\x90\x90\xEB",
		3,
		false
	};
	MultiPointer(ptrInvPacket2, 0, 0, 0x006809C7, 0x00690897);
	CodePatch invalidPackets2_patch = {
		ptrInvPacket2,
		"",
		"\x90\x90\xEB",
		3,
		false
	};

	// Don't disconnect when server has a terrain we don't have

	//1.004r
	MultiPointer(ptrMissServTerrain_1004, 0, 0, 0, 0x0057F349);
	CodePatch missingTerrain_patch_1004 = {
		ptrMissServTerrain_1004,
		"",
		"\x90\x90\x90\x90\x90",
		5,
		false
	};

	//1.003r
	MultiPointer(ptrMissServTerrain_1003, 0, 0, 0x0057B517, 0);
	CodePatch missingTerrain_patch_1003 = {
		ptrMissServTerrain_1004,
		"",
		"\x90\x90\x90\x90\x90\x90\x90\x90\x90\x8D\x8B\xA4\x06\x00\x00\x8B\xD0\x8B\xC3\xE8\x8F\xF2\xFF\xFF\x90\x90\x90\x90",
		28,
		false
	};

	// Don't disconnect when server has a different version of terrain
	MultiPointer(ptrCustServTerrain, 0, 0, 0, 0x0057F376);
	CodePatch modifiedTerrain_patch = {
		0x0057F376,
		"",
		"\x90\x90\x90\x90\x90",
		5,
		false
	};

	//If there is no $pref::packetRate defined the client defaults the rate to 10. Make it 255
	MultiPointer(ptrDefPacketRate, 0, 0, 0x0045F404, 0x00460A5C);
	CodePatch packetRateDefault_patch = {
		ptrDefPacketRate,
		"",
		"\xB9\xFF",
		//"\xB9\xE8\x03", //1024
		3,
		false
	};

	//The default value for the packrate field in the options gui
	MultiPointer(ptrDefPacketRateOpt, 0, 0, 0x0053B9CC, 0x0053DED8);
	CodePatch packetRateDefaultMax_patch = {
		ptrDefPacketRateOpt,
		"",
		"\x90\x90\x90\xEB\x05\xB8\xE8\x03",
		8,
		false
	};

	//Allow the packetrate field in the options to go above 15
	MultiPointer(ptrPacketRateCheck, 0, 0, 0x0053AFD0, 0x0053D4DC);
	CodePatch packetRateCheck_patch = {
		ptrPacketRateCheck,
		"",
		"\x90\x90\x90\x90\x90",
		5,
		false
	};

	//Packet size controls the approximate size of each packet.
	//Increase the packetSize cap from 200 to 1000
	MultiPointer(ptrPacketSize, 0, 0, 0x00680A90, 0x00690960);
	CodePatch packetSize_patch = {
		ptrPacketSize,
		"",
		"\xB9\xE8\x03",
		3,
		false
	};
	//Packet rate controls the number of packets per second sent from the server to the client.
	//Increase the packetRate cap from 30 to 255
	MultiPointer(ptrPacketRateCap, 0, 0, 0x00680A9E, 0x0069096E);
	CodePatch packetRate_patch = {
		ptrPacketRateCap,
		"",
		"\x83\xFB\xFF\x76\x07\xBB\xFF",
		//"\x90\x90\x90\xEB\x07\xBB\xE8\x03", //1024
		8,
		false
	};

	//Packet rate loopback. This is the serverside packetRate. Set to 1000
	MultiPointer(ptrLoopbackPacketRate, 0, 0, 0x0045F434, 0x00460A8C);
	CodePatch loopback_packetRate_patch = {
		 ptrLoopbackPacketRate,
		 "",
		 "\x89\x83\xE8\x03\x00\x00\xBA\x1A\x76\x6E\x00\xB9\xE8\x03\x00\x00",
		 16,
		 false
	};

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// INTERNAL
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//BuiltInFunction("modloader::patchConstructors", _mpc)
	//{
	//	if (argc == 0)
	//	{
	//		CodePatch constructorLockDisable = { 0x0040206C,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };
	//		constructorLockDisable.Apply(true);
	//	}
	//	else
	//	{
	//		CodePatch constructorLockDisable = { 0x0040206C,"","\xC6\x05\x58\x90\x6D\x00\x00",7,false };
	//		constructorLockDisable.Apply(true);
	//	}
	//	return 0;
	//}
	CodePatch constructorBypass01 = { 0x0040206C,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };
	CodePatch constructorBypass02 = { 0x005385A0,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };
	CodePatch constructorBypass03 = { 0x0053F4AF,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };

	MultiPointer(ptrTagDictLoaded, 0, 0, 0x004B408F, 0x004B6473);
	CodePatch tagDictionary_patch = { //Disable tagDictionary load check
		ptrTagDictLoaded,
		"",
		"\x90\x90\x90\x90",
		4,
		false
	};
	//Allow overwriting of tag data
	MultiPointer(ptrTagDefBypass1, 0, 0, 0x005DD386, 0x005E0C2A);
	CodePatch tagDefinitionBypass1_patch = {
		ptrTagDefBypass1,
		"",
		"\x90\x90\xEB\x1C",
		4,
		false
	};
	MultiPointer(ptrTagDefBypass2, 0, 0, 0x005DD3B2, 0x005E0C56);
	CodePatch tagDefinitionBypass2_patch = {
		ptrTagDefBypass2,
		"",
		"\x90\x90\xEB\x20",
		4,
		false
	};

	//Fixes a bug where the Landscape Editors external GUI always fails to detect the simcanvas
	MultiPointer(ptrLseExtGui, 0, 0, 0x00604A4F, 0x00611A2D);
	CodePatch lse_canvas_bug_patch = {
	ptrLseExtGui,
	"",
	"\x90\x90\xEB\x20",
	4,
	false
	};

	//Function/subroutine sub-directory file creation fix
	MultiPointer(ptrSubDirFileWrite1, 0, 0, 0x0056D394, 0x0057059C);
	CodePatch f_cr_sub0 = { ptrSubDirFileWrite1,"","\x90\x90\xEB\x25",4,false };
	MultiPointer(ptrSubDirFileWrite2, 0, 0, 0x0056D762, 0x0057096A);
	CodePatch f_cr_sub1 = { ptrSubDirFileWrite2,"","\x90\x90\xEB\x25",4,false };
	MultiPointer(ptrSubDirFileWrite3, 0, 0, 0x00570EE1, 0x005740E9);
	CodePatch f_cr_sub2 = { ptrSubDirFileWrite3,"","\x90\x90\xEB\x25",4,false };

	///
	/// Windowed Fullscreen OpenGL
	//BuiltInVariable("pref::windowedFullscreenOpenGL", bool, prefwindowedFullscreenOpenGL, false);

	//BuiltInFunction("setChatboxSize", _scbs)
	//{
	//	if (argc != 2 || atoi(argv[0]) <= 0 || atoi(argv[1]) <= 0)
	//	{
	//		Console::echo("setChatboxSize( width, height);");
	//		return 0;
	//	}
	//	char* width_int2hex_c = int2hex(atoi(argv[0]), 1);
	//	char* width_hex2char_c = hex2char(width_int2hex_c);
	//	int width_byteLength = strlen(width_hex2char_c) + 1;
	//
	//	char* height_int2hex_c = int2hex(atoi(argv[1]), 1);
	//	char* height_hex2char_c = hex2char(height_int2hex_c);
	//	int height_byteLength = strlen(height_hex2char_c) + 1;
	//	CodePatch genericCodePatch1 = { 0x0052905E,"",width_hex2char_c,width_byteLength,false };genericCodePatch1.Apply(true);
	//	CodePatch genericCodePatch2 = { 0x00529065,"",height_hex2char_c,height_byteLength,false };genericCodePatch2.Apply(true);
	//
	//	CodePatch genericCodePatch3 = { 0x00529071,"",width_hex2char_c,width_byteLength,false }; genericCodePatch3.Apply(true);
	//	CodePatch genericCodePatch4 = { 0x00529078,"",height_hex2char_c,height_byteLength,false }; genericCodePatch4.Apply(true);
	//
	//	CodePatch genericCodePatch5 = { 0x00529084,"",width_hex2char_c,width_byteLength,false }; genericCodePatch5.Apply(true);
	//	CodePatch genericCodePatch6 = { 0x0052908B,"",height_hex2char_c,height_byteLength,false }; genericCodePatch6.Apply(true);
	//	Console::eval("setHudChatDisplayType(2);setHudChatDisplayType(1);");
	//	return "true";
	//}

	BuiltInFunction("OpenGL::windowedFullscreen", _oglwf)
	{
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		HWND windowHandle = FindWindowA(NULL, "Starsiege");
		MultiPointer(ptrOGLFullScreen1, 0, 0, 0x0063CE87, 0x0064BDC8);
		MultiPointer(ptrOGLFullScreen2, 0, 0, 0x0063CEE9, 0x0064BE2A);
		if (argv[0] == "false")
		{
			CodePatch genericCodePatch = { ptrOGLFullScreen1,"","\xF0",1,false };
			genericCodePatch.Apply(true);
			CodePatch genericCodePatch0 = { ptrOGLFullScreen2,"","\x04",1,false };
			genericCodePatch0.Apply(true);
		}
		else
		{
			CodePatch genericCodePatch = { ptrOGLFullScreen1,"","\x00",1,false };
			genericCodePatch.Apply(true);
			CodePatch genericCodePatch0 = { ptrOGLFullScreen2,"","\x0A",1,false };
			genericCodePatch0.Apply(true);
			SetWindowPos(windowHandle, HWND_TOP, 0, 0, screen.x, screen.y, 0);
		}
		return "true";
	}

	///
	struct Init {
		Init() {
			//Internal
			if(VersionSnoop::GetVersion() == VERSION::v001004)
			{
				constructorBypass01.Apply(true);
				constructorBypass02.Apply(true);
				constructorBypass03.Apply(true);
			}
			tagDictionary_patch.Apply(true);
			tagDefinitionBypass1_patch.Apply(true);
			tagDefinitionBypass2_patch.Apply(true);
			f_cr_sub0.Apply(true);
			f_cr_sub1.Apply(true);
			f_cr_sub2.Apply(true);

			//Rendering
			terrainMaxVisDistance_patch.Apply(true);
			canvasWindowMaxWindowedSize_patch.Apply(true);
			canvasWindowMaxInteralRenderSizeWidth_patch.Apply(true);
			canvasWindowMaxInteralRenderSizeHeight_patch.Apply(true);

			//Networking
			remoteEvalBufferSize_S_patch.Apply(true);
			remoteEvalBufferSize_R_patch.Apply(true);
			packetRate_patch.Apply(true);
			packetRateCheck_patch.Apply(true);
			packetRateDefault_patch.Apply(true);
			packetRateDefaultMax_patch.Apply(true);
			loopback_packetRate_patch.Apply(true);
			packetSize_patch.Apply(true);
			ignoreMissingServerVolume_patch.Apply(true);
			invalidPackets1_patch.Apply(true);
			invalidPackets2_patch.Apply(true);

			if (VersionSnoop::GetVersion() == VERSION::v001004)
			{
				missingTerrain_patch_1004.Apply(true);
				modifiedTerrain_patch.Apply(true);
			}

			if (VersionSnoop::GetVersion() == VERSION::v001003)
			{
				missingTerrain_patch_1003.Apply(true);
			}

			//Gameplay
			weaponShotCap_patch.Apply(true);

			if (VersionSnoop::GetVersion() == VERSION::v001004)
			{
				datLoad_patch.Apply(true);
			}

			//Directory Creation
			CreateDirectory(".\\mods", NULL);
			CreateDirectory(".\\mods\\fonts", NULL);
			CreateDirectory(".\\mods\\replacements", NULL);
			CreateDirectory(".\\mods\\ScriptGL", NULL);
			CreateDirectory(".\\mods\\cache", NULL);
			CreateDirectory(".\\mods\\local", NULL);
		}
	} init;
};