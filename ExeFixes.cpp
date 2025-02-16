#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"
#include "Fear.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <string>
#include "IsBadMemPtr.c" //Included for the copyright notice

namespace ExeFixes {
	u32 dummy, dummy2, dummy3, dummy4, dummy5;
	//Unknown crash patches
	MultiPointer(ptr_unkCrash01, 0, 0, 0x00624F01, 0x00633E41); //This one seems related to the hud scaling
	CodePatch unkCrashPatch01 = { ptr_unkCrash01,"","\x72",1,false };

	CodePatch patchOutIncrementalQueryRetry1 = { 0x0053B16C, "", "\x90\x90", 2, false };
	CodePatch patchOutIncrementalQueryRetry2 = { 0x0053B172, "", "\x90\x90", 2, false };

	MultiPointer(ptrMasterQueryEnd, 0, 0, 0, 0x0053B22D);
	MultiPointer(ptrMasterQueryRepeat, 0, 0, 0, 0x0053B149);
	MultiPointer(ptrMasterQueryPopRetn, 0, 0, 0, 0x0053B236);
	//MultiPointer(ptrMaxMasterQueryTriesResume, 0, 0, 0, 0x004A8870);
	int incrementQuery = 1;
	CodePatch masterqueryall = { ptrMasterQueryEnd, "", "\xE9MQAL", 5, false };
	NAKED void masterQueryAll() {
		__asm {
			inc incrementQuery
			xor ebx, ebx
			mov ebx, incrementQuery
			cmp ebx, 0x7F
			jle __jle

			mov incrementQuery, 1
			add     esp, 0x300
			pop     edi
			pop     esi
			pop     ebx
			jmp ptrMasterQueryPopRetn
			__jle:
				jmp ptrMasterQueryRepeat
		}
	}

	int masterServerArray;
	static const char* masterServer;
	void echoMasterQuery()
	{
		if (strlen(masterServer))
		{
			Console::echo("Polling master server %d [%s]", masterServerArray, masterServer);
		}
	}

	MultiPointer(ptrMasterQueryCheck, 0, 0, 0, 0x0053B174);
	MultiPointer(ptrMasterQueryCheckEnd, 0, 0, 0, 0x0053B18A);
	CodePatch masterquerycheck = { ptrMasterQueryCheck, "", "\xE9MQCH", 5, false };
	NAKED void masterQueryCheck() {
		__asm {
			mov masterServer, esi
			mov masterServerArray, ebx
			call echoMasterQuery
			jmp ptrMasterQueryCheckEnd
		}
	}

	HWND getHWND() {
		MultiPointer(ptrHWND, 0, 0, 0x00705C5C, 0x007160CC);
		uintptr_t HWND_PTR = ptrHWND;
		int GAME_HWND = *reinterpret_cast<int*>(HWND_PTR);
		HWND SS_HWND = reinterpret_cast<HWND>(GAME_HWND);
		return SS_HWND;
	}
	
	MultiPointer(ptrPacketSizeSet, 0, 0, 0x00624F01, 0x00460A78);
	//BuiltInFunction("setWindowsCompat", _getWindowsCompat)
	//{
		//HKEY hKey;
		//const char* keyPath = "\"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers\"";
		//char progPath[MAX_PATH];
		//std::string(progPath, GetModuleFileName(NULL, progPath, MAX_PATH));
		//char* args;
		//strcpy(args, "reg add ");
		//strcat(args, keyPath);
		//strcat(args, " /v \"");
		//strcat(args, progPath);
		//strcat(args, "\" /d \"WIN7RTM\"");

		//open the registry with key_write access
		//int compatLayers = RegOpenKeyEx(HKEY_LOCAL_MACHINE, keyPath, 0, KEY_WRITE, &hKey);
		//if (ERROR_SUCCESS == compatLayers)
		//{
			//system("reg add \"HKEY_LOCAL_MACHINE\\\\Software\\\\Microsoft\\\\Windows NT\\\\CurrentVersion\\\\AppCompatFlags\\\\Layers\" /v \"C:\\Dynamix\\\\Patch-DEV\\\\Starsiege_4.exe\" /d \"~ WIN7RTM\" /f");
			//system("reg add \"HKEY_LOCAL_MACHINE\\\\Software\\\\Microsoft\\\\Windows NT\\\\CurrentVersion\\\\AppCompatFlags\\\\Layers\" /v \"C:\\Dynamix\\\\Patch-DEV\\\\Starsiege_4.exe\" /d \"~ HIGHDPIAWARE\" /f");
			//system(args);
			//RegCloseKey(hKey);
		//}
		//free(args);
		//return 0;
	//}

	MultiPointer(ptrVersionInt, 0, 0, 0x006D726C, 0);
	//BuiltInFunction("returnTest", _retT)
	//{
	//	uintptr_t ptr1 = 0x0074F080;
	//	int ptr1_data = *reinterpret_cast<int*>(ptr1);
	//	return tostring(ptr1_data);
	//}

	BuiltInFunction("getHWND", _getHWND)
	{
		uintptr_t ptr1 = 0x007160CC;
		int ptr1_data = *reinterpret_cast<int*>(ptr1);
		return tostring(ptr1_data);
	}

	int initialTick = GetTickCount();
	BuiltInFunction("getTickCount", _getTickCount)
	{
		int currentTick = GetTickCount();
		return tostring((currentTick - initialTick));
	}

	//NO-CD Patch
	MultiPointer(ptrCDCheck, 0, 0, 0x0053D8C6, 0x0053FDF6);
	CodePatch NoCDPatch = { ptrCDCheck,"","\x90\x90\x90\x90\x90\x90\x90\x90",8,false };

	//OpenGL
	MultiPointer(ptrOGLWidthMin, 0, 0, 0x0063C80B, 0x0064B74B);
	MultiPointer(ptrOGLWidthMax, 0, 0, 0x0063C816, 0x0064B756);
	MultiPointer(ptrOGLHeightMin, 0, 0, 0x0063C825, 0x0064B765);
	MultiPointer(ptrOGLHeightMax, 0, 0, 0x0063C835, 0x0064B775);
	MultiPointer(ptrOGLBitDepth, 0, 0, 0x0063C847, 0x0064B787);
	MultiPointer(ptrOGLMipMapping, 0, 0, 0x0063E7FD, 0x0064D90C);

	BuiltInFunction("OpenGL::disableMipMapping", _oglDmip)
	{
		CodePatch OpenGLMips = { ptrOGLMipMapping,"","\x00\x26",2,false };
		OpenGLMips.Apply(true);
		Console::eval("flushtexturecache();");
		//Console::setVariable("pref::OpenGL::MipMapping", "false");
		return "true";
	}
	BuiltInFunction("OpenGL::enableMipMapping", _oglEmip)
	{
		CodePatch OpenGLFiltering = { ptrOGLMipMapping,"","\x01\x27",2,false };
		OpenGLFiltering.Apply(true);
		Console::eval("flushtexturecache();");
		//Console::setVariable("pref::OpenGL::MipMapping", "true");
		return "true";
	}

	MultiPointer(ptrOGLTextureFilter, 0, 0, 0x0063E7DD, 0x0064D8EC);
	BuiltInFunction("OpenGL::enableGL_NEAREST", _oglEgln)
	{
		CodePatch OpenGLFiltering = { ptrOGLTextureFilter,"","\x00\x26",2,false };
		OpenGLFiltering.Apply(true);
		Console::eval("flushtexturecache();");
		return "true";
	}
	
	
	BuiltInFunction("OpenGL::disableGL_NEAREST", _oglDgln)
	{
		CodePatch OpenGLFiltering = { ptrOGLTextureFilter,"","\x01\x26",2,false };
		OpenGLFiltering.Apply(true);
		Console::eval("flushtexturecache();");
		return "true";
	}

	CodePatch OpenGLBitDepth = { ptrOGLBitDepth,"","\x20",1,false };

