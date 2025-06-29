#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"
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
#include <algorithm>
#include "Fear.h"

using namespace Fear;
using namespace std;

namespace OpenGLFixes
{
	u32 dummy, dummy2, dummy3, dummy4, dummy5;
	CodePatch Surface_TextureCache_sm_cacheMagic = { 0x0072A5A4, "", "\x00\x00\x00\x01", 4, false };
	const uint32_t TextureCache_csm_sizeIndices[10] = { 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096};
	const uint32_t TextureCache_csm_cacheDSize[10] = { 4096, 4096, 4096, 4096, 4096, 4096, 4096, 4096, 4096, 4096};
	int num_ofIndices = sizeof(TextureCache_csm_sizeIndices) / sizeof(TextureCache_csm_sizeIndices[0]);

	MultiPointer(ptrSurfaceTextureCacheGetArena, 0, 0, 0, 0x0064CEF0);
	CodePatch surfacetexturecachegetarena = { ptrSurfaceTextureCacheGetArena, "", "\xE9STCH", 5, false };
	NAKED void SurfaceTextureCacheGetArena()
	{
		__asm {
			xor eax, eax
			mov ecx, offset TextureCache_csm_sizeIndices
			
			jl__:
			cmp edx, [ecx]
			jbe retn__
			inc eax
			add ecx, 4
			cmp eax, num_ofIndices
			jl  jl__
			mov eax, 0xF00FF00F

			retn__:
			retn

		}
	}

	MultiPointer(ptrGWCanvas_onSysKey, 0, 0, 0, 0x0057AFD0);
	MultiPointer(ptrGWCanvas_onSysKeyResume, 0, 0, 0, 0x0057AFD6);
	CodePatch disableF10_sysMenu = { ptrGWCanvas_onSysKey, "", "\xE9SYSM", 5, false };
	NAKED void Disable_F10_sysMenu()
	{
		__asm {
			push ebp
			mov  ebp, esp
			push ebx
			mov  ebx, edx
			cmp  edx, 0x79 //VK_F10 system menu
			je __abort
			jmp ptrGWCanvas_onSysKeyResume

			__abort:
			pop ebx
			pop ebp
			retn
		}
	}

	//Switch the texture internal format to GL_RGBA32F
	MultiPointer(ptrTexture_interalFormat, 0, 0, 0, 0x0064D145);
	CodePatch use_GL_RGBA32F = { ptrTexture_interalFormat, "", "\x14\x88", 2, false };

	MultiPointer(ptrBitmapCaching, 0, 0, 0, 0x0064D19B);
	MultiPointer(ptrBitmapCachingResume, 0, 0, 0, 0x0064D1AB);
	//CodePatch bitmapcachingPAD = { ptrBitmapCaching, "", "\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90", 16, false };
	CodePatch bitmapcaching = { ptrBitmapCaching, "", "\xE9_BMC", 5, false };
	NAKED void BitmapCaching()
	{
		__asm {
		mov dummy, eax
		lea eax, TextureCache_csm_sizeIndices
		mov[esp + 0x30 - 0x18], eax

		lea eax, TextureCache_csm_cacheDSize
		mov[esp + 0x30 - 0x1C], eax

		mov eax, dummy
		jmp ptrBitmapCachingResume
		}
	}

	//Patch to handle custom opengl32.dll's
	MultiPointer(wglGetDefaultProcAddress, 0, 0, 0x0071B86F, 0x0072BE5B);
	CodePatch wglGetDefaultProcAddressPatch = { wglGetDefaultProcAddress, "", "glGetTexLevelParameteriv", 24, false };


	HWND getHWND() {
		MultiPointer(ptrHWND, 0, 0, 0x00705C5C, 0x007160CC);
		uintptr_t HWND_PTR = ptrHWND;
		int GAME_HWND = *reinterpret_cast<int*>(HWND_PTR);
		HWND SS_HWND = reinterpret_cast<HWND>(GAME_HWND);
		return SS_HWND;
	}

