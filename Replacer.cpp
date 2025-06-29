#include "Replacer.h"
#include "HashTable.h"
#include "FileSystem.h"
#include "Console.h"
#include "OpenGL.h"
#include "Patch.h"
#include "Callback.h"
#include "Terrain.h"
#include "OpenGL_Pointers.h"
#include <xstring>
#include <string>

using namespace std;

namespace Replacer {

#include "Originals.h"

	MultiPointer(ptr_OPENGL_FLUSH_TEXTURE_VFT, 0, 0, 0x0071a15c, 0x0072A748);

	BuiltInVariable("pref::ShowMatchedTextures", bool, prefShowMatchedTextures, false);
	BuiltInVariable("pref::echoMatchedTextures", bool, prefEchoMatchedTextures, false);

	typedef HashTable< String, TextureWithMips*, IKeyCmp, ValueDeleter<String, TextureWithMips*> > ReplacementHash;
	typedef HashTable< u32, String > OriginalHash;

	bool lastScanMatched = false;
	bool foundLastCRC = false;
	bool ignoreTexImageMips = false;
	bool ignoreTexSubImageMips = false;
	bool lastNonMipMatched = false;

	u32 lastFoundCRC = 0;
	TextureWithMips* lastMatchedTexture = NULL, * lastUsedTexture = NULL;
	FileSystem mFiles(128);
	ReplacementHash mTextures(128);
	OriginalHash mOriginals(4096);

	void Forget() {
		lastScanMatched = false;
		lastMatchedTexture = NULL;
		foundLastCRC = false;
	}

	TextureWithMips* LastMatchedTexture() {
		return (lastScanMatched) ? lastMatchedTexture : NULL;
	}

	bool LastScanWasReplaceable() {
		return foundLastCRC;
	}

	TextureWithMips* FindReplacement(const String* name) {
		// allowed to pass null
		if (!name)
			return (NULL);

		TextureWithMips** find = mTextures.Find(*name);
		if (find)
			return *find;

		TextureWithMips* texture = new TextureWithMips();
		if (!texture || !mFiles.ReadTGA(*name, *texture) || !mTextures.Insert(*name, texture)) {
			delete texture;
			texture = NULL;
		}

		return texture;
	}

	const String* FindOriginalName(u32 texture_crc) {
		return mOriginals.Find(texture_crc);
	}

	void Open() {
		mFiles.Clear();
		mTextures.Clear();
		mFiles.Grok("mods/Replacements");
	}

	char* resourceManagerAsset;
	MultiPointer(ptrResourceManagerFind, 0, 0, 0, 0x00572D5C);
	MultiPointer(ptrResourceManagerFindResume, 0, 0, 0, 0x00572D62);
	//Intercept ResourceManager::find to grab the file name that is being loaded and feed it to Scan
	CodePatch resourcemanagerloadasset = { ptrResourceManagerFind, "", "\xE9RMAL", 5, false };
	NAKED void ResourceManagerLoadAsset() {
		__asm {
			push ebx
			add esp, 0xFFFFFFF8
			mov ebx, eax
			mov resourceManagerAsset, edx
			jmp ptrResourceManagerFindResume
		}
	}

