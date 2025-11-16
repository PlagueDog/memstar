#include <filesystem>
#include "Fear.h"
#include "Console.h"
#include "windows.h"
#include "Patch.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <sstream>
#include <iostream>
#include <fstream>
#include <string>
#include <SHA.h>
#include <vector>
#include <iterator>
#include <filesystem>
#include <algorithm>
#include "hunzip.h"
#include <dirent.h>

using namespace std;
using namespace Fear;

BuiltInVariable("server::maxFileSize", int, servermaxfilesize, 5000);
BuiltInVariable("server::sendWeaponData", bool, serversendweapondata, false);
BuiltInVariable("server::sendMountData", bool, servermountdata, false);
BuiltInVariable("server::sendVehicleData", bool, servervehicledata, false);

bool isFile(const char* file)
{
	std::ifstream inputFile(file, std::ios::in);

	if (inputFile.is_open())
	{
		inputFile.close();
		return true;
	}
	return false;
}

BuiltInFunction("isFile", _novaisfile)
{
	if (argc != 1)
	{
		Console::echo("%s( file );", self);
		return 0;
	}
	string fileName = argv[0];
	fileName.erase(std::remove(fileName.begin(), fileName.end(), '"'), fileName.end());
	const char* rFileName = fileName.c_str();
	if (isFile(rFileName))
	{
		return "true";
	}
	return 0;
}

BuiltInFunction("createCacheDir", _ccd) {
	if (argc != 1)
	{
		Console::echo("%s( DirectoryName );", self);
		return "false";
	}
	string path = argv[0];
	char path_[MAX_PATH];
	strcpy(path_, ".\\mods\\cache\\");
	strcat(path_, argv[0]);
	CreateDirectory(path_, NULL);
	return 0;
}

BuiltInFunction("deleteServerCache", _dsc) {
	std::filesystem::remove_all(".\\mods\\cache");
	CreateDirectory(".\\mods\\cache", NULL);
	return 0;
}


namespace clientDataHandler {

	BuiltInFunction("Nova::findInDefaultPrefs", _NovafindInDefaultPrefs)
	{
		if (!isFile("defaultPrefs.cs"))
		{
			Console::echo("defaultPrefs.cs File not found.");
			return 0;
		}

		//Don't read the file as binary
		std::ifstream fin("defaultPrefs.cs", std::ios::in);
		//Find functions that require a client reload
		std::vector<std::string> field{ argv[0] };

		// Read complete file
		std::string fileContent(std::istreambuf_iterator<char>(fin), {});

		// Search for the lookup strings
		for (const std::string& l : field)
		{
			if (std::search(fileContent.begin(), fileContent.end(), l.begin(), l.end()) != fileContent.end())
			{
				return "true";
			}
		}
		return "false";
	}

	BuiltInFunction("Nova::purgeVehicleFiles", novapurgevehiclefiles) {
		std::filesystem::remove_all(".\\mods\\session");
		CreateDirectory(".\\mods\\session", NULL);
		return 0;
	}

	//Path + %s.fvh
	MultiPointer(ptrStandardVehicleFileLookup,	0, 0, 0x004A6A58, 0x004A8D74);
	MultiPointer(ptrStandardVehicleFileLookupResume, 0, 0, 0x004A6A5E, 0x004A8D7A);

	//Path + *.veh/fvh
	MultiPointer(ptrVehicleExtensionDirSwitchCase, 0, 0, 0x004F2CA4, 0x004F513C);
	MultiPointer(ptrVehicleExtensionDirCaseResume, 0, 0, 0x004F2CB7, 0x004F514F);

	MultiPointer(ptrVehicleSaveDir01, 0, 0, 0x00559411, 0x0055C24D);
	MultiPointer(ptrVehicleSaveDirResume01, 0, 0, 0x00559417, 0x0055C253);

	MultiPointer(ptrVehicleSaveDir02, 0, 0, 0x0046937C, 0x0046ADA0);
	MultiPointer(ptrVehicleSaveDirResume02, 0, 0, 0x00469382, 0x0046ADA6);

	MultiPointer(ptrVehicleSaveDir03, 0, 0, 0x004F1434, 0x004F38CC);
	MultiPointer(ptrVehicleSaveDirResume03, 0, 0, 0x004F143A, 0x004F38D2);
	CodePatch vehiclesavedir03 = { ptrVehicleSaveDir03, "", "\xE9VSD3", 5, false };

	MultiPointer(ptrVehicleDeleteDir, 0, 0, 0x004F2F96, 0x004F542E);
	CodePatch vehicledeletedir = { ptrVehicleDeleteDir, "", "\xE9VDDR", 5, false };