	//CodePatch OpenGLWidthMin = { ptrOGLWidthMin,"","\x3D\x40\x01",3,false };
	//CodePatch OpenGLWidthMax = { ptrOGLWidthMax,"","\x3D\x00\x0A",3,false };
	CodePatch OpenGLWidthMax = { ptrOGLWidthMax,"","\x3D\x00\x1E",3,false };
	//CodePatch OpenGLHeightMin = { ptrOGLHeightMin,"","\x81\xFA\xF0\x00",4,false };
	//CodePatch OpenGLHeightMax = { ptrOGLHeightMax,"","\x81\xF9\xA0\x05",4,false };
	CodePatch OpenGLHeightMax = { ptrOGLHeightMax,"","\x81\xF9\x00\x1E",4,false };

	//DirectDraw (Software)
	MultiPointer(ptrSoftwareResWidthCap, 0, 0, 0x0064831A, 0x00658032);
	MultiPointer(ptrSoftwareResHeightCap, 0, 0, 0x00648324, 0x0065803C);
	CodePatch SoftwareResWidthCap = { ptrSoftwareResWidthCap,	"","\x3D\x00\x05",		3,false };
	CodePatch SoftwareResHeightCap = { ptrSoftwareResHeightCap,	"","\x81\xFA\x00\x04",	4,false };
	//CodePatch SoftwareResWidthCap = { ptrSoftwareResWidthCap,	"","\x3D\x10\xE0",		3,false };
	//CodePatch SoftwareResHeightCap = { ptrSoftwareResHeightCap,	"","\x81\xFA\x00\x1E",	4,false };

	MultiPointer(ptrWinMaxIntRendSizeWidth, 0, 0, 0x006487A6, 0x006584BE);
	MultiPointer(ptrWinMaxIntRendSizeHeight, 0, 0, 0x006487C5, 0x006584DD);
	MultiPointer(ptrWinMaxWinSize, 0, 0, 0x005740FF, 0x00577307);
	BuiltInFunction("Software::EnableHires", _seh) {
		CodePatch SoftwareResWidthCap = { ptrSoftwareResWidthCap,	"","\x3D\x10\xE0",		3,false }; SoftwareResWidthCap.Apply(true);
		CodePatch SoftwareResHeightCap = { ptrSoftwareResHeightCap,	"","\x81\xFA\x00\x1E",	4,false }; SoftwareResHeightCap.Apply(true);
		CodePatch CanvasWindowMaxWindowedSize = { ptrWinMaxWinSize,"","\xC7\x02\x10\xE0\x00\x00\xC7\x42\x04\x00\x1E",11,false }; CanvasWindowMaxWindowedSize.Apply(true);
		CodePatch CanvasWindowMaxInteralRenderSizeWidth = { ptrWinMaxIntRendSizeWidth,"","\x3D\x10\xE0\x00\x00\x7E\x06\xC7\x06\x10\xE0",11,false }; CanvasWindowMaxInteralRenderSizeWidth.Apply(true);
		CodePatch CanvasWindowMaxInteralRenderSizeHeight = { ptrWinMaxIntRendSizeHeight,"","\x81\xF9\x00\x1E\x00\x00\x7E\x07\xC7\x46\x04\x00\x1E",13,false }; CanvasWindowMaxInteralRenderSizeHeight.Apply(true);
		return 0;
	}
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//1.003 OpenGL tweaks
	CodePatch unk719FC8 = { 0x00719FC8,	"","\x01\x00\x00\x00\x01\x00\x00\x00\x01\x00\x00\x00\x01\x00\x00\x00\x01\x00\x00\x00\x01\x00\x00\x00", 24,false };
	CodePatch ARB = { 0x00719FE0,	"","GL_ARB_multitexture\x00\x00\x00\x00\x00",24,false };
	CodePatch SGIS_to_ARB1 = { 0x0071A03D,	"","glMultiTexCoord2fvARB\x00",		22,false };
	CodePatch SGIS_to_ARB2 = { 0x0071A054,	"","glMultiTexCoord4fvARB\x00",	22,false };
	CodePatch SGIS_to_ARB3 = { 0x0071A06B,	"","glActiveTextureARB\x00",	19,false };
	//Patch out obsolete GL_SGIS_multitexture calls
	CodePatch GL_SGIS_multiTexPatch = { 0x00719FF9,	"","GL_ARB_multitexture\x00",	20,false };
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Fix player vehicle data not being able to be properly modified via client tree or mission editor
	MultiPointer(ptrPlayerVehicleCheck, 0, 0, 0x0046F0EB, 0x00470CAF);
	CodePatch PlayerVehiclePropertyCheckPatch = { ptrPlayerVehicleCheck,	"","\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90",	12,false };

	MultiPointer(ptrDoSFix, 0, 0, 0x0067c7e6, 0x0068C6B2);
	//                                                 |.  8DBD 38FFFFFF     LEA EDI,DWORD PTR SS:[EBP-C8]
	CodePatch dosfix = {
		ptrDoSFix,
		"",
		"\xE9OSFX",
		5,
		false
	};

	MultiPointer(ptrDroneDefine, 0, 0, 0, 0x004A8CD0);
	MultiPointer(ptrDroneDefRedir, 0, 0, 0, 0x00423078);
	//MultiPointer(ptrDroneDefCont, 0, 0, 0, 0x00417C7E);
	CodePatch dronetypeallow = {
		ptrDroneDefine,
		"",
		"\xE9VDAL",
		5,
		false
	};

	//WORK IN PROGRESS
	//NAKED void droneTypeAllow() {
	//	__asm {
	//		jmp[ptrDroneDefRedir]
	//		//test edx, edx
	//		//jnz __droneDefRedirection
	//		//jmp [ptrDroneDefCont]
	//		//__droneDefRedirection:
	//			//jmp [ptrDroneDefRedir]
	//	}
	//}

	MultiPointer(fnBitStreamReadInt, 0, 0, 0, 0x0056D4A0);
	MultiPointer(fnReadPacketAcksResume, 0, 0, 0, 0x0068C6E9);
	static const char* crashAttempt = "DoSFiX: Crash Attempt by %s";


	NAKED void DosFix() {
		__asm {
			push ebx
			lea edi, [ebp - 0xc8]
			lea ebx, [edi + (0x1a * 0x8)]
			jmp __primed_jump
			__read_ack_loop :
			lea eax, [ebp - 0xf0]
				mov edx, 0x5
				call[fnBitStreamReadInt]
				mov[edi - 0x4], eax
				inc dword ptr[ebp - 0x28]
				add edi, 0x8
				__primed_jump:
			lea eax, [ebp - 0xf0]
				mov edx, 0x3
				call[fnBitStreamReadInt]
				mov[edi], eax
				cmp edi, ebx
				jae __crash_attempt
				test eax, eax
				jnz __read_ack_loop
				__leave_loop :
			pop ebx
				jmp[fnReadPacketAcksResume]

				__crash_attempt :
				lea eax, [ebp + 0x28]
				push eax
				mov eax, [crashAttempt]
				push eax
				call Console::echo
				add esp, 0x8
				jmp __leave_loop
		}
	}

	//Fix nav crash exploit on servers
	MultiPointer(ptrNavLocTest, 0, 0, 0x00588B04, 0x0058C320);
	CodePatch navfix = {
	ptrNavLocTest,
	"",
	"\xE9NVFX",
	5,
	false
	};

	NAKED void NavFix() {
		__asm {
			test eax, eax
			jns __navArgsTestFail
			mov ecx, [eax]
			call dword ptr[ecx + 0x78]
			__navArgsTestFail:
			pop edi
				pop esi
				pop ebx
				retn
		}
	}

	//Ignore atan2 calls for navs calling invalid locations
	MultiPointer(ptrNavAtan2Call0, 0, 0, 0x0051A004, 0x0051C4A1);
	CodePatch navExploitFix0 = { ptrNavAtan2Call0, "", "\x90\x90", 2, false };
	MultiPointer(ptrNavAtan2Call1, 0, 0, 0x0051C4B2, 0x0051C4B2);
	CodePatch navExploitFix1 = { ptrNavAtan2Call1, "", "\x90\x90", 2, false };

	//Chat format control character uppercase <F>
	MultiPointer(CHATFORMAT_UPPER_CTRL_CHAR, 0, 0, 0, 0x005CA11B);
	//Chat format control character lowercase <f>
	MultiPointer(CHATFORMAT_LOWER_CTRL_CHAR, 0, 0, 0, 0x005CA127);