	void Scan(Fear::GFXBitmap* bmp) {
		if (Terrain::TexImageCheck(bmp->bitmapData))
			return;

		lastFoundCRC = HashBytes(bmp->bitmapData, bmp->width * bmp->height);

		//std::string ForceCapturing = Console::getVariable("Nova::ForceCapturing");
		//if (ForceCapturing.compare("true") == 0 && lastFoundCRC != NULL)
		//{
		//	char function[MAX_PATH];
		//	sprintf(function, "Memstar::addReplacement(\"%x\",\"temp.tga\");", lastFoundCRC);
		//	Console::eval(function);
		//}

		const String* file = FindOriginalName(lastFoundCRC);
		if (file != NULL && prefEchoMatchedTextures)
		{
			Console::echo("Matched texture %s [%x]", file->c_str(), lastFoundCRC);
		}
			//std::string loadedAsset = resourceManagerAsset;

		//if (loadedAsset.length() >= 4)
		//{
		//	if (!file && loadedAsset.substr(loadedAsset.length() - 4, 4).compare(".bmp") > 0)
		//	{
		//		char* tgaImage = const_cast<char*>(loadedAsset.replace(loadedAsset.length() - 3, 3, "tga").c_str());
		//		Console::echo("Replacer: Adding %s(hash:%x)", tgaImage, lastFoundCRC);
		//		mOriginals.Insert(lastFoundCRC, String2(tgaImage));
		//	}
		//}

			std::string capturing = Console::getVariable("Nova::capturingTextureCRC");
			if (capturing.compare("true") == 0 && file == NULL)
			{
				//Do not run if the texture dimensions are greater than 256 as those textures are 'chunked' into seperate 256x256 textures and the replacer does not work with those
				if (bmp->width <= 256 && bmp->height <= 256)
				{
					//Console::echo("texture %x", lastFoundCRC);
					char outputCRC[MAX_PATH];
					char crchash[MAX_PATH];
					sprintf(crchash, "%x", lastFoundCRC);

					//Check for bad CRC, preppend zeros if it is bad
					if (strlen(crchash) == 4)
					{
						sprintf(crchash, "0000%x", lastFoundCRC);
						Console::setVariable("Nova::textureCRC", crchash);
						sprintf(crchash, "Memstar::addReplacement(\"0000%x\",\"%s\");", lastFoundCRC, Console::getVariable("Nova::textureName"));
					}
					else if (strlen(crchash) == 5)
					{
						sprintf(crchash, "000%x", lastFoundCRC);
						Console::setVariable("Nova::textureCRC", crchash);
						sprintf(crchash, "Memstar::addReplacement(\"000%x\",\"%s\");", lastFoundCRC, Console::getVariable("Nova::textureName"));
					}
					else if (strlen(crchash) == 6)
					{
						sprintf(crchash, "00%x", lastFoundCRC);
						Console::setVariable("Nova::textureCRC", crchash);
						sprintf(crchash, "Memstar::addReplacement(\"00%x\",\"%s\");", lastFoundCRC, Console::getVariable("Nova::textureName"));
					}
					else if (strlen(crchash) == 7)
					{
						sprintf(crchash, "0%x", lastFoundCRC);
						Console::setVariable("Nova::textureCRC", crchash);
						sprintf(crchash, "Memstar::addReplacement(\"0%x\",\"%s\");", lastFoundCRC, Console::getVariable("Nova::textureName"));
					}
					else
					{
						Console::setVariable("Nova::textureCRC", crchash);
						sprintf(crchash, "Memstar::addReplacement(\"%x\",\"%s\");", lastFoundCRC, Console::getVariable("Nova::textureName"));
					}


					Console::setVariable("Nova::textureHashFunction", crchash);
					Console::setVariable("Nova::capturingTextureCRC", 0);
				}
			}

		foundLastCRC = (file != NULL);
		lastMatchedTexture = FindReplacement(file);
		lastScanMatched = (lastMatchedTexture != NULL);
	}

	BuiltInFunction("Memstar::AddReplacement", AddReplacement) {
		if (argc != 2) {
			Console::echo("%s( <texture crc>, <texture name> );", self);
		}
		else {
			u32 crc;
			if (atohhl(argv[0], &crc)) {
				const String* find = FindOriginalName(crc);
				if (find) {
					Console::echo("Replacer: Duplicate CRC! Already bound to %s", find->c_str());
				}
				else {
					Console::echo("Replacer: Adding %s(hash:%x)", argv[1], crc);
					char hashTable[MAX_PATH];
					sprintf(hashTable, "Nova::addReplacement('%x','%s');", crc, argv[1]);
					Console::eval(hashTable);
					mOriginals.Insert(crc, String2(argv[1]));
				}
			}
			else
			{
				Console::echo("Replacer: Failed to add CRC! (image:%s) (hash:%x)", argv[1], crc);
				return 0;
			}
		}

		return "true";
	}

	MultiPointer(ptrCmdLineJoinCursorOn, 0, 0, 0, 0x0053F7CF);
	CodePatch autocursordisable = { ptrCmdLineJoinCursorOn, "", "\xEB\x20", 2, false };
	BuiltInFunction("Memstar::disableCursorAutoOn", memstardisablecursorautoon)
	{
		autocursordisable.Apply(true);
		return "true";
	}


