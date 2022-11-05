#include "Console.h"
#include "Patch.h"
#include "VersionSnoop.h"
#include "MultiPointer.h"

namespace ExeFixes {

	//OpenGL
	MultiPointer(ptrOGLWidthMin, 0, 0, 0x0063C80B, 0x0064B74B);
	MultiPointer(ptrOGLWidthMax, 0, 0, 0x0063C816, 0x0064B756);
	MultiPointer(ptrOGLHeightMin, 0, 0, 0x0063C825, 0x0064B765);
	MultiPointer(ptrOGLHeightMax, 0, 0, 0x0063C835, 0x0064B775);
	CodePatch OpenGLWidthMin =	{ ptrOGLWidthMin,"","\x3D\xAA\x01",3,false };
	CodePatch OpenGLWidthMax =	{ ptrOGLWidthMax,"","\x3D\x00\x0A",3,false };
	CodePatch OpenGLHeightMin = { ptrOGLHeightMin,"","\x81\xFA\xF0\x00",4,false };
	CodePatch OpenGLHeightMax = { ptrOGLHeightMax,"","\x81\xF9\xA0\x05",4,false };

	//DirectDraw (Software)
	MultiPointer(ptrSoftwareResWidthCap, 0, 0, 0x0064831A, 0x00658032);
	MultiPointer(ptrSoftwareResHeightCap, 0, 0, 0x00648324, 0x0065803C);
	CodePatch SoftwareResWidthCap =	 { ptrSoftwareResWidthCap,	"","\x3D\x00\x05",		3,false };
	CodePatch SoftwareResHeightCap = { ptrSoftwareResHeightCap,	"","\x81\xFA\x00\x03",	4,false };
	MultiPointer(ptrDoSFix, 0, 0, 0x0067c7e6, 0x0068C6B2);

	//                                                 |.  8DBD 38FFFFFF     LEA EDI,DWORD PTR SS:[EBP-C8]
	CodePatch dosfix = {
		ptrDoSFix,
		"",
		"\xE9OSFX",
		5,
		false
	};


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

	struct Init {
		Init() {
			if (VersionSnoop::GetVersion() == VERSION::vNotGame) {
				return;
			}
			if (VersionSnoop::GetVersion() == VERSION::v001004) {
				dosfix.DoctorRelative((u32)DosFix, 1).Apply(true);
			}

			OpenGLWidthMin.Apply(true);
			OpenGLWidthMax.Apply(true);
			OpenGLHeightMin.Apply(true);
			OpenGLHeightMax.Apply(true);
			SoftwareResWidthCap.Apply(true);
			SoftwareResHeightCap.Apply(true);
		}
	} init;
}; // namespace ExeFixes