	//Console key
	MultiPointer(ptrConsoleKey, 0, 0, 0x0059D9E6, 0x005A1202);


	//Mapview grid line count
	MultiPointer(ptrMapviewPanelLineCount, 0, 0, 0, 0x00514FEC);
	CodePatch mapviewGridLineCount = { ptrMapviewPanelLineCount,	"\x00\x00\x00\x39","\x00\x00\x00\x00",	4,false };
	BuiltInVariable("pref::mapviewLineGridVisible", int, prefmapviewlinegridvisible, 1);
	BuiltInFunction("Nova::toggleMapviewGrid", _novatogglemapviewgrid)
	{
		std::string var = Console::getVariable("pref::mapviewLineGridVisible");
		if (var.compare("1") == 0 || var.compare("true") == 0 || var.compare("True") == 0)
		{
			mapviewGridLineCount.Apply(false);
		}
		else
		{
			mapviewGridLineCount.Apply(true);
		}
		return "true";
	}

	//Mapview map panel size
	MultiPointer(ptrMapviewPanelSize, 0, 0, 0, 0x00514968);
	//Mapview map zoom speed
	MultiPointer(ptrMapviewZoom, 0, 0, 0, 0x00514960);
	//Mapview satellite image size
	MultiPointer(ptrMapSatSize, 0, 0, 0, 0x00514AAC);
	//Mapview map objects update speed
	MultiPointer(ptrMapObjectUpdateSpeed, 0, 0, 0x005124D0, 0x0051496C);
	CodePatch mapviewObjectLatency = { ptrMapObjectUpdateSpeed,	"","\x00\x00\x00\x00",	4,false };
	//Mapview interface update speed
	MultiPointer(ptrMapInterfaceUpdateSpeed, 0, 0, 0x005124C4, 0x00514960);
	CodePatch mapviewInterfaceLatency = { ptrMapInterfaceUpdateSpeed,	"","\x00\x00\x00\x00",	4,false };
	//Mapview object layout scale
	MultiPointer(ptrMapObjectLayoutScale, 0, 0, 0, 0x00511E48);

	//Mapview Vehicle Sensor Range Circle Opacity
	MultiPointer(ptrMapviewSensRngOpacity, 0, 0, 0, 0x00655920);

	//Radar Nav Blips (Palette color index start index)
	MultiPointer(ptrNavBlipColorStartIndx, 0, 0, 0, 0x0041BBD8);

	//Simgui::TestButton
	MultiPointer(ptrTestButtonFillColor, 0, 0, 0x005CCDC1, 0x005D0665);
	MultiPointer(ptrTestButtonBorderColor, 0, 0, 0x005CCDDB, 0x005D067F);
	MultiPointer(ptrTestButtonSelectedBorder, 0, 0, 0x005CCDE5, 0x005D0689);
	MultiPointer(ptrTestButtonFillOpacity, 0, 0, 0x005CCE9B, 0x005D073F);
	CodePatch TestButtonFillColor = { ptrTestButtonFillColor,	"","\x00",	1,false };
	CodePatch TestButtonBorderColor = { ptrTestButtonBorderColor,	"","\xF7",	1,false };
	CodePatch TestButtonSelectedBorderColor = { ptrTestButtonSelectedBorder,	"","\xF5",	1,false };
	CodePatch TestButtonFillOpacity = { ptrTestButtonFillOpacity,	"","\xBF",	1,false };

	//Interface global scale OPENGL
	MultiPointer(ptrHudGlobalScale, 0, 0, 0, 0x00652060);
	//Internal simgui object scale (- Bigger / + Smaller) OPENGL
	MultiPointer(ptrInternalGuiObjectScale, 0, 0, 0, 0x00651F55);

	//Simgui Background Palette Color Index
	MultiPointer(ptrSimGuiBackgroundPalColorIndex, 0, 0, 0, 0x00654924);

	//Lensflare Bitmap(s) Width
	MultiPointer(ptrLensflareBitmapWidth, 0, 0, 0x006A4CA0, 0x006B4CA8);
	
	BuiltInFunction("Nova::AdjustLensflareBitmaps", _novaadjustlensflarebitmaps)
	{
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		int ratio = screen.x/screen.y;
		if (ratio >= 1.7)
		{
			CodePatch LensFlareBitmapWidthPatch = { ptrLensflareBitmapWidth,	"","\xF4",	1,false };
			LensFlareBitmapWidthPatch.Apply(true);
		}
		else
		{
			CodePatch LensFlareBitmapWidthPatch = { ptrLensflareBitmapWidth,	"","\xF5",	1,false };
			LensFlareBitmapWidthPatch.Apply(true);
		}
		return 0;
	}

	//Software wide screen fix
	//MultiPointer(ptrSoftwareRedir, 0, 0, 0, 0x00660001);
	//MultiPointer(ptrSoftwarefldFLT, 0, 0, 0, 0x007347FD);
	//MultiPointer(ptrSoftwareResume, 0, 0, 0, 0x0066000A);
	//CodePatch software_widescreen_patch = { ptrSoftwareRedir, "", "\xE9SFTW", 5, false };
	//NAKED void Software_Widescreen_Patch() {
	//	__asm {
	//		sub esi, ecx
	//		fld [ptrSoftwarefldFLT]
	//		jmp [ptrSoftwareResume]
	//	}
	//}
	//Surface Buoyancy
	MultiPointer(ptrSurfaceBuoyancy, 0, 0, 0, 0x00713D90);

	//Sky Panel Color
	MultiPointer(ptrSkyColor, 0, 0, 0, 0x0065326C);

	//Cloak Transparency
	MultiPointer(ptrCloakTransparency, 0, 0, 0, 0x00485ABB);

	//Console newline feed char
	MultiPointer(ptrConsoleNewLineFeed, 0, 0, 0x00703645, 0x00713AB5);
	CodePatch consoleNewLineChar = { ptrConsoleNewLineFeed, "", "\x00\x00", 2, false };


	MultiPointer(ptrRenderInCollisionDetailByte, 0, 0, 0x00736DF0, 0x00747408);
	//Fix IDACTION_COLLISION_DETAIL keybind not working
	CodePatch IDACTION_COLLISION_DETAILfix = {
	ptrRenderInCollisionDetailByte, "", "\x01", 1, false };

	MultiPointer(ptrVehicleWorldMovement, 0, 0, 0, 0x00598BBC);

	//Shadowmap Resolution pointers
	MultiPointer(ptrShadowResLOD0, 0, 0, 0x0067382C, 0x006836F8);
	MultiPointer(ptrShadowResLOD0_Blur, 0, 0, 0x00673833, 0x006836FF);
	MultiPointer(ptrShadowResLOD1, 0, 0, 0x0067384B, 0x00683717);
	MultiPointer(ptrShadowResLOD2, 0, 0, 0x0067386A, 0x00683736);
	MultiPointer(ptrShadowResLOD3, 0, 0, 0x00673888, 0x00683754);
	MultiPointer(ptrShadowResLOD4, 0, 0, 0x00673898, 0x00683764);

	BuiltInFunction("Nova::EnableHiresShadows", _memehs)
	{
		CodePatch hiresShadowsPatch0 = { ptrShadowResLOD0, "", "\x00\x01", 2, false }; hiresShadowsPatch0.Apply(true);
		CodePatch hiresShadowsBlurPatch0 = { ptrShadowResLOD0_Blur, "", "\x05", 1, false }; hiresShadowsBlurPatch0.Apply(true);
		CodePatch hiresShadowsPatch1 = { ptrShadowResLOD1, "", "\x00\x01", 2, false }; hiresShadowsPatch1.Apply(true);
		CodePatch hiresShadowsPatch2 = { ptrShadowResLOD2, "", "\x00\x01", 2, false }; hiresShadowsPatch2.Apply(true);
		CodePatch hiresShadowsPatch3 = { ptrShadowResLOD3, "", "\x00\x01", 2, false }; hiresShadowsPatch3.Apply(true);
		CodePatch hiresShadowsPatch4 = { ptrShadowResLOD4, "", "\x00\x01", 2, false }; hiresShadowsPatch4.Apply(true);
		Console::eval("$pref::HiresShadows = true;flushtexturecache();");
		return 0;
	}

