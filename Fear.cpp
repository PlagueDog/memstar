#include "Fear.h"
#include "Strings.h"
#include "MultiPointer.h"
#include "console.h"

namespace Fear {

	MultiPointer(ptr_SIM_PTR, 0, 0, 0x0070caec, 0x0071CF5C);
	MultiPointer(ptr_SIMSKY_VFT, 0, 0, 0x007021ec, 0x0071265C);
	MultiPointer(ptr_SIMSET_VFT, 0, 0, 0x0070452c, 0x0071492C);
	MultiPointer(ptr_SIMCANVAS_PTR, 0, 0, 0x0076f4d0, 0x0077FAE8);

	Sim* Sim::Client() {
		return *(Sim**)(ptr_SIM_PTR);
	}

	template<class T>
	T* Sim::findObject(const char* nameOrId) {
		if (!this)
			return NULL;

		__asm {
			mov eax, this
			push eax
			mov edx, [nameOrId]
			mov ecx, [eax]
			call[ecx + 0x6c]
			pop this
		}
	}

	Sky* findSky() {
		SimSet* render_set = Sim::Client()->findObject<SimSet>(13);
		if (render_set->vft == ptr_SIMSET_VFT) {
			for (SimSet::Iterator iter = render_set->Begin(); iter != render_set->End(); ++iter)
				if ((*iter)->vft == ptr_SIMSKY_VFT)
					return (Sky*)*iter;
		}
		return NULL;
	}

	PlayerPSC* findPSC() {
		return Sim::Client()->findObject<PlayerPSC>(636);
	}

	f32 Sim::getTime() {
		return (this) ? Time : 0;
	}

	static const u32 fnSkyCalcPoints = 0x005855E0;

	void Sky::calcPoints() {
		__asm {
			mov eax, this
			push eax
			call[fnSkyCalcPoints]
			pop this
		}
	}

	bool getScreenDimensions(Vector2i* dim) {
		__asm {
			// SimGui::Canvas
			mov esi, ptr_SIMCANVAS_PTR
			mov eax, ds: [esi]
			and eax, eax
			jz done

			// GFXDevice::?
			mov ebx, [eax + 0x144]
			xor eax, eax
			and ebx, ebx
			jz done

			// GFX::Surface
			mov eax, [ebx + 0x3c]

			mov edx, [dim]
			mov ecx, [eax + 0x3c]
			mov[edx + 0], ecx
			mov ecx, [eax + 0x40]
			mov[edx + 4], ecx
			mov eax, 1
			done:
		}
	}

	//BuiltInFunction("getVehicleID", _getvehicleid)
	//{
	//	Fear::Herc* vehicle = Sim::Client()->findObject<Herc>(atoi(argv[0]));
	//	char* id = reinterpret_cast<char*>(&vehicle->vehicleID)+1;
	//	Console::echo("Internal ID: %s | Memory address: %p", id, static_cast<void*>(&vehicle->vehicleID));
	//
	//	//FOV
	//	u32 fov = reinterpret_cast<u32>(&vehicle->vehicleID) + 0x108;
	//	u32* fov_ptr = reinterpret_cast<u32*>(fov);
	//	u32 fov_value = *fov_ptr;
	//	Console::echo("FOV: %x | Memory address: %p", fov_value, fov_ptr);
	//	*fov_ptr = atof(argv[0]);
	//	return "true";
	//}
	//
	BuiltInFunction("getGuiObject", _getguiobject)
	{
		Fear::SimGuiCtrl* object = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
		char* id = reinterpret_cast<char*>(&object->pos);
		char buffer[50];
		snprintf(buffer, sizeof(buffer), "x: %d, y: %d", object->pos.x, object->pos.y);

		Console::echo("%s", buffer);
		return "true";
	}

	BuiltInFunction("getGuiObjectPosition", getguiposition)
	{
		Fear::SimGuiCtrl* object = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
		char* id = reinterpret_cast<char*>(&object->pos);
		char buffer[50];
		snprintf(buffer, sizeof(buffer), "%d,%d", object->pos.x, object->pos.y);

		return buffer;
	}

	BuiltInFunction("getGuiObjectExtent", getguiextent)
	{
		Fear::SimGuiCtrl* object = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
		char* id = reinterpret_cast<char*>(&object->dimensions);
		char buffer[50];
		snprintf(buffer, sizeof(buffer), "%d,%d", object->dimensions.x, object->dimensions.y);

		return buffer;
	}

	BuiltInFunction("setGuiObjectPosition", simguisetposition) {
		Fear::SimGuiCtrl* guiObject = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
		if (guiObject) {
			guiObject->pos.x = (s32)atoi(argv[1]);
			guiObject->pos.y = (s32)atoi(argv[2]);
		}
		return "true";
	}

	BuiltInFunction("setGuiObjectExtent", simguiextent) {
		Fear::SimGuiCtrl* guiObject = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
		if (guiObject) {
			guiObject->dimensions.x = (s32)atoi(argv[1]);
			guiObject->dimensions.y = (s32)atoi(argv[2]);
		}
		return "true";
	}

	BuiltInFunction("setGuiObjectHighlightFont", simguihlfont) {
		Fear::SimGuiCtrl* guiObject = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
		if (guiObject) {
			guiObject->hlfont = (s32)atoi(argv[1]);
		}
		return "true";
	}

	BuiltInFunction("findObject", findobject)
	{
		Fear::SimGuiCtrl* vehicle = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
			u32 fov = reinterpret_cast<u32>(&vehicle->pos.x);
			u32* fov_ptr = reinterpret_cast<u32*>(fov);
			u32 fov_value = *fov_ptr;
			Console::echo("FOV: %x | Memory address: %p", fov_value, fov_ptr);
			*fov_ptr = atof(argv[0]);
			return "true";
	}

	BuiltInFunction("getInfo", _getInfo)
		{
			Fear::SimGuiCtrl* vehicle = Sim::Client()->findObject<SimGuiCtrl>(atoi(argv[0]));
			char* id = reinterpret_cast<char*>(&vehicle->pos);
			Console::echo("Internal ID: %s | Memory address: %p", id, static_cast<void*>(&vehicle->pos));
		
			//FOV
			u32 fov = reinterpret_cast<u32>(&vehicle->hlfont);
			u32* fov_ptr = reinterpret_cast<u32*>(fov);
			u32 fov_value = *fov_ptr;
			Console::echo("Value: %x | Memory address: %p", fov_value, fov_ptr);
			return "true";
		}
}; // namespace Fear