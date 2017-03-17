// std libraries
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>

// platform libraries
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <ShlObj.h>

// types
typedef struct Hotkey_t
{
	const char Name[32];
	const char HotkeyPattern[32];
} Hotkey_t;

typedef struct Lookup_t
{
	const char* Key;
	unsigned Value;
} Lookup_t;

// globals
const char g_SettingsFile[MAX_PATH];
static Hotkey_t g_Hotkeys[8];

static void InitializeSettingsPath()
{
	// get local appdata
	SHGetFolderPath(NULL, CSIDL_LOCAL_APPDATA, NULL, 0, g_SettingsFile);

	// very safe catenation
	const char* const relativeIniPath = "\\lensert\\Settings.ini";
	strncat_s(g_SettingsFile, sizeof(g_SettingsFile), relativeIniPath, sizeof(g_SettingsFile) - strnlen_s(g_SettingsFile, sizeof(g_SettingsFile)) - sizeof(relativeIniPath) - 1);
}

static int ParseHotkeysFromSettings()
{
	const char strHotkey[] = "Hotkey";

	char sectionBuffer[MAX_PATH];
	if (!GetPrivateProfileString("Settings", NULL, "", sectionBuffer, sizeof(sectionBuffer), g_SettingsFile))
		return -1;

	char* line = sectionBuffer;
	
	int index = 0;
	while (*line && index < sizeof(g_Hotkeys))
	{
		size_t lineLength = strnlen_s(line, sizeof(sectionBuffer) - (sectionBuffer - line) - 1);
		
		if (strcmp(line + lineLength - sizeof(strHotkey) + 1, strHotkey) == 0)
		{
			strncpy_s(g_Hotkeys[index].Name, 32, line, lineLength - sizeof(strHotkey) + 1);
			GetPrivateProfileString("Settings", line, "", g_Hotkeys[index].HotkeyPattern, 32, g_SettingsFile);

			++index;
		}

		line += lineLength + 1;
	}
	
	return index;
}

static int ParseModifier(const char* token, size_t len, UINT* modifier)
{
	assert(token);
	assert(len > 0);
	assert(modifier);

	static Lookup_t table[] =
	{
		{ "Alt", 0x0001 },
		{ "Control", 0x0002 },
		//{ "Alt", 0x0001 }, TODO: Norepeat?
		{ "Shift", 0x0004 },
		{ "Win", 0x0008 },
		{ NULL, 0 }
	};

	for (Lookup_t* p = table; p->Key; ++p)
	{
		if (strncmp(p->Key, token, len) == 0)
		{
			*modifier = p->Value;
			return 1;
		}
	}

	return 0;
}

static void ParseHotkey(const Hotkey_t* hotkey, UINT* modifier, UINT* vk)
{
	assert(hotkey);
	assert(modifier);
	assert(vk);

	*modifier = 0;
	*vk = 0;

	char buf[32];
	strncpy_s(buf, sizeof(buf), hotkey->HotkeyPattern, sizeof(buf) - sizeof(hotkey->HotkeyPattern) - 1);

	char* next_token;
	char* token = strtok_s(buf, ", ", &next_token);
	while (token)
	{
		UINT tmp;
		size_t len = strnlen_s(token, sizeof(buf));
		if (len == 1 && *vk == 0)
			*vk = (unsigned)token[0];
		else if (len > 1 && ParseModifier(token, len, &tmp))
			*modifier |= tmp;
		// else error?

		token = strtok_s(NULL, ", ", &next_token);
	}	
}

int main()
{
	InitializeSettingsPath();
	int hotkeys = ParseHotkeysFromSettings();

	for (int i = 0; i < hotkeys; ++i)
	{
		UINT modifier, vk;
		ParseHotkey(&g_Hotkeys[i], &modifier, &vk);

		if (!RegisterHotKey(NULL, i, modifier, vk))
			return 1;
	}

	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (msg.message != WM_HOTKEY)
			continue;

		printf("%04X %04X\n", msg.lParam & 0xFFFF, msg.lParam >> 16);
	}

	return 0;
}