	BuiltInFunction("Nova::DisableHiresShadows", _memdhs)
	{
		CodePatch hiresShadowsPatch0 = { ptrShadowResLOD0, "", "\x40\x00", 2, false }; hiresShadowsPatch0.Apply(true);
		CodePatch hiresShadowsBlurPatch0 = { ptrShadowResLOD0_Blur, "", "\x17", 1, false }; hiresShadowsBlurPatch0.Apply(true);
		CodePatch hiresShadowsPatch1 = { ptrShadowResLOD1, "", "\x40\x00", 2, false }; hiresShadowsPatch1.Apply(true);
		CodePatch hiresShadowsPatch2 = { ptrShadowResLOD2, "", "\x20\x00", 2, false }; hiresShadowsPatch2.Apply(true);
		CodePatch hiresShadowsPatch3 = { ptrShadowResLOD3, "", "\x10\x00", 2, false }; hiresShadowsPatch3.Apply(true);
		CodePatch hiresShadowsPatch4 = { ptrShadowResLOD4, "", "\x10\x00", 2, false }; hiresShadowsPatch4.Apply(true);
		Console::eval("$pref::HiresShadows = false;flushtexturecache();");
		return 0;
	}
	//Restore the win mission editor
	//MultiPointer(ptrWinMissionEditorCallIntercept, 0, 0, 0x006AB2EB, 0x006BB363);
	//BuiltInFunction("MissionCreate", _wmc)
	//{
	//	CodePatch WinMissionEditorCallSwitchIntercept = { ptrWinMissionEditorCallIntercept, "", "\xEB\x5F\x00\x00", 4, false };
	//	WinMissionEditorCallSwitchIntercept.Apply(true);
	//	Console::eval("MissionRegType(simcanvas);");
	//
	//	//Restore the switch jump table for MissionCreate functions
	//	CodePatch WinMissionEditorCallInterceptRestore = { ptrWinMissionEditorCallIntercept, "", "\x0F\x87\x42\x0E", 4, false };
	//	WinMissionEditorCallInterceptRestore.Apply(true);
	//	return 0;
	//}
	MultiPointer(ptrFunctionAllocation, 0, 0, 0x005E3514, 0x005E6DB8);
	MultiPointer(ptrPluginAllocation, 0, 0, 0x005E3178, 0x005E6A1C);
	MultiPointer(ptrSimGuiPluginVariableAllocation, 0, 0, 0x005E3474, 0x005E6D18);

	MultiPointer(ptrMissionPluginFunctionList, 0, 0, 0x006AAFCC, 0x006BB044);
	MultiPointer(ptrMissionPluginFunctionListResume, 0, 0, 0x006AAFE1, 0x006BB059);
	static const char* MEcreateFunctionName = "MissionCreate";
	static const char* MissionRegType = "MissionRegType";
	CodePatch missioncreaterestore = { ptrMissionPluginFunctionList, "", "\xE9MCFD", 5, false };
	NAKED void MissionCreateRestore() {
		__asm {
			push ebx
			push 0
			mov ecx, [MEcreateFunctionName]
			mov edx, 0
			mov eax, [ebx + 8]
			call[ptrFunctionAllocation]
			push ebx
			push 0
			mov ecx, [MissionRegType]
			mov edx, 1
			mov eax, [ebx + 8]
			call[ptrFunctionAllocation]
			jmp[ptrMissionPluginFunctionListResume]
		}
	}

	MultiPointer(ptrFunctionList01, 0, 0, 0x004A653F, 0x004A885B);
	MultiPointer(ptrFunctionList01Resume, 0, 0, 0x004A6554, 0x004A8870);
	static const char* datadumpFunctionName = "dataDump";
	static const char* gotoFunctionName = "goto";
	CodePatch datadumprestore = { ptrFunctionList01, "", "\xE9_DPR", 5, false };
	NAKED void DataDumpRestore() {
		__asm {
			push ebx
			push 0
			mov ecx, [datadumpFunctionName]
			mov edx, 0x35
			mov eax, [ebx + 8]
			call[ptrFunctionAllocation]
			push ebx
			push 0
			mov ecx, [gotoFunctionName]
			mov edx, 3
			mov eax, [ebx + 8]
			call[ptrFunctionAllocation]
			jmp[ptrFunctionList01Resume]
		}
	}

	//MISSING ADDITIONAL ASM
	//MultiPointer(ptrInteriorPluginFunctionList, 0, 0, 0x006AA25C, 0x006BA2D4);
	//MultiPointer(ptrInteriorPluginNOP, 0, 0, 0x006AA261, 0x006BA2D9);
	//static const char* interiorPlugin = "InteriorPlugin [Re-implemented]";
	//static const char* setInteriorShapeState = "setInteriorShapeState";
	//static const char* setInteriorLightState = "setInteriorLightState";
	//static const char* animateInteriorLight = "animateInteriorLight";
	//static const char* stopInteriorLightAnim = "stopInteriorLightAnim";
	//static const char* resetInteriorLight = "resetInteriorLight";
	//static const char* interiorToggleBoundingBox = "interior::toggleBoundingBox";
	//CodePatch interiorpluginrestore = { ptrInteriorPluginFunctionList, "", "\xE9IPFL", 5, false };
	//NAKED void InteriorPluginRestore() {
	//	__asm {
	//		push [interiorPlugin]
	//		mov eax, [eax + 8]
	//		push eax
	//		call [ptrPluginAllocation]
	//		add esp, 8
	//		push ebx
	//		push 0
	//		mov ecx, [setInteriorShapeState]
	//		xor edx, edx
	//		mov eax, [ebx + 8]
	//		call[ptrFunctionAllocation]
	//		push ebx
	//		push 0
	//		mov ecx, [setInteriorLightState]
	//		mov edx, 1
	//		mov eax, [ebx + 8]
	//		call[ptrFunctionAllocation]
	//		push ebx
	//		push 0
	//		mov ecx, [animateInteriorLight]
	//		mov edx, 2
	//		mov eax, [ebx + 8]
	//		call[ptrFunctionAllocation]
	//		push ebx
	//		push 0
	//		mov ecx, [stopInteriorLightAnim]
	//		mov edx, 3
	//		mov eax, [ebx + 8]
	//		call[ptrFunctionAllocation]
	//		push ebx
	//		push 0
	//		mov ecx, [resetInteriorLight]
	//		mov edx, 4
	//		mov eax, [ebx + 8]
	//		call[ptrFunctionAllocation]
	//		push ebx
	//		push 0
	//		mov ecx, [interiorToggleBoundingBox]
	//		mov edx, 5
	//		mov eax, [ebx + 8]
	//		call[ptrFunctionAllocation]
	//		retn
	//	}
	//}

	MultiPointer(ptrSimShapeDrawBBoxByte, 0, 0, 0x0072CF90, 0x0073D5A8);
	CodePatch SimShapeBBoxByte0 = { ptrSimShapeDrawBBoxByte, "", "\x00", 1, false };
	CodePatch SimShapeBBoxByte1 = { ptrSimShapeDrawBBoxByte, "", "\x01", 1, false };

	BuiltInFunction("SimShape::toggleBoundingBox", _sstbbox)
	{
		Console::eval("$SimShape::BoundingBox^=1;");
		std::string var = Console::getVariable("SimShape::BoundingBox");
		if (var.compare("1") == 0)
		{
			SimShapeBBoxByte1.Apply(true);
		}
		else
		{
			SimShapeBBoxByte0.Apply(true);
		}
		return "true";
	}

	MultiPointer(ptrGuiConsoleVert, 0, 0, 0x59179D, 0x00594FB9);
	MultiPointer(ptrGuiConsoleVertRetn, 0, 0, 0x5917A2, 0x00594FBE);
	CodePatch simguiconsolevert = { ptrGuiConsoleVert, "", "\xE9SCVT", 5, false };