	BuiltInVariable("pref::WIN_POS_X", int, prefwinposx, 0);
	BuiltInVariable("pref::WIN_POS_Y", int, prefwinposy, 0);
	BuiltInFunction("Nova::handleLoseFocus", novahandlelosefocus) { return 0; }
	//void SetWindowPriority()
	//{
	//	RECT window;
	//	GetWindowRect(getHWND(), &window);
	//	int x = window.left;
	//	int y = window.top;
		//Console::eval("bind(mouse,xaxis,TO,\"Nova::handleLoseFocus();Nova::sendWindowToFront();unbind(mouse,xaxis,make);\"); ");
	//	SetWindowPos(getHWND(), HWND_NOTOPMOST, x, y, 0, 0, SWP_NOSIZE );
	//}

	MultiPointer(ptrMinimizeWindowCall, 0, 0, 0x005777E5, 0x0057A9ED);
	MultiPointer(ptrMinimizeWindowCallRetn, 0, 0, 0x00577812, 0x0057AA1A);
	CodePatch minimizecallintercept = { ptrMinimizeWindowCall, "", "\xE9MWCL", 5, false };
	NAKED void MinimizeCallIntercept()
	{
		__asm {
			//call SetWindowPriority
			jmp ptrMinimizeWindowCallRetn
		}
	}

	MultiPointer(ptrMinimizeWindowMessage, 0, 0, 0x005777FD, 0x0057AA05);
	BuiltInFunction("Canvas::autoMinimize", canvasautominimize)
	{
		const char* str = argv[0];
		std::string arg1 = str;
		if (arg1.compare("false") == 0)
		{
			CodePatch WindowMinimizeOnLoseFocus = { ptrMinimizeWindowMessage,"","\x20",1,false };
			WindowMinimizeOnLoseFocus.Apply(true);
		}
		else if (arg1.compare("true") == 0)
		{
			CodePatch WindowMinimizeOnLoseFocus = { ptrMinimizeWindowMessage,"","\x30",1,false };
			WindowMinimizeOnLoseFocus.Apply(true);
		}
		return 0;
	}

	BuiltInFunction("Nova::getDesktopResolution", _novagetdesktopresolution)
	{
		RECT desktop;
		const HWND hDesktop = GetDesktopWindow();
		GetWindowRect(hDesktop, &desktop);
		int width = desktop.right;
		int height = desktop.bottom;
		char res[16];
		strcpy(res, tostring(width));
		strcat(res, "x");
		strcat(res, tostring(height));
		return res;
	}

	MultiPointer(ptrChangeDisplaySettings, 0, 0, 0x0063CEF0, 0x0064BE30);
	MultiPointer(ptrChangeDisplaySettingsByte, 0, 0, 0x0083BC04, 0x008528BC);
	MultiPointer(ptrChangeDisplaySettingsCont, 0, 0, 0x0063CEFC, 0x0064BE3C);
	CodePatch patchchangedisplaysettings = { ptrChangeDisplaySettings, "", "\xE9XDST", 5, false };
	NAKED void patchChangeDisplaySettings()
	{
		__asm {
			call ChangeDisplaySettingsW
			mov ptrChangeDisplaySettingsByte, 1
			jmp[ptrChangeDisplaySettingsCont]
		}
	}

	NAKED void unpatchChangeDisplaySettings()
	{
		__asm {
			call ChangeDisplaySettingsA
			mov ptrChangeDisplaySettingsByte, 1
			jmp[ptrChangeDisplaySettingsCont]
		}
	}

