#include "Fear.h"
#include "Console.h"
#include "Windows.h"
#include "Patch.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "Strings.h"
#include <sstream>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <iterator>
#include <filesystem>
#include <dirent.h>
#include "VersionSnoop.h"
#include <bitset>

using namespace std;
using namespace Fear;

namespace conversionFunctions
{
	//Big endian to little endian
	string BEtoLE(string& str)
	{
		string it;
		std::reverse(str.begin(), str.end());
		for (auto it = str.begin(); it != str.end(); it += 2) {
			std::swap(it[0], it[1]);
		}
		return it;
	}

	char* hexToASCII3(char* input)
	{
		// initialize the ASCII code string as empty.
		char buffer[MAX_PATH];
		strcpy(buffer, "00");
		strcat(buffer, input);
		string hex = buffer;
		string ascii = "";
		for (size_t i = 0; i < hex.length(); i += 2)
		{
			// extract two characters from hex string
			string part = hex.substr(i, 2);

			// change it into base 16 and
			// typecast as the character
			char ch = stoul(part, nullptr, 16);

			// add this char to final ASCII string
			ascii += part;
		}
		char* ascii_cstr = const_cast<char*>(ascii.c_str());
		//char* ascii_cstr_ = const_cast<char*>(ascii_cstr);
		free(buffer);
		return ascii_cstr;
	}

	string hexToASCII(string input)
	{
		int length = input.length();
		string result;
		for (int i = 0; i < length; i += 2)
		{
			string byte = input.substr(i, 2);
			char chr = (char)(int)strtol(byte.c_str(), NULL, 16);
			result.push_back(chr);
		}
		char* ascii = const_cast<char*>(result.c_str());
		return ascii;
	}

	string hexToASCII2(std::string hex) {
		stringstream ss;
		for (size_t i = 0; i < hex.length(); i += 2) {
			unsigned char byte = stoi(hex.substr(i, 2), nullptr, 16);
			ss << byte;
		}
		return ss.str();
	}

	char* hex2char(char* hexString = "00")
	{
		const std::string hex = hexString;
		std::basic_string<uint8_t> bytes;
		for (size_t i = 0; i < hex.length(); i += 2)
		{
			uint16_t byte;
			std::string nextbyte = hex.substr(i, 2);
			std::istringstream(nextbyte) >> std::hex >> byte;
			bytes.push_back(static_cast<uint8_t>(byte));
		}
		std::string result(begin(bytes), end(bytes));
		//Escaped hex strings in local variables dont parse correctly with CodePatch so they are passed pre-parsed
		char* rawHexString = const_cast<char*>(result.c_str());
		return rawHexString;
	}

	char* int2hex(int input = 0, int endian = 0)
	{
		char hex_string[MAX_PATH];
		char bit[2] = "0";
		sprintf(hex_string, "%X", input);
		if (strlen(hex_string) % 2 != 0)
		{
			if (endian == 1)
			{
				std::string final_input = strcat(bit, hex_string);
				const char* result = BEtoLE(final_input).c_str();
				char* output = const_cast<char*>(result);
				return(output);
			}
		}
		return hex_string;
	}

	char* flt2hex(float input = 0.00, int type = 0)
	{
		const unsigned char* pf = reinterpret_cast<const unsigned char*>(&input);

		char hexString[MAX_PATH];
		if (type == 0)
		{
			strcpy(hexString, int2hex(pf[3], 1));
			strcat(hexString, int2hex(pf[2], 1));
			strcat(hexString, int2hex(pf[1], 1));
			strcat(hexString, int2hex(pf[0], 1));
		}
		else
		{
			strcpy(hexString, int2hex(pf[0], 1));
			strcat(hexString, int2hex(pf[1], 1));
			strcat(hexString, int2hex(pf[2], 1));
			strcat(hexString, int2hex(pf[3], 1));
		}
		return hexString;
	}
}