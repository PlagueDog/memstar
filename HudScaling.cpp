#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <string>
#include <filesystem>

using namespace std;
MultiPointer(ptrhudpbafix, 0, 0, 0x0063F6B1, 0x0064EAA5);
MultiPointer(HudPBAResume, 0, 0, 0x0063F609, 0x0064E9FD);
CodePatch hudpbafix = { ptrhudpbafix, "", "\xE9PBAF" , 5, false };
//Fixes custom size PBA hud elements crashing the game
NAKED void HudPBAFix() {
	__asm {
		mov edx, [edi + 4]
		test edx, edx
		jns __popHudPBAroutine
		cmp ebp, edx
		jl __JL_HudPBAResume
		__JL_HudPBAResume :
		jmp[HudPBAResume]
			__popHudPBAroutine :
			pop ebp
			pop edi
			pop esi
			pop ebx
			retn
	}

}



//Hud Elements Constants Set Patch
//Allow hud elements to always refresh their constants after being set
MultiPointer(ptrRadarConstantsSetBoolean, 0, 0, 0x00516CA4, 0x00519140);
MultiPointer(ptrShieldConstantsSetBoolean, 0, 0, 0x0051B785, 0x0051DC25);
CodePatch RadarElementsAlwaysRefresh = { ptrRadarConstantsSetBoolean,	"","\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90",	14,false };
CodePatch ShieldElementsAlwaysRefresh = { ptrShieldConstantsSetBoolean,	"","\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90",	13,false };
MultiPointer(RadarRefreshByte, 0, 0, 0, 0x007027C0);
CodePatch RadarVectorsAlwaysRefresh = { RadarRefreshByte,	"","\x00",	1,false };
MultiPointer(ShieldsRefresh, 0, 0, 0, 0x0051E0C6);
CodePatch ShieldVectorsAlwaysRefresh = { ShieldsRefresh,	"","\xEB\x0D",	2,false };


//Simgui::HudMtrWeaponDisplay pointers
MultiPointer(WeaponDisplay_FontTag, 0, 0, 0x00521EB6, 0x00524356);
MultiPointer(WeaponDisplay_WeaponSelectedImageTag, 0, 0, 0x00521EC0, 0x00524360);
MultiPointer(WeaponDisplay_WeaponUnselectedImageTag, 0, 0, 0x00521ECA, 0x0052436A);
MultiPointer(WeaponDisplay_BoxHighlightLength, 0, 0, 0x00521ED4, 0x00524374);
MultiPointer(WeaponDisplay_RowVerticalSpacing, 0, 0, 0x00521EDE, 0x0052437E);
MultiPointer(WeaponDisplay_ColumnWidth, 0, 0, 0x00521EF2, 0x00524392);
MultiPointer(WeaponDisplay_WeaponNameBoxLength, 0, 0, 0x00521EFC, 0x0052439C);
MultiPointer(WeaponDisplay_AmmoBoxLength, 0, 0, 0x00521F06, 0x005243A6);
MultiPointer(WeaponDisplay_RowHeight, 0, 0, 0x00521F10, 0x005243B0);
MultiPointer(WeaponDisplay_WeaponGroupRectangularColumn, 0, 0, 0x00521F1A, 0x005243BA);
MultiPointer(WeaponDisplay_WeaponRollPitchError_FontTag, 0, 0, 0x00521F1A, 0x00524441);

//Simgui::HudMtrWeaponDisplay Palette Color Sets
MultiPointer(WeaponDisplay_OpaqueBackground, 0, 0, 0, 0x005249C5);
MultiPointer(WeaponDisplay_GroupOpaqueBackground, 0, 0, 0, 0x00524A47);
MultiPointer(WeaponDisplay_WeaponChargeOutline, 0, 0, 0, 0x00524DD2);
MultiPointer(WeaponDisplay_WeaponChargeBar, 0, 0, 0, 0x00524E5E);
MultiPointer(WeaponDisplay_SelectedWeapon, 0, 0, 0, 0x00524F36);
MultiPointer(WeaponDisplay_SelectedWeaponGroup, 0, 0, 0, 0x00524F36);


//Simgui::HudMtrRadar pointers
MultiPointer(RadarDisplay_FontTag, 0, 0, 0x00516C4D, 0x005190E9);
MultiPointer(RadarDisplay_Scale, 0, 0, 0x00516EAC, 0x00519348);
MultiPointer(RadarDisplay_SensorModeHorizontalOffset, 0, 0, 0x00516ECA, 0x00519366);
MultiPointer(RadarDisplay_SensorModeVerticalOffset, 0, 0, 0x00516ED4, 0x00519370);
MultiPointer(RadarDisplay_SpeedTextHorizontalOffset, 0, 0, 0x00516EDE, 0x0051937A);
MultiPointer(RadarDisplay_SpeedTextVerticalOffset, 0, 0, 0x00516EE8, 0x00519384);
MultiPointer(RadarDisplay_ContainerWidth, 0, 0, 0x00516EF2, 0x0051938E);
MultiPointer(RadarDisplay_ContainerHeight, 0, 0, 0x00516EFC, 0x00519398);
MultiPointer(RadarDisplay_LinesScale, 0, 0, 0x00516F34, 0x005193D0);
MultiPointer(RadarDisplay_ZenithLength, 0, 0, 0x00516F16, 0x005193B2);
MultiPointer(RadarDisplay_ZenithWidth, 0, 0, 0x00516F20, 0x005193BC);
MultiPointer(RadarDisplay_DiagonalOffset, 0, 0, 0x00516EB6, 0x00519352);
MultiPointer(RadarDisplay_VerticalOffset, 0, 0, 0x00516EC0, 0x0051935C);
MultiPointer(RadarDisplay_NorthSouthEastWestLines, 0, 0, 0x00516F2A, 0x005193C6);
MultiPointer(RadarDisplay_Perspective, 0, 0, 0x00516EA4, 0x00519340);
MultiPointer(RadarDisplay_EnergyLabelHorizontalOffset, 0, 0, 0x00516FB4, 0x00519450);
MultiPointer(RadarDisplay_EnergyLabelVerticalOffset, 0, 0, 0x00516FBE, 0x0051945A);
MultiPointer(RadarDisplay_EnergyStatusHorizontalOffset, 0, 0, 0x00516FC8, 0x00519464);
MultiPointer(RadarDisplay_EnergyStatusVerticalOffset, 0, 0, 0x00516FD2, 0x0051946E);
MultiPointer(RadarDisplay_ThrottleArrowWidth, 0, 0, 0x00517036, 0x005194D2);
MultiPointer(RadarDisplay_ThrottleArrowLength, 0, 0, 0x0051702C, 0x005194C8);
MultiPointer(RadarDisplay_ThrottleArrowHorizontalOffset, 0, 0, 0x00517040, 0x005194DC);
MultiPointer(RadarDisplay_EnergyArrowWidth, 0, 0, 0x00516FA0, 0x0051943C);
MultiPointer(RadarDisplay_EnergyArrowLength, 0, 0, 0x00516F96, 0x00519432);
MultiPointer(RadarDisplay_EnergyArrowHorizontalOffset, 0, 0, 0x00516FAA, 0x00519446);

//Simgui::HudMtrRadar pointers Palette Color Sets
MultiPointer(RadarDisplay_FOVLeftLine, 0, 0, 0, 0x0051A8D7);//F5
MultiPointer(RadarDisplay_FOVRightLine, 0, 0, 0, 0x0051A8E8);//F5
MultiPointer(RadarDisplay_SweepLine, 0, 0, 0, 0x0051AA3B); //F8
MultiPointer(RadarDisplay_NorthSouthEastWest, 0, 0, 0, 0x0051ABE8); //F4
MultiPointer(RadarDisplay_NorthSouthEastWestMids, 0, 0, 0, 0x0051AE18); //F4
MultiPointer(RadarDisplay_EnergyArrow, 0, 0, 0, 0x0051AF08); //F8
MultiPointer(RadarDisplay_ThrottleArrow, 0, 0, 0, 0x0051B20B); //F8

//Simgui::HudMtrDamage pointers
MultiPointer(VehicleDisplay_FontTag, 0, 0, 0x005232FC, 0x0052579C);
MultiPointer(VehicleDisplay_BoxHeight, 0, 0, 0x0052333F, 0x005257DF);
MultiPointer(VehicleDisplay_BoxWidth, 0, 0, 0x00523325, 0x005257C5);
MultiPointer(VehicleDisplay_RenderBoxHeight, 0, 0, 0x0052487C, 0x00526D1C);
MultiPointer(VehicleDisplay_RenderBoxWidth, 0, 0, 0x00524887, 0x00526D27);
MultiPointer(VehicleDisplay_RenderBoxHeight2, 0, 0, 0x005233E0, 0x00525880);
MultiPointer(VehicleDisplay_RenderBoxWidth2, 0, 0, 0x005233E5, 0x00525885);
MultiPointer(VehicleDisplay_TargetRenderBoxHeight, 0, 0, 0x0052586E, 0x00527D0E);
MultiPointer(VehicleDisplay_RenderSize, 0, 0, 0x00523EA9, 0x00526349);
MultiPointer(VehicleDisplay_RenderCullPoint1, 0, 0, 0x00524C73, 0x00527113);
MultiPointer(VehicleDisplay_RenderCullPoint2, 0, 0, 0x00524C90, 0x00527130);
MultiPointer(VehicleDisplay_RenderCullPoint3, 0, 0, 0x00524C9B, 0x0052713B);
MultiPointer(VehicleDisplay_RenderCullPoint4, 0, 0, 0x00524CB8, 0x00527158);

//Simgui::HudMtrChatRecieve pointers
MultiPointer(ChatRec_FontTag, 0, 0, 0x00526BB2, 0x00529052);
MultiPointer(ChatRec_FontTag_Dim, 0, 0, 0x00526BA2, 0x00529042);
MultiPointer(ChatRec_FontTag_Gray, 0, 0, 0x00526B98, 0x00529038);

MultiPointer(ChatRec_FontTag_Default, 0, 0, 0, 0x0052CB35);

MultiPointer(ChatRec_PresetWidth1, 0, 0, 0x00526BBE, 0x0052905E);
MultiPointer(ChatRec_PresetHeight1, 0, 0, 0x00526BC5, 0x00529065);

MultiPointer(ChatRec_PresetWidth2, 0, 0, 0x00526BD1, 0x00529071);
MultiPointer(ChatRec_PresetHeight2, 0, 0, 0x00526BD8, 0x00529078);

MultiPointer(ChatRec_PresetWidth3, 0, 0, 0x00526BE4, 0x00529084);
MultiPointer(ChatRec_PresetHeight3, 0, 0, 0x00526BEB, 0x0052908B);

//Simgui::HudMtrTimer pointers
MultiPointer(HudTimer_FontTag, 0, 0, 0x00528066, 0x0052A506);

//Simgui::HudMtrCommand pointers
MultiPointer(CmdMenu_FontTag, 0, 0, 0x005292F8, 0x0052B798);
MultiPointer(CmdMenu_Width, 0, 0, 0x0052931F, 0x0052B7BF);

//Simgui::HudMtrShields pointers
MultiPointer(ShieldDisplay_FontTag, 0, 0, 0x0051B763, 0x0051DC03);
MultiPointer(ShieldDisplay_LabelTextHorizontalOffset, 0, 0, 0x0051B888, 0x0051DD28);
MultiPointer(ShieldDisplay_LabelTextVerticalOffset, 0, 0, 0x0051B892, 0x0051DD32);
MultiPointer(ShieldDisplay_StrengthTextHorizontalOffset, 0, 0, 0x0051B89C, 0x0051DD3C);
MultiPointer(ShieldDisplay_StrengthTextVerticalOffset, 0, 0, 0x0051B8A6, 0x0051DD46);

//Also determines the shield left/right line when using a SMOD component
MultiPointer(ShieldDisplay_StrengthArrowHorizontalOffset, 0, 0, 0x0051B84C, 0x0051DCEC); // 1X: 97  2x: E0  3X: 2501  4X: 7001

//Also determines the shield header/footer line when using a SMOD component
MultiPointer(ShieldDisplay_StrengthArrowVerticalOffset, 0, 0, 0x0051B856, 0x0051DCF6); // 1X: F2  2x: FA  3X: F4 DA  4X: EF

