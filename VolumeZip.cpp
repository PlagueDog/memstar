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
#include "zlib/unzip.h"
#include "zlib/zip.h"
u32 dummy, dummy2;
//MultiPointer(ptrNewObjectFunc, 0, 0, 0, 0x0059F92C);
MultiPointer(ptrhInstance, 0, 0, 0, 0x00400000);
MultiPointer(ptrNewObjectFunc, 0, 0, 0, 0x0059F92C);
MultiPointer(ptrConsoleExecute, 0, 0, 0x00712b34, 0x00722FA4);

using namespace std;
using namespace Fear;

NAKED void newobject(const char* args, ...) {
	__asm {
		pop[dummy]
		push dword ptr ds : [ptrConsoleExecute]
		call[ptrNewObjectFunc]
		add esp, 4
		jmp[dummy]
	}
}

bool _isFile(const char* file)
{
	std::ifstream inputFile(file, std::ios::in);

	if (inputFile.is_open())
	{
		inputFile.close();
		return true;
	}
	return false;
}

//BuiltInFunction("testloadzip", _tlz) {
//	newobject(argv[0], argv[1], argv[2]);
//	return 0;
//}

#define WRITEBUFFERSIZE (4.5e+7) // 45MB buffer
string readZipFile(string zipFile, string fileInZip) {
	int err = UNZ_OK;                 // error status
	uInt size_buf = WRITEBUFFERSIZE;  // byte size of buffer to store raw csv data
	void* buf;                        // the buffer  
	string sout;                      // output strings
	char filename_inzip[256];         // for unzGetCurrentFileInfo
	unz_file_info file_info;          // for unzGetCurrentFileInfo   

	unzFile uf = unzOpen(zipFile.c_str()); // open zipfile stream
	if (uf == NULL) {
		Console::echo("Cannot open zip file.");
			return 0;
	} // file is open

	if (unzLocateFile(uf, fileInZip.c_str(), 1)) { // try to locate file inside zip
		// second argument of unzLocateFile: 1 = case sensitive, 0 = case-insensitive
		Console::echo("Unable to locate %s in zip file.", fileInZip.c_str());
			return 0;
	} // file inside zip found

	if (unzGetCurrentFileInfo(uf, &file_info, filename_inzip, sizeof(filename_inzip), NULL, 0, NULL, 0)) {
		Console::echo("Unable to get file info of %s inside zip file %s.", fileInZip.c_str(), zipFile.c_str());
			return 0;
	} // obtained the necessary details about file inside zip

	buf = (void*)malloc(size_buf); // setup buffer
	if (buf == NULL) {
		Console::echo("Error allocating memory for read buffer");
		return 0;
	} // buffer ready

	err = unzOpenCurrentFilePassword(uf, NULL); // Open the file inside the zip (password = NULL)
	if (err != UNZ_OK) {
		cerr << "Error " << err << " with zipfile " << zipFile << " in unzOpenCurrentFilePassword." << endl;
		return 0;
	} // file inside the zip is open

	// Copy contents of the file inside the zip to the buffer
	Console::echo("Extracting: %s from %s into memory buffer.", filename_inzip, zipFile.c_str());
	do {
		err = unzReadCurrentFile(uf, buf, size_buf);
		if (err < 0) {
			Console::echo("Error with zipfile in unzReadCurrentFile");
			sout = ""; // empty output string
			break;
		}
		// copy the buffer to a string
		if (err > 0) for (int i = 0; i < (int)err; i++) sout.push_back(*(((char*)buf) + i));
	} while (err > 0);

	//err = unzCloseCurrentFile(uf);  // close the zipfile
	//if (err != UNZ_OK) {
	//	Console::echo("Error with zipfile in unzCloseCurrentFile");
	//	sout = ""; // empty output string
	//}

	free(buf); // free up buffer memory
	return sout;
}

