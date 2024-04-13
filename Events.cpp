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
	MultiPointer(ptrVehicleEventListResume, 0, 0, 0, 0x0047A246);

	//This subroutine checks to see if a function has actually been defined
	MultiPointer(ptrFunctionVerify, 0, 0, 0, 0x0044ACD4);
	CodePatch extendvehicleevents = { ptrVehicleEventListInit,"","\xE9VELI",5,false };

	static const char* VehicleEventDebug1 = "vehicle::debugOne";
	NAKED void ExtendVehicleEvents() {
		__asm {
			lea eax, [ebx + 0x33C]
			mov edx, [VehicleEventDebug1]
			push eax
			mov eax, ebx
			mov ecx, esi
			call [ptrFunctionVerify]
			jmp[ptrVehicleEventListResume]
		}
	}

	struct Init {
		Init() {
			//extendvehicleevents.DoctorRelative((u32)ExtendVehicleEvents, 1).Apply(true);
		}
	}init;
}