	int consoleVert = -10;
	NAKED void SimGuiConsoleVert_Default() {
		__asm {
			sub eax, edx
			add eax, consoleVert //-10
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	void consoleVertOffset(int offset)
	{
		consoleVert = offset;
	}

	BuiltInFunction("Console::RenderOffset", _cro)
	{
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		//Just need the height
		int height = screen.y;
		if (argc == 0)
		{
			consoleVertOffset(-10);
		}
		else if(atoi(argv[0]) == 0)
		{
			consoleVertOffset((-height) + 480 - 10);
		}
		else
		{
			consoleVertOffset(atoi(argv[0]));
		}
		simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_Default, 1).Apply(true);
		return 0;
	}

	//MultiPointer(ptrVideoModesBoxSize, 0, 0, 0, 0x004E17CA);
	MultiPointer(ptrComboBoxVertSizeMult, 0, 0, 0x004DE41E, 0x004E08AE);
	MultiPointer(ptrComboBoxVertSizeMultResume, 0, 0, 0x004DE41E, 0x004E08B6);
	CodePatch comboboxfixedsize = { ptrComboBoxVertSizeMult, "", "\xE9_CBFS", 5, false };
	NAKED void ComboboxFixedSize() {
		__asm {
			mov [ebp - 0x8], 0xFF
			add edi, edx
			mov ecx, [ebp - 0x8]
			jmp ptrComboBoxVertSizeMultResume
		}
	}

	//MultiPointer(ptrFullscreenInitWidth, 0, 0, 0, 0x0059B7A2);
	//MultiPointer(ptrFullscreenInitWidthRetn, 0, 0, 0, 0x0059B7A8);
	//MultiPointer(ptrFullscreenInitHeight, 0, 0, 0, 0x0059B7BE);
	//CodePatch fullscreeninitwidth = { ptrFullscreenInitWidth, "", "\xE9IFWD", 5, false };
	//CodePatch fullscreeninitheight = { ptrFullscreenInitHeight, "", "\xE9IFHT", 5, false };

	MultiPointer(ptrSnowfallMaxIntensity, 0, 0, 0x0054DFF8, 0x00550540);
	MultiPointer(ptrSnowfallMaxFlakes, 0, 0, 0x0054EADC, 0x00551024); //LOOP COUNT
	//MultiPointer(ptrSnowfallMaxFlakes, 0, 0, 0x0054EADC, 0x0055033C); //Modify the variable set in Snowfall::onAdd() internally
	MultiPointer(ptrSnowfallFlakeWidth, 0, 0, 0x0054E8D5, 0x00550E1D);
	MultiPointer(ptrSnowfallRenderRadius, 0, 0, 0x0054D5F7, 0x0054FB2B);
	//CodePatch SnowfallFlakeWidth = { ptrSnowfallFlakeWidth, "", "\x3E", 1, false };

	MultiPointer(ptrRainfallRenderRadius, 0, 0, 0x0054DDDF, 0x00550313);
	MultiPointer(ptrRainfallMaxDroplets, 0, 0, 0x0054DDFB, 0x0055032F);
	MultiPointer(ptrRainfallSpeed, 0, 0, 0x0054DD25, 0x00550259);
	MultiPointer(ptrRainDropletSize1, 0, 0, 0x0054E48E, 0x005509D6);
	MultiPointer(ptrRainDropletSize2, 0, 0, 0x0054E498, 0x005509E0);
	MultiPointer(ptrRainDropletSize3, 0, 0, 0x0054E4A2, 0x005509EA);

	//CodePatch RainfallDropletSize1 = { ptrRainDropletSize1, "", "\xCD\xCC\xFC\x3D", 4, false };
	//CodePatch RainfallDropletSize2 = { ptrRainDropletSize2, "", "\x9A\x99\xFC\x3E", 4, false };
	//CodePatch RainfallDropletSize3 = { ptrRainDropletSize3, "", "\xCD\xCC\xFC\x3E", 4, false };
	BuiltInFunction("Nova::enableFarWeather", _memefw)
	{
		CodePatch SnowfallMaxIntensity = { ptrSnowfallMaxIntensity, "", "\x00\x00\x48\x42", 4, false };
		CodePatch SnowfallMaxFlakes = { ptrSnowfallMaxFlakes, "", "\x00\xA0\x00\x00", 4, false };
		CodePatch SnowfallRadius = { ptrSnowfallRenderRadius, "", "\x00\x00\x40\x44", 4, false };
		//SNOW
		SnowfallMaxFlakes.Apply(true);
		SnowfallMaxIntensity.Apply(true);
		SnowfallRadius.Apply(true);

		CodePatch RainfallRenderRadius = { ptrRainfallRenderRadius, "", "\x00\x00\x00\x44", 4, false };
		CodePatch RainfallMaxDroplets = { ptrRainfallMaxDroplets, "", "\x00\xA0\x00\x00", 4, false }; //Anything higher than 4096 will halt the game
		CodePatch RainfallSpeed = { ptrRainfallSpeed, "", "\x00\x00\x80\xC3", 4, false };
		//RAIN
		RainfallRenderRadius.Apply(true);
		RainfallSpeed.Apply(true);
		RainfallMaxDroplets.Apply(true);
		Console::eval("if(isObject(628)){storeObject(628,'temp\\\\clip.tmp');loadObject(initSnowfall, 'temp\\\\clip.tmp');deleteObject(initSnowfall);}"); //Force run "static void initSnow(snowGlobals *snow)"
		return 0;
	}

	BuiltInFunction("Nova::disableFarWeather", _memdfw)
	{
		CodePatch SnowfallMaxIntensity = { ptrSnowfallMaxIntensity, "", "\x00\x00\x80\x3F", 4, false };
		CodePatch SnowfallMaxFlakes = { ptrSnowfallMaxFlakes, "", "\x00\x10\x00\x00", 4, false };
		CodePatch SnowfallRadius = { ptrSnowfallRenderRadius, "", "\x00\x00\x48\x43", 4, false };
		//SNOW
		SnowfallMaxFlakes.Apply(true);
		SnowfallMaxIntensity.Apply(true);
		SnowfallRadius.Apply(true);

		CodePatch RainfallRenderRadius = { ptrRainfallRenderRadius, "", "\x00\x00\xDC\x42", 4, false };
		CodePatch RainfallMaxDroplets = { ptrRainfallMaxDroplets, "", "\x00\x10\x00\x00", 4, false };
		CodePatch RainfallSpeed = { ptrRainfallSpeed, "", "\x00\x00\xAA\xC2", 4, false };
		//RAIN
		RainfallRenderRadius.Apply(true);
		RainfallSpeed.Apply(true);
		RainfallMaxDroplets.Apply(true);
		Console::eval("if(isObject(628)){storeObject(628,'temp\\\\clip.tmp');loadObject(initSnowfall, 'temp\\\\clip.tmp');deleteObject(initSnowfall);}");
		return 0;
	}

	MultiPointer(ptrMaxAnimationsDTS, 0, 0, 0, 0x0073DB78);

	//Hud Transparency
	MultiPointer(ptrHudTransparency, 0, 0, 0, 0x00654FEB);

	//Events
	MultiPointer(ptrWindowAltTab, 0, 0, 0, 0x0057B524);
	MultiPointer(ptrKeyPress_Alt, 0, 0, 0, 0x0057AFD0);
	MultiPointer(ptrKeyPress, 0, 0, 0, 0x00579A78);
	MultiPointer(ptrKeyPressGLOBAL, 0, 0, 0, 0x005C945C);
	MultiPointer(ptrWM_SYSCHAR, 0, 0, 0, 0x00579AB0);
	MultiPointer(ptrWM_KEYFIRST, 0, 0, 0, 0x00579A28);


	//Fix broken keyboard input with dinputto8 https://github.com/elishacloud/dinputto8
	// DISABLED for now as this is not a fully functioning fix
	//MultiPointer(ptrForceKeyInput, 0, 0, 0x005C5C12, 0x005C948A);
	//CodePatch dinput8fix = { ptrForceKeyInput, "", "\x82", 1, false };
	
	//Apply CS to the window
	MultiPointer(ptrWindowStyle, 0, 0, 0x00573F49, 0x00577151);
	MultiPointer(ptrWindowStyleRetn, 0, 0, 0x00573F50, 0x00577158);
	CodePatch windowstyle = { ptrWindowStyle, "", "\xE9WSTY", 5, false };
	NAKED void WindowStyle() {
		__asm {
			mov ebx, eax
			mov edi, [ebp + 0xC]
			mov esi, ecx
			xor edx, edx
			mov edx, CS_NOCLOSE
			//mov edx, WS_POPUP
			jmp [ptrWindowStyleRetn]
		}
	}

	//World rendering
	//MultiPointer(ptrWorldRender, 0, 0, 0x00589EE8, 0x0058D704);
	//BuiltInFunction("Nova::EnableWorldRender", _novaEnableWorldRender)
	//{
	//	CodePatch WorldRenderPatch = { ptrWorldRender, "", "\x53", 1, false }; // push ebx
	//	WorldRenderPatch.Apply(true);
	//	return "true";
	//}
	//
	//BuiltInFunction("Nova::DisableWorldRender", _novaDisableWorldRender)
	//{
	//	CodePatch WorldRenderPatch = { ptrWorldRender, "", "\xC3", 1, false }; // retn
	//	WorldRenderPatch.Apply(true);
	//	return "true";
	//}

	//Catch no-terrain crash
	MultiPointer(ptrWorldRend, 0, 0, 0x00589F98, 0x0058D7B4);
	MultiPointer(ptrWorldRendCont, 0, 0, 0x00589FA2, 0x0058D7BE);
	MultiPointer(ptrWorldRendDetour, 0, 0, 0x00589FC5, 0x0058D7E1);
	CodePatch terraincrashcatcher = { ptrWorldRend, "", "\xE9NTCR", 5, false };
	NAKED void TerrainCrashCatcher() {
		__asm {
			mov edx, 0x8
			mov ecx, [eax]
			call dword ptr [ecx + 0x68]
			test eax, eax //Test for 0x00000000 (We are missing the terrain!)
			jz __crashEscape
			jmp [ptrWorldRendCont]
				__crashEscape:
					jmp [ptrWorldRendDetour]
		}
	}

	void* inputPtr = 0x0;
	int isBadPtr = 0;
	void simVolumetricCheckForBadPtr()
	{
		if (IsBadMemPtr(FALSE, inputPtr, 4)) //We have read access at this memory address
		{
			isBadPtr = 0;
		}
		else
		{
			isBadPtr = 1;
		}
	}

	//Catch Volumetric DML render crash
	MultiPointer(ptrVolumetRend, 0, 0, 0, 0x0062E7CA);
	//MultiPointer(ptrVolumetRendDWORD, 0, 0, 0, 0x00728570);
	MultiPointer(ptrVolumetRendResume, 0, 0, 0, 0x0062E7D5);
	CodePatch volumetriccrashcatcher = { ptrVolumetRend, "", "\xE9VDCR", 5, false };
	NAKED void VolumetricCrashCatcher() {
		__asm {
			mov [dummy], 0
			mov [dummy2], 0
			mov [dummy3], 0
			mov [dummy4], 0
			mov [dummy5], 0
			mov [isBadPtr], 0

			mov dummy, eax
			mov dummy2, ecx
			mov dummy4, esi
			mov dummy5, ebx

			lea edx, [ecx + edx * 8]
			mov inputPtr, edx
			mov dummy3, edx //Save the memory address of edx after we LEA
			call simVolumetricCheckForBadPtr

			//Move the memory addresses that were wiped by our simVolumetricCheckForBadPtr() back into their associated registers
			mov eax, dummy
			mov ecx, dummy2
			mov edx, dummy3
			mov esi, dummy4
			mov ebx, dummy5

			cmp isBadPtr, 0
			je __je
			mov ecx, [edx]
			mov [eax + 0x20], ecx
			mov ecx, [edx + 4]
			jmp ptrVolumetRendResume
			__je:
				jmp ptrVolumetRendResume
		}
	}

	//Volumetric Face Rendering
	MultiPointer(ptrVolumetricFaceRender1, 0, 0, 0, 0x006810BC);
	MultiPointer(ptrVolumetricFaceRender2, 0, 0, 0, 0x00681202);
	MultiPointer(ptrVolumetricFaceRender3, 0, 0, 0, 0x00681348);
	CodePatch VolumetricFaceColor1 = { ptrVolumetricFaceRender1, "", "\xBA\xFD", 2, false };
	CodePatch VolumetricFaceColor2 = { ptrVolumetricFaceRender2, "", "\xBA\xFD", 2, false };
	CodePatch VolumetricFaceColor3 = { ptrVolumetricFaceRender3, "", "\xBA\xFD", 2, false };
	MultiPointer(ptrVolumetricDMLrender, 0, 0, 0, 0x0062E7CA);

	//Terrain Tile Rendering
	MultiPointer(ptrTerrainTileRender, 0, 0, 0, 0x00600AB1);
	//Terrain Mip Rendering
	MultiPointer(ptrTerrainMipRender, 0, 0, 0, 0x00601AAC);

	//Catch invalid damage part for internal mount crashes (i.e Clicking on internal mount config buttons in the vehicle lab with a flyer vehicle selected)
	MultiPointer(ptrVehicleMountConfigButtonFn, 0, 0, 0x004EA7AA, 0x004ECC3E);
	MultiPointer(ptrVehicleMountConfigButtonResume, 0, 0, 0x004EA7AF, 0x004ECC43);
	MultiPointer(ptrVehicleMountConfigButtonNext, 0, 0, 0x004EA7DF, 0x004ECC73);
	CodePatch invalidparentpartcrashcatcher = { ptrVehicleMountConfigButtonFn, "", "\xE9IPPC", 5, false };
	NAKED void InvalidParentPartCrashCatcher() {
		__asm {
			test ecx, ecx //Test for 0x00000000 (Invalid part parent for mount)
			je __crashEscape
			mov eax, ecx
			mov ecx, [eax]
			jmp[ptrVehicleMountConfigButtonResume] //Jump back into the native function
			__crashEscape:
			jmp[ptrVehicleMountConfigButtonNext]
		}
	}

	//Fix the invalid damage part fix
	MultiPointer(ptrVehicleMountConfigButtonFn02, 0, 0, 0x004EA7BA, 0x004ECC4E);
	MultiPointer(ptrVehicleMountConfigButtonResume02, 0, 0, 0x004EA7C3, 0x004ECC57);
	CodePatch invalidpartfixfix = { ptrVehicleMountConfigButtonFn02, "", "\xE9IPC1", 5, false };
	NAKED void InvalidPartFixFix() {
		__asm {
			test eax, eax //Test for 0x00000000
			je __crashEscape
			mov edx, [eax + 0xC]
			mov ecx, edx
			not ecx
			and edi, ecx
			jmp[ptrVehicleMountConfigButtonResume02] //Jump back into the native function
			__crashEscape:
			jmp[ptrVehicleMountConfigButtonResume02]
		}
	}

	MultiPointer(ptrGuiConsoleOpen, 0, 0, 0x0059D9EA, 0x005A1206);
	MultiPointer(ptrGuiConsoleOpenJNZ, 0, 0, 0x0059DA20, 0x005A123C);
	MultiPointer(ptrGuiConsoleOpenRetn, 0, 0, 0x0059D9F0, 0x005A120C);
	CodePatch guiconsoleopen = { ptrGuiConsoleOpen, "", "\xE9GCOP", 5, false };
	static const char* s_NovaConsoleOpen = "Nova::pushConsole();";
	NAKED void GuiConsoleOpen() {
		__asm {
			push eax
			mov eax, [s_NovaConsoleOpen]
			push eax
			call Console::eval
			add esp, 0x8
			mov dl, [ebx + 0x2B]
			cmp dl, 1
			jnz __jnz
			jmp[ptrGuiConsoleOpenRetn]
			__jnz:
			jmp [ptrGuiConsoleOpenJNZ]
		}
	}

	BuiltInFunction("Nova::Console::onOpen", _novaconsoleonopen)
	{
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		//Just need the height
		int height = screen.y;
		if (argc > 1)
		{
			consoleVertOffset(atoi(argv[0]));
		}
		else
		{
			consoleVertOffset((-height) + 480 - atoi(argv[0]));
		}
		simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_Default, 1).Apply(true);
		return 0;
	}


