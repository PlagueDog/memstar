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

namespace ExtendedEvents
{
	//Extended Vehicle Events
	MultiPointer(ptrVehicleEventListInit, 0, 0, 0, 0x0047A23A);
	MultiPointer(ptrVehicleEventListResume, 0, 0, 0, 0x0047A3F7);

	//This subroutine checks to see if a function has actually been defined
	//MultiPointer(ptrFunctionVerify, 0, 0, 0, 0x0044ACD4);
	MultiPointer(sub_44ACD4, 0, 0, 0, 0x0044ACD4);
	MultiPointer(sub_47A01C, 0, 0, 0, 0x0047A01C);
	CodePatch extendvehicleevents = { ptrVehicleEventListInit,"","\xE9VELI",5,false };

	//static const char* VehicleEventDebug1 = "vehicle::debugOne";
	//NAKED void ExtendVehicleEvents() {
	//	__asm {
	//		lea eax, [ebx + 0x33C]
	//		mov edx, [VehicleEventDebug1]
	//		push eax
	//		mov eax, ebx
	//		mov ecx, esi
	//		call [ptrFunctionVerify]
	//		jmp[ptrVehicleEventListResume]
	//	}
	//}

	static const char* VehicleEventDebug1 = "vehicle::debugOne";
	static const char* aVehicleOnadd = "vehicle::onAdd";
	static const char* aVehicleOnattac = "vehicle::onAttack";
	static const char* aVehicleOnenabl = "vehicle::onEnable";
	static const char* aVehicleOndisab = "vehicle::onDisable";
	static const char* aVehicleOndestr = "vehicle::onDestroy";
	static const char* aVehicleOnarriv = "vehicle::onArrive";
	static const char* aVehicleOnscan = "vehicle::onScan";
	static const char* aVehicleOnspot = "vehicle::onSpot";
	static const char* aVehicleOnnewle = "vehicle::onNewLeader";
	static const char* aVehicleOnnewta = "vehicle::onNewTarget";
	static const char* aVehicleOntarge = "vehicle::onTarget";
	static const char* aVehicleOnmessa = "vehicle::onMessage";
	static const char* aVehicleOnactio = "vehicle::onAction";
	NAKED void ExtendVehicleEvents() {
		__asm {
			mov     eax, [ebx + 33Ch]
			push    eax
			call    sub_44ACD4
			pop     ecx
			mov     edx, [ebx + 340h]
			push    edx
			call    sub_44ACD4
			pop     ecx
			mov     ecx, [ebx + 344h]
			push    ecx
			call    sub_44ACD4
			pop     ecx
			mov     eax, [ebx + 348h]
			push    eax
			call    sub_44ACD4
			pop     ecx
			mov     edx, [ebx + 34Ch]
			push    edx
			call    sub_44ACD4
			pop     ecx
			mov     ecx, [ebx + 350h]
			push    ecx
			call    sub_44ACD4
			pop     ecx
			mov     eax, [ebx + 35Ch]
			push    eax
			call    sub_44ACD4
			pop     ecx
			mov     edx, [ebx + 360h]
			push    edx
			call    sub_44ACD4
			pop     ecx
			mov     ecx, [ebx + 354h]
			push    ecx
			call    sub_44ACD4
			pop     ecx
			mov     eax, [ebx + 358h]
			push    eax
			call    sub_44ACD4
			pop     ecx
			mov     edx, [ebx + 368h]
			push    edx
			call    sub_44ACD4
			pop     ecx
			mov     ecx, [ebx + 36Ch]
			push    ecx
			call    sub_44ACD4
			pop     ecx
			mov     eax, [ebx + 370h]
			push    eax
			call    sub_44ACD4
			pop     ecx

			mov     eax, [ebx + 338h]
			push    eax
			call    sub_44ACD4
			pop     ecx

			mov     eax, [ebx + 334h]
			push    eax
			call    sub_44ACD4
			pop     ecx

			mov     eax, [ebx + 330h]
			push    eax
			call    sub_44ACD4
			pop     ecx

			mov     eax, [ebx + 32Ch]
			push    eax
			call    sub_44ACD4
			pop     ecx

			mov     esi, [ebx + 4]
			lea     eax, [ebx + 33Ch]
			push    eax; int
			mov     edx, aVehicleOnadd; "vehicle::onAdd"
			mov     ecx, esi; int
			mov     eax, ebx; int
			call    sub_47A01C
			lea     edx, [ebx + 340h]
			mov     ecx, esi; int
			push    edx; int
			mov     edx, aVehicleOnattac; "vehicle::onAttacked"
			mov     eax, ebx; int
			call    sub_47A01C
			lea     eax, [ebx + 344h]
			mov     edx, aVehicleOnenabl; "vehicle::onEnabled"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C
			lea     edx, [ebx + 348h]
			mov     ecx, esi; int
			push    edx; int
			mov     edx, aVehicleOndisab; "vehicle::onDisabled"
			mov     eax, ebx; int
			call    sub_47A01C
			lea     eax, [ebx + 34Ch]
			mov     edx, aVehicleOndestr; "vehicle::onDestroyed"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C
			lea     edx, [ebx + 350h]
			mov     ecx, esi; int
			push    edx; int
			mov     edx, aVehicleOnarriv; "vehicle::onArrived"
			mov     eax, ebx; int
			call    sub_47A01C
			lea     eax, [ebx + 35Ch]
			mov     edx, aVehicleOnscan; "vehicle::onScan"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C
			lea     edx, [ebx + 360h]
			mov     ecx, esi; int
			push    edx; int
			mov     edx, aVehicleOnspot; "vehicle::onSpot"
			mov     eax, ebx; int
			call    sub_47A01C
			lea     eax, [ebx + 354h]
			mov     edx, aVehicleOnnewle; "vehicle::onNewLeader"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C
			lea     edx, [ebx + 358h]
			mov     ecx, esi; int
			push    edx; int
			mov     edx, aVehicleOnnewta; "vehicle::onNewTarget"
			mov     eax, ebx; int
			call    sub_47A01C
			lea     eax, [ebx + 368h]
			mov     edx, aVehicleOntarge; "vehicle::onTargeted"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C
			lea     edx, [ebx + 36Ch]
			mov     ecx, esi; int
			push    edx; int
			mov     edx, aVehicleOnmessa; "vehicle::onMessage"
			mov     eax, ebx; int
			call    sub_47A01C
			lea     eax, [ebx + 370h]
			mov     edx, aVehicleOnactio; "vehicle::onAction"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C

			lea     eax, [ebx + 338h]
			mov     edx, VehicleEventDebug1; "vehicle::debugOne"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C

			lea     eax, [ebx + 334h]
			mov     edx, VehicleEventDebug1; "vehicle::debugOne"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C

			lea     eax, [ebx + 330h]
			mov     edx, VehicleEventDebug1; "vehicle::debugOne"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C

			lea     eax, [ebx + 32Ch]
			mov     edx, VehicleEventDebug1; "vehicle::debugOne"
			push    eax; int
			mov     eax, ebx; int
			mov     ecx, esi; int
			call    sub_47A01C
			jmp[ptrVehicleEventListResume]
		}
	}

	struct Init {
		Init() {
			//extendvehicleevents.DoctorRelative((u32)ExtendVehicleEvents, 1).Apply(true);
		}
	}init;
}