	MultiPointer(ptrGetVehicleName, 0, 0, 0, 0x004F500C);
	MultiPointer(ptrGetVehicleNameResume, 0, 0, 0, 0x004F5012);
	CodePatch grabdepotvehiclename = { ptrGetVehicleName, "", "\xE9GVNM", 5, false };
	static const char* callgetdepotvehicle = "getDepotVehicle();";
	char* vehicleName_;
	NAKED void GrabDepotVehicleName() {
		__asm {
			push ebx
			push esi
			push edi
			mov esi, edx
			push esi
			jmp [ptrGetVehicleNameResume]
		}
	}

	//BuiltInFunction("getDepotVehicle", _getdepotvehicle)
	//{
	//	char vehiclename[127];
	//	strcpy(vehiclename, "$Nova::depotVehicle = \"");
	//	strcat(vehiclename, vehicleName_);
	//	strcat(vehiclename, "\";");
	//	Console::echo(vehicleName_);
	//	Console::echo(vehiclename);
	//	Console::eval(vehiclename);
	//	free(vehiclename);
	//	return 0;
	//}

	BuiltInFunction("setVehicleDir", _svd)
	{
		if (argc != 1 || !strlen(argv[0]))
		{
			Console::echo("%s( dir );", self);
			return 0;
		}
		string path = argv[0];
		string ext = path.substr(path.size() - 4, path.size());
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot set a vehicle directory located outside of the Starsiege directory.");
			return "false";
		}

		char pathExt[MAX_PATH];
		char pathExt2[MAX_PATH];
		char pathExt3[MAX_PATH];
		strcpy(pathExt, argv[0]);
		Console::setVariable("zzmodloader::VehicleDir", pathExt);
		strcpy(pathExt2, pathExt);
		strcpy(pathExt3, pathExt);
		strcat(pathExt, "\\*.veh");
		strcat(pathExt2, "\\%s");
		strcat(pathExt3, "\\%s.veh");

		int byteLength = strlen(pathExt) + 1;
		int byteLength3 = strlen("mods\\session\\%s.fvh") + 1;
		int byteLength4 = strlen("mods\\session\\*.fvh") + 1;

		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			//New factory vehicle lookup dir path
			CodePatch genericCodePatch5 = { 0x006D79B2,"","mods\\session\\%s.fvh",byteLength3,false }; genericCodePatch5.Apply(true);
			CodePatch genericCodePatch7 = { 0x004A8D75,"","\x68\xB2\x79\x6D",4,false }; genericCodePatch7.Apply(true);
			
			//vehicles\\*.fvh
			CodePatch genericCodePatch4 = { 0x006D79EE,"","mods\\session\\*.fvh",byteLength4,false }; genericCodePatch4.Apply(true);
			CodePatch genericCodePatch8 = { 0x004F514A,"","\xBD\xEE\x79\x6D",4,false }; genericCodePatch8.Apply(true);
			
			//New vehicle lookup dir path
			CodePatch genericCodePatch0 = { 0x006D7900,"",pathExt,byteLength,false }; genericCodePatch0.Apply(true);
			
			//New vehicle lookup dir path PTR ("vehicles\\*.veh")
			CodePatch genericCodePatch1 = { 0x004F5143,"","\xBD\x00\x79\x6D",4,false }; genericCodePatch1.Apply(true);
			
			//new vehicle save dir path
			CodePatch genericCodePatch2 = { 0x006D793A,"",pathExt2,byteLength+4,false }; genericCodePatch2.Apply(true);
			
			//new vehicle save dir path PTR ("vehicles\\%s")
			CodePatch genericCodePatch1a = { 0x0055C24E,"","\x68\x3A\x79\x6D",4,false }; genericCodePatch1a.Apply(true);
			CodePatch genericCodePatch1b = { 0x0046ADA1,"","\x68\x3A\x79\x6D",4,false }; genericCodePatch1b.Apply(true);
			CodePatch genericCodePatch3 = { 0x004F38CD,"","\x68\x3A\x79\x6D",4,false }; genericCodePatch3.Apply(true);
			
			//New vehicle delete dir path
			CodePatch genericCodePatch9 = { 0x006D7976,"",pathExt3,byteLength+7,false }; genericCodePatch9.Apply(true);
			