	BuiltInFunction("Nova::openHashDirectory", _novaopenhashdirectory) {
		if (argc >= 1)
		{
			return "true";
			exit;
		}
		ShellExecute(0, "explore", ".\\mods\\textureHasher", NULL, NULL, SW_SHOWNORMAL);
		return "true";
	}

	//BuiltInFunction("Nova::openHashOutput", _novaopenhashoutput) {
	//
	//	if (argc >= 1)
	//	{
	//		return "true";
	//		exit;
	//	}
	//	ShellExecuteA(0, "edit", ".\\mods\\textureHasher\\textureHasherOutput.cs", NULL, NULL, SW_SHOWDEFAULT);
	//	return "true";
	//}

	bool SetClipboardText(const std::string& text) {
		if (!OpenClipboard(nullptr)) {
			return false;
		}
		EmptyClipboard();
		HGLOBAL hGlobal = GlobalAlloc(GMEM_MOVEABLE, text.size() + 1);
		if (!hGlobal) {
			CloseClipboard();
			return false;
		}
		char* data = static_cast<char*>(GlobalLock(hGlobal));
		if (!data) {
			CloseClipboard();
			GlobalFree(hGlobal);
			return false;
		}
		strcpy_s(data, text.size() + 1, text.c_str());
		GlobalUnlock(hGlobal);
		SetClipboardData(CF_TEXT, hGlobal);
		CloseClipboard();
		return true;
	}

	BuiltInFunction("Nova::copyToClipboard", _novacopytoclipboard) {

		if (argc != 1)
		{
			Console::echo("%s( string );", self);
			return 0;
		}
		SetClipboardText(argv[0]);
		return "true";
	}

	u32 fnglTexImage2D, fnglTexSubImage2D, fnFlushTexture;

	MultiPointer(fnResumeCacheBitmap, 0, 0, 0x0063e71a, 0x0064D822);
	MultiPointer(fnIsFromCurrentRound, 0, 0, 0x0063dedc, 0x0064CFAC);

	MultiPointer(ptr_cacheBitmapPatch, 0, 0, 0x0063e714, 0x0064D81C);
	CodePatch cacheBitmapPatch = {
		ptr_cacheBitmapPatch,
		"",
		"\xE9tccb",
		5,
		false
	};

	NAKED void OnCacheBitmap() {
		__asm {
			pushad

			mov edi, ecx
			mov esi, edx
			mov ebx, eax
			mov edx, edi
			mov eax, ebx
			call[fnIsFromCurrentRound]
			test al, al
			jnz __ignore

			push esi
			call Scan
			add esp, 4

			__ignore:
			popad

				push ebp
				mov ebp, esp
				add esp, -0x18
				jmp[fnResumeCacheBitmap]
		}
	}

	NAKED void OnFlushTexture() {
		__asm {
			pushad

			mov edi, edx
			mov ebx, eax
			mov edx, edi
			mov eax, [ebx + 0x158]
			call[fnIsFromCurrentRound]
			test al, al
			jz __ignore

			push dword ptr[edi + 0x38]
			call Terrain::TexSubImageCheck
			add esp, 4

			__ignore:
			popad
				jmp[fnFlushTexture]
		}
	}