	//Waitroomgui crash fix for when players load the waitroomgui in a team based server while being on the neutral team
	MultiPointer(ptrTeamColorSel, 0, 0, 0, 0x00693F2D);
	MultiPointer(ptrTeamColorSelResume, 0, 0, 0, 0x00693F33);
	MultiPointer(ptrTeamColorSelJL, 0, 0, 0, 0x00693F39);
	CodePatch teamselectioncrashfix = { ptrTeamColorSel, "", "\xE9TSCRF", 5, false };
	NAKED void TeamSelectionCrashFix() {
		__asm {
			test edx, edx //Test for 0x00000000
			je __skipBL
			mov bl, [edx]
			cmp al, 0x61
			jl __jl
			jmp[ptrTeamColorSelResume] //Jump back into the native function

			__skipBL:
			cmp al, 0x61
				jl __jl

				__jl:
			jmp[ptrTeamColorSelJL]
		}
	}

	//Interior property apply crash fix for opengl (Part 1)
	MultiPointer(ptrInteriorRender01, 0, 0, 0, 0x0061D058);
	MultiPointer(ptrInteriorRender01Resume, 0, 0, 0, 0x0061D061);
	CodePatch interiorcrashpatch01 = { ptrInteriorRender01, "", "\xE9ICF1", 5, false };
	NAKED void InteriorCrashPatch01() {
		__asm {
			mov ecx, [edx + 0x80]
			test ecx, ecx //Test for 0x00000000
			jz __jz
			mov eax, [ecx + eax * 0x4]
			jmp[ptrInteriorRender01Resume] //Jump back into the native function
				__jz:
			jmp[ptrInteriorRender01Resume]
		}
	}