	MultiPointer(ptrOGLFullScreen1, 0, 0, 0x0063CE88, 0x0064BDC8);
	MultiPointer(ptrOGLFullScreen2, 0, 0, 0x0063CEEA, 0x0064BE2A);
	BuiltInFunction("OpenGL::windowedFullscreen", _openglwindowedfullscreen)
	{
		const char* str = argv[0];
		std::string arg1 = str;
		//Vector2i screen;
		//Fear::getScreenDimensions(&screen);
		//HWND windowHandle = FindWindowA(NULL, "Starsiege");
		if (arg1.compare("false") == 0)
		{
			CodePatch WindowMinimizeOnLoseFocus = { ptrMinimizeWindowMessage,"","\x20",1,false };
			WindowMinimizeOnLoseFocus.Apply(true);
			CodePatch genericCodePatch = { ptrOGLFullScreen1,"","\xF0",1,false };
			genericCodePatch.Apply(true);
			CodePatch genericCodePatch0 = { ptrOGLFullScreen2,"","\x04",1,false };
			genericCodePatch0.Apply(true);
			//patchchangedisplaysettings.DoctorRelative((u32)unpatchChangeDisplaySettings, 1).Apply(true);
		}
		else if (arg1.compare("true") == 0)
		{
			CodePatch WindowMinimizeOnLoseFocus = { ptrMinimizeWindowMessage,"","\x30",1,false };
			WindowMinimizeOnLoseFocus.Apply(true);
			CodePatch genericCodePatch = { ptrOGLFullScreen1,"","\x00",1,false };
			//genericCodePatch.Apply(true);
			CodePatch genericCodePatch0 = { ptrOGLFullScreen2,"", "\x0A",1,false}; //CDS_FULLSCREEN
			genericCodePatch0.Apply(true);
			//patchchangedisplaysettings.DoctorRelative((u32)patchChangeDisplaySettings, 1).Apply(true);
			//SetWindowPos(windowHandle, HWND_TOP, 0, 0, screen.x, screen.y, 0);
		}
		return "true";
	}

	static char* guiloadL = "guiload(*100014);#";
	MultiPointer(ptrLoadingGuiPatch0, 0, 0, 0x006F6BD0, 0x00706E5C);
	MultiPointer(ptrLoadingGuiPatch1, 0, 0, 0x006F94D3, 0x007097B3);
	MultiPointer(ptrLoadingGuiPatch2, 0, 0, 0x006FA248, 0x0070A528);
	MultiPointer(ptrWindowSettings,	  0, 0, 0x0063CEFC, 0x0064BE3C);
	CodePatch GuiLoadLoad0 = { ptrLoadingGuiPatch0, "", guiloadL, 19, false };
	CodePatch GuiLoadLoad1 = { ptrLoadingGuiPatch1, "", guiloadL, 19, false };
	CodePatch GuiLoadLoad2 = { ptrLoadingGuiPatch2, "", guiloadL, 19, false };
	CodePatch windowproperiespatch = { ptrWindowSettings, "", "\xE9WPRP", 5, false };
	MultiPointer(ptrWindowSettingsCont, 0, 0, 0x0063CF04, 0x0064BE44);
	NAKED void WindowPropertiesPatch() {
		__asm {
			push ebp
			call SetForegroundWindow
			push SWP_NOZORDER
			jmp [ptrWindowSettingsCont]
		}
	}

	//Wireframing
	//MultiPointer(ptrTerrainOpenGLDraw_TEX0, 0, 0, 0, 0x00652B7C);
	//MultiPointer(ptrTerrainOpenGLDraw_TEX1, 0, 0, 0, 0x00652B84);
	MultiPointer(ptrTerrainOpenGLDraw, 0, 0, 0, 0x006524BF);
	//1.0003r <Terrain and Interior are tied to the same GLenabled
	MultiPointer(ptrTerrIntOpenGLDraw, 0, 0, 0x00642E96, NULL);

	MultiPointer(ptrHazeOpenGLDraw, 0, 0, 0x0064213B, 0x00651AC3);
	MultiPointer(ptrHazeEdgeOpenGLDraw, 0, 0, 0x006421A4, 0x00651B2C);

	MultiPointer(ptrNonTexturedFaceOpenGLDraw, 0, 0, 0x00643163, 0x00652D24);

