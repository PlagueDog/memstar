#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <string>
#include "Fear.h"

using namespace Fear;
namespace OpenGLFixes
{
	BuiltInFunction("OpenGL::WindowLoseFocusMinimize", _oglwlfm)
	{
		const char* str = argv[0];
		std::string arg1 = str;
		MultiPointer(ptrWindowMessage, 0, 0, 0x005777FD, 0x0057AA05);
		if (arg1.compare("true") == 0)
		{
			CodePatch WindowMinimizeOnLoseFocus = { ptrWindowMessage,"","\x20",1,false };
			WindowMinimizeOnLoseFocus.Apply(true);
		}
		else
		{
			CodePatch WindowMinimizeOnLoseFocus = { ptrWindowMessage,"","\x30",1,false };
			WindowMinimizeOnLoseFocus.Apply(true);
		}
		return 0;
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
			jmp [ptrChangeDisplaySettingsCont]
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

	BuiltInFunction("patchWindowedOGL", _pwogl)
	{
		const char* str = argv[0];
		std::string arg1 = str;
		if (arg1.compare("true") == 0)
		{
			patchchangedisplaysettings.DoctorRelative((u32)patchChangeDisplaySettings, 1).Apply(true);
		}
		else if (arg1.compare("false") == 0)
		{
			patchchangedisplaysettings.DoctorRelative((u32)unpatchChangeDisplaySettings, 1).Apply(true);
		}
		return 0;
	}

	MultiPointer(ptrSplash480, 0, 0, 0x0063C5CE, 0x0064B50E);
	MultiPointer(ptrSplash640, 0, 0, 0x0063C5C5, 0x0064B505);
	BuiltInFunction("disableSplash640x480", _ds640x480)
	{
		const char* str = argv[0];
		std::string arg1 = str;
		if (arg1.compare("true") == 0)
		{
			CodePatch goSplash640 = { ptrSplash640, "", "\x81\x02", 2, false };
			CodePatch goSplash480 = { ptrSplash480, "", "\xE1\x01", 2, false };
			goSplash640.Apply(true);
			goSplash480.Apply(true);
		}
		else
		{
			CodePatch goSplash640 = { ptrSplash640, "", "\x80\x02", 2, false };
			CodePatch goSplash480 = { ptrSplash480, "", "\xE0\x01", 2, false };
			goSplash640.Apply(true);
			goSplash480.Apply(true);
		}
		return 0;
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
	MultiPointer(ptrTerrainOpenGLDraw, 0, 0, 0, 0x00652BA4);
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
	//MultiPointer(ptrInteriorOpenGLDraw_TEX1, 0, 0, 0, 0x00652A49);
	//MultiPointer(ptrInteriorOpenGLDraw_TEX0, 0, 0, 0, 0x00652A51);

	//Shadow Multiplier
	MultiPointer(ptrShadowMult, 0, 0, 0, 0x006505A6);

	//Game canvas window resizable
	MultiPointer(ptrWindowResizable, 0, 0, 0, 0x005A255E);

	//OpenGL Gui Object Internal Scale
	MultiPointer(ptrOpenGLGuiIntScale, 0, 0, 0x006425E8, 0x00651F70);
	CodePatch BitmapCtrlLineFix = { ptrOpenGLGuiIntScale, "", "\xED\x0D\xBE\xBA", 4, false }; //Fix Simgui::BitmapCtrl line artifacts when upscaling the OpenGL renderer

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

	//CodePatch tempPatch = { 0x0065FF39, "", "\x0F\xFC\x65\x00", 4, false };
	CodePatch tempPatch = { 0x0065FF39, "", "\x81\xED\x65\x00", 4, false };
	CodePatch goSplash640 = { ptrSplash640, "", "\x70\x0D", 2, false };
	CodePatch goSplash480 = { ptrSplash480, "", "\xA0\x05", 2, false };
	CodePatch wglinfowindow = { 0x0064B674, "", "\xEB", 1, false };
	struct Init {
		Init() {
			//GuiLoadLoad0.Apply(true);
			//GuiLoadLoad1.Apply(true);
			//GuiLoadLoad2.Apply(true);
			//WndInsertAfter0.Apply(true);
			//wglinfowindow.Apply(true);
			windowproperiespatch.DoctorRelative((u32)WindowPropertiesPatch, 1).Apply(true);
			BitmapCtrlLineFix.Apply(true);
			//goSplash640.Apply(true);
			//goSplash480.Apply(true);
			//tempPatch.Apply(true);
		}
	} init;
};