BuiltInFunction("loadZipFile", _loadZipFile) {
	Console::echo("debug0");
	unzFile uf = unzOpen(argv[0]); // open zipfile stream
	if (uf == NULL) {
		Console::echo("Cannot open zip file.");
		return 0;
	} // file is open
	int err = UNZ_OK;                 // error status
	uInt size_buf = WRITEBUFFERSIZE;  // byte size of buffer to store raw csv data
	void* buf;                        // the buffer  
	string sout;                      // output strings
	char filename_inzip[256];         // for unzGetCurrentFileInfo
	unz_file_info file_info;          // for unzGetCurrentFileInfo   

	HZIP zipFile = OpenZip(argv[0], 0);
	//unzFile uf = unzOpen(argv[0]); // open zipfile stream
	ZIPENTRY nestedFile; GetZipItem(zipFile, -1, &nestedFile);
	int numitems = nestedFile.index;
	Console::echo("debug1");
	for (int i = 0; i < numitems; i++)
	{
		ZIPENTRY nestedFile;
		GetZipItem(zipFile, i, &nestedFile);
		//readZipFile(argv[0], nestedFile.name);
		//char* buffer = new char[nestedFile.unc_size]; // Memory to load the file into
		//UnzipItem(zipFile, i, buffer, NULL);
		//delete[] buffer;

		if (unzLocateFile(uf, nestedFile.name, 0)) { // try to locate file inside zip
			// second argument of unzLocateFile: 1 = case sensitive, 0 = case-insensitive
			Console::echo("Unable to locate %s in zip file.", nestedFile.name);
			return 0;
		} // file inside zip found

		if (unzGetCurrentFileInfo(uf, &file_info, filename_inzip, sizeof(filename_inzip), NULL, 0, NULL, 0)) {
			Console::echo("Unable to get file info of %s inside zip file %s.", nestedFile.name, argv[0]);
			return 0;
		} // obtained the necessary details about file inside zip

		buf = (void*)malloc(size_buf); // setup buffer
		if (buf == NULL) {
			Console::echo("Error allocating memory for read buffer");
			return 0;
		} // buffer ready

		err = unzOpenCurrentFilePassword(uf, NULL); // Open the file inside the zip (password = NULL)
		if (err != UNZ_OK) {
			Console::echo("Error %s with zipfile %s in unzOpenCurrentFilePassword.", err, argv[0]);
			return 0;
		} // file inside the zip is open

		// Copy contents of the file inside the zip to the buffer
		Console::echo("Extracting: %s from %s into memory buffer.", filename_inzip, argv[0]);
		do {
			err = unzReadCurrentFile(uf, buf, size_buf);
			if (err < 0) {
				Console::echo("Error with zipfile in unzReadCurrentFile");
				sout = ""; // empty output string
				break;
			}
			// copy the buffer to a string
			if (err > 0) for (int i = 0; i < (int)err; i++) sout.push_back(*(((char*)buf) + i));
		} while (err > 0);

		//err = unzCloseCurrentFile(uf);  // close the zipfile
		//if (err != UNZ_OK) {
		//	Console::echo("Error with zipfile in unzCloseCurrentFile");
		//	sout = ""; // empty output string
		//}

		//free(buf); // free up buffer memory
	}
	Console::echo("debugEND");
		return "true";
	//Console::eval("appendSearchPath();");//Update the file list else Starsiege won't 'see' the newly loaded zip files
	CloseZip(zipFile);
	return "true";
}

HWND getProcessHWND() {
	MultiPointer(ptrHWND, 0, 0, 0x00705C5C, 0x007160CC);
	uintptr_t HWND_PTR = ptrHWND;
	int GAME_HWND = *reinterpret_cast<int*>(HWND_PTR);
	HWND SS_HWND = reinterpret_cast<HWND>(GAME_HWND);
	return SS_HWND;
}

BuiltInFunction("loadZipFile3", _loadZipFile3) {
	std::stringstream HWND_HEXED;
	HWND_HEXED << std::hex << getProcessHWND();
	HMODULE hInstance = GetModuleHandleA(HWND_HEXED.str().c_str());
	Console::echo("%s", HWND_HEXED.str().c_str());
	if (!strlen(argv[0]) || !_isFile(argv[0]))
	{
		Console::echo("File not found");
		return 0;
	}
	HZIP hz = OpenZip(argv[0], 0);
	ZIPENTRY ze;
	GetZipItem(hz,-1,&ze);
	int numitems=ze.index;
	for (int i=0; i<numitems; i++)
	{
		GetZipItem(hz,i,&ze);
		UnzipItem(hz,i, ze.name);
	}
	return "true";
}