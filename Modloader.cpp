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

MultiPointer(ptrMemLibModule, 0, 0, 0, 0x0044ABFC);
namespace modloaderFunctions
{
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

//OpenGL
MultiPointer(ptrOGLWidthMin, 0, 0, 0x0063C80C, 0x0064B74C);
MultiPointer(ptrOGLWidthMax, 0, 0, 0x0063C817, 0x0064B757);
MultiPointer(ptrOGLHeightMin, 0, 0, 0x0063C827, 0x0064B767);
MultiPointer(ptrOGLHeightMax, 0, 0, 0x0063C837, 0x0064B777);
void OpenGLenumDesktopModes()
{
	RECT desktop;
	const HWND hDesktop = GetDesktopWindow();
	GetWindowRect(hDesktop, &desktop);
	int width = desktop.right;
	int height = desktop.bottom;

	string OGLwidthBuffer = hexToASCII2(int2hex(width, 1));
	char* OGLwidthResult = const_cast<char*>(OGLwidthBuffer.c_str());

	CodePatch OpenGLWidthMax = { ptrOGLWidthMax,"",OGLwidthResult,4,false };

	string OGLheightBuffer = hexToASCII2(int2hex(height, 1));
	char* OGLheightResult = const_cast<char*>(OGLheightBuffer.c_str());
	CodePatch OpenGLHeightMax = { ptrOGLHeightMax,"",OGLheightResult,4,false };

	OpenGLWidthMax.Apply(true);
	OpenGLHeightMax.Apply(true);
}

//MultiPointer(ptrBayOpenTestBit, 0, 0, 0, 0x00445FD4);
//void patchBackBayEdit()
//{
//	CodePatch BayTestBit2 = { ptrBayOpenTestBit,"","\x83",1,false };
//	BayTestBit2.Apply(true);
//	Console::eval("deleteobject(bayObject);");
//}
//
//MultiPointer(ptrBayEditInit, 0, 0, 0, 0x00445FD9);
//MultiPointer(ptrBayEditInitResume, 0, 0, 0, 0x00445FE6);
//CodePatch bayeditjnb = { ptrBayEditInit,"","\xE9PBAY",5,false};
//NAKED void BayEditJNB() {
//	__asm {
//		push 0x190
//		call [ptrMemLibModule]
//		pop ecx
//		call [patchBackBayEdit]
//		jmp [ptrBayEditInitResume]
//	}
//}

//BuiltInFunction("Nova::EditAI", _novaeditai)
//{
//	Console::eval("newobject(bayObject,turret,1);");
//	CodePatch BayTestBit = { ptrBayOpenTestBit,"","\x84",1,false };
//	BayTestBit.Apply(true);
//	bayeditjnb.DoctorRelative((u32)BayEditJNB, 1).Apply(true);
//	return "true";
//}

MultiPointer(ptrGuiBitmapCtrlImageTag, 0, 0, 0x005CED21, 0x005D25C5);
MultiPointer(ptrGuiBitmapCtrlImageTagResume, 0, 0, 0x005CED2B, 0x005D25CF);
CodePatch simgui_guibitmapctrl_defaultimage = { ptrGuiBitmapCtrlImageTag,"","\xE9SGDI",5,false };
BuiltInVariable("engine::GuiBitmapCtrl::DefaultImage", int, engine_GuiBitmapCtrl_DefaultImage, 169998);
NAKED void Simgui_GuiBitmapCtrl_DefaultImage() {
	__asm {
		mov ecx, engine_GuiBitmapCtrl_DefaultImage
		mov dword ptr[ebx + 0x1C0], ecx
		xor ecx, ecx
		jmp ptrGuiBitmapCtrlImageTagResume
	}
}

//BuiltInFunction("Simgui::GuiBitmapCtrl::SetDefaultImage", _SGSDI)
//{
//	return 0;
//	if (argc != 1)
//	{
//		Console::echo("MAIN_MENU, SP_MAIN, MFD, LOADING");
//	}
//	string background = argv[0];
//	if (background.compare("MAIN_MENU") == 0)
//	{
//		CodePatch genericCodePatch = { ptrGuiBitmapCtrlImageTag,"","\x1D\x71",2,false }; genericCodePatch.Apply(true);
//	}
//	if (background.compare("SP_MAIN") == 0)
//	{
//		CodePatch genericCodePatch = { ptrGuiBitmapCtrlImageTag,"","\x38\x71",2,false }; genericCodePatch.Apply(true);
//	}
//	if (background.compare("MFD") == 0)
//	{
//		CodePatch genericCodePatch = { ptrGuiBitmapCtrlImageTag,"","\x15\x71",2,false }; genericCodePatch.Apply(true);
//	}
//	if (background.compare("LOADING") == 0)
//	{
//		CodePatch genericCodePatch = { ptrGuiBitmapCtrlImageTag,"","\x2C\x71",2,false }; genericCodePatch.Apply(true);
//	}
//
//	return "true";
//}

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
			//Console::eval("OpenGL::winKeyOut();");
		}
	}
}
	MultiPointer(ptrSplash480, 0, 0, 0x0063C5CE, 0x0064B50E);
	MultiPointer(ptrSplash640, 0, 0, 0x0063C5C5, 0x0064B505);

	MultiPointer(ptrExtSplash640_1, 0, 0, 0x006485AE, 0x006582C6);
	MultiPointer(ptrExtSplash640_2, 0, 0, 0x006485CA, 0x006582E2);

	MultiPointer(ptrExtSplash480_1, 0, 0, 0x006485B5, 0x006582CD);
	MultiPointer(ptrExtSplash480_2, 0, 0, 0x006485D1, 0x006582E9);

	BuiltInFunction("setSplash640x480", _ss640x480)
	{
		if (argc != 2 || atoi(argv[1]) < 1 )
		{
			Console::echo("%s( width/height, int);", self);
			return 0;
		}
		string type = argv[0];
		//string hex = int2hex(atoi(argv[1]), 1);
		//if (type.compare("width") != 0 || type.compare("height") != 0)
		//{
		//	Console::echo("%s ( width/height, int);");
		//	return 0;
		//}
		string buffer = hexToASCII2(int2hex(atoi(argv[1]),1));
		char* result = const_cast<char*>(buffer.c_str());
		if (type.compare("width") == 0)
		{
			CodePatch goSplash640 = { ptrSplash640, "", result, 2, false };goSplash640.Apply(true);
			CodePatch goExtSplash640_1 = { ptrExtSplash640_1, "", result, 2, false };goExtSplash640_1.Apply(true);
			CodePatch goExtSplash640_2 = { ptrExtSplash640_2, "", result, 2, false };goExtSplash640_2.Apply(true);
		}
		else if (type.compare("height") == 0)
		{
			CodePatch goSplash480 = { ptrSplash480, "", result, 2, false };goSplash480.Apply(true);
			CodePatch goExtSplash480_1 = { ptrExtSplash480_1, "", result, 2, false }; goExtSplash480_1.Apply(true);
			CodePatch goExtSplash480_2 = { ptrExtSplash480_2, "", result, 2, false }; goExtSplash480_2.Apply(true);
		}
		//Console::echo(result);
		//Console::echo(int2hex(atoi(argv[1]), 1));
		return 0;
	}
	//MultiPointer(ptrSetSplash, 0, 0, 0, 0x0064B503);
	//MultiPointer(ptrSetSplashResume, 0, 0, 0, 0x0064B512);
	//CodePatch setsplashres = { ptrSetSplash, "", "\xE9SSPR", 5, false };
	//int fullscreenWidth = 480;
	//int fullscreenHeight = 640;
	//NAKED void setSplashRes() {
	//	__asm {
	//		push edx
	//		mov edx, fullscreenWidth
	//		mov dword ptr[eax], edx
	//		xor ecx, ecx
	//		push edx
	//		mov edx, fullscreenHeight
	//		mov dword ptr[eax + 4], edx
	//		pop edx
	//		jmp[ptrSetSplashResume]
	//	}
	//}
	//
	//BuiltInFunction("setSplashRes", _setslpashres)
	//{
	//	setsplashres.DoctorRelative((u32)setSplashRes, 1).Apply(true);
	//	return 0;
	//}

	BuiltInFunction("Nova::remainCloakedWhenNoEnergy", _acwne) {
		MultiPointer(ptrCloakEnergyCheck, 0, 0, 0x00410AB3, 0x004115EB);
		if (!argv[0])
		{
			CodePatch CloakEnergyCheck = { ptrCloakEnergyCheck,"","\x84\xC9\x75",3,false }; CloakEnergyCheck.Apply(true);
		}
		else
		{
			CodePatch CloakEnergyCheck = { ptrCloakEnergyCheck,"","\x90\x90\xEB",3,false }; CloakEnergyCheck.Apply(true);
		}
		return "true";
	}

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

	HWND getGameHWND() {
		MultiPointer(ptrHWND, 0, 0, 0x00705C5C, 0x007160CC);
		uintptr_t HWND_PTR = ptrHWND;
		int GAME_HWND = *reinterpret_cast<int*>(HWND_PTR);
		HWND SS_HWND = reinterpret_cast<HWND>(GAME_HWND);
		return SS_HWND;
	}

	//BuiltInFunction("OpenGL::toggleWindowedFullscreen", _togl) {
		//toggleWindowedOpenGL(FindWindowA(NULL, "Starsiege"), NULL, NULL, NULL);
		//toggleWindowedOpenGL(getGameHWND(), NULL, NULL, NULL);
		//return "true";
	//}

	BuiltInFunction("disableWindowBorder", _dwd) {
		LONG lStyle = GetWindowLong(getGameHWND(), GWL_STYLE);
		lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
		SetWindowLong(getGameHWND(), GWL_STYLE, lStyle);
		return "true";
	}

	BuiltInFunction("enableWindowBorder", _ewb) {
		LONG lStyle = GetWindowLong(getGameHWND(), GWL_STYLE);
		lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
		//SetWindowLong(getGameHWND(), GWL_STYLE, lStyle | WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_SYSMENU);
		SetWindowLong(getGameHWND(), GWL_STYLE, lStyle | WS_CAPTION | WS_THICKFRAME | WS_SYSMENU);
		return "true";
	}

	BuiltInFunction("setWindowPos", _swp) {
		if (argc != 4)
		{
			Console::echo("%s( int_xPosition, int_yPosition, int_windowWidth, int_windowHeight );", self);
			return 0;
		}
		SetWindowPos(getGameHWND(), HWND_TOPMOST, atoi(argv[0]), atoi(argv[1]), atoi(argv[2]), atoi(argv[3]), NULL);
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
		GetWindowRect(getGameHWND(), &window);
		int x = window.left;
		int y = window.top;
		std::string arg1 = argv[0];
		if (arg1.compare("x"))
		{
			return tostring(y);
		}
		if (arg1.compare("y"))
		{
			return tostring(x);
		}
		return 0;
	}

	//BuiltInFunction("getWindowSize", _gws) {
	//	RECT window;
	//	GetWindowRect(getGameHWND(), &window);
	//	int width = window.right - window.left;
	//	int height = window.bottom - window.top;
	//	if (atoi(argv[0]) == 1)
	//	{
	//		return tostring(width);
	//	}
	//	else
	//	{
	//		return tostring(height);
	//	}
	//	return 0;
	//}

	//BuiltInFunction("hex2str", _h2s) {
	//	if (argc != 1)
	//	{
	//		Console::echo("%s ( hexString );");
	//		return 0;
	//	}
	//	char* arg0 = const_cast<char*>(argv[0]);
	//	char* hexString = hexToASCII(arg0);
	//	return hexString;
	//}
	BuiltInFunction("getWindowSize", _gws) {
		if (argc != 1)
		{
			Console::echo("%s( width/height );", self);
			return 0;
		}
		if (strlen(argv[0]) == 0)
		{
			Console::echo("%s( width/height );", self);
			return 0;
		}
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		int width = screen.x;
		int height = screen.y;
		std::string arg1 = argv[0];
		if (arg1.compare("width") == 0 || atoi(argv[0]) == 1)
		{
			return tostring(width);
		}
		else if (arg1.compare("height") == 0 || atoi(argv[0]) == 2)
		{
			return tostring(height);
		}
		return 0;
	}

	BuiltInFunction("int2hex", _i2h) {
		if (!strlen(argv[0]) || !atoi(argv[0]) > 0)
		{
			Console::echo("%s( integer endianness[0|1] );", self);
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
		if (!atof(argv[0]) > 0)
		{
			Console::echo("%s( float, endianness[0|1] );", self);
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

	//Render Height = -1.0 @ 480
	MultiPointer(ptrOpenGLRenderHeight, 0, 0, 0x0063D9FC, 0x0064C954);

	//Render Width = 1.0 @ 640
	MultiPointer(ptrOpenGLRenderWidth, 0, 0, 0x0063DA01, 0x0064C959);

	//Render Vertical Offset = -1.0 @ 640
	MultiPointer(ptrOpenGLRenderVert, 0, 0, 0x0063D9A8, 0x0064C900);

	MultiPointer(ptrOGLshift, 0, 0, 0x0063D9AD, 0x0064C905);
	BuiltInFunction("OpenGL::shiftGUI", _oglshgui) {
		if (argc != 1)
		{
			Console::echo("%s( int/flt ); Additive values shift the GUI to the right and vice versa. Default Value: -1", self);
			return 0;
		}
		//char* flt2hex_c = flt2hex(atof(argv[0]), 1);
		//Convert hex string to raw hex
		//char* hex2char_c = hex2char(flt2hex_c);

		char* flt2hex_c = flt2hex(stof(argv[0]), 1);
		string buffer = hexToASCII2(flt2hex_c);
		char* hex2char_c = const_cast<char*>(buffer.c_str());
		CodePatch genericCodePatch = { ptrOGLshift,"",hex2char_c,4,false }; genericCodePatch.Apply(true);
		return "true";
	}

	MultiPointer(ptrOpenGLRenderAltVert, 0, 0, 0x0063D9A8, 0x0064C900);
	MultiPointer(ptrOGLoffset, 0, 0, 0x0063DAFC, 0x0064CA54);

	int canvasWidth = 640;
	int canvasHeight = 480;
	void getCanvasDimensions()
	{
		Vector2i canvas;
		Fear::getScreenDimensions(&canvas);
		canvasWidth = canvas.x;
		canvasHeight = canvas.y;
		//Console::echo("%d %d", canvasWidth, canvasHeight);
	}
	MultiPointer(ptrSimGUIBitmapCtrl_TexScale, 0, 0, 0x005CF0EE, 0x005D2992);
	MultiPointer(ptrSimGUIBitmapCtrl_TexScaleResume, 0, 0, 0x005CF0FA, 0x005D299E);
	CodePatch guibitmapctrl_tile = { ptrSimGUIBitmapCtrl_TexScale, "", "\xE9SGTS", 5, false };
	NAKED void GuiBitmapCTRL_Tile() {
		__asm {
			add ecx, [ebx + 0x1A4]
			cmp canvasWidth, ecx //Check BitmapCtrl WIDTH gainst canvas width
			je __canvasWidthMatches
			jmp __standardRoutine

			__canvasWidthMatches:
				mov ecx, 0
				add esi, [ebx + 0x1A8]
				cmp canvasHeight, esi //Check BitmapCtrl HEIGHT gainst canvas width
				je __canvasDimensionsMatch
				jmp __standardRoutine

				__canvasDimensionsMatch: //Both axis match so scale the Bitmapctrl to 640x480
				mov esi, 0
				add ecx, 0x280
				add esi, 0x1E0
				jmp[ptrSimGUIBitmapCtrl_TexScaleResume]

			__standardRoutine: //Dimensions did not much. Continue with standard routine
				add ecx, [ebx + 0x1A4]
				add esi, [ebx + 0x1A8]
				jmp[ptrSimGUIBitmapCtrl_TexScaleResume]
		}
	}

	NAKED void GuiBitmapCTRL_Tile_revert() {
		__asm {
			add ecx, [ebx + 0x1A4]
			add esi, [ebx + 0x1A8]
			jmp[ptrSimGUIBitmapCtrl_TexScaleResume]
		}
	}

	BuiltInFunction("OpenGL::restoreBitmapCtrl", _openglshrinkbitmapctrl) {
		guibitmapctrl_tile.DoctorRelative((u32)GuiBitmapCTRL_Tile_revert, 1).Apply(true);
		return 0;
	}
	BuiltInFunction("OpenGL::patchBitmapCtrl", _openglunshrinkbitmapctrl) {
		guibitmapctrl_tile.DoctorRelative((u32)GuiBitmapCTRL_Tile, 1).Apply(true);
		return 0;
	}
	//TO DO (Implement new GUI upscaling method. Optimize GUI alteration for black bars and offseting. Remove storeobject() and loadobject() from methods.
	//Flips the internal OpenGL scale so that it scales downward diagonally to the right instead of upward diagonally
	//Doing this simplifies handling the upscaled GUI so that we only need to scale, shift, create a simgui::bitmapctrl, create a TScontrol, and move them behind the native gui shell objects
	//We can also stop the hall of mirrors effect on the shift by setting $Gui::noPalTrans to false so that the screen refreshes on a palette change
	BuiltInFunction("OpenGL::flipScaler", _openglflipscaler) {
		getCanvasDimensions();
		CodePatch genericCodePatch1 = { ptrOGLoffset,"","\x00\x00\x00\x00",4,false}; genericCodePatch1.Apply(true);
		CodePatch genericCodePatch2 = { ptrOpenGLRenderVert,"","\x00\x00\x80\x3F",4,false}; genericCodePatch2.Apply(true);
		guibitmapctrl_tile.DoctorRelative((u32)GuiBitmapCTRL_Tile, 1).Apply(true);
		return 0;
	}

	BuiltInFunction("OpenGL::unflipScaler", _openglunflipscaler) {
		CodePatch genericCodePatch1 = { ptrOGLoffset,"","\x00\x00\x00\x3F",4,false }; genericCodePatch1.Apply(true);
		CodePatch genericCodePatch2 = { ptrOpenGLRenderVert,"","\x00\x00\x80\xBF",4,false }; genericCodePatch2.Apply(true);
		guibitmapctrl_tile.DoctorRelative((u32)GuiBitmapCTRL_Tile_revert, 1).Apply(true);
		return 0;
	}

	//DEPRECATED
	//BuiltInFunction("OpenGL::offsetGUI", _oglogui) {
	//	if (argc != 1)
	//	{
	//		Console::echo("%s( int/flt ); //Offsets the GUI vertically. Default: 0.5", self);
	//		return 0;
	//	}
	//	char* flt2hex_c = flt2hex(atof(argv[0]), 1);
	//	//Convert hex string to raw hex
	//	char* hex2char_c = hex2char(flt2hex_c);
	//	CodePatch genericCodePatch = { ptrOGLoffset,"",hex2char_c,4,false }; genericCodePatch.Apply(true);
	//	return "true";
	//}

	//BuiltInFunction("OpenGL::UpscaleGUI", _OpenGLUpscaleGUI) {
	//
	//	//Check args
	//	if (argc != 2 || atoi(argv[0]) < 640 || atoi(argv[1]) < 480)
	//	{
	//		Console::echo("%s( width, height );", self);
	//		return 0;
	//	}
	//	//char* relativeWidth = flt2hex((atof(argv[0])/640), 1);
	//	char* relativeWidth = flt2hex((atof(argv[1])/atof(argv[0]), 1));
	//	Console::echo("Width %s", relativeWidth);
	//	char* relativeWidth_Hx = hex2char(relativeWidth);
	//	//Patch in the new width
	//	CodePatch genericCodePatch01 = { ptrOpenGLRenderWidth,"",relativeWidth_Hx,4,false }; genericCodePatch01.Apply(true);
	//	free(relativeWidth_Hx);
	//
	//	char* relativeHeight = flt2hex(-(atof(argv[1])/480), 1);
	//	Console::echo("Height %s", relativeHeight);
	//	char* relativeHeight_Hx = hex2char(relativeHeight);
	//	//Patch in the new height
	//	CodePatch genericCodePatch02 = { ptrOpenGLRenderHeight,"",relativeHeight_Hx,4,false }; genericCodePatch02.Apply(true);
	//	free(relativeHeight_Hx);
	//
	//	char* relativeVertOffset = flt2hex(-(atof(argv[1])/480), 1);
	//	Console::echo("Offset %s", relativeVertOffset);
	//	char* relativeVertOffset_Hx = hex2char(relativeVertOffset);
	//	//Patch in the new vertical offset
	//	CodePatch genericCodePatch03 = { ptrOpenGLRenderVert,"",relativeVertOffset_Hx,4,false }; genericCodePatch03.Apply(true);
	//	free(relativeVertOffset_Hx);
	//	return "true";
	//}

	MultiPointer(ptrOGLscale, 0, 0, 0x0063DAF8, 0x0064CA50);
	BuiltInFunction("OpenGL::scaleGUI", _oglsgui) {
		if (argc != 1)
		{
			Console::echo("%s( int/flt ); //Changes the internal GUI rendering scale. Default: 2", self);
			return 0;
		}
		//float scale = atof(argv[0]);
		//char* scale_c = flt2hex(scale, 1);
		//char* scale_hString = hex2char(scale_c);
		float scale = stof(argv[0])+0.00001;
		char* flt2hex_c = flt2hex(scale, 1);
		string buffer = hexToASCII2(flt2hex_c);
		char* hex2char_c = const_cast<char*>(buffer.c_str());
	
		CodePatch GUIscalePatch = { ptrOGLscale,"",hex2char_c,4,false }; GUIscalePatch.Apply(true);
		//Console::setVariable("Opengl::scaleGUI", scale_c);
		return "true";
	}

	BuiltInFunction("setCursorPos", _scp) {
		if (argc != 2)
		{
			Console::echo("%s( x, y );", self);
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

	MultiPointer(ptrCampaignByte, 0, 0, 0x00765810, 0x00775E28);
	BuiltInFunction("isCampaign", _iscampaign) {
		uintptr_t ptr = ptrCampaignByte;
		int boolean = *reinterpret_cast<int*>(ptr);
		if (boolean == 1)
		{
			return "true";
		}
		return 0;
	}

	BuiltInFunction("isNumeric", _isnumeric)
	{
		if (argc != 1 || argv[0] == NULL)
		{
			Console::echo("%s( string );", self);
			return 0;
		}
		if (is_number(argv[0]))
		{
			return "true";
		}
		return 0;
	}
	//Test function to run a subroutine
	//BuiltInFunction("subroutine", _hc) {
		//typedef int (*FunctionType)(int,int);
		//typedef int (*FunctionType)(u8 r, u8 g, u8 b);
		//FunctionType hardcallf = (FunctionType)0x005C45CC; //0x004458A4
		//FunctionType hardcallf = (FunctionType)0x0054FAC0; //0x004458A4
		//char* arg = const_cast<char*>(argv[0]);
		//u8 r = reinterpret_cast<u8>(&argv[0]);
		//u8 b = reinterpret_cast<u8>(&argv[1]);
		//u8 g = reinterpret_cast<u8>(&argv[2]);
		//hardcallf(r, g, b);
		//return "true";
	//}

	//BuiltInFunction("SimStarfield::setBottomVisible", _ssfsbv) {
	//	typedef int (*FunctionType)(bool v);
	//	FunctionType EXEC_SUBR = (FunctionType)0x006B1F80;
	//	EXEC_SUBR(argv[0]);
	//	return "true";
	//}

	////////////////////////////////////////////////////////
	// RENDERING
	////////////////////////////////////////////////////////
	//Sets the maximum size of the windowed game canvas to 1280x1024. Game does not render beyond 1280x1024 in windowed mode.
	//Allows Glide to run at 1280x1024 in windowed mode.
	//"\xC7\x02\x00\x05\x00\x00\xC7\x42\x04\x00\x04",

	//MultiPointer(ptrFixedTerrainDetailFloat, 0, 0, 0x005804AC, 0x00583BCF);
	//BuiltInFunction("Nova::setTerrainDetail", _novasetterraindetail)
	//{
	//	return 0;
	//	if (argc != 1 || !is_number(argv[0]))
	//	{
	//		Console::echo("%s( int );", self);
	//		return 0;
	//	}
	//
	//	char* flt2hex_c = flt2hex(stof(argv[0]), 1);
	//	string buffer = hexToASCII2(flt2hex_c);
	//	char* hex2char_c = const_cast<char*>(buffer.c_str());
	//	CodePatch terrainDetailValue = { ptrFixedTerrainDetailFloat, "", hex2char_c, 4, false };
	//	terrainDetailValue.Apply(true);
	//	Console::setVariable("pref::terrainDetail", tostring(trunc(atof(argv[0]))));
	//	return "true";
	//}

	MultiPointer(ptrWinMaxWinSize, 0, 0, 0x005740FF, 0x00577307);
	CodePatch canvasWindowMaxWindowedSize_patch = {
		ptrWinMaxWinSize,
		"",
		"\xC7\x02\x80\x16\x00\x00\xC7\x42\x04\x00\x1E",
		//"\xC7\x02\x10\xE0\x00\x00\xC7\x42\x04\x00\x1E",
		11,
		false
	};

	MultiPointer(ptrWinMaxIntRendSizeWidth, 0, 0, 0x006487A6, 0x006584BE);
	CodePatch canvasWindowMaxInteralRenderSizeWidth_patch = {
		ptrWinMaxIntRendSizeWidth,
		"",
		"\x3D\x00\x1E\x00\x00\x7E\x06\xC7\x06\x00\x1E",
		//"\x3D\x10\xE0\x00\x00\x7E\x06\xC7\x06\x10\xE0",
		11,
		false
	};
	MultiPointer(ptrWinMaxIntRendSizeHeight, 0, 0, 0x006487C5, 0x006584DD);
	CodePatch canvasWindowMaxInteralRenderSizeHeight_patch = {
		ptrWinMaxIntRendSizeHeight,
		"",
		"\x81\xF9\x80\x16\x00\x00\x7E\x07\xC7\x46\x04\x80\x16",
		//"\x81\xF9\x00\x1E\x00\x00\x7E\x07\xC7\x46\x04\x00\x1E",
		13,
		false
	};

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
	MultiPointer(ptrTankSpeedCoeff, 0, 0, 0, 0x00436C64);
	MultiPointer(ptrTankVehicleScale, 0, 0, 0, 0x00433DF0);
	MultiPointer(ptrHercVehicleScale, 0, 0, 0, 0x004A2BA0);

	MultiPointer(ptrHardPointScale, 0, 0, 0, 0x00498628);
	MultiPointer(ptrDTSCreationScale, 0, 0, 0, 0x0063DC60); //The scale for dts shape objects upon spawning in world


	MultiPointer(ptrTankAlignmentSpeed, 0, 0, 0x004344D4, 0x0043598C);
	//BuiltInFunction("Nova::toggleSmoothTankAligns", _novatogglesmoothtankalign) {
	//	std::string var = Console::getVariable("pref::smoothTankSurfaceAlign");
	//	if (var.compare("1") == 0)
	//	{
	//		CodePatch tankAlignment = { ptrTankAlignmentSpeed, "", "\x00\x00\xA0\x3C", 4, false };
	//		tankAlignment.Apply(true);
	//	}
	//	else
	//	{
	//		CodePatch tankAlignment = { ptrTankAlignmentSpeed, "", "\xCD\xCC\x4C\x3e", 4, false };
	//		tankAlignment.Apply(true);
	//	}
	//	return "true";
	//}

	MultiPointer(ptr_tankAlignmentSpeed, 0, 0, 0x00434405, 0x004358BD);
	MultiPointer(ptr_tankAlignmentSpeedResume, 0, 0, 0x00434431, 0x004358E9);
	CodePatch tankalignmentspeed = { ptr_tankAlignmentSpeed,"","\xE9TALR",5,false };
	float tankAlignSpeed = 0.2;
	void getPrefSmoothTankAlignment()
	{
		if (atoi(Console::getVariable("pref::smoothTankSurfaceAlign")) == 1)
		{
			tankAlignSpeed = 0.0195;
		}
		else
		{
			tankAlignSpeed = 0.2;
		}
	}
	NAKED void tankAlignmentSpeed() {
		__asm {
			call getPrefSmoothTankAlignment
			fld tankAlignSpeed
			fmul [esp + 0x170 - 0x138]
			fstp [esp + 0x170 - 0x138]
			fld tankAlignSpeed
			fmul [esp + 0x170 - 0x134]
			fstp [esp + 0x170 - 0x134]
			fld tankAlignSpeed
			fmul [esp + 0x170 - 0x130]
			lea edx, [ebx + 0x0CC8]
			jmp ptr_tankAlignmentSpeedResume
		}
	}

	//MultiPointer(ptrFlyerElevationControl, 0, 0, 0x004386E5, 0x00439B9D);
	//MultiPointer(ptrFlyerElevationControlResume, 0, 0, 0x004386F0, 0x00439BA8);
	//CodePatch playerflyerelevationcontrol = { ptrFlyerElevationControl, "", "\xE9_FEC", 5, false };
	//NAKED void PlayerFlyerElevationControl() {
	//__asm {
	//	cmp ptrCampaignByte, 1
	//	je __je
	//	mov byte ptr[eax + 4], 0
	//	mov dword ptr[eax + 0x40], 0x42480000
	//	jmp ptrFlyerElevationControlResume
	//	__je :
	//		mov byte ptr[eax + 4], 1
	//		mov dword ptr[eax + 0x40], 0x42480000
	//		jmp ptrFlyerElevationControlResume
	//	}
	//}

	MultiPointer(ptrFlyerThrottleInit, 0, 0, 0x004386E8, 0x00439BA0);
	BuiltInFunction("Nova::flyerCampaignStateCheck", _novaflyercampaignstatecheck)
	{
		uintptr_t ptr = ptrCampaignByte;
		int boolean = *reinterpret_cast<int*>(ptr);

		if(boolean)
		{
			CodePatch AIFlyerThrottle = { ptrFlyerThrottleInit, "", "\x01", 1, false };
			AIFlyerThrottle.Apply(true);
		}
		else
		{
			CodePatch PlayerFlyerThrottle = { ptrFlyerThrottleInit, "", "\x00", 1, false };
			PlayerFlyerThrottle.Apply(true);
		}
		return "true";
	}

	MultiPointer(ptrAllowVehicleBypass1, 0, 0, 0, 0x00413BDD);
	MultiPointer(ptrAllowVehicleBypass2, 0, 0, 0, 0x00413BFA);
	CodePatch allowVehiclePatch1 = { ptrAllowVehicleBypass1, "", "\x90\x90", 2, false };
	CodePatch allowVehiclePatch2 = { ptrAllowVehicleBypass2, "", "\x89", 1, false };

	MultiPointer(ptrVehicleSpeedCoeff, 0, 0, 0x00595688, 0x00598EA4);
	BuiltInFunction("Nova::setVehicleSpeedCoeff", _novasetvehiclespeedcoeff) {
		if (argc != 1)
		{
			Console::echo("%s( int/float );", self);
			return "false";
		}

		float input = stof(argv[0]);
		char* flt2hex_c = flt2hex(input, 1);
		string buffer = hexToASCII2(flt2hex_c);
		char* hex2char_c = const_cast<char*>(buffer.c_str());

		CodePatch speedCoeffPatch = { ptrVehicleSpeedCoeff,"",hex2char_c,4,false }; speedCoeffPatch.Apply(true);
		return "true";
	}

	MultiPointer(fnCockpitShake, 0, 0, 0x0046A40C, 0x0046BE30);
	BuiltInFunction("Nova::toggleCockpitShake", _novatogglecockpitshake)
	{
		std::string var = Console::getVariable("pref::cockpitShake");
		if (var.compare("0") == 0)
		{
			CodePatch cockpitShake = { fnCockpitShake, "", "\x53", 1, false };
			cockpitShake.Apply(true);
		}
		else
		{
			CodePatch cockpitShake = { fnCockpitShake, "", "\xC3", 1, false };
			cockpitShake.Apply(true);
		}
		return "true";
	}


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

	MultiPointer(ptrTScontrolFOV, 0, 0, 0, 0x005D90EC);
	MultiPointer(ptrTScontrolViewPortTerrainCullingRange, 0, 0, 0, 0x0049FCBC);

	MultiPointer(ptrInitFov, 0, 0, 0x004689F2, 0x0046A416);
	MultiPointer(ptrZoomFov, 0, 0, 0x0046D827, 0x0046F349);
	MultiPointer(ptrEditCameraFov, 0, 0, 0x0040D5F4, 0x0040D6FC);
	MultiPointer(ptrTestZoomFov, 0, 0, 0x0046D827, 0x0046F349);
	BuiltInFunction("fov", _fov) {
		if (argc != 1)
		{
			Console::echo("%s( 50 - 120 );", self);
			return "false";
		}
		if (atoi(argv[0]) < 50 && atoi(argv[0]) > 120)
		{
			Console::echo("%s( 50 - 120 );", self);
			return "false";
		}

		float radial = (stof(argv[0]) * 3.14159265359 / 180) / 2;
		char* flt2hex_c = flt2hex(radial, 1);
		string buffer = hexToASCII2(flt2hex_c);
		char* hex2char_c = const_cast<char*>(buffer.c_str());

		CodePatch initialFovPatch = { ptrInitFov,"",hex2char_c,4,false }; initialFovPatch.Apply(true);
		CodePatch postZoomFovPatch = { ptrZoomFov,"",hex2char_c,4,false }; postZoomFovPatch.Apply(true);
		CodePatch editCameraFovPatch = { ptrEditCameraFov,"",hex2char_c,4,false }; editCameraFovPatch.Apply(true);
		Console::setVariable("client::fov", argv[0]);
		Console::eval("export(\"client::*\", \"playerPrefs.cs\");");
		return "true";
	}
	MultiPointer(ptr_patchMipDetail, 0, 0, 0, 0x006020F8);
	MultiPointer(ptr_patchFlatPaneMipDetail, 0, 0, 0, 0x00601AAE);
	//BuiltInFunction("Nova::setTerrainDetail", _novasetterraindetail) {
		//if (argc != 1)
		//{
			//Console::echo("%s( 0 - 2.0 );", self);
			//return "false";
		//}
		//if (atof(argv[0]) < 0.1 && atof(argv[0]) > 2.0)
		//{
			//Console::echo("%s( 0 - 2.0 );", self);
			//return "false";
		//}
		//Convert float to hex string
		//char* flt2hex_c = flt2hex(atof(argv[0]), 1);
		//char* flt2hex_c2 = flt2hex(-atof(argv[0]), 1);
		//Convert hex string to raw hex
		//char* hex2char_c = hex2char(flt2hex_c);
		//char* hex2char_c2 = hex2char(flt2hex_c);

		//CodePatch patch = { ptr_patchMipDetail,"",hex2char_c,4,false }; patch.Apply(true);
		//CodePatch patch2 = { ptr_patchFlatPaneMipDetail,"",hex2char_c2,false }; patch2.Apply(true);
		//Console::setVariable("client::fov", argv[0]);
		//Console::eval("export(\"client::*\", \"playerPrefs.cs\");");

		//return "true";
	//}

	BuiltInFunction("modloader::patchEvents", _mlpatchPlayerEvents) {

		//Mission Events
		MultiPointer(ptrMissionEnd1, 0, 0, 0x006CBB62, 0x006DBC7A);
		CodePatch MissionEnd1 = { ptrMissionEnd1,"","MLMissionEnd",12,false }; MissionEnd1.Apply(true);
		MultiPointer(ptrMissionEnd2, 0, 0, 0x006F977C, 0x00709A5C);
		CodePatch MissionEnd2 = { ptrMissionEnd2,"","MLMissionEnd",12,false }; MissionEnd2.Apply(true);

		//Player Events
		MultiPointer(ptrPlayerAddEvent1, 0, 0, 0x006CBAE9, 0x006DBC01);
		CodePatch PlayerEvent1 = { ptrPlayerAddEvent1,"","MLplyr::onAdd",13,false }; PlayerEvent1.Apply(true);
		MultiPointer(ptrPlayerAddEvent2, 0, 0, 0x006CBAFA, 0x006DBC12);
		CodePatch PlayerEvent2 = { ptrPlayerAddEvent2,"","MLplyr::onAdd",13,false }; PlayerEvent2.Apply(true);
		MultiPointer(ptrPlayerRemoveEvent1, 0, 0, 0x006CBB26, 0x006DBC3E);
		CodePatch PlayerEvent3 = { ptrPlayerRemoveEvent1,"","MLplyr::onRemove",16,false }; PlayerEvent3.Apply(true);
		MultiPointer(ptrPlayerRemoveEvent2, 0, 0, 0x006CBB3A, 0x006DBC52);
		CodePatch PlayerEvent4 = { ptrPlayerRemoveEvent2,"","MLplyr::onRemove",16,false }; PlayerEvent4.Apply(true);

		//Vehicle Events
		MultiPointer(ptrVehicleOnAdd, 0, 0, 0x006D97DC, 0x006E9A22);
		CodePatch VehicleOnAdd = { ptrVehicleOnAdd,"","MLveh::onAdd\x00\x00",14,false }; VehicleOnAdd.Apply(true);
	//	MultiPointer(ptrVehicleOnAttacked,	0, 0, 0x006D97EB, 0x006E9A31);
	//	CodePatch VehicleOnAttacked = { ptrVehicleOnAttacked,"","MLveh::onAttacked\x00\x00",19,false };
	//	MultiPointer(ptrVehicleOnEnabled,	0, 0, 0x006D97FF, 0x006E9A45);
	//	CodePatch VehicleOnEnabled = { ptrVehicleOnEnabled,"","MLveh::onEnabled\x00\x00",18,false };
	//	MultiPointer(ptrVehicleOnDisabled,	0, 0, 0x006D9812, 0x006E9A58);
	//	CodePatch VehicleOnDisabled = { ptrVehicleOnDisabled,"","MLveh::onDisabled\x00\x00",19,false };
	//	MultiPointer(ptrVehicleOnDestroyed, 0, 0, 0x006D9826, 0x006E9A6C);
	//	CodePatch VehicleOnDestroyed = { ptrVehicleOnDestroyed,"","MLveh::onDestroyed\x00\x00",20,false};
	//	MultiPointer(ptrVehicleOnArrived,	0, 0, 0x006D983B, 0x006E9A81);
	//	CodePatch VehicleOnArrived = { ptrVehicleOnArrived,"","MLveh::onArrived\x00\x00",18,false };
	//	MultiPointer(ptrVehicleOnScan,		0, 0, 0x006D984E, 0x006E9A94);
	//	CodePatch VehicleOnScan = { ptrVehicleOnScan,"","MLveh::onScan\x00\x00",15,false };			
	//	MultiPointer(ptrVehicleOnSpot,		0, 0, 0x006D985E, 0x006E9AA4);
	//	CodePatch VehicleOnSpot = { ptrVehicleOnSpot,"","MLveh::onSpot\x00\x00",15,false };			
	//	MultiPointer(ptrVehicleOnNewLeader, 0, 0, 0x006D986E, 0x006E9AB4);
	//	CodePatch VehicleOnNewLeader = { ptrVehicleOnNewLeader,"","MLveh::onNewLeader\x00\x00",20,false};
	//	MultiPointer(ptrVehicleOnNewTarget, 0, 0, 0x006D9883, 0x006E9AC9);
	//	CodePatch VehicleOnNewTarget = { ptrVehicleOnNewTarget,"","MLveh::onNewTarget\x00\x00",20,false};
	//	MultiPointer(ptrVehicleOnTargeted,	0, 0, 0x006D9898, 0x006E9ADE);
	//	CodePatch VehicleOnTargeted = { ptrVehicleOnTargeted,"","MLveh::onTargeted\x00\x00",19,false };
	//	MultiPointer(ptrVehicleOnMessage,	0, 0, 0x006D98AC, 0x006E9AF2);
	//	CodePatch VehicleOnMessage = { ptrVehicleOnMessage,"","MLveh::onMessage\x00\x00",18,false };
	//	MultiPointer(ptrVehicleOnAction,	0, 0, 0x006D98BF, 0x006E9B05);
	//	CodePatch VehicleOnAction = { ptrVehicleOnAction,"","MLveh::onAction\x00\x00",17,false };
	//
	//	if (std::filesystem::exists("modloader.vol"))
	//	{
	//		VehicleOnAttacked.Apply(true);
	//		VehicleOnEnabled.Apply(true);
	//		VehicleOnDisabled.Apply(true);
	//		VehicleOnDestroyed.Apply(true);
	//		VehicleOnArrived.Apply(true);
	//		VehicleOnScan.Apply(true);
	//		VehicleOnSpot.Apply(true);
	//		VehicleOnNewLeader.Apply(true);
	//		VehicleOnNewTarget.Apply(true);
	//		VehicleOnTargeted.Apply(true);
	//		VehicleOnMessage.Apply(true);
	//		VehicleOnAction.Apply(true);
	//	}
		return "true";
	}

	BuiltInFunction("quitGame", _quitGame) {
		PostQuitMessage(0);
		return "true";
	}

	BuiltInVariable("pref::NoCockpitFadein", bool, prefCockpitFadein, true);
	BuiltInFunction("Nova::toggleCockpitFadeIn", _tcf) {
		MultiPointer(ptrSimFadein01, 0, 0, 0x0045AA7E, 0x0045BF42);
		MultiPointer(ptrSimFadein02, 0, 0, 0x0045AA97, 0x0045BF5B);
		if (!prefCockpitFadein)
		{
			CodePatch SimFadein01 = { ptrSimFadein01,"","\x9A\x99\x99\x3F",4,false }; SimFadein01.Apply(true);
			CodePatch SimFadein02 = { ptrSimFadein02,"","\x40",1,false }; SimFadein02.Apply(true);
		}
		else
		{
			CodePatch SimFadein01 = { ptrSimFadein01,"","\x00\x00\x00\x00",4,false }; SimFadein01.Apply(true);
			CodePatch SimFadein02 = { ptrSimFadein02,"","\x00",1,false }; SimFadein02.Apply(true);
		}
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
	//MultiPointer(ptrWeaponCount, 0, 0, 0x00738864, 0x00748E7C);
	//BuiltInFunction("Nova::getWeaponIDcount", _NovagetweaponIDcount)
	//{
	//	uintptr_t ptr = ptrWeaponCount;
	//	int value = *reinterpret_cast<int*>(ptr);
	//	return tostring(value);
	//}
	//
	//MultiPointer(ptrIntMountCount, 0, 0, 0, 0x00746634);
	//BuiltInFunction("Nova::getIntMountIDcount", _NovageintmountIDcount)
	//{
	//	uintptr_t ptr = ptrIntMountCount;
	//	int value = *reinterpret_cast<int*>(ptr);
	//	return tostring(value);
	//}
	//
	//MultiPointer(ptrTankCount, 0, 0, 0, 0x00746A54);
	//BuiltInFunction("Nova::getTankIDcount", _NovagettankIDcount) //Includes drones
	//{
	//	uintptr_t ptr = ptrTankCount;
	//	int value = *reinterpret_cast<int*>(ptr);
	//	return tostring(value);
	//}
	//
	//MultiPointer(ptrFlyerCount, 0, 0, 0, 0x00746A74);
	//BuiltInFunction("Nova::getFlyerIDcount", _NovagetflyerIDcount)
	//{
	//	uintptr_t ptr = ptrFlyerCount;
	//	int value = *reinterpret_cast<int*>(ptr);
	//	return tostring(value);
	//}
	//
	//MultiPointer(ptrHercCount, 0, 0, 0, 0x00747E1C);
	//BuiltInFunction("Nova::getHercIDcount", _NovagethercIDcount)
	//{
	//	uintptr_t ptr = ptrHercCount;
	//	int value = *reinterpret_cast<int*>(ptr);
	//	return tostring(value);
	//}


	
	MultiPointer(ptrHercNetUpdate1, 0, 0, 0, 0x004A72F0);
	MultiPointer(ptrHercNetUpdate2, 0, 0, 0, 0x004A72F4);
	MultiPointer(ptrHercNetUpdate3, 0, 0, 0, 0x004A72F8);
	CodePatch HercNetUpdate1 = { ptrHercNetUpdate1,"","\x00\x00\x80\x47",1,false };
	CodePatch HercNetUpdate2 = { ptrHercNetUpdate2,"","\x00\x00\x80\x47",1,false };
	CodePatch HercNetUpdate3 = { ptrHercNetUpdate3,"","\x00\x00\x80\x47",1,false };

	MultiPointer(ptrTankNetUpdate1, 0, 0, 0, 0x004383A0);
	MultiPointer(ptrTankNetUpdate2, 0, 0, 0, 0x004383A4);
	MultiPointer(ptrTankNetUpdate3, 0, 0, 0, 0x004383A8);
	CodePatch TankNetUpdate1 = { ptrTankNetUpdate1,"","\x00\x00\x80\x47",1,false };
	CodePatch TankNetUpdate2 = { ptrTankNetUpdate2,"","\x00\x00\x80\x47",1,false };
	CodePatch TankNetUpdate3 = { ptrTankNetUpdate3,"","\x00\x00\x80\x47",1,false };

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
	MultiPointer(ptrMissServTerrain, 0, 0, 0x00683430, 0x00693328);
	CodePatch missingTerrain_patch = {
		ptrMissServTerrain,
		"",
		"\xC3",
		1,
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

	//The default value for the packetrate field in the options gui
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
		"\xB9\x09\x01",
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

	//Packet rate loopback. This is the serverside packetRate. Set to 4096
	MultiPointer(ptrLoopbackPacketRate, 0, 0, 0x0045F440, 0x00460A98);
	CodePatch loopback_packetRate_patch = {
		 ptrLoopbackPacketRate,
		 "",
		 "\x00\x10\x00\x00",
		 4,
		 false
	};

	MultiPointer(ptrBadRateDivisor, 0, 0, 0, 0x00690A85);
	CodePatch badPacketRate_patch = {
		 ptrBadRateDivisor,
		 "",
		 "\x90\x90",
		 2,
		 false
	};

	//Constructor bypasses for 1.004. 1.003 does not have any
	CodePatch constructorBypass01 = { 0x0040206C,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };
	CodePatch constructorBypass02 = { 0x005385A0,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };
	CodePatch constructorBypass03 = { 0x0053F4AF,"","\xC6\x05\x58\x90\x6D\x00\x01",7,false };

	MultiPointer(ptrNewHerc,		0, 0, 0x006C8FBF, 0x006D90B7);
	MultiPointer(ptrHercBase,		0, 0, 0x006C8FC7, 0x006D90BF);
	MultiPointer(ptrHercCpit,		0, 0, 0x006C8FF7, 0x006D90EF);
	MultiPointer(ptrNewTank,		0, 0, 0x006C9021, 0x006D9119);
	MultiPointer(ptrTankBase,		0, 0, 0x006C9029, 0x006D9121);
	MultiPointer(ptrTankCpit,		0, 0, 0x006C9042, 0x006D913A);
	MultiPointer(ptrNewDrone,		0, 0, 0x006C9089, 0x006D9181);
	MultiPointer(ptrDroneBase,		0, 0, 0x006C9092, 0x006D918A);
	MultiPointer(ptrNewFlyer,		0, 0, 0x006C90E7, 0x006D91DF);
	MultiPointer(ptrFlyerBase,		0, 0, 0x006C90F0, 0x006D91E8);
	MultiPointer(ptrFlyerCpit,		0, 0, 0x006C910C, 0x006D9204);
	MultiPointer(ptrNewTurret,		0, 0, 0x006C916F, 0x006D9267);
	MultiPointer(ptrTurretBase,		0, 0, 0x006C9179, 0x006D9271);
	MultiPointer(ptrNewWeapon,		0, 0, 0x006C91F7, 0x006D92EF);
	MultiPointer(ptrWeaponInfo1,	0, 0, 0x006C9201, 0x006D92F9);
	MultiPointer(ptrNewBullet,		0, 0, 0x006C9257, 0x006D934F);
	MultiPointer(ptrNewMissile,		0, 0, 0x006C9261, 0x006D9359);
	MultiPointer(ptrNewEnergy,		0, 0, 0x006C926C, 0x006D9364);
	MultiPointer(ptrNewBeam,		0, 0, 0x006C9276, 0x006D936E);
	MultiPointer(ptrNewMine,		0, 0, 0x006C927E, 0x006D9376);
	MultiPointer(ptrNewBomb,		0, 0, 0x006C9286, 0x006D937E);
	MultiPointer(ptrNewMountable,	0, 0, 0x006C928E, 0x006D9386);
	MultiPointer(ptrMountInfo1,		0, 0, 0x006C929B, 0x006D9393);
	MultiPointer(ptrNewEngine,		0, 0, 0x006C92B1, 0x006D93A9);
	MultiPointer(ptrEngineInfo1,	0, 0, 0x006C92BB, 0x006D93B3);
	MultiPointer(ptrNewSensor,		0, 0, 0x006C92D3, 0x006D93CB);
	MultiPointer(ptrSensorInfo1,	0, 0, 0x006C92DD, 0x006D93D5);
	MultiPointer(ptrNewReactor,		0, 0, 0x006C9300, 0x006D93F8);
	MultiPointer(ptrReactorInfo1,	0, 0, 0x006C930B, 0x006D9403);
	MultiPointer(ptrNewShield,		0, 0, 0x006C9325, 0x006D941D);
	MultiPointer(ptrShieldInfo1,	0, 0, 0x006C932F, 0x006D9427);
	MultiPointer(ptrNewModulator,	0, 0, 0x006C9347, 0x006D943F);
	MultiPointer(ptrNewAmplifier,	0, 0, 0x006C9354, 0x006D944C);
	MultiPointer(ptrNewCapacitor,	0, 0, 0x006C9361, 0x006D9459);
	MultiPointer(ptrNewComputer,	0, 0, 0x006C936E, 0x006D9466);
	MultiPointer(ptrNewBooster,		0, 0, 0x006C937A, 0x006D9472);
	MultiPointer(ptrNewRepair,		0, 0, 0x006C9385, 0x006D947D);
	MultiPointer(ptrNewCloak,		0, 0, 0x006C938F, 0x006D9487);
	MultiPointer(ptrNewArmor,		0, 0, 0x006C93AE, 0x006D94A6);
	MultiPointer(ptrArmorInfo1,		0, 0, 0x006C93B7, 0x006D94AF);
	MultiPointer(ptrNewECM,			0, 0, 0x006C93DE, 0x006D94D6);
	MultiPointer(ptrNewThermal,		0, 0, 0x006C93E5, 0x006D94DD);
	MultiPointer(ptrNewBattery,		0, 0, 0x006C93F0, 0x006D94E8);

	CodePatch constructorsPatch1 =		{ ptrNewHerc,		"","MLNHerc",		7,false };
	CodePatch constructorsPatch1a =		{ ptrHercBase,		"","MLhcBase",		8,false };
	CodePatch constructorsPatch2 =		{ ptrHercCpit,		"","MLhcCpit",		8,false };
	CodePatch constructorsPatch3 =		{ ptrNewTank,		"","MLNTank",		7,false };
	CodePatch constructorsPatch3a =		{ ptrTankBase,		"","MLtkBase",		8,false };
	CodePatch constructorsPatch4 =		{ ptrTankCpit,		"","MLtkCpit",		8,false };
	CodePatch constructorsPatch5 =		{ ptrNewDrone,		"","MLNDrone",		8,false };
	CodePatch constructorsPatch5a =		{ ptrDroneBase,		"","MLDrnBase",		8,false };
	CodePatch constructorsPatch6 =		{ ptrNewFlyer,		"","MLNFlyer",		8,false };
	CodePatch constructorsPatch6a =		{ ptrFlyerBase,		"","MLflyBase",		9,false };
	CodePatch constructorsPatch7 =		{ ptrFlyerCpit,		"","MLflyCpit",		9,false };
	CodePatch constructorsPatch8 =		{ ptrNewTurret,		"","MLNTurret",		9,false };
	CodePatch constructorsPatch9 =		{ ptrTurretBase,	"","MLturrBase",	10,false };
	CodePatch constructorsPatch10 =		{ ptrNewWeapon,		"","MLNWeapon",		9,false };
	CodePatch constructorsPatch10a =	{ ptrWeaponInfo1,	"","MLweapInfo1",	11,false };
	CodePatch constructorsPatch11 =		{ ptrNewBullet,		"","MLNBullet",		9,false };
	CodePatch constructorsPatch12 =		{ ptrNewMissile,	"","MLNMissile",	10,false };
	CodePatch constructorsPatch13 =		{ ptrNewEnergy,		"","MLNEnergy",		9,false };
	CodePatch constructorsPatch14 =		{ ptrNewBeam,		"","MLNBeam",		7,false };
	CodePatch constructorsPatch15 =		{ ptrNewMine,		"","MLNMine",		7,false };
	CodePatch constructorsPatch16 =		{ ptrNewBomb,		"","MLNBomb",		7,false };
	CodePatch constructorsPatch17 =		{ ptrNewMountable,	"","MLNMountable",	12,false };
	CodePatch constructorsPatch39 =		{ ptrMountInfo1,	"","MLmntInfo1",	10,false };
	CodePatch constructorsPatch19 =		{ ptrNewEngine,		"","MLNEngine",		9,false };
	CodePatch constructorsPatch20 =		{ ptrEngineInfo1,	"","MLengiInfo1",	11,false };
	CodePatch constructorsPatch21 =		{ ptrNewSensor,		"","MLNSensor",		9,false };
	CodePatch constructorsPatch22 =		{ ptrSensorInfo1,	"","MLsensInfo1",	11,false };
	CodePatch constructorsPatch23 =		{ ptrNewReactor,	"","MLNReactor",	10,false };
	CodePatch constructorsPatch24 =		{ ptrReactorInfo1,	"","MLreactInfo1",	12,false };
	CodePatch constructorsPatch25 =		{ ptrNewShield,		"","MLNShield",		9,false };
	CodePatch constructorsPatch26 =		{ ptrShieldInfo1,	"","MLshldInfo1",	11,false };
	CodePatch constructorsPatch27 =		{ ptrNewModulator,	"","MLNModulator",	12,false };
	CodePatch constructorsPatch28 =		{ ptrNewAmplifier,	"","MLNAmplifier",	12,false };
	CodePatch constructorsPatch29 =		{ ptrNewCapacitor,	"","MLNCapacitor",	12,false };
	CodePatch constructorsPatch30 =		{ ptrNewComputer,	"","MLNComputer",	11,false };
	CodePatch constructorsPatch31 =		{ ptrNewBooster,	"","MLNBooster",	10,false };
	CodePatch constructorsPatch32 =		{ ptrNewRepair,		"","MLNRepair",		9,false };
	CodePatch constructorsPatch33 =		{ ptrNewCloak,		"","MLNCloak",		8,false };
	CodePatch constructorsPatch34 =		{ ptrNewArmor,		"","MLNArmor",		8,false };
	CodePatch constructorsPatch35 =		{ ptrArmorInfo1,	"","MLarmInfo1",	10,false };
	CodePatch constructorsPatch36 =		{ ptrNewECM,		"","MLNECM",		6,false };
	CodePatch constructorsPatch37 =		{ ptrNewThermal,	"","MLNThermal",	10,false };
	CodePatch constructorsPatch38 =		{ ptrNewBattery,	"","MLNBattery",	10,false };

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


	//Move the "Variable Ref. Before Assign." verboose to $console::printlevel of 3. Also fix the typo and reword the verboose.
	MultiPointer(ptrVarRefBefAssign, 0, 0, 0x005E4DCD, 0x005E8671);
	CodePatch VarRefBefAssign_patch = { ptrVarRefBefAssign, "", "\x03", 1, false };
	MultiPointer(ptrVarRefBefAssignVerb, 0, 0, 0x00712ECB, 0x0072333B);
	CodePatch VarRefBefAssignVerb_patch = { ptrVarRefBefAssignVerb, "", "%s referenced before it has been assigned", 41, false };

	//BuiltInFunction("patchWindowedOGL", _pwogl)
	//{
	//	const char* str = argv[0];
	//	std::string arg1 = str;
	//	if (arg1.compare("true") == 0)
	//	{
	//		patchchangedisplaysettings.DoctorRelative((u32)patchChangeDisplaySettings, 1).Apply(true);
	//	}
	//	else if (arg1.compare("false") == 0)
	//	{
	//		patchchangedisplaysettings.DoctorRelative((u32)unpatchChangeDisplaySettings, 1).Apply(true);
	//	}
	//	return 0;
	//}

	void LeftClick()
	{
		INPUT    Input = { 0 };
		// left down 
		Input.type = INPUT_MOUSE;
		Input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
		::SendInput(1, &Input, sizeof(INPUT));

		// left up
		::ZeroMemory(&Input, sizeof(INPUT));
		Input.type = INPUT_MOUSE;
		Input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
		::SendInput(1, &Input, sizeof(INPUT));
	}

	BuiltInFunction("client::performLeftClick", _cplc)
	{
		LeftClick();
		return 0;
	}
	//BuiltInFunction("memstar::codePatch_INTERNAL", _mcp)
	//{
		//if (!strlen(argv[0]) || !strlen(argv[1]))
		//{
			//Console::echo("memstar::codePatch(address, hexString);");
			//return 0;
		//}
			//uint64_t address;
			//std::istringstream(argv[0]) >> std::hex >> address;
			//string hexStr = argv[1];
			//Parse the hexString
			//string rawHexString = hexStr.c_str();
			//Plug into CodePatch
			//char* rawHexString_cstr = const_cast<char*>(rawHexString.c_str());
			//int byteLen = strlen(hex2char(rawHexString_cstr));
			//CodePatch consoleCodePatch = { address,"",hex2char(rawHexString_cstr),byteLen,false }; consoleCodePatch.Apply(true);
		//return "true";
	//}
	///

	//theTeamGUI modloader compat fix
	MultiPointer(ptrTeamgui, 0, 0, 0x0054D320, 0x0054F854);
	CodePatch teamPicRenderCompat = { ptrTeamgui,	"","\xC3",		1,false };

	static const char* GameEndFrame = "Engine::EndFrame";
	MultiPointer(ptrEndFrameFunctionCall, 0, 0, 0x0059D774, 0x005A0F90);
	MultiPointer(ptrEndFrameFunctionCallRetn, 0, 0, 0x0059D780, 0x005A0F97);
	//MultiPointer(ptrConsoleEval, 0, 0, 0x00712B34, 0x00722FA4);
	CodePatch gameendframepatch = { ptrEndFrameFunctionCall,	"","\xE9GMEF",		5,false };
	NAKED void GameEndFramePatch() {
		__asm {
			push [GameEndFrame]
			push 1
			jmp [ptrEndFrameFunctionCallRetn]
		}
	}

	MultiPointer(ptrTerrainVolume, 0, 0, 0x00773D68, 0x00784380);
	BuiltInFunction("getTerrainGridFile", _getTerrainGridFile)
	{
		uintptr_t ptr1 = ptrTerrainVolume;
		char* ptr1_string = reinterpret_cast<char*>(ptr1);
		if (!strlen(ptr1_string))
		{
			Console::echo("Simterrain not found.");
			return 0;
		}
		else
		{
			return ptr1_string;
		}
		return 0;
	}

	MultiPointer(ptrCampaignInit, 0, 0, 0x00557F37, 0x0055AD73);
	MultiPointer(ptrCampaignResume, 0, 0, 0x00557F4C, 0x0055AD88);
	CodePatch campaigninit = { ptrCampaignInit, "", "\xE9_CPI", 5, false };
	static const char* modloader_append_pilots = "exec($temp);modloader::appendPilotData();";
	NAKED void CampaignInit() {
		__asm {
			push eax
			mov eax, [modloader_append_pilots]
			push eax
			call Console::eval
			add esp, 0x8
			//pop eax
			jmp[ptrCampaignResume]
		}
	}
	MultiPointer(ptrVehicleCollMeshRender, 0, 0, 0x004836EF, 0x00485923);
	BuiltInFunction("Nova::toggleCollisionMesh", _novatogglecollisionmesh)
	{
		std::string var = Console::getVariable("pref::collisionMesh");
		if (var.compare("1") == 0)
		{
			CodePatch coll0Mesh = { ptrVehicleCollMeshRender, "", "\x75", 1, false };
			coll0Mesh.Apply(true);
		}
		else
		{
			CodePatch coll0Mesh = { ptrVehicleCollMeshRender, "", "\x74", 1, false };
			coll0Mesh.Apply(true);
		}
		//Console::eval("$pref::collisionMesh^=1;");
		return "true";
	}

	struct Init {
		Init() {
			//Internal
			gameendframepatch.DoctorRelative((u32)GameEndFramePatch, 1).Apply(true);

			if(VersionSnoop::GetVersion() == VERSION::v001004)
			{
				//constructorBypass01.Apply(true); //No longer used
				constructorBypass02.Apply(true);
				constructorBypass03.Apply(true);
			}
			if (std::filesystem::exists("Nova.vol"))
			{
				teamPicRenderCompat.Apply(true);
				constructorsPatch1.Apply(true);
				constructorsPatch1a.Apply(true);
				constructorsPatch2.Apply(true);
				constructorsPatch3.Apply(true);
				constructorsPatch3a.Apply(true);
				constructorsPatch4.Apply(true);
				constructorsPatch5.Apply(true);
				constructorsPatch5a.Apply(true);
				constructorsPatch6.Apply(true);
				constructorsPatch6a.Apply(true);
				constructorsPatch7.Apply(true);
				constructorsPatch8.Apply(true);
				constructorsPatch9.Apply(true);
				constructorsPatch10.Apply(true);
				constructorsPatch10a.Apply(true);
				//constructorsPatch11.Apply(true);
				//constructorsPatch12.Apply(true);
				//constructorsPatch13.Apply(true);
				//constructorsPatch14.Apply(true);
				//constructorsPatch15.Apply(true);
				//constructorsPatch16.Apply(true);
				constructorsPatch17.Apply(true);
				constructorsPatch19.Apply(true);
				constructorsPatch20.Apply(true);
				constructorsPatch21.Apply(true);
				constructorsPatch22.Apply(true);
				constructorsPatch23.Apply(true);
				constructorsPatch24.Apply(true);
				constructorsPatch25.Apply(true);
				constructorsPatch26.Apply(true);
				constructorsPatch27.Apply(true);
				constructorsPatch28.Apply(true);
				constructorsPatch29.Apply(true);
				constructorsPatch30.Apply(true);
				constructorsPatch31.Apply(true);
				constructorsPatch32.Apply(true);
				constructorsPatch33.Apply(true);
				constructorsPatch34.Apply(true);
				constructorsPatch35.Apply(true);
				constructorsPatch36.Apply(true);
				constructorsPatch37.Apply(true);
				constructorsPatch38.Apply(true);
				constructorsPatch39.Apply(true);
			}
			tagDictionary_patch.Apply(true);
			tagDictionary_patch.Apply(true);
			tagDefinitionBypass1_patch.Apply(true);
			tagDefinitionBypass2_patch.Apply(true);
			f_cr_sub0.Apply(true);
			f_cr_sub1.Apply(true);
			f_cr_sub2.Apply(true);
			VarRefBefAssign_patch.Apply(true);
			VarRefBefAssignVerb_patch.Apply(true);

			//Rendering
			//Damage Status Display function: 46668B  (Colors)
			simgui_guibitmapctrl_defaultimage.DoctorRelative((u32)Simgui_GuiBitmapCtrl_DefaultImage, 1).Apply(true);

			terrainMaxVisDistance_patch.Apply(true);
			canvasWindowMaxWindowedSize_patch.Apply(true);
			canvasWindowMaxInteralRenderSizeWidth_patch.Apply(true);
			canvasWindowMaxInteralRenderSizeHeight_patch.Apply(true);

			//Networking
			HercNetUpdate1.Apply(true);
			HercNetUpdate2.Apply(true);
			HercNetUpdate3.Apply(true);
			TankNetUpdate1.Apply(true);
			TankNetUpdate2.Apply(true);
			TankNetUpdate3.Apply(true);

			remoteEvalBufferSize_S_patch.Apply(true);
			remoteEvalBufferSize_R_patch.Apply(true);
			packetRate_patch.Apply(true);
			packetRateCheck_patch.Apply(true);
			packetRateDefault_patch.Apply(true);
			packetRateDefaultMax_patch.Apply(true);
			loopback_packetRate_patch.Apply(true);
			//packetSize_patch.Apply(true);
			ignoreMissingServerVolume_patch.Apply(true);
			invalidPackets1_patch.Apply(true);
			invalidPackets2_patch.Apply(true);
			missingTerrain_patch.Apply(true);
			//Fix clients having higher packet rates than the server causing the server to crash
			badPacketRate_patch.Apply(true);

			//Gameplay
			//playerflyerelevationcontrol.DoctorRelative((u32)PlayerFlyerElevationControl, 1).Apply(true);//Allow player flyers to adjust fly height using the throttle
			tankalignmentspeed.DoctorRelative((u32)tankAlignmentSpeed, 1).Apply(true);

			weaponShotCap_patch.Apply(true);
				//Allow AllowVehicle(); clientSide for 1.004r
			allowVehiclePatch1.Apply(true);
			allowVehiclePatch2.Apply(true);

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
			CreateDirectory(".\\mods\\session", NULL);
			CreateDirectory(".\\temp", NULL);
			CreateDirectory(".\\savedGames", NULL);

			//Modloader: Append pilot data to campaign
			campaigninit.DoctorRelative((u32)CampaignInit, 1).Apply(true);

			//For Upscaling OpenGL
			//Tiles the Simgui::BitmapCTRL so that the background fits the upscale
			//MOVE TO FUNCTIONS
			//guibitmapctrl_tile.DoctorRelative((u32)GuiBitmapCTRL_Tile, 1).Apply(true);

			//Enumerate OpenGL resolutions up to the desktop resolution
			OpenGLenumDesktopModes();
		}
	} init;
	}