	MultiPointer(ptrSkyOpenGLDraw, 0, 0, 0, 0x00652F7B);
	MultiPointer(ptrSkyOpenGLDraw_f_RED, 0, 0, 0x006438BD, 0x00653279);
	MultiPointer(ptrSkyOpenGLDraw_f_GREEN, 0, 0, 0x006438C4, 0x00653280);
	MultiPointer(ptrSkyOpenGLDraw_f_BLUE, 0, 0, 0x006438CB, 0x00653287);

	MultiPointer(ptrInteriorOpenGLDraw, 0, 0, 0, 0x00652A71);

	MultiPointer(ptrDTSOpenGLDraw, 0, 0, 0, 0x00652F7A);
	//MultiPointer(ptrInteriorOpenGLDraw_TEX1, 0, 0, 0, 0x00652A49);
	//MultiPointer(ptrInteriorOpenGLDraw_TEX0, 0, 0, 0, 0x00652A51);

	//Shadow Multiplier
	MultiPointer(ptrShadowMult, 0, 0, 0, 0x006505A6);

	//Game canvas window resizable
	MultiPointer(ptrWindowResizable, 0, 0, 0, 0x005A255E);

	//OpenGL Gui Object Internal Scale
	MultiPointer(ptrOpenGLGuiIntScale, 0, 0, 0x006425E8, 0x00651F70);
	CodePatch BitmapCtrlLineFix = { ptrOpenGLGuiIntScale, "", "\xED\x0D\xBE\xBA", 4, false }; //Fix Simgui::BitmapCtrl line artifacts when upscaling the OpenGL renderer

	//IntroGUI to MainmenuGUI crash fix for OGL Upscaler
	MultiPointer(ptrIntroToMain, 0, 0, 0, 0x00545635);
	MultiPointer(ptrIntroToMainResume, 0, 0, 0, 0x0054563F);
	CodePatch introtomaincrashfix = { ptrIntroToMain, "", "\xE9ITOM", 5, false };
	NAKED void IntroToMainCrashFix()
	{
		__asm {
			test eax, eax 
			jno __crashEscape
			mov dword ptr [eax + 0x2F4], 0x1FBDC
			jmp[ptrIntroToMainResume] //Jump back into the native function
			__crashEscape:
				pop esi
				pop ebx
				retn
		}
	}