			//New vehicle delete dir path PTR ("vehicles\\%s.veh")
			CodePatch genericCodePatch1c = { 0x004F542F,"","\x68\x76\x79\x6D",4,false }; genericCodePatch1c.Apply(true);
		}
		else
		{
			//New factory vehicle lookup dir path
			CodePatch genericCodePatch5 = { 0x006C788F,"","mods\\session\\%s.fvh",byteLength3,false }; genericCodePatch5.Apply(true);
			CodePatch genericCodePatch7 = { 0x004A6A59,"","\x68\x8F\x78\x6C",4,false }; genericCodePatch7.Apply(true);
			
			//vehicles\\*.fvh
			CodePatch genericCodePatch4 = { 0x006C78F3,"","mods\\session\\*.fvh",byteLength4,false }; genericCodePatch4.Apply(true);
			CodePatch genericCodePatch8 = { 0x004F2CB2,"","\xBD\xF3\x78\x6C",4,false }; genericCodePatch8.Apply(true);
			
			//New vehicle lookup dir path
			CodePatch genericCodePatch0 = { 0x006C7957,"",pathExt,byteLength,false }; genericCodePatch0.Apply(true);
			
			//New vehicle lookup dir path PTR ("vehicles\\*.veh")
			CodePatch genericCodePatch1 = { 0x004F2CAB,"","\xBD\x57\x79\x6C",4,false }; genericCodePatch1.Apply(true);
			
			//new vehicle save dir path
			CodePatch genericCodePatch2 = { 0x006C79BB,"",pathExt2,byteLength+4,false }; genericCodePatch2.Apply(true);
			
			//new vehicle save dir path PTR ("vehicles\\%s")
			CodePatch genericCodePatch1a = { 0x00559412,"","\x68\xBB\x79\x6C",4,false }; genericCodePatch1a.Apply(true);
			CodePatch genericCodePatch1b = { 0x0046937D,"","\x68\xBB\x79\x6C",4,false }; genericCodePatch1b.Apply(true);
			CodePatch genericCodePatch3 = { 0x004F1435,"","\x68\xBB\x79\x6C",4,false }; genericCodePatch3.Apply(true);
			
			//New vehicle delete dir path
			CodePatch genericCodePatch9 = { 0x006C7A1F,"",pathExt3,byteLength+7,false }; genericCodePatch9.Apply(true);
			
			//New vehicle delete dir path PTR ("vehicles\\%s.veh")
			CodePatch genericCodePatch1c = { 0x004F2F97,"","\x68\x1F\x7A\x6C",4,false }; genericCodePatch1c.Apply(true);
		}
		//Console::echo(pathExt);
		return "true";
	}

	BuiltInFunction("getSHA1", _gsha1) {

		if (argc != 1)
		{
			Console::echo("%s( file, [string] );", self);
			return 0;
		}
		if (!isFile(argv[0]))
		{
			Console::echo("%s: File not found.", self);
			Console::echo(argv[0]);
			return 0;
		}
		string path = argv[0];
		string ext = path.substr(path.size() - 4, path.size());
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot read files outside of the Starsiege directories.");
			return "false";
		}
		//char* fileExt[] = { ".cs", ".mis", ".dts", ".wav", ".bmp", ".vol", ".smk", ".gui", ".pba", ".pft", ".veh", ".fvh", ".div", ".dil", ".dig", ".mlv" };
		//for (int iter = 0; iter <= sizeof(fileExt); iter++)
		//{
		//	Console::echo("Disallowed file extension.");
		//	return "false";
		//}
		return SHA1::from_file(path).c_str();
	}

	BuiltInFunction("getFileSize", _gfs) {
		if (argc != 1)
		{
			Console::echo("%s( file );", self);
			return 0;
		}
		if (!isFile(argv[0]))
		{
			Console::echo("getFileSize: File not found.");
			return 0;
		}
		string path = argv[0];
		string ext = path.substr(path.size() - 4, path.size());
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot read files that are located outside of the Starsiege directory.");
			return "false";
		}
		//std::ifstream inputFile(path, std::ios::in);

		ifstream in_file(argv[0], ios::binary);
		in_file.seekg(0, ios::end);
		int file_size = in_file.tellg();
		return tostring(file_size);
	}

	BuiltInFunction("resetScriptFile", _rcsf) {
		if (argc != 1)
		{
			Console::echo("%s( fileName.cs );", self);
			return "false";
		}
		string fileName = argv[0];
		string ext = fileName.substr(fileName.size() - 3, fileName.size());
		if (ext != ".cs")
		{
			Console::echo("Disallowed file extension.");
			return "false";
		}
		std::ofstream file;
		file.open(argv[0], std::ofstream::out | std::ofstream::trunc);
		if (file.is_open())
		{
			file.close();
		}
		return 0;
	}

	BuiltInFunction("fileWriteHex", _fwh) {
		if (argc != 2)
		{
			Console::echo("fileWriteHex(filename, hex_string[0123456789ABCDEF]);");
			return "false";
		}
		string path = argv[0];
		string ext = path.substr(path.size() - 4, path.size());
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot write files outside of the Starsiege directories.");
			return "false";
		}
		// Input
		std::string hex = argv[1]; // (or longer)

		std::basic_string<uint8_t> bytes;

		// Iterate over every pair of hex values in the input string (e.g. "18", "0f", ...)
		for (size_t i = 0; i < hex.length(); i += 2)
		{
			uint16_t byte;

			// Get current pair and store in nextbyte
			std::string nextbyte = hex.substr(i, 2);

			// Put the pair into an istringstream and stream it through std::hex for
			// conversion into an integer value.
			// This will calculate the byte value of your string-represented hex value.
			std::istringstream(nextbyte) >> std::hex >> byte;

			// As the stream above does not work with uint8 directly,
			// we have to cast it now.
			// As every pair can have a maximum value of "ff",
			// which is "11111111" (8 bits), we will not lose any information during this cast.
			// This line adds the current byte value to our final byte "array".
			bytes.push_back(static_cast<uint8_t>(byte));
		}

		// we are now generating a string obj from our bytes-"array"
		// this string object contains the non-human-readable binary byte values
		// therefore, simply reading it would yield a String like ".0n..:j..}p...?*8...3..x"
		// however, this is very useful to output it directly into a binary file like shown below
		std::string result(begin(bytes), end(bytes));

		//Then you can simply write this string to a file like this:

		std::ofstream output_file(argv[0], std::ios::app | std::ios::binary | std::ios::out);
		if (output_file.is_open())
		{
			output_file << result;
			output_file.close();
		}
		else
		{
			Console::echo("Error could not create file.");
			return "false";
		}
		return "true";
	}

	BuiltInFunction("modloader::uploadFiletoClient", _mluftc) {
		if (argc != 3 || !strlen(argv[0]) || !strlen(argv[1]) || !strlen(argv[2]))
		{
			Console::echo("%s( file, playerID, token);", self);
			return 0;
		}

		if (!isFile(argv[0]))
		{
			Console::echo("%s: File not found.", self);
			return 0;
		}

		string path = argv[0];
		string ext = path.substr(path.size() - 4, path.size());

		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot read files outside of the Starsiege directories.");
			return "false";
		}
		unsigned char x;
		std::ifstream fin(argv[0], std::ios::binary);
		std::stringstream buffer;
		fin >> std::noskipws;

		while (!fin.eof()) {
			fin >> x;
			buffer << std::hex << std::setw(2) << std::setfill('0') << static_cast<int>(x);
		}

		const std::string tmp = buffer.str();
		const std::string tmp_trim = tmp.substr(0, tmp.length() - 1);
		int counter = 0;
		float throttle = 0.000;
		string data = tmp.substr(counter, 254);

		while (counter <= tmp_trim.size())
		{
			string data1 = tmp_trim.substr(counter, 700); //Start at index 0 and increment by 254 each loop
			char eval[900];
			//sprintf(eval, "schedule('modloader::parseFileData(%s,\"%s\",\"%s\",\"%s\");',%s);", argv[1], argv[0], data1.c_str(), argv[2], tostring(throttle += 0.015));
			sprintf(eval, "modloader::parseFileData(%s, '%s','%s','%s');", argv[1], argv[0], data1.c_str(), argv[2]);
			Console::eval(eval);
			counter += 700;


		}
		return 0;
	}

	BuiltInFunction("removeCacheFile", _rcf) {
		if (argc != 1)
		{
			Console::echo("%s( file );", self);
			return 0;
		}
		string path = argv[0];
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot remove files that are located outside of the Starsiege directory.");
			return "false";
		}
		char cachePath[MAX_PATH];
		strcpy(cachePath, "mods\\");
		strcat(cachePath, argv[0]);
		if (!isFile(cachePath))
		{
			Console::echo("%s: File not found.", self);
			return 0;
		}
		std::remove(cachePath);
		return "true";
	}

	struct Init {
	Init() {
		//grabdepotvehiclename.DoctorRelative((u32)GrabDepotVehicleName, 1).Apply(true);
		}
	} init;
}

