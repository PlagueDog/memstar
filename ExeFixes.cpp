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

namespace ExeFixes {

	HWND getHWND() {
		MultiPointer(ptrHWND, 0, 0, 0x00705C5C, 0x007160CC);
		uintptr_t HWND_PTR = ptrHWND;
		int GAME_HWND = *reinterpret_cast<int*>(HWND_PTR);
		HWND SS_HWND = reinterpret_cast<HWND>(GAME_HWND);
		return SS_HWND;
	}

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

	int initialTick = static_cast<int>(GetTickCount());
	BuiltInFunction("getTickCount", _getTickCount)
	{
		int currentTick = static_cast<int>(GetTickCount());
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
		//Console::setVariable("pref::OpenGL::GL_NEAREST", "true");
		return "true";
	}

	BuiltInFunction("OpenGL::disableGL_NEAREST", _oglDgln)
	{
		CodePatch OpenGLFiltering = { ptrOGLTextureFilter,"","\x01\x26",2,false };
		OpenGLFiltering.Apply(true);
		Console::eval("flushtexturecache();");
		//Console::setVariable("pref::OpenGL::GL_NEAREST", "false");
		return "true";
	}
	CodePatch OpenGLBitDepth = { ptrOGLBitDepth,"","\x20",1,false };

	CodePatch OpenGLWidthMin = { ptrOGLWidthMin,"","\x3D\x40\x01",3,false };
	//CodePatch OpenGLWidthMax = { ptrOGLWidthMax,"","\x3D\x00\x0A",3,false };
	CodePatch OpenGLWidthMax = { ptrOGLWidthMax,"","\x3D\x00\x1E",3,false };
	CodePatch OpenGLHeightMin = { ptrOGLHeightMin,"","\x81\xFA\xF0\x00",4,false };
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

	MultiPointer(ptrVolumetricERR, 0, 0, 0, 0x0062E7CA);
	MultiPointer(ptrVolumetricERRresume, 0, 0, 0, 0x0062E7D5);
	CodePatch volumetricfix = {
	ptrVolumetricERR,
	"",
	"\xE9VMFX",
	5,
	false
	};

