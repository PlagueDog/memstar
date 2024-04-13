#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <string>

namespace GlideLib {

//Add Glide to the list of windowed devices
MultiPointer(ptrGlideModes, 0, 0, 0x00636B53, 0x00645A93);
CodePatch GlideModes = { ptrGlideModes ,		"", "\x03", 1, false };

//Glide Resolutions (Combo Box)
MultiPointer(ptrGlideResHeight_1, 0, 0, 0x00637368, 0x006462A8);
MultiPointer(ptrGlideResWidth_1, 0, 0, 0x0063736F, 0x006462AF);
CodePatch GlideComboBoxRes_1 = { ptrGlideResWidth_1,		"", "\x80\x02", 2, false };
CodePatch GlideComboBoxRes_1a = { ptrGlideResHeight_1,		"", "\xE0\x01", 2, false };

MultiPointer(ptrGlideResHeight_2, 0, 0, 0x006373CE, 0x0064630E);
MultiPointer(ptrGlideResWidth_2, 0, 0, 0x006373D6, 0x00646316);
CodePatch GlideComboBoxRes_2 = { ptrGlideResWidth_2,		"", "\x20\x03", 2, false };
CodePatch GlideComboBoxRes_2a = { ptrGlideResHeight_2,		"", "\x58\x02", 2, false };

MultiPointer(ptrGlideResHeight_3, 0, 0, 0x00637436, 0x00646376);
MultiPointer(ptrGlideResWidth_3, 0, 0, 0x0063743E, 0x0064637E);
CodePatch GlideComboBoxRes_3 = { ptrGlideResWidth_3,		"", "\x00\x04", 2, false };
CodePatch GlideComboBoxRes_3a = { ptrGlideResHeight_3,		"", "\x00\x03", 2, false };

MultiPointer(ptrGlideResHeight_4, 0, 0, 0x006374AF, 0x006463EF);
MultiPointer(ptrGlideResWidth_4, 0, 0, 0x006374AA, 0x006463EA);
CodePatch GlideComboBoxRes_4 = { ptrGlideResWidth_4,		"", "\x00\x05", 2, false };
CodePatch GlideComboBoxRes_4a = { ptrGlideResHeight_4,		"", "\x00\x04", 2, false };

MultiPointer(ptrGlideResHeight_5, 0, 0, 0x00637520, 0x00646460);
MultiPointer(ptrGlideResWidth_5, 0, 0, 0x0063751B, 0x0064645B);
CodePatch GlideComboBoxRes_5 = { ptrGlideResWidth_5,		"", "\x40\x06", 2, false };
CodePatch GlideComboBoxRes_5a = { ptrGlideResHeight_5,		"", "\xB0\x04", 2, false };

//Glide Resolutions (Internal)
MultiPointer(ptrGlideResIntHeight_1, 0, 0, 0x0063690A, 0x0064584A);
MultiPointer(ptrGlideResIntWidth_1, 0, 0, 0x00636911, 0x00645851);
CodePatch GlideInternalBoxRes_1 = { ptrGlideResIntWidth_1,	"", "\x80\x02", 2, false };
CodePatch GlideInternalBoxRes_1a = { ptrGlideResIntHeight_1,"", "\xE0\x01", 2, false };

MultiPointer(ptrGlideResIntHeight_2, 0, 0, 0x00636960, 0x006458A0);
MultiPointer(ptrGlideResIntWidth_2, 0, 0, 0x00636968, 0x006458A8);
CodePatch GlideInternalBoxRes_2 = { ptrGlideResIntWidth_2,	"", "\x20\x03", 2, false };
CodePatch GlideInternalBoxRes_2a = { ptrGlideResIntHeight_2,"", "\x58\x02", 2, false };

MultiPointer(ptrGlideResIntHeight_3, 0, 0, 0x006369B8, 0x006458F8);
MultiPointer(ptrGlideResIntWidth_3, 0, 0, 0x006369C0, 0x00645900);
CodePatch GlideInternalBoxRes_3 = { ptrGlideResIntWidth_3,	"", "\x00\x04", 2, false };
CodePatch GlideInternalBoxRes_3a = { ptrGlideResIntHeight_3,"", "\x00\x03", 2, false };

MultiPointer(ptrGlideResIntHeight_4, 0, 0, 0x00636A10, 0x00645950);
MultiPointer(ptrGlideResIntWidth_4, 0, 0, 0x00636A18, 0x00645958);
CodePatch GlideInternalBoxRes_4 = { ptrGlideResIntWidth_4,	"", "\x00\x05", 2, false };
CodePatch GlideInternalBoxRes_4a = { ptrGlideResIntHeight_4,"", "\x00\x04", 2, false };

MultiPointer(ptrGlideResIntHeight_5, 0, 0, 0x00636A68, 0x006459A8);
MultiPointer(ptrGlideResIntWidth_5, 0, 0, 0x00636A70, 0x006459B0);
CodePatch GlideInternalBoxRes_5 = { ptrGlideResIntWidth_5,	"", "\x40\x06", 2, false };
CodePatch GlideInternalBoxRes_5a = { ptrGlideResIntHeight_5,"", "\xB0\x04", 2, false };

//Glide Resolutions (Canvas)
MultiPointer(ptrGlideResCanvHeight_1, 0, 0, 0, 0x006465D8);
MultiPointer(ptrGlideResCanvWidth_1, 0, 0, 0, 0x006465D0);
MultiPointer(ptrGlideRes_1, 0, 0, 0, 0x006465DF);
CodePatch GlideDLLRes_1 = { ptrGlideResCanvWidth_1,			"", "\x80\x02", 2, false };
CodePatch GlideDLLRes_1a = { ptrGlideResCanvHeight_1,		"", "\xE0\x01", 2, false };
CodePatch GlideDLLRes_1b = { ptrGlideRes_1,	"",				"\x07", 1, false };

MultiPointer(ptrGlideResCanvHeight_2, 0, 0, 0, 0x006465EF);
MultiPointer(ptrGlideResCanvWidth_2, 0, 0, 0, 0x006465E7);
MultiPointer(ptrGlideRes_2, 0, 0, 0, 0x006465F6);
CodePatch GlideDLLRes_2 = { ptrGlideResCanvWidth_2,			"", "\x20\x03", 2, false };
CodePatch GlideDLLRes_2a = { ptrGlideResCanvHeight_2,		"", "\x58\x02", 2, false };
CodePatch GlideDLLRes_2b = { ptrGlideRes_2,	"",				"\x08", 1, false };

MultiPointer(ptrGlideResCanvHeight_3, 0, 0, 0, 0x00646606);
MultiPointer(ptrGlideResCanvWidth_3, 0, 0, 0, 0x006465FE);
MultiPointer(ptrGlideRes_3, 0, 0, 0, 0x0064660D);
CodePatch GlideDLLRes_3 = { ptrGlideResCanvWidth_3,			"", "\x00\x04", 2, false };
CodePatch GlideDLLRes_3a = { ptrGlideResCanvHeight_3,		"", "\x00\x03", 2, false };
CodePatch GlideDLLRes_3b = { ptrGlideRes_3,	"",				"\x0C", 1, false };

MultiPointer(ptrGlideResCanvHeight_4, 0, 0, 0, 0x0064661D);
MultiPointer(ptrGlideResCanvWidth_4, 0, 0, 0, 0x00646615);
MultiPointer(ptrGlideRes_4, 0, 0, 0, 0x00646624);
CodePatch GlideDLLRes_4 = { ptrGlideResCanvWidth_4,			"", "\x00\x05", 2, false };
CodePatch GlideDLLRes_4a = { ptrGlideResCanvHeight_4,		"", "\x00\x04", 2, false };
CodePatch GlideDLLRes_4b = { ptrGlideRes_4,	"",				"\x0D", 1, false };

MultiPointer(ptrGlideResCanvHeight_5, 0, 0, 0, 0x00646634);
MultiPointer(ptrGlideResCanvWidth_5, 0, 0, 0, 0x0064662C);
MultiPointer(ptrGlideRes_5, 0, 0, 0, 0x0064663B);
CodePatch GlideDLLRes_5 = { ptrGlideResCanvWidth_5,			"", "\x40\x06", 2, false };
CodePatch GlideDLLRes_5a = { ptrGlideResCanvHeight_5,		"", "\xB0\x04", 2, false };
CodePatch GlideDLLRes_5b = { ptrGlideRes_5,	"",				"\x0E", 1, false };

MultiPointer(ptrGlideResEnum, 0, 0, 0x0063768E, 0x006465CE);
MultiPointer(ptrGlideResEnumEnd, 0, 0, 0x00637701, 0x00646641);
MultiPointer(ptrGlideResCont, 0, 0, 0x00637718, 0x00646658);
CodePatch glideresenum = { ptrGlideResEnum, "", "\xE9GLRS", 5, false };
NAKED void GlideResEnum() {
	__asm {
		cmp esi, 0x280
		jnz __GlideRes_0x8
		cmp edi, 0x1E0
		jnz __GlideRes_0x8
		mov eax, 0x7
		jmp [ptrGlideResCont]
		__GlideRes_0x8:
			cmp esi, 0x320
			jnz __GlideRes_0xC
			cmp edi, 0x258
			jnz __GlideRes_0xC
			mov eax, 0x8
			jmp[ptrGlideResCont]
			__GlideRes_0xC:
				cmp esi, 0x400
				jnz __GlideRes_0xD
				cmp edi, 0x300
				jnz __GlideRes_0xD
				mov eax, 0xC
				jmp[ptrGlideResCont]
				__GlideRes_0xD:
					cmp esi, 0x500
					jnz __GlideRes_0xE
					cmp edi, 0x400
					jnz __GlideRes_0xE
					mov eax, 0xD
					jmp[ptrGlideResCont]
					__GlideRes_0xE:
						cmp esi, 0x640
						jnz __GlideResEnumEnd
						cmp edi, 0x4B0
						jnz __GlideResEnumEnd
						mov eax, 0xE
						jmp[ptrGlideResCont]
						__GlideResEnumEnd:
							jmp[ptrGlideResEnumEnd]
	}
}

struct Init {
	Init() {

	//Glide Resolution Patches (Combo box)
	GlideComboBoxRes_1.Apply(true);
	GlideComboBoxRes_1a.Apply(true);
	GlideComboBoxRes_2.Apply(true);
	GlideComboBoxRes_2a.Apply(true);
	GlideComboBoxRes_3.Apply(true);
	GlideComboBoxRes_3a.Apply(true);
	GlideComboBoxRes_4.Apply(true);
	GlideComboBoxRes_4a.Apply(true);
	GlideComboBoxRes_5.Apply(true);
	GlideComboBoxRes_5a.Apply(true);

	//Internal
	GlideInternalBoxRes_1.Apply(true);
	GlideInternalBoxRes_1a.Apply(true);
	GlideInternalBoxRes_2.Apply(true);
	GlideInternalBoxRes_2a.Apply(true);
	GlideInternalBoxRes_3.Apply(true);
	GlideInternalBoxRes_3a.Apply(true);
	GlideInternalBoxRes_4.Apply(true);
	GlideInternalBoxRes_4a.Apply(true);
	GlideInternalBoxRes_5.Apply(true);
	GlideInternalBoxRes_5a.Apply(true);

	//DLL
	//GlideDLLRes_1.Apply(true);
	//GlideDLLRes_1a.Apply(true);
	//GlideDLLRes_1b.Apply(true);
	//GlideDLLRes_2.Apply(true);
	//GlideDLLRes_2a.Apply(true);
	//GlideDLLRes_2b.Apply(true);
	//GlideDLLRes_3.Apply(true);
	//GlideDLLRes_3a.Apply(true);
	//GlideDLLRes_3b.Apply(true);
	//GlideDLLRes_4.Apply(true);
	//GlideDLLRes_4a.Apply(true);
	//GlideDLLRes_4b.Apply(true);
	//GlideDLLRes_5.Apply(true);
	//GlideDLLRes_5a.Apply(true);
	//GlideDLLRes_5b.Apply(true);

	glideresenum.DoctorRelative((u32)GlideResEnum, 1).Apply(true);
	GlideModes.Apply(true);
	}
	} init;
}; // namespace ExeFixes