namespace serverDataHandler {



	//Convert a file to hexedecimal
	BuiltInFunction("fileToHex", _fth) {
		if (argc != 1)
		{
			Console::echo("%s( file );", self);
			return 0;
		}
		if (!isFile(argv[0]))
		{
			Console::echo("File not found.");
			return 0;
		}
		string path = argv[0];
		string ext = path.substr(path.size() - 4, path.size());
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot read files outside of the Starsiege directories.");
			return "false";
		}
		unsigned char x;

		std::fstream fin(argv[0], std::ios::binary);

		std::stringstream buffer;
		fin >> std::noskipws;
		while (!fin.eof()) {
			fin >> x;
			buffer << std::hex << std::setw(2) << std::setfill('0') << static_cast<int>(x);
		}

		const std::string tmp = buffer.str();
		const char* cstr = tmp.c_str();

		if (strlen(cstr) > 500000) //245KB (Buffer) Don't allow over 245KB else we encounter a buffer overflow and crash
		{
			Console::echo("Unable to convert file. File exceeds 245KB.");
			Console::echo("Note:: Echo'ing the return value WILL cause a buffer overflow in the console and crash the game. Use String::getsubstr to return portions.");
			return 0;
		}
		return cstr;
	}

	BuiltInFunction("fileHasConstructors", _fhc) {
		if (argc != 1 || !strlen(argv[0]))
		{
			Console::echo("fileHasConstructors( file ); \\Scans a file for vehicle|component|weapon|projectile constructors and returns a boolean", self);
			return 0;
		}
		if (!isFile(argv[0]))
		{
			Console::echo("fileHasConstructors: File not found.");
			return 0;
		}
		string path = argv[0];
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot read files outside of the Starsiege directories.");
			return "false";
		}

		//Don't read the file as binary
		std::ifstream fin(argv[0], std::ios::in);
		//Find functions that require a client reload
		std::vector<std::string> constructors{ "newHerc", "newTank", "newFlyer", "newDrone", "newTurret", "newBullet", "newMissile", "newEnergy", "newBeam", "newMine", "newBomb", "newWeapon",
			"newSensor", "newReactor", "newShield", "newEngine", "newComputer", "newECM", "newThermal", "newCloak", "newModulator", "newCapacitor", "newAmplifier", "newMountable",
			"newBooster", "newRepair", "newBattery", "newArmor", "newHardPoint", "newMountPoint", "newComponent", "newConfiguration", "hardPointSpecial" };

		// Read complete file
		std::string fileContent(std::istreambuf_iterator<char>(fin), {});

		// Search for the lookup strings
		for (const std::string& l : constructors)
		{
			if (std::search(fileContent.begin(), fileContent.end(), l.begin(), l.end()) != fileContent.end())
			{
				return "true";
			}
		}
		return "false";
	}

	BuiltInFunction("fileHasPilotData", _fhpd) {
		if (argc != 1 || !strlen(argv[0]))
		{
			Console::echo("fileHasPilotData( file ); \\\\Checks a file to see if it contains pilot definitions. Returns a boolean.", self);
			return 0;
		}
		if (!isFile(argv[0]))
		{
			Console::echo("fileHasPilotData: File not found.");
			return 0;
		}
		string path = argv[0];
		if (path.find(":") != -1 || path.find("..") != -1)
		{
			Console::echo("Cannot read files outside of the Starsiege directories.");
			return "false";
		}

		//Don't read the file as binary
		std::ifstream fin(argv[0], std::ios::in);
		std::vector<std::string> constructors{ "pilot\x20", "Pilot\x20", "skill\x20=", "skill=" };

		// Read complete file
		std::string fileContent(std::istreambuf_iterator<char>(fin), {});

		// Search for the lookup strings
		for (const std::string& l : constructors)
		{
			if (std::search(fileContent.begin(), fileContent.end(), l.begin(), l.end()) != fileContent.end())
			{
				return "true";
			}
		}
		return "false";
	}

	BuiltInFunction("Nova::purgeTedFiles", _novapurgetedfiles) {
		struct dirent* entry;
		DIR* dir = opendir(".");

		if (dir == NULL) {
			return 0;
		}

		std::stringstream fileListing;
		int counter = 0;
		while ((entry = readdir(dir)) != NULL) {
			std::string fileName = entry->d_name;
			if (fileName.find("terrain.dat") != -1 || fileName.find(".grid.dat") != -1 || fileName.find(".terrain.dml") != -1)
			{
				std::remove(entry->d_name);
			}
		}
		closedir(dir);
		Console::eval("appendSearchPath();");
		return "true";
	}
}