	BuiltInFunction("OpenGL::Wireframe", _oglwf)
	{
		if (argc != 1)
		{
			return 0;
		}
		const char* str = argv[0];
		std::string arg1 = str;
		if (arg1.compare("true") == 0 || arg1.compare("1") == 0)
		{
			//Terrain
			//CodePatch TEROpenGLWired_TEX0 = { ptrTerrainOpenGLDraw_TEX0, "", "\x00\x00\x00\x00", 4, false }; TEROpenGLWired_TEX0.Apply(true);
			//CodePatch TEROpenGLWired_TEX1 = { ptrTerrainOpenGLDraw_TEX1, "", "\x00\x00\x00\x00", 4, false }; TEROpenGLWired_TEX1.Apply(true);
			CodePatch TEROpenGLWired = { ptrTerrainOpenGLDraw, "", "\x02", 1, false }; TEROpenGLWired.Apply(true);

			//Terrain/Interior (1.0003r)
			CodePatch TERDISOpenGLWired = { ptrTerrIntOpenGLDraw, "", "\x02", 1, false }; TERDISOpenGLWired.Apply(true);
			
			//Map Haze
			CodePatch HAZOpenGLWired0 = { ptrHazeOpenGLDraw, "", "\x00", 1, false }; HAZOpenGLWired0.Apply(true);
			CodePatch HAZOpenGLWired1 = { ptrHazeEdgeOpenGLDraw, "", "\x00", 1, false }; HAZOpenGLWired1.Apply(true);

			//Interiors
			//CodePatch DISOpenGLWired_TEX0 = { ptrInteriorOpenGLDraw_TEX0, "", "\x00\x00\x00\x00", 4, false }; DISOpenGLWired_TEX0.Apply(true);
			//CodePatch DISOpenGLWired_TEX1 = { ptrInteriorOpenGLDraw_TEX1, "", "\x00\x00\x00\x00", 4, false }; DISOpenGLWired_TEX1.Apply(true);
			CodePatch DISOpenGLWired = { ptrInteriorOpenGLDraw, "", "\x02", 1, false }; DISOpenGLWired.Apply(true);

			//Sky RGB
			CodePatch SKYOpenGLcolorR = { ptrSkyOpenGLDraw_f_RED, "", "\x00\x00\x00\x00", 4, false }; SKYOpenGLcolorR.Apply(true);
			CodePatch SKYOpenGLcolorG = { ptrSkyOpenGLDraw_f_GREEN, "", "\x00\x00\x00\x00", 4, false }; SKYOpenGLcolorG.Apply(true);
			CodePatch SKYOpenGLcolorB = { ptrSkyOpenGLDraw_f_BLUE, "", "\x00\x00\x00\x00", 4, false }; SKYOpenGLcolorB.Apply(true);
		}
		else
		{
			//Terrain
			//CodePatch TEROpenGLWired_TEX1 = { ptrTerrainOpenGLDraw_TEX0, "", "\x01\x1E\x00\x00", 4, false }; TEROpenGLWired_TEX1.Apply(true);
			//CodePatch TEROpenGLWired_TEX0 = { ptrTerrainOpenGLDraw_TEX1, "", "\x00\x20\x00\x00", 4, false }; TEROpenGLWired_TEX0.Apply(true);
			CodePatch TEROpenGLWired = { ptrTerrainOpenGLDraw, "", "\x06", 1, false }; TEROpenGLWired.Apply(true);

			//Terrain/Interior (1.0003r)
			CodePatch TERDISOpenGLWired = { ptrTerrIntOpenGLDraw, "", "\x06", 1, false }; TERDISOpenGLWired.Apply(true);

			//Map Haze
			CodePatch HAZOpenGLWired0 = { ptrHazeOpenGLDraw, "", "\x04", 1, false }; HAZOpenGLWired0.Apply(true);
			CodePatch HAZOpenGLWired1 = { ptrHazeEdgeOpenGLDraw, "", "\x06", 1, false }; HAZOpenGLWired1.Apply(true);

			//Interiors
			//CodePatch DISOpenGLWired_TEX1 = { ptrInteriorOpenGLDraw_TEX0, "", "\x00\x20\x00\x00", 4, false }; DISOpenGLWired_TEX1.Apply(true);
			//CodePatch DISOpenGLWired_TEX0 = { ptrInteriorOpenGLDraw_TEX1, "", "\x01\x1E\x00\x00", 4, false }; DISOpenGLWired_TEX0.Apply(true);
			CodePatch DISOpenGLWired = { ptrInteriorOpenGLDraw, "", "\x06", 1, false }; DISOpenGLWired.Apply(true);

			//Sky RGB
			CodePatch SKYOpenGLcolorR = { ptrSkyOpenGLDraw_f_RED, "", "\x00\x00\x80\x3F", 4, false }; SKYOpenGLcolorR.Apply(true);
			CodePatch SKYOpenGLcolorG = { ptrSkyOpenGLDraw_f_GREEN, "", "\x00\x00\x80\x3F", 4, false }; SKYOpenGLcolorG.Apply(true);
			CodePatch SKYOpenGLcolorB = { ptrSkyOpenGLDraw_f_BLUE, "", "\x00\x00\x80\x3F", 4, false }; SKYOpenGLcolorB.Apply(true);
		}
		return 0;
	}

