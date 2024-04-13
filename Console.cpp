#include "Console.h"
#include "Callback.h"
#include "Strings.h"
#include "VersionSnoop.h"
namespace Console {

	#define CONSOLE_PTR_1004 0x722FA4
	#define CONSOLE_PTR_1003 0x712b34
	u32 dummy, dummy2;

	VariableConstructor* VariableConstructor::mFirst = NULL;
	ConsoleConstructor* ConsoleConstructor::mFirst = NULL;

	typedef List<const char*> StringList;
	typedef StringList::Iterator StringIter;
	StringList variables, functions;

	MultiPointer(ptrConsole, 0, 0, 0x00712b34, 0x00722FA4);

	MultiPointer(fnAddFunction, 0, 0, 0x005e34dc, 0x005E6D80);
	void addFunction(const char* name, void* cb) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				push[cb]
				push 0
				mov ecx, [name]
				xor edx, edx
				mov eax, ds: [CONSOLE_PTR_1004]
				call[fnAddFunction]
			}
		}
		else
		{
			__asm {
				push[cb]
				push 0
				mov ecx, [name]
				xor edx, edx
				mov eax, ds: [CONSOLE_PTR_1003]
				call[fnAddFunction]
			}
		}
	}

	MultiPointer(fnAddVariable, 0, 0, 0x005e3474, 0x005E6D18);
	void addVariable(const char* name, const void* address, const VariableType var_type) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				push[var_type]
				push[address]
				mov ecx, [name]
				xor edx, edx
				mov eax, ds: [CONSOLE_PTR_1004]
				call[fnAddVariable]
			}
		}
		else
		{
			__asm {
				push[var_type]
				push[address]
				mov ecx, [name]
				xor edx, edx
				mov eax, ds: [CONSOLE_PTR_1003]
				call[fnAddVariable]
			}
		}
	}

	MultiPointer(fnDBEcho, 0x005d3a44, 0x005d4b1c, 0x005e2900, 0x005e61a4);
	NAKED void dbecho(u32 level, const char* fmt, ...) {
		__asm {
			pop[dummy]
			xor esi, esi
			mov esi, ptrConsole
			push dword ptr ds:[esi]
			call[fnDBEcho]
			add esp, 4
			jmp[dummy]
		}
	}

	MultiPointer(fnEcho, 0, 0, 0x005e3178, 0x005E6A1C);
	NAKED void echo(const char* fmt, ...) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				pop[dummy]
				push dword ptr ds : [CONSOLE_PTR_1004]
				call[fnEcho]
				add esp, 4
				jmp[dummy]
			}
		}
		else
		{
			__asm {
				pop[dummy]
				push dword ptr ds : [CONSOLE_PTR_1003]
				call[fnEcho]
				add esp, 4
				jmp[dummy]
			}
		}
	}

	MultiPointer(fnExecFunction, 0, 0, 0x005e362c, 0x005E6ED0);
	NAKED const char* execFunction(u32 argc, char* function, ...) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				pop[dummy2]
				push dword ptr ds : [CONSOLE_PTR_1004]
				inc dword ptr[esp + 0x4] // argc
				call[fnExecFunction]
				add esp, 4
				jmp[dummy2]
			}
		}
		else
		{
			__asm {
				pop[dummy2]
				push dword ptr ds : [CONSOLE_PTR_1003]
				inc dword ptr[esp + 0x4] // argc
				call[fnExecFunction]
				add esp, 4
				jmp[dummy2]
			}
		}
	}

	MultiPointer(fnEval, 0, 0, 0x005e354c, 0x005E6DF0);
	void eval(const char* cmd) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				push 0
				push 0
				mov edx, [cmd]
				xor ecx, ecx
				mov eax, ds: [CONSOLE_PTR_1004]
				call[fnEval]
			}
		}
		else
		{
			__asm {
				push 0
				push 0
				mov edx, [cmd]
				xor ecx, ecx
				mov eax, ds: [CONSOLE_PTR_1003]
				call[fnEval]
			}
		}
	}

	MultiPointer(fnFunctionExists, 0, 0, 0x005e387c, 0x005E7120);
	bool functionExists(const char* name) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				mov edx, [name]
				mov eax, ds: [CONSOLE_PTR_1004]
				call[fnFunctionExists]
			}
		}
		else
		{
			__asm {
				mov edx, [name]
				mov eax, ds: [CONSOLE_PTR_1003]
				call[fnFunctionExists]
			}
		}
	}

	MultiPointer(fnGetVariable, 0, 0, 0x005e3394, 0x005E6C38);
	const char* getVariable(const char* name) {
		if (VersionSnoop::GetVersion() == VERSION::v001004)
		{
			__asm {
				mov eax, ds: [CONSOLE_PTR_1004]
				mov edx, [name]
				call[fnGetVariable]
			}
		}
		else
		{
			__asm {
				mov eax, ds: [CONSOLE_PTR_1003]
				mov edx, [name]
				call[fnGetVariable]
			}
		}
	}

	MultiPointer(fnSetVariable, 0, 0, 0x005e32ac, 0x005E6B50);
	MultiPointer(ptrSetVariable, 0, 0, 0x00712b34, 0x00722FA4);
	void setVariable(const char* name, const char* value) {
		__asm {
			mov eax, ds: [ptrSetVariable]
			mov edx, [name]
			mov ecx, [value]
			call[fnSetVariable]
		}
		//if (VersionSnoop::GetVersion() == VERSION::v001004)
		//{
		//	__asm {
		//		mov eax, ds: [CONSOLE_PTR_1004]
		//		mov edx, [name]
		//		mov ecx, [value]
		//		call[fnSetVariable]
		//	}
		//}
		//else
		//{
		//	__asm {
		//		mov eax, ds: [CONSOLE_PTR_1003]
		//		mov edx, [name]
		//		mov ecx, [value]
		//		call[fnSetVariable]
		//	}
		//}
	}

	void OnStarted(bool active) {
		VariableConstructor::Process();
		ConsoleConstructor::Process();
		Console::echo("[mem.dll] (Modloader) Initalized Successfully");
		Console::echo("---------------------------------");
		Console::execFunction(0, "Memstar::version");
		Console::echo("---------------------------------");
		Console::setVariable("$pref::OpenGL::NoPackedTextures", "false");
	}

	struct Init {
		Init() {
			Callback::attach(Callback::OnStarted, OnStarted);
		}
	} init;


	struct StringSorter {
		bool operator() (const char*& a, const char*& b) const {
			return _stricmp(a, b) < 0;
		}
	};

	struct FunctionPrinter {
		void operator() (const char*& in) const {
			Console::echo(" %s()", in);
		}
	};

	struct VariablePrinter {
		void operator() (const char*& in) const {
			Console::echo(" $%s = \"%s\";", in, Console::getVariable(in));
		}
	};


	template <typename Printer>
	void DumpStrings(StringList& strings) {
		Printer p;
		strings.Sort(StringSorter());
		for (StringIter iter = strings.Begin(); iter != strings.End(); ++iter)
			p(iter.value());
	}

	BuiltInFunction("Memstar::dumpAddons", dumpAddons) {
		Console::echo("------------------");
		Console::echo("Memstar Functions");
		DumpStrings<FunctionPrinter>(functions);
		Console::echo("------------------");
		Console::echo("Memstar Variables");
		DumpStrings<VariablePrinter>(variables);
		return "true";
	}

}; // namespace Console