	//Interior property apply crash fix for opengl (Part 2)
	MultiPointer(ptrInteriorRender02, 0, 0, 0, 0x0061D0F1);
	MultiPointer(ptrInteriorRender02Resume, 0, 0, 0, 0x0061D0F7);
	CodePatch interiorcrashpatch02 = { ptrInteriorRender02, "", "\xE9ICF2", 5, false };
	NAKED void InteriorCrashPatch02() {
		__asm {
			test edx, edx //Test for 0x00000000
			jz __jz
			mov edx, [edx + 0xC]
			mov edx, [edx + eax * 0x4]
			jmp[ptrInteriorRender02Resume] //Jump back into the native function
			__jz:
			jmp[ptrInteriorRender02Resume]
		}
	}

	//Interior property apply crash fix for opengl (Part 3)
	MultiPointer(ptrInteriorRender03, 0, 0, 0, 0x0061D247);
	MultiPointer(ptrInteriorRender03Resume, 0, 0, 0, 0x0061D24D);
	CodePatch interiorcrashpatch03 = { ptrInteriorRender03, "", "\xE9ICF3", 5, false };
	NAKED void InteriorCrashPatch03() {
		__asm {
			test edx, edx //Test for 0x00000000
			jz __jz
			mov ecx, [edx + 0xC]
			mov edx, [ecx + eax * 0x4]
			jmp[ptrInteriorRender03Resume] //Jump back into the native function
			__jz:
			jmp[ptrInteriorRender03Resume]
		}
	}


	//No computer crash fix
	MultiPointer(ptrComputerSounds, 0, 0, 0x004763A5, 0x00477F79);
	MultiPointer(ptrComputerSoundsResume, 0, 0, 0x004763AE, 0x00477F82);
	MultiPointer(ptrComputerSoundsPop, 0, 0, 0x004764A6, 0x0047807D);
	CodePatch nocomputerfix = { ptrComputerSounds, "", "\xE9NCPF", 5, false };
	NAKED void NoComputerFix() {
		__asm {
			mov edx, [ebx + 0x0AFC]
			test edx, edx //Test for 0x00000000
			jz __jz
			mov ecx, [edx + 0x1C]
			jmp[ptrComputerSoundsResume] //Jump back into the native function
			__jz:
			jmp[ptrComputerSoundsPop]
		}
	}

	//Loading from WaitroomGUI to WaitroomGUI crash fix
	MultiPointer(ptrWaitroomCrash, 0, 0, 0x00540BA1, 0x005430D1);
	MultiPointer(ptrWaitroomCrashResume, 0, 0, 0x00540BA9, 0x005430D9);
	MultiPointer(ptrWaitroomCrashPop, 0, 0, 0x00540C04, 0x00543134);
	CodePatch waitroomtowaitroomfix = { ptrWaitroomCrash, "", "\xE9WRLC", 5, false };
	NAKED void WaitroomToWaitroomFix() {
		__asm {
			mov edx, [eax]
			test edx, edx
			jz __jz
			cmp [edx + 0x100], 0x70FE54
			je __jz
			call dword ptr [edx + 0x100]
			jmp[ptrWaitroomCrashResume] //Jump back into the native function
			__jz:
			jmp[ptrWaitroomCrashPop]
		}
	}


	MultiPointer(ptrDamageStatusUpdateDelay, 0, 0, 0x006F0680, 0x00700908);
	MultiPointer(ptrTargetDamageStatusUpdateDelay, 0, 0, 0x006F069C, 0x00700924);
	MultiPointer(ptrStatusRotateInc, 0, 0, 0x00524EB8, 0x00527358);
	CodePatch DamageStatDelay = { ptrDamageStatusUpdateDelay, "", "\x00\x00\x00\x00\x00", 5, false };
	CodePatch TargetDamageStatDelay = { ptrTargetDamageStatusUpdateDelay, "", "\x00\x00\x00\x00\x00", 5, false };
	CodePatch StatusRotateAmount = { ptrStatusRotateInc, "", "\x8F\xC2\x75\x3C", 4, false };
	
	MultiPointer(ptrRadarUpdateRate, 0, 0, 0x006F05F8, 0x00700880);
	CodePatch RadarUpdateRate = { ptrRadarUpdateRate, "", "\x00", 1, false };

	//Disable control letter <r><R><l><L> on chat messages
	MultiPointer(ptr_ControlLetterIndentationBypass, 0, 0, 0x005C6AB2, 0x005CA355);
	CodePatch filterRLFormatters = { ptr_ControlLetterIndentationBypass, "", "\xEB\xF4", 2, false };

	MultiPointer(ptrHercCameraAnimationRate, 0, 0, 0, 0x0049E988);
	MultiPointer(ptrHercShapeAnimationRate, 0, 0, 0, 0x0049E990);
	MultiPointer(ptrHercNetSyncRate, 0, 0, 0, 0x0049E937);
	CodePatch uncapHercCameraAnimationRate = { ptrHercCameraAnimationRate, "", "\x00\x00\x00\x00\x00\x00\x00\x00", 8, false };
	CodePatch uncapHercShapeAnimationRate = { ptrHercCameraAnimationRate, "", "\x00\x00\x00\x00\x00\x00\x00\x00", 8, false };
	CodePatch uncapHercNetSync = { ptrHercNetSyncRate, "", "\x68\x00\x00\x00\x00", 5, false };

	MultiPointer(ptrSimguiGuiTextEditThans, 0, 0, 0, 0x004E6CCD);
	CodePatch allowLessThanGreaterThans = { ptrSimguiGuiTextEditThans, "", "\xEB\x06", 2, false };

	MultiPointer(ptrSPBarSlideOutIncrementSize, 0, 0, 0, 0x0054416A);
	CodePatch SPBarSlideOutInc = { ptrSPBarSlideOutIncrementSize, "", "\x02", 1, false };
	MultiPointer(ptrSPBarSlideInIncrementSize, 0, 0, 0, 0x0054419B);
	CodePatch SPBarSlideInInc = { ptrSPBarSlideInIncrementSize, "", "\x02", 1, false };
	MultiPointer(ptrSPBarSlideDuration, 0, 0, 0, 0x005441C8);
	CodePatch SPBarSlideDuration = { ptrSPBarSlideDuration, "", "\x00\x00\x00\x3A", 4, false };