	MultiPointer(ptrHercCameraAntialiasing, 0, 0, 0, 0x0049FCBC);
	MultiPointer(ptrTankCameraAntialiasing, 0, 0, 0, 0x00433040);
	CodePatch AMDhercAA = { ptrHercCameraAntialiasing, "\xCD\xCC\xCC\x3E", "\x00\x00\x20\x3F", 4, false };
	CodePatch AMDtankAA = { ptrTankCameraAntialiasing, "\xCD\xCC\xCC\x3E", "\x00\x00\x60\x3F", 4, false };
	BuiltInFunction("OpenGL::AMDAATweak", _opengladjustamdantialiasing)
	{
		if (argc != 1)
		{
			return 0;
		}
		const char* str = argv[0];
		std::string arg1 = str;
		if (arg1.compare("true") == 0 || arg1.compare("1") == 0 || arg1.compare("True") == 0)
		{
			AMDhercAA.Apply(true);
			AMDtankAA.Apply(true);
		}
		else
		{
			AMDhercAA.Apply(false);
			AMDtankAA.Apply(false);
		}
	}

	MultiPointer(ptrCanvasSurfaceChanged, 0, 0, 0, 0x005C8F60);
	CodePatch disableCanvasForcedOverlap = { ptrCanvasSurfaceChanged, "", "\xC3", 1, false };

	//OPENGL SCALING FUNCTIONS
	//Scaling
	MultiPointer(ptrGuiScale, 0, 0, 0x0063D9BE, 0x0064C916);
	MultiPointer(ptrGuiScaleResume, 0, 0, 0x0063D9CD, 0x0064C92B);
	//CodePatch setguiscale = { ptrGuiScale, "", "\xE9SGUS", 5, false };
	//float guiScale = 2;
	//NAKED void setGuiScale() {
	//	__asm {
	//		fdivr guiScale
	//		add esp, 0FFFFFFFCh
	//		fstp [esp + 0x14 - 0x14]
	//		fild dword ptr [ebx + 0x3C]
	//		fdivr guiScale
	//		jmp[ptrGuiScaleResume]
	//	}
	//}
	//
	//void cmdSetGuiScale(float flt)
	//{
	//	guiScale = flt;
	//	setguiscale.DoctorRelative((u32)setGuiScale, 1).Apply(true);
	//}
	//
	//BuiltInFunction("OpenGL::scaleGUI", _openglscalegui) {
	//	if (argc != 1 || atoi(argv[0]) < 2)
	//	{
	//		Console::echo("%s( int/flt ); Changes the internal GUI rendering scale. Min Value: 2", self);
	//		return 0;
	//	}
	//	cmdSetGuiScale(atof(argv[0]));
	//	return "true";
	//}
	 
	//
	//void cmdSetGuiShift(float flt)
	//{
	//	guiShift = flt;
	//	setguishift.DoctorRelative((u32)setGuiShift, 1).Apply(true);
	//}
	//
	//BuiltInFunction("OpenGL::shiftGUI", _openglshiftgui) {
	//	if (argc != 1 || atoi(argv[0]) < -1)
	//	{
	//		Console::echo("%s( int/flt ); Shifts the internal GUI rendering to the right. Min Value: -1", self);
	//		return 0;
	//	}
	//	cmdSetGuiShift(atof(argv[0]));
	//	return "true";
	//}

	//u32 PFD_OGL_BITS = 0x24;
	//
	//void GDIvarBool()
	//{
	//	//Don't read the file as binary
	//	std::ifstream fin("defaultPrefs.cs", std::ios::in); //Input file
	//	std::vector<std::string> field{ "$pref::UseGDI = \"1\";" }; //String to find
	//	std::string fileContent(std::istreambuf_iterator<char>(fin), {});
	//	for (const std::string& l : field)
	//	{
	//		if (std::search(fileContent.begin(), fileContent.end(), l.begin(), l.end()) != fileContent.end())
	//		{
	//			PFD_OGL_BITS = 0x1034;
	//			break;
	//		}
	//	}
	//}