	NAKED void VolumetricFix() {
		__asm {
			lea edx, [ecx + edx * 0x8]
			cmp byte ptr[edx], 0xF4D8A358
			je __errBypass
			mov ecx, [edx]
			mov[eax + 0x20], ecx
			mov ecx, [edx + 0x4]
			jmp[ptrVolumetricERRresume]
			__errBypass:
			jmp[ptrVolumetricERRresume]
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
	CodePatch TestButtonBorderColor = { ptrTestButtonBorderColor,	"","\xE9",	1,false };
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
	
	BuiltInFunction("Mem::AdjustLensflareBitmaps", _memalb)
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

	BuiltInFunction("Mem::EnableHiresShadows", _memehs)
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

	BuiltInFunction("Mem::DisableHiresShadows", _memdhs)
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

	//Offset formula (Resolution-height) - 640 + 170 = 1:1 vertical offset of the console
	NAKED void SimGuiConsoleVert_720p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFF05
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_800p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFEB5
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_864p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFE75
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_990p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFDF7
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_1080p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFD9D
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_1200p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFD25
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_1440p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFC35
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_1536p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFBD5
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_1620p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFB81
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_1800p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFACD
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_2160p() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFF965
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	NAKED void SimGuiConsoleVert_Default() {
		__asm {
			sub eax, edx
			add eax, 0xFFFFFFF6
			jmp[ptrGuiConsoleVertRetn]
		}
	}

	BuiltInFunction("Console::RenderOffsetReset", _cror)
	{
		simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_Default, 1).Apply(true);
		return 0;
	}

	BuiltInFunction("Console::RenderOffset", _cro)
	{
		Vector2i screen;
		Fear::getScreenDimensions(&screen);
		//Just need the height
		int height = screen.y;

		//Reversed height lookup incase the player is using a resolution height we don't have programmed. This will position the console partially in the middle of the window instead of being cut off on the bottom.
		if (height <= 720) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_720p, 1).Apply(true); return 0; }
		if (height <= 800) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_800p, 1).Apply(true); return 0; }
		if (height <= 864) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_864p, 1).Apply(true); return 0; }
		if (height <= 990) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_990p, 1).Apply(true); return 0; }
		if (height <= 1080) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_1080p, 1).Apply(true); return 0; }
		if (height <= 1200) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_1200p, 1).Apply(true); return 0; }
		if (height <= 1440) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_1440p, 1).Apply(true); return 0; }
		if (height <= 1536) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_1536p, 1).Apply(true); return 0; }
		if (height <= 1620) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_1620p, 1).Apply(true); return 0; }
		if (height <= 1800) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_1800p, 1).Apply(true); return 0; }
		if (height <= 2160) { simguiconsolevert.DoctorRelative((u32)SimGuiConsoleVert_2160p, 1).Apply(true); return 0; }
		return 0;
	}

	//MultiPointer(ptrVideoModesBoxSize, 0, 0, 0, 0x004E17CA);
	MultiPointer(ptrComboBoxVertSizeMult, 0, 0, 0x004DE41E, 0x004E08AE);
	CodePatch ComboBoxFixedSize = { ptrComboBoxVertSizeMult, "", "\x90\x90\x90", 3, false };
	//MultiPointer(ptrComboBoxPadding, 0, 0, 0x004DF36B, 0x004E17FB);


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
	BuiltInFunction("Mem::enableFarWeather", _memefw)
	{
		CodePatch SnowfallMaxIntensity = { ptrSnowfallMaxIntensity, "", "\x00\x00\x48\x42", 4, false };
		CodePatch SnowfallMaxFlakes = { ptrSnowfallMaxFlakes, "", "\x00\xB1\x00\x00", 4, false };
		CodePatch SnowfallRadius = { ptrSnowfallRenderRadius, "", "\x00\x00\x40\x44", 4, false };
		//SNOW
		SnowfallMaxFlakes.Apply(true);
		SnowfallMaxIntensity.Apply(true);
		SnowfallRadius.Apply(true);

		CodePatch RainfallRenderRadius = { ptrRainfallRenderRadius, "", "\x00\x00\x00\x44", 4, false };
		CodePatch RainfallMaxDroplets = { ptrRainfallMaxDroplets, "", "\x00\x10\x00\x00", 4, false }; //Anything higher than 4096 will halt the game
		CodePatch RainfallSpeed = { ptrRainfallSpeed, "", "\x00\x00\x00\xC3", 4, false };
		//RAIN
		RainfallRenderRadius.Apply(true);
		RainfallSpeed.Apply(true);
		RainfallMaxDroplets.Apply(true);
		Console::eval("if(isObject(628)){storeObject(628,'temp\\\\clip.tmp');loadObject(initSnowfall, 'temp\\\\clip.tmp');deleteObject(initSnowfall);}"); //Force run "static void initSnow(snowGlobals *snow)"
		return 0;
	}

	BuiltInFunction("Mem::disableFarWeather", _memdfw)
	{
		CodePatch SnowfallMaxIntensity = { ptrSnowfallMaxIntensity, "", "\x00\x00\x80\x3F", 4, false };
		CodePatch SnowfallMaxFlakes = { ptrSnowfallMaxFlakes, "", "\x00\x10\x00\x00", 4, false };
		CodePatch SnowfallRadius = { ptrSnowfallRenderRadius, "", "\x00\x00\x48\x43", 4, false };
		//SNOW
		SnowfallMaxFlakes.Apply(true);
		SnowfallMaxIntensity.Apply(true);
		SnowfallRadius.Apply(true);

		CodePatch RainfallRenderRadius = { ptrRainfallRenderRadius, "", "\x00\x00\xDC\x42", 4, false };
		CodePatch RainfallMaxDroplets = { ptrRainfallMaxDroplets, "", "\x00\xB1\x00\x00", 4, false };
		CodePatch RainfallSpeed = { ptrRainfallSpeed, "", "\x00\x00\xAA\xC2", 4, false };
		//RAIN
		RainfallRenderRadius.Apply(true);
		RainfallSpeed.Apply(true);
		RainfallMaxDroplets.Apply(true);
		Console::eval("if(isObject(628)){storeObject(628,'temp\\\\clip.tmp');loadObject(initSnowfall, 'temp\\\\clip.tmp');deleteObject(initSnowfall);}");
		return 0;
	}

	MultiPointer(ptrMaxAnimationsTSS, 0, 0, 0, 0x0073DB78);

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

	//Catch Volumetric DML render crash
	MultiPointer(ptrVolumetRend, 0, 0, 0, 0x0062E7CA);
	MultiPointer(ptrVolumetRendCont, 0, 0, 0, 0x0062E7D5);
	CodePatch volumetriccrashcatcher = { ptrVolumetRend, "", "\xE9VDCR", 5, false };
	NAKED void VolumetricCrashCatcher() {
		__asm {
			//test edx, edx //Test for greater than 0xF7000000
			//cmp edx, 0xF7000000
			//ja __fixptr
			//lea edx, [ecx + edx * 8]
			lea edx, [ecx + edx * 8]
			mov ecx, [edx]
			mov [eax + 0x20], ecx
			mov ecx, [edx + 4]
			jmp[ptrVolumetRendCont]
				//__fixptr:
				//sub edx, 0xF7000000
				//mov ecx, [edx - 0xF7000000]
				//mov [eax + 0x20], ecx
				//mov ecx, [edx - 0xF7000000 + 4]
				//jmp[ptrVolumetRendCont]
		}
	}

	//Volumetric Face Rendering
	MultiPointer(ptrVolumetricFaceRender, 0, 0, 0, 0x006810BC);
	//Terrain Tile Rendering
	MultiPointer(ptrTerrainTileRender, 0, 0, 0, 0x00600AB1);
	//Terrain Mip Rendering
	MultiPointer(ptrTerrainMipRender, 0, 0, 0, 0x00601AAC);

	struct Init {
		Init() {
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
			
			//missioncreaterestore.DoctorRelative((u32)MissionCreateRestore, 1).Apply(true);
			//interiorpluginrestore.DoctorRelative((u32)InteriorPluginRestore, 1).Apply(true);

			//ITRpatch.DoctorRelative((u32)ITRPatch, 1).Apply(true);
			//volumetricfix.DoctorRelative((u32)VolumetricFix, 1).Apply(true);
			//catch_flr_crash.DoctorRelative((u32)Catch_FLR_Crash0, 1).Apply(true);
			//software_widescreen_patch.DoctorRelative((u32)Software_Widescreen_Patch, 1).Apply(true);
			navExploitFix0.Apply(true);
			navExploitFix1.Apply(true);
			IDACTION_COLLISION_DETAILfix.Apply(true);
			mapviewObjectLatency.Apply(true);
			mapviewInterfaceLatency.Apply(true);

			OpenGLBitDepth.Apply(true);
			OpenGLWidthMin.Apply(true);
			OpenGLWidthMax.Apply(true);
			OpenGLHeightMin.Apply(true);
			OpenGLHeightMax.Apply(true);
			SoftwareResWidthCap.Apply(true);
			SoftwareResHeightCap.Apply(true);

			NoCDPatch.Apply(true);

			//Null the console newline feed character to prevent the white screen of death
			consoleNewLineChar.Apply(true);

			//Fix player vehicle properties
			//PlayerVehiclePropertyCheckPatch.Apply(true);

			//Allow drones to be pilotable by players (NYI)
			//dronetypeallow.DoctorRelative((u32)droneTypeAllow, 1).Apply(true);

			//Make all combo boxes have a fixed vertical size to prevent them from extending offscreen in upscaled OGL
			ComboBoxFixedSize.Apply(true);

			//Adjust Simgui::TestButton colors
			TestButtonFillColor.Apply(true);
			TestButtonBorderColor.Apply(true);
			TestButtonSelectedBorderColor.Apply(true);
			TestButtonFillOpacity.Apply(true);

			//Event Patches
			//dinput8fix.Apply(true);

			//Window Patches
			windowstyle.DoctorRelative((u32)WindowStyle, 1).Apply(true);

			terraincrashcatcher.DoctorRelative((u32)TerrainCrashCatcher, 1).Apply(true);
			//volumetriccrashcatcher.DoctorRelative((u32)VolumetricCrashCatcher, 1).Apply(true);
			//recordcrashcatcher.DoctorRelative((u32)RecordCrashCatcher, 1).Apply(true);
		}
	} init;
}; // namespace ExeFixes