MultiPointer(ShieldDisplay_StrengthArrowPerspective, 0, 0, 0x0051B860, 0x0051DD00);
MultiPointer(ShieldDisplay_StrengthArrowWidth, 0, 0, 0x0051B87E, 0x0051DD1E);
MultiPointer(ShieldDisplay_StrengthArrowLength, 0, 0, 0x0051B874, 0x0051DD14);
MultiPointer(ShieldDisplay_DirectionalRingHorizontalOffset, 0, 0, 0x0051B8CE, 0x0051DD6E);
MultiPointer(ShieldDisplay_DirectionalRingVerticalOffset, 0, 0, 0x0051B8D8, 0x0051DD78);
MultiPointer(ShieldDisplay_DirectionalTextGapSize, 0, 0, 0x0051B8C4, 0x0051DD64); // 1X: 20 2x: 25  3X: 5A 4X: 55

//Effects vertical offset from the directional line
MultiPointer(ShieldDisplay_DirectionalInfoLineTextVerticalOffset, 0, 0, 0x0051B8E2, 0x0051DD82); // 1X: 03 2x: 03  3X: 03  4X: 03 

MultiPointer(ShieldDisplay_DirectionalInfoLine, 0, 0, 0x0051B842, 0x0051DCE2);
MultiPointer(ShieldDisplay_DirectionalInfoLineFooterTextZaxis, 0, 0, 0, 0x0051DC42); // 1X: 9A 2x: E0  3X: 2501  4X: 7001

//SimGui::HudMtrAimReticle pointers
MultiPointer(AimRet_FontTag, 0, 0, 0x0051D2D7, 0x0051F777);
MultiPointer(AimRet_TargetOutlineCornerScale, 0, 0, 0x0051D337, 0x0051F7D7);
MultiPointer(AimRet_TargetOutlineCornerLength, 0, 0, 0x0051D341, 0x0051F7E1);
MultiPointer(AimRet_TargetDistanceTextHorizontalOffset, 0, 0, 0x0051D326, 0x0051F7C6);
MultiPointer(AimRet_TargetDistanceTextVerticalOffset, 0, 0, 0x0051D32D, 0x0051F7CD);
MultiPointer(AimRet_LeadPointerVerticalScale, 0, 0, 0x0051D30E, 0x0051F7AE);
MultiPointer(AimRet_LeadPointerHorizontalScale, 0, 0, 0x0051D31D, 0x0051F7C0);
MultiPointer(AimRet_OffscreenPointerLength, 0, 0, 0x0051355, 0x0051F7F5);
MultiPointer(AimRet_OffscreenPointerWidth, 0, 0, 0x0051D35F, 0x0051F7FF);
//////
	//SimGui::HudMtrAimRetile(Target offscreen directional arrow)
	MultiPointer(AimRet_DiractionalArrowWidth, 0, 0, 0x0051ED5D, 0x005211FD);
	MultiPointer(AimRet_DiractionalArrowWholeScale, 0, 0, 0x0051ED4B, 0x005211EB); //1x:AA FE 3F, 2x: 80 FF 3F, 3x:D0 FF 3F 4x:A0 00 40
	MultiPointer(AimRet_DiractionalArrowPaletteColor, 0, 0, 0x0051ECD1, 0x00521171);
	MultiPointer(AimRet_DiractionalArrowTargetLOS, 0, 0, 0x0051E86D, 0x00520D0D);

//Simgui::HudMtrIntDamage (Internals)
MultiPointer(IntD_Dim_FontTag, 0, 0, 0x00525EC4, 0x00528364);
MultiPointer(IntD_BoxWidth, 0, 0, 0x00525ED4, 0x00528374);

//Simgui::HudMtrSelectPopUp (hud config window)
MultiPointer(HDCFG_Title_FontTag, 0, 0, 0x005277F1, 0x00529C91);
MultiPointer(HDCFG_VerticalSpacing, 0, 0, 0x005277DD, 0x00529C7D);
MultiPointer(HDCFG_WindowWidth, 0, 0, 0x005277CF, 0x00529C6F);
MultiPointer(HDCFG_ButtonFontTag, 0, 0, 0x0050DD9A, 0x00510232);
MultiPointer(HDCFG_ButtonHighlightFontTag, 0, 0, 0x0050DDA4, 0x0051023C);
MultiPointer(HDCFG_CycleColorsButtonFontTag, 0, 0, 0x0050E134, 0x005105CC);
MultiPointer(HDCFG_CycleColorsButtonHighlightFontTag, 0, 0, 0x0050E13E, 0x005105D6);

//HudObjectives.dlg
MultiPointer(HDOBJ_StatusActive_FontTag, 0, 0, 0x0052BC18, 0x0052E0D8);
MultiPointer(HDOBJ_StatusFailed_FontTag, 0, 0, 0x0052BC22, 0x0052E0EC);
MultiPointer(HDOBJ_StatusDisabled_FontTag, 0, 0, 0x0052BC2C, 0x0052E0E2);
MultiPointer(HDOBJ_WindowWidth, 0, 0, 0x0052BC5D, 0x0052E11D);
//MultiPointer(HDOBJ_WindowWidth, 0, 0, 0, 0x0052E11D);
//MultiPointer(HDOBJ_WindowHeight, 0, 0, 0, 0x0052E038);

//Simgui::HudDlgScoreBoard
MultiPointer(HudScoreboard_FontTag, 0, 0, 0, 0x0052CF3D);

//Simgui::HudDlgPrefPopUp
MultiPointer(HudGamePrefs_FontTag0, 0, 0, 0x0052AE58, 0x0052D318);
MultiPointer(HudGamePrefs_FontTag1, 0, 0, 0x0052B6B1, 0x0052DB71);
MultiPointer(HudGamePrefs_FontTag2, 0, 0, 0x0052B6CA, 0x0052DB8A);
MultiPointer(HudGamePrefs_FontTag3, 0, 0, 0x0052B7C4, 0x0052DC84);
MultiPointer(HudGamePrefs_FontTag4, 0, 0, 0x0052B7DD, 0x0052DC9D);
MultiPointer(HudGamePrefs_FontTag5, 0, 0, 0x0052B871, 0x0052DD31);
MultiPointer(HudGamePrefs_TextDimensions, 0, 0, 0x0052AE8A, 0x0052D34A);
MultiPointer(HudGamePrefs_ComboboxTextVertOffset, 0, 0, 0x0052AE9E, 0x0052D35E);

//Mapview font
MultiPointer(Mapview_FontFile, 0, 0, 0x006F1D68, 0x00701FF0);

//HudLabelFont
MultiPointer(HudLabel_FontTag, 0, 0, 0, 0x0050DBD4);