	MultiPointer(ptrOpenGL_dwFlags, 0, 0, 0, 0x0064BA89);
	MultiPointer(ptrOpenGL_dwFlags_resume, 0, 0, 0, 0x0064BA95);
	CodePatch gdi_opengl = { ptrOpenGL_dwFlags, "", "\xE9OGLF", 5, false };
	NAKED void GDI_OpenGL() {
		__asm {
			//mov dummy, eax
			//call GDIvarBool
			//mov eax, dummy
			mov edx, PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_GENERIC_ACCELERATED// PFD_DRAW_TO_WINDOW (0x4) | PFD_SUPPORT_GDI (0x10)
			jmp [ptrOpenGL_dwFlags_resume]
		}
	}

	//
	//BuiltInVariable("pref::UseGDI", bool, prefusegdi, false);
	////And our function to toggle GDI
	//BuiltInFunction("Nova::UseGDI", _novatogglegdi)
	//{
	//	if (!argv[0])
	//	{
	//		argv[0] = "false";
	//	}
	//	const char* arg0 = argv[0];
	//	std::string boolean = arg0;
	//	if (boolean.compare("false") == 0 || boolean.compare("False") == 0 || boolean.compare("0") == 0)
	//	{
	//		Console::eval("deleteVariables(\"pref::UseGDI\");");
	//	}
	//	if (boolean.compare("true") == 0 || boolean.compare("True") == 0 || boolean.compare("1") == 0)
	//	{
	//		Console::setVariable("pref::UseGDI", "True");
	//	}
	//	Console::eval("export('pref::*', 'defaultPrefs.cs');");
	//	return "true";
	//}

	//CodePatch tempPatch = { 0x0065FF39, "", "\x0F\xFC\x65\x00", 4, false };
	CodePatch tempPatch = { 0x0065FF39, "", "\x81\xED\x65\x00", 4, false };
	CodePatch wglinfowindow_bypass = { 0x0064B674, "", "\xEB", 1, false };


	//Use alternative interior surface rendering instead of the OpenGL interior surface rendering
	MultiPointer(ptrSoftwareInteriorRendering, 0, 0, 0, 0x0061C3C3);
	CodePatch interiorsurfacerendering = { ptrSoftwareInteriorRendering, "", "\x74", 1, false };

	struct Init {
		Init() {
			wglGetDefaultProcAddressPatch.Apply(true);
			//GuiLoadLoad0.Apply(true);
			//GuiLoadLoad1.Apply(true);
			//GuiLoadLoad2.Apply(true);
			//WndInsertAfter0.Apply(true);
			//wglinfowindow_bypass.Apply(true);
			windowproperiespatch.DoctorRelative((u32)WindowPropertiesPatch, 1).Apply(true);
			//BitmapCtrlLineFix.Apply(true); //Hide seams in chunked bitmaps //CAUSES ARTIFACTS IN RESOLUTION SCALING DOWN
			//introtomaincrashfix.DoctorRelative((u32)IntroToMainCrashFix, 1).Apply(true);

			//goSplash640.Apply(true);
			//goSplash480.Apply(true);
			//tempPatch.Apply(true);
			//gdi_opengl.DoctorRelative((u32)GDI_OpenGL, 1).Apply(true);

			//Expanded cache sizes and indices
			surfacetexturecachegetarena.DoctorRelative((u32)SurfaceTextureCacheGetArena, 1).Apply(true);
			bitmapcaching.DoctorRelative((u32)BitmapCaching, 1).Apply(true);

			//Surface_TextureCache_sm_cacheMagic.Apply(true);
			//interiorsurfacerendering.Apply(true);

			//use_GL_RGBA32F.Apply(true);

			disableF10_sysMenu.DoctorRelative((u32)Disable_F10_sysMenu, 1).Apply(true);

			minimizecallintercept.DoctorRelative((u32)MinimizeCallIntercept, 1).Apply(true);
			disableCanvasForcedOverlap.Apply(true); //Used with minimizecallintercept to disable Canvas::surfaceChanged
		}
	} init;
};