	//Crash occuring on map load
	MultiPointer(unk667E90, 0, 0, 0, 0x00667EA6);
	MultiPointer(unk667E90_loop, 0, 0, 0, 0x00667EAC);
	CodePatch maploadPatch = { unk667E90, "", "\xE9MPPT", 5, false };
	NAKED void mapLoadPatch() {
		__asm {
			mov ebx, [eax + 8]
			cmp ecx, 0
			je __je
			mov [ecx + ebx * 4], edx
			jmp [unk667E90_loop]
			__je:
			jmp [unk667E90_loop]
		}
	}

	//setHudMapViewOffset - patched to allow the client to change their mapview offset regardless if they are the server or not
	MultiPointer(ptrSetHudMapViewOffset1, 0, 0, 0, 0x004BCB40);
	MultiPointer(ptrSetHudMapViewOffset2, 0, 0, 0, 0x004BCB6C);
	MultiPointer(ptrSetHudMapViewOffset3, 0, 0, 0, 0x0041E176);
	CodePatch setHudMapViewOffsetPatch1 = { ptrSetHudMapViewOffset1, "", "\xEB", 1, false };
	CodePatch setHudMapViewOffsetPatch2 = { ptrSetHudMapViewOffset2, "", "\xEB\x03", 2, false };
	CodePatch setHudMapViewOffsetPatch3 = { ptrSetHudMapViewOffset3, "", "\x90\x90\x90", 3, false };

	struct Init {
		Init() {
			//WindowsCompatMode();
			if (VersionSnoop::GetVersion() == VERSION::vNotGame) {
				return;
			}

			//MultiPointer(ptrNavRedirect, 0, 0, 0, 0x0058C322);
			if (VersionSnoop::GetVersion() == VERSION::v001004) {
				dosfix.DoctorRelative((u32)DosFix, 1).Apply(true);
				//CodePatch NavRedirJump = { ptrNavRedirect,"","\xEA\x00\x0E\xB9\x14",5,false }; NavRedirJump.Apply(true);
			}

			//if (VersionSnoop::GetVersion() == VERSION::v001003) {
			//	unk719FC8.Apply(true);
			//	ARB.Apply(true);
			//	SGIS_to_ARB1.Apply(true);
			//	SGIS_to_ARB2.Apply(true);
			//	SGIS_to_ARB3.Apply(true);
			//	GL_SGIS_multiTexPatch.Apply(true);
			//}
			navfix.DoctorRelative((u32)NavFix, 1).Apply(true);
			
			missioncreaterestore.DoctorRelative((u32)MissionCreateRestore, 1).Apply(true);
			datadumprestore.DoctorRelative((u32)DataDumpRestore, 1).Apply(true);
			//interiorpluginrestore.DoctorRelative((u32)InteriorPluginRestore, 1).Apply(true);

			//ITRpatch.DoctorRelative((u32)ITRPatch, 1).Apply(true);
			VolumetricFaceColor1.Apply(true);
			VolumetricFaceColor2.Apply(true);
			VolumetricFaceColor3.Apply(true);
			//catch_flr_crash.DoctorRelative((u32)Catch_FLR_Crash0, 1).Apply(true);
			//software_widescreen_patch.DoctorRelative((u32)Software_Widescreen_Patch, 1).Apply(true);
			navExploitFix0.Apply(true);
			navExploitFix1.Apply(true);
			IDACTION_COLLISION_DETAILfix.Apply(true);
			mapviewObjectLatency.Apply(true);
			mapviewInterfaceLatency.Apply(true);

			//OpenGLBitDepth.Apply(true);
			//OpenGLWidthMin.Apply(true);
			//OpenGLWidthMax.Apply(true);
			//OpenGLHeightMin.Apply(true);
			//OpenGLHeightMax.Apply(true);
			SoftwareResWidthCap.Apply(true);
			SoftwareResHeightCap.Apply(true);

			NoCDPatch.Apply(true);

			//Null the console newline feed character to prevent the white screen of death
			consoleNewLineChar.Apply(true);
			//Filter <L> <R> chat formatters
			filterRLFormatters.Apply(true);

			//Fix player vehicle properties
			PlayerVehiclePropertyCheckPatch.Apply(true);

			//Allow drones to be pilotable by players (NYI)
			//dronetypeallow.DoctorRelative((u32)droneTypeAllow, 1).Apply(true);

			//Make all combo boxes have a fixed vertical size to prevent them from extending offscreen in upscaled OGL
			comboboxfixedsize.DoctorRelative((u32)ComboboxFixedSize, 1).Apply(true);

			//Adjust Simgui::TestButton colors
			TestButtonFillColor.Apply(true);
			TestButtonBorderColor.Apply(true);
			TestButtonSelectedBorderColor.Apply(true);
			TestButtonFillOpacity.Apply(true);

			//Event Patches
			//dinput8fix.Apply(true);

			//Window Patches
			//windowstyle.DoctorRelative((u32)WindowStyle, 1).Apply(true);

			terraincrashcatcher.DoctorRelative((u32)TerrainCrashCatcher, 1).Apply(true);
			invalidparentpartcrashcatcher.DoctorRelative((u32)InvalidParentPartCrashCatcher, 1).Apply(true);
			invalidpartfixfix.DoctorRelative((u32)InvalidPartFixFix, 1).Apply(true);

			//Fix material lists crashing when applied to a SimVolumetric
			volumetriccrashcatcher.DoctorRelative((u32)VolumetricCrashCatcher, 1).Apply(true);
			//recordcrashcatcher.DoctorRelative((u32)RecordCrashCatcher, 1).Apply(true);

			//Console open
			guiconsoleopen.DoctorRelative((u32)GuiConsoleOpen, 1).Apply(true);

			//Neutral team crash fix
			teamselectioncrashfix.DoctorRelative((u32)TeamSelectionCrashFix, 1).Apply(true);

			//Interior apply crash fix (Editor)
			interiorcrashpatch01.DoctorRelative((u32)InteriorCrashPatch01, 1).Apply(true);
			interiorcrashpatch02.DoctorRelative((u32)InteriorCrashPatch02, 1).Apply(true);
			interiorcrashpatch03.DoctorRelative((u32)InteriorCrashPatch03, 1).Apply(true);

			//No computer fix
			nocomputerfix.DoctorRelative((u32)NoComputerFix, 1).Apply(true);

			//Waitroomgui to waitroomgui fix
			//waitroomtowaitroomfix.DoctorRelative((u32)WaitroomToWaitroomFix, 1).Apply(true);

			//Patches for crashes I don't know what the actual causes are
			unkCrashPatch01.Apply(true);

			//Update Frequency of damage displays
			DamageStatDelay.Apply(true);
			TargetDamageStatDelay.Apply(true);
			StatusRotateAmount.Apply(true);//Slow down the rotation to align with the uncapped status display fps

			//Herc
			uncapHercCameraAnimationRate.Apply(true);
			uncapHercShapeAnimationRate.Apply(true);
			//uncapHercNetSync.Apply(true); - NOPE, this breaks vehicles when they shoot

			//Query the ENTIRE $Inet::Master array, not just the 1st entry.
			masterqueryall.DoctorRelative((u32)masterQueryAll, 1).Apply(true);
			masterquerycheck.DoctorRelative((u32)masterQueryCheck, 1).Apply(true);
			//patchOutIncrementalQueryRetry1.Apply(true);
			//patchOutIncrementalQueryRetry2.Apply(true);

			//Allow Less-Thans Greater-Thans '<>'
			allowLessThanGreaterThans.Apply(true);

			//Smooth the SPBar slide animation in the campaign GUI
			SPBarSlideOutInc.Apply(true);
			SPBarSlideInInc.Apply(true);
			SPBarSlideDuration.Apply(true);

			//Map load patch which fixes crashing after creating a local server, dropping into the map, exiting the server, and repeating
			maploadPatch.DoctorRelative((u32)mapLoadPatch, 1).Apply(true);

			//Allow setHudMapViewOffset on other servers
			setHudMapViewOffsetPatch1.Apply(true);
			setHudMapViewOffsetPatch2.Apply(true);
			setHudMapViewOffsetPatch3.Apply(true);
		}
	} init;
}; // namespace ExeFixes