BuiltInFunction("HudManager::Multiplier", _HMM)
{
	if (argc != 2)
	{
		Console::echo("///Changing the scale of the radar/shields will require a re-launch of the game in order for some of their elements to display properly");
		Console::echo("HudManager::Multiplier( [weapons | radar | status | chat | orders | shields | retical | internals | timers | text], [1 | 2 | 3 | 4]);");
		return 0;
	}

	const char* str = argv[0];
	std::string arg1 = str;

	if (atoi(argv[1]) >= 1 && atoi(argv[1]) <= 4)
	{
		//1x Scale
		if (atoi(argv[1]) == 1)
		{
			//Console::echo("Debug1");
			if (arg1.compare("text") == 0)
			{
				CodePatch SB_Patch0 = { HudScoreboard_FontTag,	"","\xE4\x4B\x02",3,false }; SB_Patch0.Apply(true);
				Console::eval("IDFNT_HUD_HIGH_RES_I     		= 00150085, 'hud_high_i.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES     			= 00150087, 'hud_high.pft'; ");		   //F6
				Console::eval("IDFNT_HUD_HIGH_RES_DIM     		= 00150089, 'hud_high_dim.pft'; ");	   //F5
				Console::eval("IDFNT_HUD_HIGH_RES_GRAY     		= 00150091, 'hud_high_gray.pft'; ");   //FB
				Console::eval("IDFNT_HUD_HIGH_RES_YELLOW     	= 00150093, 'hud_high_yellow.pft'; "); //F0
				Console::eval("IDFNT_HUD_HIGH_RES_BLUE     		= 00150095, 'hud_high_blue.pft'; ");   //F1
				Console::eval("IDFNT_HUD_HIGH_RES_PURPLE     	= 00150097, 'hud_high_purple.pft'; "); //F3
				Console::eval("IDFNT_HUD_HIGH_RES_RED     		= 00150099, 'hud_high_red.pft'; ");	   //F2
				CodePatch HDOBJ_Patch0 = { HDOBJ_WindowWidth,	"","\x90\x01",2,false }; HDOBJ_Patch0.Apply(true);
				CodePatch MAPVW_Patch0 = { Mapview_FontFile,	"","hud_high.pft",11,false }; MAPVW_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch0 = { HudGamePrefs_TextDimensions,	"","\x0B",1,false }; HDGMCFG_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch1 = { HudGamePrefs_ComboboxTextVertOffset,	"","\x03\x00\x00\x00",4,false }; HDGMCFG_Patch1.Apply(true);
				Console::eval("$pref::hudScale::text = 1;");
			}
			else if (arg1.compare("weapons") == 0)
			{
				CodePatch WD_Patch0 = { WeaponDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; WD_Patch0.Apply(true);
				CodePatch WD_Patch0a = { WeaponDisplay_WeaponRollPitchError_FontTag,"","\xE6\x4B\x02",3,false }; WD_Patch0a.Apply(true);
				Console::eval("IDBMP_WP_CIRCLE_FILLED     		= 00160052, 'hud_circle_f.bmp';");
				Console::eval("IDBMP_WP_CIRCLE_OPEN     		= 00160053, 'hud_circle_o.bmp';");
				CodePatch WD_Patch3 = { WeaponDisplay_BoxHighlightLength,			"","\x04",1,false }; WD_Patch3.Apply(true);
				CodePatch WD_Patch4 = { WeaponDisplay_RowVerticalSpacing,			"","\x05",1,false }; WD_Patch4.Apply(true);
				CodePatch WD_Patch5 = { WeaponDisplay_ColumnWidth,					"","\x0F",1,false }; WD_Patch5.Apply(true);
				CodePatch WD_Patch6 = { WeaponDisplay_WeaponNameBoxLength,			"","\x3C\x00",2,false }; WD_Patch6.Apply(true);
				CodePatch WD_Patch7 = { WeaponDisplay_AmmoBoxLength,				"","\x3C\x00",2,false }; WD_Patch7.Apply(true);
				CodePatch WD_Patch8 = { WeaponDisplay_RowHeight,					"","\x08",1,false }; WD_Patch8.Apply(true);
				CodePatch WD_Patch9 = { WeaponDisplay_WeaponGroupRectangularColumn,	"","\x04",1,false }; WD_Patch9.Apply(true);
				Console::eval("$pref::hudScale::weapons = 1;");
			}
			else if (arg1.compare("radar") == 0)
			{
				Console::eval("IDPBA_RADAR_HIGH = 00170017, \"radar_high.pba\";");
				CodePatch RD_PatchF = { RadarDisplay_FontTag,								"","\xE4\x4B\x02",3,false }; RD_PatchF.Apply(true);
				CodePatch RD_Patch0 = { RadarDisplay_Scale,									"","\x42\x00",2,false }; RD_Patch0.Apply(true);
				CodePatch RD_Patch1 = { RadarDisplay_ContainerWidth,						"","\x23\x00",2,false }; RD_Patch1.Apply(true);
				CodePatch RD_Patch2 = { RadarDisplay_ContainerHeight,						"","\x0A\x00",2,false }; RD_Patch2.Apply(true);
				CodePatch RD_Patch3 = { RadarDisplay_LinesScale,							"","\x0C",1,false }; RD_Patch3.Apply(true);
				CodePatch RD_Patch4 = { RadarDisplay_ZenithLength,							"","\x0B",1,false }; RD_Patch4.Apply(true);
				CodePatch RD_Patch5 = { RadarDisplay_ZenithWidth,							"","\x0E",1,false }; RD_Patch5.Apply(true);
				CodePatch RD_Patch6 = { RadarDisplay_DiagonalOffset,						"","\xCE\x00\x00\x00",4,false }; RD_Patch6.Apply(true);
				CodePatch RD_Patch7 = { RadarDisplay_VerticalOffset,						"","\xE0\xFF\xFF\xFF",4,false }; RD_Patch7.Apply(true);
				CodePatch RD_Patch9 = { RadarDisplay_NorthSouthEastWestLines,				"","\x06",1,false }; RD_Patch9.Apply(true);
				CodePatch RD_Patch10 = { RadarDisplay_ThrottleArrowWidth,					"","\x09",1,false }; RD_Patch10.Apply(true);
				CodePatch RD_Patch11 = { RadarDisplay_ThrottleArrowLength,					"","\x17",1,false }; RD_Patch11.Apply(true);
				CodePatch RD_Patch12 = { RadarDisplay_ThrottleArrowHorizontalOffset,		"","\x5E\x00",2,false }; RD_Patch12.Apply(true);
				CodePatch RD_Patch13 = { RadarDisplay_EnergyArrowWidth,						"","\x08",1,false }; RD_Patch13.Apply(true);
				CodePatch RD_Patch14 = { RadarDisplay_EnergyArrowLength,					"","\x1E",1,false }; RD_Patch14.Apply(true);
				CodePatch RD_Patch15 = { RadarDisplay_EnergyArrowHorizontalOffset,			"","\x5F",1,false }; RD_Patch15.Apply(true);
				CodePatch RD_Patch16 = { RadarDisplay_SensorModeHorizontalOffset,			"","\x5B\x00",2,false }; RD_Patch16.Apply(true);
				CodePatch RD_Patch17 = { RadarDisplay_SensorModeVerticalOffset,				"","\xF6\xFF\xFF\xFF",4,false }; RD_Patch17.Apply(true);
				CodePatch RD_Patch18 = { RadarDisplay_SpeedTextHorizontalOffset,			"","\xF4\xFF\xFF\xFF",4,false }; RD_Patch18.Apply(true);
				CodePatch RD_Patch19 = { RadarDisplay_SpeedTextVerticalOffset,				"","\x1C\x00",2,false }; RD_Patch19.Apply(true);
				CodePatch RD_Patch20 = { RadarDisplay_EnergyLabelHorizontalOffset,			"","\x99\x00",2,false }; RD_Patch20.Apply(true);
				CodePatch RD_Patch21 = { RadarDisplay_EnergyLabelVerticalOffset,			"","\x69\x00",2,false }; RD_Patch21.Apply(true);
				CodePatch RD_Patch22 = { RadarDisplay_EnergyStatusHorizontalOffset,			"","\xBE\x00",2,false }; RD_Patch22.Apply(true);
				CodePatch RD_Patch23 = { RadarDisplay_EnergyStatusVerticalOffset,			"","\x74\x00",2,false }; RD_Patch23.Apply(true);
				CodePatch RD_Patch24 = { RadarDisplay_Perspective,							"","\x96\x43",2,false }; RD_Patch24.Apply(true);
				Console::eval("$pref::hudScale::radar = 1;");
			}
			else if (arg1.compare("status") == 0)
			{
				CodePatch VD_Patch0 = { VehicleDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; VD_Patch0.Apply(true);
				CodePatch HL_Patch = { HudLabel_FontTag,							"","\xE4\x4B\x02",3,false }; HL_Patch.Apply(true);
				CodePatch VD_Patch1 = { VehicleDisplay_BoxHeight,					"","\x1E",1,false }; VD_Patch1.Apply(true);
				CodePatch VD_Patch1a = { VehicleDisplay_TargetRenderBoxHeight,		"","\x1E",1,false }; VD_Patch1a.Apply(true);
				CodePatch VD_Patch2 = { VehicleDisplay_BoxWidth,					"","\x0A\x00\x00\x00",4,false }; VD_Patch2.Apply(true);
				CodePatch VD_Patch3 = { VehicleDisplay_RenderBoxHeight,				"","\x40\x00",2,false }; VD_Patch3.Apply(true);
				CodePatch VD_Patch4 = { VehicleDisplay_RenderBoxWidth,				"","\x40\x00",2,false }; VD_Patch4.Apply(true);
				CodePatch VD_Patch5 = { VehicleDisplay_RenderBoxHeight2,			"","\x40\x00",2,false }; VD_Patch5.Apply(true);
				CodePatch VD_Patch6 = { VehicleDisplay_RenderBoxWidth2,				"","\x40\x00",2,false }; VD_Patch6.Apply(true);
				CodePatch VD_Patch7 = { VehicleDisplay_RenderSize,					"","\x00\x00\x80\x42",4,false }; VD_Patch7.Apply(true);
				CodePatch VD_Patch8 = { VehicleDisplay_RenderCullPoint1,			"","\x80\x42",2,false }; VD_Patch8.Apply(true);
				CodePatch VD_Patch9 = { VehicleDisplay_RenderCullPoint2,			"","\x80\x42",2,false }; VD_Patch9.Apply(true);
				CodePatch VD_Patch10 = { VehicleDisplay_RenderCullPoint3,			"","\x80\x42",2,false }; VD_Patch10.Apply(true);
				CodePatch VD_Patch11 = { VehicleDisplay_RenderCullPoint4,			"","\x80\x42",2,false }; VD_Patch11.Apply(true);
				Console::eval("$pref::hudScale::status = 1;");
			}
			else if (arg1.compare("chat") == 0)
			{
				CodePatch CR_Patch0 = { ChatRec_PresetWidth1,						"","\x90\x01",2,false }; CR_Patch0.Apply(true);
				CodePatch CR_Patch1 = { ChatRec_PresetWidth2,						"","\x90\x01",2,false }; CR_Patch1.Apply(true);
				CodePatch CR_Patch2 = { ChatRec_PresetWidth3,						"","\x90\x01",2,false }; CR_Patch2.Apply(true);
				CodePatch CR_Patch3 = { ChatRec_PresetHeight1,						"","\x27\x00",2,false }; CR_Patch3.Apply(true);
				CodePatch CR_Patch4 = { ChatRec_PresetHeight2,						"","\x53\x00",2,false }; CR_Patch4.Apply(true);
				CodePatch CR_Patch5 = { ChatRec_PresetHeight3,						"","\x74\x00",2,false }; CR_Patch5.Apply(true);
				CodePatch CR_Patch6 = { ChatRec_FontTag,							"","\x47\x4A\x02",3,false }; CR_Patch6.Apply(true);
				CodePatch CR_Patch7 = { ChatRec_FontTag_Dim,						"","\x49\x4A\x02",3,false }; CR_Patch7.Apply(true);
				CodePatch CR_Patch8 = { ChatRec_FontTag_Gray,						"","\x4B\x4A\x02",3,false }; CR_Patch8.Apply(true);
				Console::eval("$pref::hudScale::chat = 1;");
			}
			else if (arg1.compare("timers") == 0)
			{
				CodePatch HT_Patch0 = { HudTimer_FontTag,							"","\xE4\x4B\x02",3,false }; HT_Patch0.Apply(true);
				Console::eval("$pref::hudScale::timers = 1;");
			}
			else if (arg1.compare("orders") == 0)
			{
				CodePatch CM_Patch0 = { CmdMenu_FontTag,							"","\xE4\x4B\x02",3,false }; CM_Patch0.Apply(true);
				CodePatch CM_Patch1 = { CmdMenu_Width,								"","\xA0\x00",2,false }; CM_Patch1.Apply(true);
				Console::eval("$pref::hudScale::orders = 1;");
			}
			else if (arg1.compare("shields") == 0)
			{
				Console::eval("IDBMP_SHIELDS = 00160012, \"shields.bmp\";");
				Console::eval("IDBMP_SHIELDS_SOLID = 00160166, \"shields_solid.bmp\";");
				Console::eval("IDBMP_STR_CIRCLE = 00160013, \"strCircle.bmp\";");
				CodePatch SD_Patch1 = { ShieldDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; SD_Patch1.Apply(true);
				CodePatch SD_Patch2 = { ShieldDisplay_DirectionalRingHorizontalOffset,"","\x0C",1,false }; SD_Patch2.Apply(true);
				CodePatch SD_Patch3 = { ShieldDisplay_DirectionalRingVerticalOffset,"","\x0E",1,false }; SD_Patch3.Apply(true);
				CodePatch SD_Patch4 = { ShieldDisplay_LabelTextHorizontalOffset,	"","\x30",1,false }; SD_Patch4.Apply(true);
				CodePatch SD_Patch5 = { ShieldDisplay_LabelTextVerticalOffset,		"","\x4C",1,false }; SD_Patch5.Apply(true);
				CodePatch SD_Patch6 = { ShieldDisplay_StrengthTextHorizontalOffset,	"","\x4E",1,false }; SD_Patch6.Apply(true);
				CodePatch SD_Patch7 = { ShieldDisplay_StrengthTextVerticalOffset,	"","\x57",1,false }; SD_Patch7.Apply(true);
				CodePatch SD_Patch8 = { ShieldDisplay_StrengthArrowVerticalOffset,	"","\xF2\xFF\xFF\xFF",4,false }; SD_Patch8.Apply(true);
				CodePatch SD_Patch9 = { ShieldDisplay_StrengthArrowHorizontalOffset,"","\x97\x00",2,false }; SD_Patch9.Apply(true);
				CodePatch SD_Patch10 = { ShieldDisplay_StrengthArrowPerspective,	"","\x00\x00\x96\x43",4,false }; SD_Patch10.Apply(true);
				CodePatch SD_Patch11 = { ShieldDisplay_StrengthArrowLength,			"","\x19",1,false }; SD_Patch11.Apply(true);
				CodePatch SD_Patch12 = { ShieldDisplay_StrengthArrowWidth,			"","\x08",1,false }; SD_Patch12.Apply(true);
				CodePatch SD_Patch13 = { ShieldDisplay_DirectionalTextGapSize,		"","\x20",1,false }; SD_Patch13.Apply(true);
				CodePatch SD_Patch14 = { ShieldDisplay_DirectionalInfoLineTextVerticalOffset,"","\x03",1,false }; SD_Patch14.Apply(true);
				CodePatch SD_Patch16 = { ShieldDisplay_DirectionalInfoLine,			"","\x37",1,false }; SD_Patch16.Apply(true);
				CodePatch SD_Patch17 = { ShieldDisplay_DirectionalInfoLineFooterTextZaxis,	 "","\x9A\x00",2,false }; SD_Patch17.Apply(true);
				Console::eval("$pref::hudScale::shields = 1;");
			}
			else if (arg1.compare("retical") == 0)
			{
				CodePatch AR_Patch0 = { AimRet_FontTag,								"","\xE4\x4B\x02",3,false }; AR_Patch0.Apply(true);
				CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x02",1,false }; AR_Patch1.Apply(true);
				CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x0A",1,false }; AR_Patch2.Apply(true);
				CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\x12\x00\x00\x00",4,false }; AR_Patch3.Apply(true);
				CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\xE0\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x18",1,false }; AR_Patch5.Apply(true);
				CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x03",1,false }; AR_Patch6.Apply(true);
				CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\xAA\xFE\x3F",3,false }; AR_DIRARROW.Apply(true);
				Console::eval("$pref::hudScale::retical = 1;");
			}
			else if (arg1.compare("internals") == 0)
			{
				CodePatch ID_Patch0 = { IntD_Dim_FontTag,							"","\xE4\x4B\x02",3,false }; ID_Patch0.Apply(true);
				CodePatch ID_Patch1 = { IntD_BoxWidth,								"","\xD2\x00",2,false }; ID_Patch1.Apply(true);
				Console::eval("$pref::hudScale::internals = 1;");
			}
			else if (arg1.compare("config") == 0)
			{
				CodePatch HDCFG_Patch0 = { HDCFG_Title_FontTag,							"","\xE4\x4B\x02",3,false }; HDCFG_Patch0.Apply(true);
				CodePatch HDCFG_Patch1 = { HDCFG_VerticalSpacing,						"","\x0E",1,false }; HDCFG_Patch1.Apply(true);
				Console::eval("$Localize::HudPrefExtent_HR = 161;");
				CodePatch HDCFG_Patch2 = { HDCFG_ButtonFontTag,							"","\xE5\x4B\x02",3,false }; HDCFG_Patch2.Apply(true);
				CodePatch HDCFG_Patch3 = { HDCFG_ButtonHighlightFontTag,				"","\xE4\x4B\x02",3,false }; HDCFG_Patch3.Apply(true);
				CodePatch HDCFG_Patch4 = { HDCFG_CycleColorsButtonFontTag,				"","\xE5\x4B\x02",3,false }; HDCFG_Patch4.Apply(true);
				CodePatch HDCFG_Patch5 = { HDCFG_CycleColorsButtonHighlightFontTag,		"","\xE4\x4B\x02",3,false }; HDCFG_Patch5.Apply(true);
				Console::eval("$Localize::HudPrefOffset_HR = 60;");
				Console::eval("$pref::hudScale::config = 1;");
			}
		}

		//2x Scale
		else if (atoi(argv[1]) == 2)
		{
			//Console::echo("Debug2");
			if (arg1.compare("text") == 0)
			{
				CodePatch SB_Patch0 = { HudScoreboard_FontTag,	"","\xE4\x4B\x02",3,false }; SB_Patch0.Apply(true);
				Console::eval("IDFNT_HUD_HIGH_RES_I     		= 00150085, 'hud_high_i_2x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES     			= 00150087, 'hud_hi2x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_DIM     		= 00150089, 'hud_high_dim_2x.pft'; ");	 
				Console::eval("IDFNT_HUD_HIGH_RES_GRAY     		= 00150091, 'hud_high_gray_2x.pft'; ");  
				Console::eval("IDFNT_HUD_HIGH_RES_YELLOW     	= 00150093, 'hud_high_yellow_2x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_BLUE     		= 00150095, 'hud_high_blue_2x.pft'; ");  
				Console::eval("IDFNT_HUD_HIGH_RES_PURPLE     	= 00150097, 'hud_high_purple_2x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_RED     		= 00150099, 'hud_high_red_2x.pft'; ");	 
				CodePatch HDOBJ_Patch0 = { HDOBJ_WindowWidth,	"","\x50\x03",2,false }; HDOBJ_Patch0.Apply(true);
				CodePatch MAPVW_Patch0 = { Mapview_FontFile,	"","hud_hi2x.pft",11,false }; MAPVW_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch0 = { HudGamePrefs_TextDimensions,	"","\x11",1,false }; HDGMCFG_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch1 = { HudGamePrefs_ComboboxTextVertOffset,	"","\xFE\xFF\xFF\xFF",4,false }; HDGMCFG_Patch1.Apply(true);
				Console::eval("$pref::hudScale::text = 2;");
			}
			else if (arg1.compare("weapons") == 0)
			{
				CodePatch WD_Patch0 = { WeaponDisplay_FontTag,						"","\xFF\x51\x02",3,false }; WD_Patch0.Apply(true);
				Console::eval("IDBMP_WP_CIRCLE_FILLED     		= 00160052, 'hud_circle_f2x.bmp';");
				Console::eval("IDBMP_WP_CIRCLE_OPEN     		= 00160053, 'hud_circle_o2x.bmp';");
				CodePatch WD_Patch0a = { WeaponDisplay_WeaponRollPitchError_FontTag,"","\xFC\x51\x02",3,false }; WD_Patch0a.Apply(true);
				CodePatch WD_Patch3 = { WeaponDisplay_BoxHighlightLength,			"","\x08",1,false }; WD_Patch3.Apply(true);
				CodePatch WD_Patch4 = { WeaponDisplay_RowVerticalSpacing,			"","\x0A",1,false }; WD_Patch4.Apply(true);
				CodePatch WD_Patch5 = { WeaponDisplay_ColumnWidth,					"","\x1E",1,false }; WD_Patch5.Apply(true);
				CodePatch WD_Patch6 = { WeaponDisplay_WeaponNameBoxLength,			"","\x78\x00",2,false }; WD_Patch6.Apply(true);
				CodePatch WD_Patch7 = { WeaponDisplay_AmmoBoxLength,				"","\x78\x00",2,false }; WD_Patch7.Apply(true);
				CodePatch WD_Patch8 = { WeaponDisplay_RowHeight,					"","\x10",1,false }; WD_Patch8.Apply(true);
				CodePatch WD_Patch9 = { WeaponDisplay_WeaponGroupRectangularColumn,	"","\x08",1,false }; WD_Patch9.Apply(true);
				Console::eval("$pref::hudScale::weapons = 2;");
			}
			else if (arg1.compare("radar") == 0)
			{
				Console::eval("IDPBA_RADAR_HIGH = 00170017, \"radar_high2x.pba\";");
				CodePatch RD_PatchF = { RadarDisplay_FontTag,								"","\xFF\x51\x02",3,false }; RD_PatchF.Apply(true);

				CodePatch RD_Patch0 = { RadarDisplay_Scale,									"","\x64\x00",2,false }; RD_Patch0.Apply(true);

				CodePatch RD_Patch1 = { RadarDisplay_ContainerWidth,						"","\x23\x00",2,false }; RD_Patch1.Apply(true);

				CodePatch RD_Patch2 = { RadarDisplay_ContainerHeight,						"","\x0A\x00",2,false }; RD_Patch2.Apply(true);

				CodePatch RD_Patch3 = { RadarDisplay_LinesScale,							"","\x18",1,false }; RD_Patch3.Apply(true);

				CodePatch RD_Patch4 = { RadarDisplay_ZenithLength,							"","\x16",1,false }; RD_Patch4.Apply(true);

				CodePatch RD_Patch5 = { RadarDisplay_ZenithWidth,							"","\x1C",1,false }; RD_Patch5.Apply(true);

				CodePatch RD_Patch6 = { RadarDisplay_DiagonalOffset,						"","\x70\x01",2,false }; RD_Patch6.Apply(true);

				CodePatch RD_Patch7 = { RadarDisplay_VerticalOffset,						"","\xE0\xFF\xFF\xFF",4,false }; RD_Patch7.Apply(true);

				CodePatch RD_Patch10 = { RadarDisplay_ThrottleArrowWidth,					"","\x12",1,false }; RD_Patch10.Apply(true);

				CodePatch RD_Patch11 = { RadarDisplay_ThrottleArrowLength,					"","\x2E",1,false }; RD_Patch11.Apply(true);

				CodePatch RD_Patch12 = { RadarDisplay_ThrottleArrowHorizontalOffset,		"","\x8A\x00",2,false }; RD_Patch12.Apply(true);

				CodePatch RD_Patch13 = { RadarDisplay_EnergyArrowWidth,						"","\x10",1,false }; RD_Patch13.Apply(true);

				CodePatch RD_Patch14 = { RadarDisplay_EnergyArrowLength,					"","\x3C",1,false }; RD_Patch14.Apply(true);

				CodePatch RD_Patch15 = { RadarDisplay_EnergyArrowHorizontalOffset,			"","\x90",1,false }; RD_Patch15.Apply(true);

				CodePatch RD_Patch16 = { RadarDisplay_SensorModeHorizontalOffset,			"","\xB0\x00",2,false }; RD_Patch16.Apply(true);

				CodePatch RD_Patch17 = { RadarDisplay_SensorModeVerticalOffset,				"","\xF6\xFF\xFF\xFF",4,false }; RD_Patch17.Apply(true);

				CodePatch RD_Patch18 = { RadarDisplay_SpeedTextHorizontalOffset,			"","\x1C\x00\x00\x00",4,false }; RD_Patch18.Apply(true);

				CodePatch RD_Patch19 = { RadarDisplay_SpeedTextVerticalOffset,				"","\x2A\x00",2,false }; RD_Patch19.Apply(true);

				CodePatch RD_Patch20 = { RadarDisplay_EnergyLabelHorizontalOffset,			"","\xF2\x00",2,false }; RD_Patch20.Apply(true);

				CodePatch RD_Patch21 = { RadarDisplay_EnergyLabelVerticalOffset,			"","\x92\x00",2,false }; RD_Patch21.Apply(true);

				CodePatch RD_Patch22 = { RadarDisplay_EnergyStatusHorizontalOffset,			"","\x20\x01",2,false }; RD_Patch22.Apply(true);

				CodePatch RD_Patch23 = { RadarDisplay_EnergyStatusVerticalOffset,			"","\xA4\x00",2,false }; RD_Patch23.Apply(true);

				CodePatch RD_Patch24 = { RadarDisplay_Perspective,							"","\xB4\x43",2,false }; RD_Patch24.Apply(true);
				Console::eval("$pref::hudScale::radar = 2;");
			}
			else if (arg1.compare("status") == 0)
			{
				CodePatch VD_Patch0 = { VehicleDisplay_FontTag,						"","\xFF\x51\x02",3,false }; VD_Patch0.Apply(true);
				CodePatch HL_Patch = { HudLabel_FontTag,							"","\xFF\x51\x02",3,false }; HL_Patch.Apply(true);
				CodePatch VD_Patch1 = { VehicleDisplay_BoxHeight,					"","\x5A",1,false }; VD_Patch1.Apply(true);
				CodePatch VD_Patch1a = { VehicleDisplay_TargetRenderBoxHeight,		"","\x5A",1,false }; VD_Patch1a.Apply(true);
				CodePatch VD_Patch2 = { VehicleDisplay_BoxWidth,					"","\xEB\xFF\xFF\xFF",4,false }; VD_Patch2.Apply(true);
				CodePatch VD_Patch3 = { VehicleDisplay_RenderBoxHeight,				"","\x80\x00",2,false }; VD_Patch3.Apply(true);
				CodePatch VD_Patch4 = { VehicleDisplay_RenderBoxWidth,				"","\x80\x00",2,false }; VD_Patch4.Apply(true);
				CodePatch VD_Patch5 = { VehicleDisplay_RenderBoxHeight2,			"","\x80\x00",2,false }; VD_Patch5.Apply(true);
				CodePatch VD_Patch6 = { VehicleDisplay_RenderBoxWidth2,				"","\x80\x00",2,false }; VD_Patch6.Apply(true);
				CodePatch VD_Patch7 = { VehicleDisplay_RenderSize,					"","\x00\x00\x00\x43",4,false }; VD_Patch7.Apply(true);
				CodePatch VD_Patch8 = { VehicleDisplay_RenderCullPoint1,			"","\x00\x43",2,false }; VD_Patch8.Apply(true);
				CodePatch VD_Patch9 = { VehicleDisplay_RenderCullPoint2,			"","\x00\x43",2,false }; VD_Patch9.Apply(true);
				CodePatch VD_Patch10 = { VehicleDisplay_RenderCullPoint3,			"","\x00\x43",2,false }; VD_Patch10.Apply(true);
				CodePatch VD_Patch11 = { VehicleDisplay_RenderCullPoint4,			"","\x00\x43",2,false }; VD_Patch11.Apply(true);
				Console::eval("$pref::hudScale::status = 2;");
			}
			else if (arg1.compare("chat") == 0)
			{
				CodePatch CR_Patch0 = { ChatRec_PresetWidth1,						"","\x58\x02",2,false }; CR_Patch0.Apply(true);
				CodePatch CR_Patch1 = { ChatRec_PresetWidth2,						"","\x58\x02",2,false }; CR_Patch1.Apply(true);
				CodePatch CR_Patch2 = { ChatRec_PresetWidth3,						"","\x58\x02",2,false }; CR_Patch2.Apply(true);
				CodePatch CR_Patch3 = { ChatRec_PresetHeight1,						"","\x4E\x00",2,false }; CR_Patch3.Apply(true);
				CodePatch CR_Patch4 = { ChatRec_PresetHeight2,						"","\xA6\x00",2,false }; CR_Patch4.Apply(true);
				CodePatch CR_Patch5 = { ChatRec_PresetHeight3,						"","\xE8\x00",2,false }; CR_Patch5.Apply(true);
				CodePatch CR_Patch6 = { ChatRec_FontTag,							"","\xFF\x51\x02",3,false }; CR_Patch6.Apply(true);
				CodePatch CR_Patch7 = { ChatRec_FontTag_Dim,						"","\xFE\x51\x02",3,false }; CR_Patch7.Apply(true);
				CodePatch CR_Patch8 = { ChatRec_FontTag_Gray,						"","\x00\x52\x02",3,false }; CR_Patch8.Apply(true);
				CodePatch CR_Patch9 = { ChatRec_FontTag_Default,					"","\xFF\x51\x02",3,false }; CR_Patch9.Apply(true);
				Console::eval("$pref::hudScale::chat = 2;");
			}
			else if (arg1.compare("timers") == 0)
			{
				CodePatch HT_Patch0 = { HudTimer_FontTag,							"","\xFF\x51\x02",3,false }; HT_Patch0.Apply(true);
				Console::eval("$pref::hudScale::timers = 2;");
			}
			else if (arg1.compare("orders") == 0)
			{
				CodePatch CM_Patch0 = { CmdMenu_FontTag,							"","\xFF\x51\x02",3,false }; CM_Patch0.Apply(true);
				CodePatch CM_Patch1 = { CmdMenu_Width,								"","\x40\x01",2,false }; CM_Patch1.Apply(true);
				Console::eval("$pref::hudScale::orders = 2;");
			}
			else if (arg1.compare("shields") == 0)
			{
				Console::eval("IDBMP_SHIELDS = 00160012, \"shields2x.bmp\";");
				Console::eval("IDBMP_SHIELDS_SOLID = 00160166, \"shields_solid2x.bmp\";");
				Console::eval("IDBMP_STR_CIRCLE = 00160013, \"strCircle2x.bmp\";");
				CodePatch SD_Patch1 = { ShieldDisplay_FontTag,								 "","\xFF\x51\x02",3,false }; SD_Patch1.Apply(true);
				CodePatch SD_Patch2 = { ShieldDisplay_DirectionalRingHorizontalOffset,		 "","\x21",1,false }; SD_Patch2.Apply(true);
				CodePatch SD_Patch3 = { ShieldDisplay_DirectionalRingVerticalOffset,		 "","\x11",1,false }; SD_Patch3.Apply(true);
				CodePatch SD_Patch4 = { ShieldDisplay_LabelTextHorizontalOffset,			 "","\x55",1,false }; SD_Patch4.Apply(true);
				CodePatch SD_Patch5 = { ShieldDisplay_LabelTextVerticalOffset,				 "","\x70",1,false }; SD_Patch5.Apply(true);
				CodePatch SD_Patch6 = { ShieldDisplay_StrengthTextHorizontalOffset,			 "","\x82",1,false }; SD_Patch6.Apply(true);
				CodePatch SD_Patch7 = { ShieldDisplay_StrengthTextVerticalOffset,			 "","\x80",1,false }; SD_Patch7.Apply(true);
				CodePatch SD_Patch8 = { ShieldDisplay_StrengthArrowVerticalOffset,			 "","\xFA\xFF\xFF\xFF",4,false }; SD_Patch8.Apply(true);
				CodePatch SD_Patch9 = { ShieldDisplay_StrengthArrowHorizontalOffset,		 "","\xE0\x00",2,false }; SD_Patch9.Apply(true);
				CodePatch SD_Patch10 = { ShieldDisplay_StrengthArrowPerspective,			 "","\x00\x00\x00\x47",4,false }; SD_Patch10.Apply(true);
				CodePatch SD_Patch11 = { ShieldDisplay_StrengthArrowLength,					 "","\x19",1,false }; SD_Patch11.Apply(true);
				CodePatch SD_Patch12 = { ShieldDisplay_StrengthArrowWidth,					 "","\x08",1,false }; SD_Patch12.Apply(true);
				CodePatch SD_Patch13 = { ShieldDisplay_DirectionalTextGapSize,				 "","\x25",1,false }; SD_Patch13.Apply(true);
				CodePatch SD_Patch14 = { ShieldDisplay_DirectionalInfoLineTextVerticalOffset,"","\x03",1,false }; SD_Patch14.Apply(true);
				CodePatch SD_Patch16 = { ShieldDisplay_DirectionalInfoLine,					 "","\x37",1,false }; SD_Patch16.Apply(true);
				CodePatch SD_Patch17 = { ShieldDisplay_DirectionalInfoLineFooterTextZaxis,	 "","\xE0\x00",2,false }; SD_Patch17.Apply(true);
				Console::eval("$pref::hudScale::shields = 2;");
			}
			else if (arg1.compare("retical") == 0)
			{
				CodePatch AR_Patch0 = { AimRet_FontTag,								"","\xFF\x51\x02",3,false }; AR_Patch0.Apply(true);
				//CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x04",1,false }; AR_Patch1.Apply(true);
				//CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x14",1,false }; AR_Patch2.Apply(true);
				//CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\x00\x00\x00\x00",4,false }; AR_Patch3.Apply(true);
				//CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\xC1\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				//CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x30",1,false }; AR_Patch5.Apply(true);
				//CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x06",1,false }; AR_Patch6.Apply(true);
				//CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\x80\xFF\x3F",3,false }; AR_DIRARROW.Apply(true);
				CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x02",1,false }; AR_Patch1.Apply(true);
				CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x0A",1,false }; AR_Patch2.Apply(true);
				CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\x12\x00\x00\x00",4,false }; AR_Patch3.Apply(true);
				CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\xE0\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x18",1,false }; AR_Patch5.Apply(true);
				CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x03",1,false }; AR_Patch6.Apply(true);
				CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\xAA\xFE\x3F",3,false }; AR_DIRARROW.Apply(true);

				Console::eval("$pref::hudScale::retical = 2;");
			}
			else if (arg1.compare("internals") == 0)
			{
				CodePatch ID_Patch0 = { IntD_Dim_FontTag,							"","\xFE\x51\x02",3,false }; ID_Patch0.Apply(true);
				CodePatch ID_Patch1 = { IntD_BoxWidth,								"","\x36\x01",2,false }; ID_Patch1.Apply(true);
				Console::eval("$pref::hudScale::internals = 2;");
			}
			else if (arg1.compare("config") == 0)
			{
				CodePatch HDCFG_Patch0 = { HDCFG_Title_FontTag,							"","\xFF\x51\x02",3,false }; HDCFG_Patch0.Apply(true);
				CodePatch HDCFG_Patch1 = { HDCFG_VerticalSpacing,						"","\x15",1,false }; HDCFG_Patch1.Apply(true);
				Console::eval("$Localize::HudPrefExtent_HR = 190;");//Window Width
				CodePatch HDCFG_Patch2 = { HDCFG_ButtonFontTag,							"","\xFE\x51\x02",3,false }; HDCFG_Patch2.Apply(true);
				CodePatch HDCFG_Patch3 = { HDCFG_ButtonHighlightFontTag,				"","\xFF\x51\x02",3,false }; HDCFG_Patch3.Apply(true);
				CodePatch HDCFG_Patch4 = { HDCFG_CycleColorsButtonFontTag,				"","\xFE\x51\x02",3,false }; HDCFG_Patch4.Apply(true);
				CodePatch HDCFG_Patch5 = { HDCFG_CycleColorsButtonHighlightFontTag,		"","\xFF\x51\x02",3,false }; HDCFG_Patch5.Apply(true);
				Console::eval("$Localize::HudPrefOffset_HR = 70;");//Button subtext indentation offset
				Console::eval("$pref::hudScale::config = 2;");
			}
		}

		//3x Scale
		else if (atoi(argv[1]) == 3)
		{
			//Console::echo("Debug3");
			if (arg1.compare("text") == 0)
			{
				CodePatch SB_Patch0 = { HudScoreboard_FontTag,	"","\xE4\x4B\x02",3,false }; SB_Patch0.Apply(true);
				Console::eval("IDFNT_HUD_HIGH_RES_I     		= 00150085, 'hud_high_i_3x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES     			= 00150087, 'hud_hi3x.pft'; ");		 
				Console::eval("IDFNT_HUD_HIGH_RES_DIM     		= 00150089, 'hud_high_dim_3x.pft'; ");	 
				Console::eval("IDFNT_HUD_HIGH_RES_GRAY     		= 00150091, 'hud_high_gray_3x.pft'; ");  
				Console::eval("IDFNT_HUD_HIGH_RES_YELLOW     	= 00150093, 'hud_high_yellow_3x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_BLUE     		= 00150095, 'hud_high_blue_3x.pft'; ");  
				Console::eval("IDFNT_HUD_HIGH_RES_PURPLE     	= 00150097, 'hud_high_purple_3x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_RED     		= 00150099, 'hud_high_red_3x.pft'; ");
				CodePatch HDOBJ_Patch0 = { HDOBJ_WindowWidth,	"","\x50\x03",2,false }; HDOBJ_Patch0.Apply(true);
				CodePatch MAPVW_Patch0 = { Mapview_FontFile,	"","hud_hi3x.pft",11,false }; MAPVW_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch0 = { HudGamePrefs_TextDimensions,	"","\x13",1,false }; HDGMCFG_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch1 = { HudGamePrefs_ComboboxTextVertOffset,	"","\xFC\xFF\xFF\xFF",4,false }; HDGMCFG_Patch1.Apply(true);
				Console::eval("$pref::hudScale::text = 3;");
			}
			else if (arg1.compare("weapons") == 0)
			{
				CodePatch WD_Patch0 = { WeaponDisplay_FontTag,						"","\x7B\x4E\x02",3,false }; WD_Patch0.Apply(true);
				Console::eval("IDBMP_WP_CIRCLE_FILLED     		= 00160052, 'hud_circle_f3x.bmp';");
				Console::eval("IDBMP_WP_CIRCLE_OPEN     		= 00160053, 'hud_circle_o3x.bmp';");
				CodePatch WD_Patch0a = { WeaponDisplay_WeaponRollPitchError_FontTag,"","\x78\x4E\x02",3,false }; WD_Patch0a.Apply(true);
				CodePatch WD_Patch3 = { WeaponDisplay_BoxHighlightLength,			"","\x0C",1,false }; WD_Patch3.Apply(true);
				CodePatch WD_Patch4 = { WeaponDisplay_RowVerticalSpacing,			"","\x0F",1,false }; WD_Patch4.Apply(true);
				CodePatch WD_Patch5 = { WeaponDisplay_ColumnWidth,					"","\x2D",1,false }; WD_Patch5.Apply(true);
				CodePatch WD_Patch6 = { WeaponDisplay_WeaponNameBoxLength,			"","\xBD\x00",2,false }; WD_Patch6.Apply(true);
				CodePatch WD_Patch7 = { WeaponDisplay_AmmoBoxLength,				"","\xBD\x00",2,false }; WD_Patch7.Apply(true);
				CodePatch WD_Patch8 = { WeaponDisplay_RowHeight,					"","\x18",1,false }; WD_Patch8.Apply(true);
				CodePatch WD_Patch9 = { WeaponDisplay_WeaponGroupRectangularColumn,	"","\x0C",1,false }; WD_Patch9.Apply(true);
				Console::eval("$pref::hudScale::weapons = 3;");
			}
			else if (arg1.compare("radar") == 0)
			{
				Console::eval("IDPBA_RADAR_HIGH = 00170017, \"radar_high3x.pba\";");
				CodePatch RD_PatchF = { RadarDisplay_FontTag,								"","\x7B\x4E\x02",3,false }; RD_PatchF.Apply(true);

				CodePatch RD_Patch0 = { RadarDisplay_Scale,									"","\x7D\x00",2,false }; RD_Patch0.Apply(true);

				CodePatch RD_Patch1 = { RadarDisplay_ContainerWidth,						"","\x3A\x00",2,false }; RD_Patch1.Apply(true);

				CodePatch RD_Patch2 = { RadarDisplay_ContainerHeight,						"","\x0A\x00",2,false }; RD_Patch2.Apply(true);

				CodePatch RD_Patch3 = { RadarDisplay_LinesScale,							"","\x24",1,false }; RD_Patch3.Apply(true);

				CodePatch RD_Patch4 = { RadarDisplay_ZenithLength,							"","\x21",1,false }; RD_Patch4.Apply(true);

				CodePatch RD_Patch5 = { RadarDisplay_ZenithWidth,							"","\x2A",1,false }; RD_Patch5.Apply(true);

				CodePatch RD_Patch6 = { RadarDisplay_DiagonalOffset,						"","\xAA\x01",2,false }; RD_Patch6.Apply(true);

				CodePatch RD_Patch7 = { RadarDisplay_VerticalOffset,						"","\xD2\xFF\xFF\xFF",4,false }; RD_Patch7.Apply(true);

				CodePatch RD_Patch10 = { RadarDisplay_ThrottleArrowWidth,					"","\x1B",1,false }; RD_Patch10.Apply(true);

				CodePatch RD_Patch11 = { RadarDisplay_ThrottleArrowLength,					"","\x2E",1,false }; RD_Patch11.Apply(true);

				CodePatch RD_Patch12 = { RadarDisplay_ThrottleArrowHorizontalOffset,		"","\xA5\x00",2,false }; RD_Patch12.Apply(true);

				CodePatch RD_Patch13 = { RadarDisplay_EnergyArrowWidth,						"","\x18",1,false }; RD_Patch13.Apply(true);

				CodePatch RD_Patch14 = { RadarDisplay_EnergyArrowLength,					"","\x3C",1,false }; RD_Patch14.Apply(true);

				CodePatch RD_Patch15 = { RadarDisplay_EnergyArrowHorizontalOffset,			"","\xB5",1,false }; RD_Patch15.Apply(true);

				CodePatch RD_Patch16 = { RadarDisplay_SensorModeHorizontalOffset,			"","\xC6\x00",2,false }; RD_Patch16.Apply(true);

				CodePatch RD_Patch17 = { RadarDisplay_SensorModeVerticalOffset,				"","\xEE\xFF\xFF\xFF",4,false }; RD_Patch17.Apply(true);

				CodePatch RD_Patch18 = { RadarDisplay_SpeedTextHorizontalOffset,			"","\x10\x00\x00\x00",4,false }; RD_Patch18.Apply(true);

				CodePatch RD_Patch19 = { RadarDisplay_SpeedTextVerticalOffset,				"","\x2A\x00",2,false }; RD_Patch19.Apply(true);

				CodePatch RD_Patch20 = { RadarDisplay_EnergyLabelHorizontalOffset,			"","\x20\x01",2,false }; RD_Patch20.Apply(true);

				CodePatch RD_Patch21 = { RadarDisplay_EnergyLabelVerticalOffset,			"","\xAA\x00",2,false }; RD_Patch21.Apply(true);

				CodePatch RD_Patch22 = { RadarDisplay_EnergyStatusHorizontalOffset,			"","\x75\x01",2,false }; RD_Patch22.Apply(true);

				CodePatch RD_Patch23 = { RadarDisplay_EnergyStatusVerticalOffset,			"","\xC0\x00",2,false }; RD_Patch23.Apply(true);

				CodePatch RD_Patch24 = { RadarDisplay_Perspective,							"","\xB4\x43",2,false }; RD_Patch24.Apply(true);
				Console::eval("$pref::hudScale::radar = 3;");
			}
			else if (arg1.compare("status") == 0)
			{
				CodePatch VD_Patch0 = { VehicleDisplay_FontTag,						"","\x7B\x4E\x02",3,false }; VD_Patch0.Apply(true);
				CodePatch HL_Patch = { HudLabel_FontTag,							"","\x7B\x4E\x02",3,false }; HL_Patch.Apply(true);
				CodePatch VD_Patch1 = { VehicleDisplay_BoxHeight,					"","\x90",1,false }; VD_Patch1.Apply(true);
				CodePatch VD_Patch1a = { VehicleDisplay_TargetRenderBoxHeight,		"","\x8F",1,false }; VD_Patch1a.Apply(true);
				CodePatch VD_Patch2 = { VehicleDisplay_BoxWidth,					"","\xCB\xFF\xFF\xFF",4,false }; VD_Patch2.Apply(true);
				CodePatch VD_Patch3 = { VehicleDisplay_RenderBoxHeight,				"","\xC0\x00",2,false }; VD_Patch3.Apply(true);
				CodePatch VD_Patch4 = { VehicleDisplay_RenderBoxWidth,				"","\xC0\x00",2,false }; VD_Patch4.Apply(true);
				CodePatch VD_Patch5 = { VehicleDisplay_RenderBoxHeight2,			"","\xC0\x00",2,false }; VD_Patch5.Apply(true);
				CodePatch VD_Patch6 = { VehicleDisplay_RenderBoxWidth2,				"","\xC0\x00",2,false }; VD_Patch6.Apply(true);
				CodePatch VD_Patch7 = { VehicleDisplay_RenderSize,					"","\x00\x00\x40\x43",4,false }; VD_Patch7.Apply(true);
				CodePatch VD_Patch8 = { VehicleDisplay_RenderCullPoint1,			"","\x40\x43",2,false }; VD_Patch8.Apply(true);
				CodePatch VD_Patch9 = { VehicleDisplay_RenderCullPoint2,			"","\x40\x43",2,false }; VD_Patch9.Apply(true);
				CodePatch VD_Patch10 = { VehicleDisplay_RenderCullPoint3,			"","\x40\x43",2,false }; VD_Patch10.Apply(true);
				CodePatch VD_Patch11 = { VehicleDisplay_RenderCullPoint4,			"","\x40\x43",2,false }; VD_Patch11.Apply(true);
				Console::eval("$pref::hudScale::status = 3;");
			}
			else if (arg1.compare("chat") == 0)
			{
				CodePatch CR_Patch0 = { ChatRec_PresetWidth1,						"","\x20\x03",2,false }; CR_Patch0.Apply(true);
				CodePatch CR_Patch1 = { ChatRec_PresetWidth2,						"","\x20\x03",2,false }; CR_Patch1.Apply(true);
				CodePatch CR_Patch2 = { ChatRec_PresetWidth3,						"","\x20\x03",2,false }; CR_Patch2.Apply(true);
				CodePatch CR_Patch3 = { ChatRec_PresetHeight1,						"","\x4E\x00",2,false }; CR_Patch3.Apply(true);
				CodePatch CR_Patch4 = { ChatRec_PresetHeight2,						"","\xA6\x00",2,false }; CR_Patch4.Apply(true);
				CodePatch CR_Patch5 = { ChatRec_PresetHeight3,						"","\xE8\x00",2,false }; CR_Patch5.Apply(true);
				CodePatch CR_Patch6 = { ChatRec_FontTag,							"","\x7B\x4E\x02",3,false }; CR_Patch6.Apply(true);
				CodePatch CR_Patch7 = { ChatRec_FontTag_Dim,						"","\x7A\x4E\x02",3,false }; CR_Patch7.Apply(true);
				CodePatch CR_Patch8 = { ChatRec_FontTag_Gray,						"","\x7C\x4E\x02",3,false }; CR_Patch8.Apply(true);
				Console::eval("$pref::hudScale::chat = 3;");
			}
			else if (arg1.compare("timers") == 0)
			{
				CodePatch HT_Patch0 = { HudTimer_FontTag,							"","\x7B\x4E\x02",3,false }; HT_Patch0.Apply(true);
				Console::eval("$pref::hudScale::timers = 3;");
			}
			else if (arg1.compare("orders") == 0)
			{
				CodePatch CM_Patch0 = { CmdMenu_FontTag,							"","\x7B\x4E\x02",3,false }; CM_Patch0.Apply(true);
				CodePatch CM_Patch1 = { CmdMenu_Width,								"","\xE0\x01",2,false }; CM_Patch1.Apply(true);
				Console::eval("$pref::hudScale::orders = 3;");
			}
			else if (arg1.compare("shields") == 0)
			{
				Console::eval("IDBMP_SHIELDS = 00160012, \"shields3x.bmp\";");
				Console::eval("IDBMP_SHIELDS_SOLID = 00160166, \"shields_solid3x.bmp\";");
				Console::eval("IDBMP_STR_CIRCLE = 00160013, \"strCircle3x.bmp\";");
				CodePatch SD_Patch1 = { ShieldDisplay_FontTag,								"","\x7B\x4E\x02",3,false }; SD_Patch1.Apply(true);
				CodePatch SD_Patch2 = { ShieldDisplay_DirectionalRingHorizontalOffset,		"","\x26",1,false }; SD_Patch2.Apply(true);
				CodePatch SD_Patch3 = { ShieldDisplay_DirectionalRingVerticalOffset,		"","\x13",1,false }; SD_Patch3.Apply(true);
				CodePatch SD_Patch4 = { ShieldDisplay_LabelTextHorizontalOffset,			"","\x60",1,false }; SD_Patch4.Apply(true);
				CodePatch SD_Patch5 = { ShieldDisplay_LabelTextVerticalOffset,				"","\x95",1,false }; SD_Patch5.Apply(true);
				CodePatch SD_Patch6 = { ShieldDisplay_StrengthTextHorizontalOffset,			"","\xB2",1,false }; SD_Patch6.Apply(true);
				CodePatch SD_Patch7 = { ShieldDisplay_StrengthTextVerticalOffset,			"","\xA8",1,false }; SD_Patch7.Apply(true);
				CodePatch SD_Patch8 = { ShieldDisplay_StrengthArrowVerticalOffset,			"","\xF4\xFF\xFF\xFF",4,false }; SD_Patch8.Apply(true);
				CodePatch SD_Patch9 = { ShieldDisplay_StrengthArrowHorizontalOffset,		"","\x25\x01",2,false }; SD_Patch9.Apply(true);
				CodePatch SD_Patch10 = { ShieldDisplay_StrengthArrowPerspective,			"","\x00\x00\x96\x45",2,false }; SD_Patch10.Apply(true);
				CodePatch SD_Patch11 = { ShieldDisplay_StrengthArrowLength,					"","\x25",1,false }; SD_Patch11.Apply(true);
				CodePatch SD_Patch12 = { ShieldDisplay_StrengthArrowWidth,					"","\x0C",1,false }; SD_Patch12.Apply(true);
				CodePatch SD_Patch13 = { ShieldDisplay_DirectionalTextGapSize,				"","\x40",1,false }; SD_Patch13.Apply(true);
				CodePatch SD_Patch14 = { ShieldDisplay_DirectionalInfoLineTextVerticalOffset,"","\x03",1,false }; SD_Patch14.Apply(true);
				CodePatch SD_Patch16 = { ShieldDisplay_DirectionalInfoLine,					"","\x37",1,false }; SD_Patch16.Apply(true);
				CodePatch SD_Patch17 = { ShieldDisplay_DirectionalInfoLineFooterTextZaxis,	"","\x25\x01",2,false }; SD_Patch17.Apply(true);
				Console::eval("$pref::hudScale::shields = 3;");
			}
			else if (arg1.compare("retical") == 0)
			{
				CodePatch AR_Patch0 = { AimRet_FontTag,								"","\x7B\x4E\x02",3,false }; AR_Patch0.Apply(true);
				//CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x06",1,false }; AR_Patch1.Apply(true);
				//CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x1E",1,false }; AR_Patch2.Apply(true);
				//CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\xE7\xFF\xFF\xFF",4,false }; AR_Patch3.Apply(true);
				//CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\xA2\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				//CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x48",1,false }; AR_Patch5.Apply(true);
				//CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x09",1,false }; AR_Patch6.Apply(true);
				//CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\xD0\xFF\x3F",3,false }; AR_DIRARROW.Apply(true);
				CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x04",1,false }; AR_Patch1.Apply(true);
				CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x14",1,false }; AR_Patch2.Apply(true);
				CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\x00\x00\x00\x00",4,false }; AR_Patch3.Apply(true);
				CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\xC1\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x30",1,false }; AR_Patch5.Apply(true);
				CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x06",1,false }; AR_Patch6.Apply(true);
				CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\x80\xFF\x3F",3,false }; AR_DIRARROW.Apply(true);
				Console::eval("$pref::hudScale::retical = 3;");
			}
			else if (arg1.compare("internals") == 0)
			{
				CodePatch ID_Patch0 = { IntD_Dim_FontTag,							"","\x7A\x4E\x02",3,false }; ID_Patch0.Apply(true);
				CodePatch ID_Patch1 = { IntD_BoxWidth,								"","\x9A\x01",2,false }; ID_Patch1.Apply(true);
				Console::eval("$pref::hudScale::internals = 3;");
			}
			else if (arg1.compare("config") == 0)
			{
				CodePatch HDCFG_Patch0 = { HDCFG_Title_FontTag,							"","\x7B\x4E\x02",3,false }; HDCFG_Patch0.Apply(true);
				CodePatch HDCFG_Patch1 = { HDCFG_VerticalSpacing,						"","\x1C",1,false }; HDCFG_Patch1.Apply(true);
				Console::eval("$Localize::HudPrefExtent_HR = 300;");//Window Width
				CodePatch HDCFG_Patch2 = { HDCFG_ButtonFontTag,							"","\x7A\x4E\x02",3,false }; HDCFG_Patch2.Apply(true);
				CodePatch HDCFG_Patch3 = { HDCFG_ButtonHighlightFontTag,				"","\x7B\x4E\x02",3,false }; HDCFG_Patch3.Apply(true);
				CodePatch HDCFG_Patch4 = { HDCFG_CycleColorsButtonFontTag,				"","\x7A\x4E\x02",3,false }; HDCFG_Patch4.Apply(true);
				CodePatch HDCFG_Patch5 = { HDCFG_CycleColorsButtonHighlightFontTag,		"","\x7B\x4E\x02",3,false }; HDCFG_Patch5.Apply(true);
				Console::eval("$Localize::HudPrefOffset_HR = 140;");//Button subtext indentation offset
				Console::eval("$pref::hudScale::config = 3;");
			}
		}

		//4x Scale
		else if (atoi(argv[1]) == 4)
		{
			//Console::echo("Debug4");
			if (arg1.compare("text") == 0)
			{
				Console::eval("IDFNT_HUD_HIGH_RES_I     		= 00150085, 'hud_high_i_4x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES     			= 00150087, 'hud_hi4x.pft'; ");		 
				Console::eval("IDFNT_HUD_HIGH_RES_DIM     		= 00150089, 'hud_high_dim_4x.pft'; ");	 
				Console::eval("IDFNT_HUD_HIGH_RES_GRAY     		= 00150091, 'hud_high_gray_4x.pft'; ");  
				Console::eval("IDFNT_HUD_HIGH_RES_YELLOW     	= 00150093, 'hud_high_yellow_4x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_BLUE     		= 00150095, 'hud_high_blue_4x.pft'; ");  
				Console::eval("IDFNT_HUD_HIGH_RES_PURPLE     	= 00150097, 'hud_high_purple_4x.pft'; ");
				Console::eval("IDFNT_HUD_HIGH_RES_RED     		= 00150099, 'hud_high_red_4x.pft'; ");
				CodePatch HDOBJ_Patch0 = { HDOBJ_WindowWidth,	"","\x50\x03",2,false }; HDOBJ_Patch0.Apply(true);
				CodePatch MAPVW_Patch0 = { Mapview_FontFile,	"","hud_hi4x.pft",11,false }; MAPVW_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch0 = { HudGamePrefs_TextDimensions,	"","\x16",1,false }; HDGMCFG_Patch0.Apply(true);
				CodePatch HDGMCFG_Patch1 = { HudGamePrefs_ComboboxTextVertOffset,	"","\xFB\xFF\xFF\xFF",4,false }; HDGMCFG_Patch1.Apply(true);
				Console::eval("$pref::hudScale::text = 4;");
			}
			else if (arg1.compare("weapons") == 0)
			{
				CodePatch WD_Patch0 = { WeaponDisplay_FontTag,						"","\xCB\x4E\x02",3,false }; WD_Patch0.Apply(true);
				Console::eval("IDBMP_WP_CIRCLE_FILLED     		= 00160052, 'hud_circle_f4x.bmp';");
				Console::eval("IDBMP_WP_CIRCLE_OPEN     		= 00160053, 'hud_circle_o4x.bmp';");
				CodePatch WD_Patch0a = { WeaponDisplay_WeaponRollPitchError_FontTag,"","\xC8\x4E\x02",3,false }; WD_Patch0a.Apply(true);
				CodePatch WD_Patch3 = { WeaponDisplay_BoxHighlightLength,			"","\x10",1,false }; WD_Patch3.Apply(true);
				CodePatch WD_Patch4 = { WeaponDisplay_RowVerticalSpacing,			"","\x14",1,false }; WD_Patch4.Apply(true);
				CodePatch WD_Patch5 = { WeaponDisplay_ColumnWidth,					"","\x3C",1,false }; WD_Patch5.Apply(true);
				CodePatch WD_Patch6 = { WeaponDisplay_WeaponNameBoxLength,			"","\xFC\x00",2,false }; WD_Patch6.Apply(true);
				CodePatch WD_Patch7 = { WeaponDisplay_AmmoBoxLength,				"","\xFC\x00",2,false }; WD_Patch7.Apply(true);
				CodePatch WD_Patch8 = { WeaponDisplay_RowHeight,					"","\x20",1,false }; WD_Patch8.Apply(true);
				CodePatch WD_Patch9 = { WeaponDisplay_WeaponGroupRectangularColumn,	"","\x10",1,false }; WD_Patch9.Apply(true);
				Console::eval("$pref::hudScale::weapons = 4;");
			}
			else if (arg1.compare("radar") == 0)
			{
				Console::eval("IDPBA_RADAR_HIGH = 00170017, \"radar_high4x.pba\";");
				CodePatch RD_PatchF = { RadarDisplay_FontTag,								"","\xCB\x4E\x02",3,false }; RD_PatchF.Apply(true);

				CodePatch RD_Patch0 = { RadarDisplay_Scale,									"","\x9D\x00",2,false }; RD_Patch0.Apply(true);

				CodePatch RD_Patch1 = { RadarDisplay_ContainerWidth,						"","\x70\x00",2,false }; RD_Patch1.Apply(true);

				CodePatch RD_Patch2 = { RadarDisplay_ContainerHeight,						"","\x20\x00",2,false }; RD_Patch2.Apply(true);

				CodePatch RD_Patch3 = { RadarDisplay_LinesScale,							"","\x24",1,false }; RD_Patch3.Apply(true);

				CodePatch RD_Patch4 = { RadarDisplay_ZenithLength,							"","\x21",1,false }; RD_Patch4.Apply(true);

				CodePatch RD_Patch5 = { RadarDisplay_ZenithWidth,							"","\x2A",1,false }; RD_Patch5.Apply(true);

				CodePatch RD_Patch6 = { RadarDisplay_DiagonalOffset,						"","\xE9\x01",2,false }; RD_Patch6.Apply(true);

				CodePatch RD_Patch7 = { RadarDisplay_VerticalOffset,						"","\xC5\xFF\xFF\xFF",4,false }; RD_Patch7.Apply(true);

				CodePatch RD_Patch10 = { RadarDisplay_ThrottleArrowWidth,					"","\x1B",1,false }; RD_Patch10.Apply(true);

				CodePatch RD_Patch11 = { RadarDisplay_ThrottleArrowLength,					"","\x2E",1,false }; RD_Patch11.Apply(true);

				CodePatch RD_Patch12 = { RadarDisplay_ThrottleArrowHorizontalOffset,		"","\xDF",1,false }; RD_Patch12.Apply(true);

				CodePatch RD_Patch13 = { RadarDisplay_EnergyArrowWidth,						"","\x18",1,false }; RD_Patch13.Apply(true);

				CodePatch RD_Patch14 = { RadarDisplay_EnergyArrowLength,					"","\x3C",1,false }; RD_Patch14.Apply(true);

				CodePatch RD_Patch15 = { RadarDisplay_EnergyArrowHorizontalOffset,			"","\xF0\x00",2,false }; RD_Patch15.Apply(true);

				CodePatch RD_Patch16 = { RadarDisplay_SensorModeHorizontalOffset,			"","\xE2\x00",2,false }; RD_Patch16.Apply(true);

				CodePatch RD_Patch17 = { RadarDisplay_SensorModeVerticalOffset,				"","\xE0\xFF\xFF\xFF",4,false }; RD_Patch17.Apply(true);

				CodePatch RD_Patch18 = { RadarDisplay_SpeedTextHorizontalOffset,			"","\xF5\xFF\xFF\xFF",4,false }; RD_Patch18.Apply(true);

				CodePatch RD_Patch19 = { RadarDisplay_SpeedTextVerticalOffset,				"","\x2A\x00",2,false }; RD_Patch19.Apply(true);

				CodePatch RD_Patch20 = { RadarDisplay_EnergyLabelHorizontalOffset,			"","\x50\x01",2,false }; RD_Patch20.Apply(true);

				CodePatch RD_Patch21 = { RadarDisplay_EnergyLabelVerticalOffset,			"","\xC0\x00",2,false }; RD_Patch21.Apply(true);

				CodePatch RD_Patch22 = { RadarDisplay_EnergyStatusHorizontalOffset,			"","\xCA\x01",2,false }; RD_Patch22.Apply(true);

				CodePatch RD_Patch23 = { RadarDisplay_EnergyStatusVerticalOffset,			"","\xDD\x00",2,false }; RD_Patch23.Apply(true);

				CodePatch RD_Patch24 = { RadarDisplay_Perspective,							"","\xB4\x43",2,false }; RD_Patch24.Apply(true);
				Console::eval("$pref::hudScale::radar = 4;");
			}
			else if (arg1.compare("status") == 0)
			{
				CodePatch VD_Patch0 = { VehicleDisplay_FontTag,						"","\xCB\x4E\x02",3,false }; VD_Patch0.Apply(true);
				CodePatch HL_Patch = { HudLabel_FontTag,							"","\xCB\x4E\x02",3,false }; HL_Patch.Apply(true);
				CodePatch VD_Patch1 = { VehicleDisplay_BoxHeight,					"","\xC5",1,false }; VD_Patch1.Apply(true);
				CodePatch VD_Patch1a = { VehicleDisplay_TargetRenderBoxHeight,		"","\xC5",1,false }; VD_Patch1a.Apply(true);
				CodePatch VD_Patch2 = { VehicleDisplay_BoxWidth,					"","\xAB\xFF\xFF\xFF",4,false }; VD_Patch2.Apply(true);
				CodePatch VD_Patch3 = { VehicleDisplay_RenderBoxHeight,				"","\x00\x01",2,false }; VD_Patch3.Apply(true);
				CodePatch VD_Patch4 = { VehicleDisplay_RenderBoxWidth,				"","\x00\x01",2,false }; VD_Patch4.Apply(true);
				CodePatch VD_Patch5 = { VehicleDisplay_RenderBoxHeight2,			"","\x00\x01",2,false }; VD_Patch5.Apply(true);
				CodePatch VD_Patch6 = { VehicleDisplay_RenderBoxWidth2,				"","\x00\x01",2,false }; VD_Patch6.Apply(true);
				CodePatch VD_Patch7 = { VehicleDisplay_RenderSize,					"","\x00\x00\x80\x43",4,false }; VD_Patch7.Apply(true);
				CodePatch VD_Patch8 = { VehicleDisplay_RenderCullPoint1,			"","\x80\x43",2,false }; VD_Patch8.Apply(true);
				CodePatch VD_Patch9 = { VehicleDisplay_RenderCullPoint2,			"","\x80\x43",2,false }; VD_Patch9.Apply(true);
				CodePatch VD_Patch10 = { VehicleDisplay_RenderCullPoint3,			"","\x80\x43",2,false }; VD_Patch10.Apply(true);
				CodePatch VD_Patch11 = { VehicleDisplay_RenderCullPoint4,			"","\x80\x43",2,false }; VD_Patch11.Apply(true);
				Console::eval("$pref::hudScale::status = 4;");
			}
			else if (arg1.compare("chat") == 0)
			{
				CodePatch CR_Patch0 = { ChatRec_PresetWidth1,						"","\xE8\x03",2,false }; CR_Patch0.Apply(true);
				CodePatch CR_Patch1 = { ChatRec_PresetWidth2,						"","\xE8\x03",2,false }; CR_Patch1.Apply(true);
				CodePatch CR_Patch2 = { ChatRec_PresetWidth3,						"","\xE8\x03",2,false }; CR_Patch2.Apply(true);
				CodePatch CR_Patch3 = { ChatRec_PresetHeight1,						"","\x4E\x00",2,false }; CR_Patch3.Apply(true);
				CodePatch CR_Patch4 = { ChatRec_PresetHeight2,						"","\xA6\x00",2,false }; CR_Patch4.Apply(true);
				CodePatch CR_Patch5 = { ChatRec_PresetHeight3,						"","\xE8\x00",2,false }; CR_Patch5.Apply(true);
				CodePatch CR_Patch6 = { ChatRec_FontTag,							"","\xCB\x4E\x02",3,false }; CR_Patch6.Apply(true);
				CodePatch CR_Patch7 = { ChatRec_FontTag_Dim,						"","\xCA\x4E\x02",3,false }; CR_Patch7.Apply(true);
				CodePatch CR_Patch8 = { ChatRec_FontTag_Gray,						"","\xCC\x4E\x02",3,false }; CR_Patch8.Apply(true);
				Console::eval("$pref::hudScale::chat = 4;");
			}
			else if (arg1.compare("timers") == 0)
			{
				CodePatch HT_Patch0 = { HudTimer_FontTag,							"","\xCB\x4E\x02",3,false }; HT_Patch0.Apply(true);
				Console::eval("$pref::hudScale::timers = 4;");
			}
			else if (arg1.compare("orders") == 0)
			{
				CodePatch CM_Patch0 = { CmdMenu_FontTag,							"","\xCB\x4E\x02",3,false }; CM_Patch0.Apply(true);
				CodePatch CM_Patch1 = { CmdMenu_Width,								"","\x80\x02",2,false }; CM_Patch1.Apply(true);
				Console::eval("$pref::hudScale::orders = 4;");
			}
			else if (arg1.compare("shields") == 0)
			{
				Console::eval("IDBMP_SHIELDS = 00160012, \"shields4x.bmp\";");
				Console::eval("IDBMP_SHIELDS_SOLID = 00160166, \"shields_solid4x.bmp\";");
				Console::eval("IDBMP_STR_CIRCLE = 00160013, \"strCircle4x.bmp\";");
				CodePatch SD_Patch1 = { ShieldDisplay_FontTag,								"","\xCB\x4E\x02",3,false }; SD_Patch1.Apply(true);
				CodePatch SD_Patch2 = { ShieldDisplay_DirectionalRingHorizontalOffset,		"","\x2B",1,false }; SD_Patch2.Apply(true);
				CodePatch SD_Patch3 = { ShieldDisplay_DirectionalRingVerticalOffset,		"","\x14",1,false }; SD_Patch3.Apply(true);
				CodePatch SD_Patch4 = { ShieldDisplay_LabelTextHorizontalOffset,			"","\x6C",1,false }; SD_Patch4.Apply(true);
				CodePatch SD_Patch5 = { ShieldDisplay_LabelTextVerticalOffset,				"","\xBA",1,false }; SD_Patch5.Apply(true);
				CodePatch SD_Patch6 = { ShieldDisplay_StrengthTextHorizontalOffset,			"","\xE0",1,false }; SD_Patch6.Apply(true);
				CodePatch SD_Patch7 = { ShieldDisplay_StrengthTextVerticalOffset,			"","\xD4",1,false }; SD_Patch7.Apply(true);
				CodePatch SD_Patch8 = { ShieldDisplay_StrengthArrowVerticalOffset,			"","\xEF\xFF\xFF\xFF",4,false }; SD_Patch8.Apply(true);
				CodePatch SD_Patch9 = { ShieldDisplay_StrengthArrowHorizontalOffset,		"","\x70\x01",2,false }; SD_Patch9.Apply(true);
				CodePatch SD_Patch10 = { ShieldDisplay_StrengthArrowPerspective,			"","\x40\x54\x09\x4C",4,false }; SD_Patch10.Apply(true);
				CodePatch SD_Patch11 = { ShieldDisplay_StrengthArrowLength,					"","\x4A",1,false }; SD_Patch11.Apply(true);
				CodePatch SD_Patch12 = { ShieldDisplay_StrengthArrowWidth,					"","\x12",1,false }; SD_Patch12.Apply(true);
				CodePatch SD_Patch13 = { ShieldDisplay_DirectionalTextGapSize,				"","\x55",1,false }; SD_Patch13.Apply(true);
				CodePatch SD_Patch14 = { ShieldDisplay_DirectionalInfoLineTextVerticalOffset,"","\x03",1,false }; SD_Patch14.Apply(true);
				CodePatch SD_Patch16 = { ShieldDisplay_DirectionalInfoLine,					"","\x55",1,false }; SD_Patch16.Apply(true);
				CodePatch SD_Patch17 = { ShieldDisplay_DirectionalInfoLineFooterTextZaxis,	"","\x70\x01",2,false }; SD_Patch17.Apply(true);
				Console::eval("$pref::hudScale::shields = 4;");
			}
			else if (arg1.compare("retical") == 0)
			{
				CodePatch AR_Patch0 = { AimRet_FontTag,								"","\xCB\x4E\x02",3,false }; AR_Patch0.Apply(true);
				//CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x06",1,false }; AR_Patch1.Apply(true);
				//CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x28",1,false }; AR_Patch2.Apply(true);
				//CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\xDB\xFF\xFF\xFF",4,false }; AR_Patch3.Apply(true);
				//CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\x83\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				//CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x60",1,false }; AR_Patch5.Apply(true);
				//CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x0C",1,false }; AR_Patch6.Apply(true);
				//CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\xA0\x00\x40",3,false }; AR_DIRARROW.Apply(true);
				CodePatch AR_Patch1 = { AimRet_TargetOutlineCornerScale,			"","\x04",1,false }; AR_Patch1.Apply(true);
				CodePatch AR_Patch2 = { AimRet_TargetOutlineCornerLength,			"","\x14",1,false }; AR_Patch2.Apply(true);
				CodePatch AR_Patch3 = { AimRet_TargetDistanceTextHorizontalOffset,	"","\x00\x00\x00\x00",4,false }; AR_Patch3.Apply(true);
				CodePatch AR_Patch4 = { AimRet_TargetDistanceTextVerticalOffset,	"","\xC1\xFF\xFF\xFF",4,false }; AR_Patch4.Apply(true);
				CodePatch AR_Patch5 = { AimRet_LeadPointerVerticalScale,			"","\x30",1,false }; AR_Patch5.Apply(true);
				CodePatch AR_Patch6 = { AimRet_LeadPointerHorizontalScale,			"","\x06",1,false }; AR_Patch6.Apply(true);
				CodePatch AR_DIRARROW = { AimRet_DiractionalArrowWholeScale,		"","\x80\xFF\x3F",3,false }; AR_DIRARROW.Apply(true);
				Console::eval("$pref::hudScale::retical = 4;");
			}
			else if (arg1.compare("internals") == 0)
			{
				CodePatch ID_Patch0 = { IntD_Dim_FontTag,							"","\xCA\x4E\x02",3,false }; ID_Patch0.Apply(true);
				CodePatch ID_Patch1 = { IntD_BoxWidth,								"","\xFE\x01",2,false }; ID_Patch1.Apply(true);
				Console::eval("$pref::hudScale::internals = 4;");
			}
			else if (arg1.compare("config") == 0)
			{
				CodePatch HDCFG_Patch0 = { HDCFG_Title_FontTag,							"","\xCB\x4E\x02",3,false }; HDCFG_Patch0.Apply(true);
				CodePatch HDCFG_Patch1 = { HDCFG_VerticalSpacing,						"","\x25",1,false }; HDCFG_Patch1.Apply(true);
				Console::eval("$Localize::HudPrefExtent_HR = 495;");//Window Width
				CodePatch HDCFG_Patch2 = { HDCFG_ButtonFontTag,							"","\xCA\x4E\x02",3,false }; HDCFG_Patch2.Apply(true);
				CodePatch HDCFG_Patch3 = { HDCFG_ButtonHighlightFontTag,				"","\xCB\x4E\x02",3,false }; HDCFG_Patch3.Apply(true);
				CodePatch HDCFG_Patch4 = { HDCFG_CycleColorsButtonFontTag,				"","\xCA\x4E\x02",3,false }; HDCFG_Patch4.Apply(true);
				CodePatch HDCFG_Patch5 = { HDCFG_CycleColorsButtonHighlightFontTag,		"","\xCB\x4E\x02",3,false }; HDCFG_Patch5.Apply(true);
				Console::eval("$Localize::HudPrefOffset_HR = 195;");//Button subtext indentation offset
				Console::eval("$pref::hudScale::config = 4;");
			}
		}
	}
	return 0;
}

BuiltInFunction("Nova::RefreshRadarVectorObjects", _novarefreshvectors)
{
	RadarVectorsAlwaysRefresh.Apply(true);
	return "true";
}
struct Init {
	Init() {
		hudpbafix.DoctorRelative((u32)HudPBAFix, 1).Apply(true);
		RadarElementsAlwaysRefresh.Apply(true);
		ShieldElementsAlwaysRefresh.Apply(true);
		ShieldVectorsAlwaysRefresh.Apply(true);

		//Assign default hud fonts to alternative IDs to avoid conflicts with text scaling
		if (std::filesystem::exists("Nova.vol"))
		{
			CodePatch SB_Patch0 = { HudScoreboard_FontTag,						"","\xE4\x4B\x02",3,false }; SB_Patch0.Apply(true);
			CodePatch WD_Patch0 = { WeaponDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; WD_Patch0.Apply(true);
			CodePatch WD_Patch0a = { WeaponDisplay_WeaponRollPitchError_FontTag,"","\xE6\x4B\x02",3,false }; WD_Patch0a.Apply(true);
			CodePatch RD_PatchF = { RadarDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; RD_PatchF.Apply(true);
			CodePatch VD_Patch0 = { VehicleDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; VD_Patch0.Apply(true);
			CodePatch HT_Patch0 = { HudTimer_FontTag,							"","\xE4\x4B\x02",3,false }; HT_Patch0.Apply(true);
			CodePatch CM_Patch0 = { CmdMenu_FontTag,							"","\xE4\x4B\x02",3,false }; CM_Patch0.Apply(true);
			CodePatch SD_Patch1 = { ShieldDisplay_FontTag,						"","\xE4\x4B\x02",3,false }; SD_Patch1.Apply(true);
			CodePatch AR_Patch0 = { AimRet_FontTag,								"","\xE4\x4B\x02",3,false }; AR_Patch0.Apply(true);
			CodePatch ID_Patch0 = { IntD_Dim_FontTag,							"","\xE4\x4B\x02",3,false }; ID_Patch0.Apply(true);
			CodePatch HDCFG_Patch0 = { HDCFG_Title_FontTag,						"","\xE4\x4B\x02",3,false }; HDCFG_Patch0.Apply(true);
			CodePatch HDCFG_Patch2 = { HDCFG_ButtonFontTag,						"","\xE5\x4B\x02",3,false }; HDCFG_Patch2.Apply(true);
			CodePatch HDCFG_Patch3 = { HDCFG_ButtonHighlightFontTag,			"","\xE4\x4B\x02",3,false }; HDCFG_Patch3.Apply(true);
			CodePatch HDCFG_Patch4 = { HDCFG_CycleColorsButtonFontTag,			"","\xE5\x4B\x02",3,false }; HDCFG_Patch4.Apply(true);
			CodePatch HDCFG_Patch5 = { HDCFG_CycleColorsButtonHighlightFontTag,	"","\xE4\x4B\x02",3,false }; HDCFG_Patch5.Apply(true);
		}
		//CodePatch HDGMCFG_Patch0 = { HudGamePrefs_FontTag0,					"","\xE4\x4B\x02",3,false }; HDGMCFG_Patch0.Apply(true);
		//CodePatch HDGMCFG_Patch1 = { HudGamePrefs_FontTag1,					"","\xE4\x4B\x02",3,false }; HDGMCFG_Patch1.Apply(true);
		//CodePatch HDGMCFG_Patch2 = { HudGamePrefs_FontTag2,					"","\xE4\x4B\x02",3,false }; HDGMCFG_Patch2.Apply(true);
		//CodePatch HDGMCFG_Patch3 = { HudGamePrefs_FontTag3,					"","\xE4\x4B\x02",3,false }; HDGMCFG_Patch3.Apply(true);
		//CodePatch HDGMCFG_Patch4 = { HudGamePrefs_FontTag4,					"","\xE4\x4B\x02",3,false }; HDGMCFG_Patch4.Apply(true);
		//CodePatch HDGMCFG_Patch5 = { HudGamePrefs_FontTag5,					"","\xE4\x4B\x02",3,false }; HDGMCFG_Patch5.Apply(true);

	}
}init;