	void GLAPIENTRY OnGlTexImage2D(GLenum target, GLint level, GLint internalformat, GLsizei width, GLsizei height, GLint border, GLenum format, GLenum type, GLvoid* pixels) {
		typedef void (GLAPIENTRY* fn)(GLenum target, GLint level, GLint internalformat, GLsizei width, GLsizei height, GLint border, GLenum format, GLenum type, const GLvoid* pixels);

		bool isMip = (level != 0), needDefault = true;
		if (ignoreTexImageMips && isMip)
			return;

		lastNonMipMatched &= isMip;
		ignoreTexImageMips = false;

		if (!isMip) {
			if (prefShowMatchedTextures && foundLastCRC) {
				static const RGBA colors[16] = {
					RGBA(204,204,204,64),RGBA(255,102,  0,64),RGBA(255, 51,  0,64),RGBA(153,204,  0,64),
					RGBA(255,204,102,64),RGBA(153,153,  0,64),RGBA(255,  0,  0,64),RGBA(153,153, 51,64),
					RGBA(255, 51, 51,64),RGBA(102,153,  0,64),RGBA(153,  0, 51,64),RGBA(102,255, 51,64),
					RGBA(204,153,102,64),RGBA(153,204,102,64),RGBA(102,204,102,64),RGBA(255,153,255,64),
				};
				((fn)fnglTexImage2D)(target, 0, GL_RGBA8, 1, 1, 0, GL_RGBA, GL_UNSIGNED_BYTE, &colors[lastFoundCRC & 0x0f]);
				ignoreTexImageMips = true;
				needDefault = false;
			}
			else if (lastMatchedTexture) {
				((fn)fnglTexImage2D)(target, level, GL_RGBA8, lastMatchedTexture->mWidth, lastMatchedTexture->mHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, lastMatchedTexture->mData);
				lastUsedTexture = lastMatchedTexture;
				lastNonMipMatched = true;
				needDefault = false;
			}
			else {
				RGBA** terrain = Terrain::CheckForTerrainTile();
				if (terrain) {
					u32 mipWidth = width, mipLevel = 0;
					while (mipWidth > 0) {
						if (!*terrain)
							break;
						((fn)fnglTexImage2D)(target, mipLevel, GL_RGBA8, mipWidth, mipWidth, 0, GL_RGBA, GL_UNSIGNED_BYTE, *terrain++);
						mipLevel++;
						mipWidth >>= 1;
					}
					ignoreTexImageMips = true;
					needDefault = false;
				}
			}
		}
		else if (lastNonMipMatched && (level == 1)) {
			lastUsedTexture->GenerateMipMaps();
			for (u32 i = 0; lastUsedTexture->mMipMaps[i]; i++) {
				Texture* mip = lastUsedTexture->mMipMaps[i];
				((fn)fnglTexImage2D)(target, i + 1, GL_RGBA8, mip->mWidth, mip->mHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, mip->mData);
			}
			ignoreTexImageMips = true;
			needDefault = false;
		}

		if (needDefault)
			((fn)fnglTexImage2D)(target, level, internalformat, width, height, border, format, type, pixels);
		Forget();
	}

	void GLAPIENTRY OnGlTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, GLvoid* pixels) {
		typedef void (GLAPIENTRY* fn)(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, const GLvoid* pixels);

		if (ignoreTexSubImageMips) {
			if (level != 0)
				return;
			ignoreTexSubImageMips = false;
		}

		bool needDefault = true;
		RGBA** terrain = Terrain::CheckForTerrainTile();
		if (terrain) {
			u32 mipWidth = width, mipLevel = 0;
			while (mipWidth > 0) {
				if (!*terrain)
					break;
				((fn)fnglTexSubImage2D)(target, mipLevel, xoffset, yoffset, mipWidth, mipWidth, GL_RGBA, GL_UNSIGNED_BYTE, *terrain++);
				mipLevel++;
				mipWidth >>= 1;
			}
			ignoreTexSubImageMips = true;
			needDefault = false;
		}

		if (needDefault)
			((fn)fnglTexSubImage2D)(target, level, xoffset, yoffset, width, height, format, type, pixels);
	}

	void OnStarted(bool state) {
		Open();
		
		for (int i = 0; i < __ORIGINAL_COUNT__; i++) {
			mOriginals.Insert(mOriginalsTable[i].mCrc, String2(mOriginalsTable[i].mName));
		}
		
		fnglTexImage2D = Patch::ReplaceHook((void*)OpenGLPtrs::ptr_OPENGL32_GLTEXIMAGE2D, OnGlTexImage2D);
		fnglTexSubImage2D = Patch::ReplaceHook((void*)OpenGLPtrs::ptr_OPENGL32_GLTEXSUBIMAGE2D, OnGlTexSubImage2D);
		fnFlushTexture = Patch::ReplaceHook((void*)ptr_OPENGL_FLUSH_TEXTURE_VFT, OnFlushTexture);
		
		cacheBitmapPatch.DoctorRelative((u32)&OnCacheBitmap, 1).Apply(true);
	}

	struct Init {
		Init() {
			if (VersionSnoop::GetVersion() == VERSION::vNotGame) {
				return;
			}
			if (VersionSnoop::GetVersion() < VERSION::v001003) {
				return;
			}
			Callback::attach(Callback::OnStarted, OnStarted);
			//resourcemanagerloadasset.DoctorRelative((u32)ResourceManagerLoadAsset, 1).Apply(true);
		}
	} init;

}; // namespace Replacer