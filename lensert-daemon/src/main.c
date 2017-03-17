// std libraries
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>

// platform libraries
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <ShlObj.h>

// macros
#define BUFSIZE 32
#define ERR -1
#define OK 1

// types
typedef struct Hotkey_t
{
	const char Name[BUFSIZE];
	const char HotkeyPattern[BUFSIZE];
	LPARAM EventLParam;
} Hotkey_t;

typedef struct Lookup_t
{
	const char* Key;
	unsigned Value;
} Lookup_t;

// globals
const char g_SettingsFile[MAX_PATH];
static Hotkey_t g_Hotkeys[8];

static int InitializeSettingsPath()
{
	// get local appdata
	if (SHGetFolderPath(NULL, CSIDL_LOCAL_APPDATA, NULL, 0, g_SettingsFile) != S_OK)
		return ERR;

	// very safe catenation
	const char* const relativeIniPath = "\\lensert\\Settings.ini";
	strncat_s(g_SettingsFile, sizeof(g_SettingsFile), relativeIniPath, sizeof(g_SettingsFile) - strnlen_s(g_SettingsFile, sizeof(g_SettingsFile)) - sizeof(relativeIniPath) - 1);
	return OK;
}

static int ParseHotkeysFromSettings()
{
	const char strHotkey[] = "Hotkey";

	char sectionBuffer[MAX_PATH];
	if (!GetPrivateProfileString("Settings", NULL, "", sectionBuffer, sizeof(sectionBuffer), g_SettingsFile))
		return ERR;

	char* line = sectionBuffer;
	
	int index = 0;
	while (*line && index < sizeof(g_Hotkeys))
	{
		size_t lineLength = strnlen_s(line, sizeof(sectionBuffer) - (sectionBuffer - line) - 1);
		
		if (strcmp(line + lineLength - sizeof(strHotkey) + 1, strHotkey) == 0)
		{
			strncpy_s(g_Hotkeys[index].Name, BUFSIZE, line, lineLength - sizeof(strHotkey) + 1);
			GetPrivateProfileString("Settings", line, "", g_Hotkeys[index].HotkeyPattern, BUFSIZE, g_SettingsFile);

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
			return OK;
		}
	}

	return ERR;
}

static int ParseHotkey(const Hotkey_t* hotkey, UINT* modifier, UINT* vk)
{
	assert(hotkey);
	assert(modifier);
	assert(vk);

	*modifier = 0;
	*vk = 0;

	char buf[BUFSIZE];
	strncpy_s(buf, sizeof(buf), hotkey->HotkeyPattern, sizeof(buf) - sizeof(hotkey->HotkeyPattern) - 1);

	char* next_token;
	char* token = strtok_s(buf, ", ", &next_token);
	if (token == NULL)
		return ERR;
	
	while (token)
	{
		UINT tmp;
		size_t len = strnlen_s(token, sizeof(buf));
		if (len == 1 && *vk == 0)
			*vk = (unsigned)token[0];
		else if (len > 1 && ParseModifier(token, len, &tmp) == OK)
			*modifier |= tmp;
		else
			return ERR;

		token = strtok_s(NULL, ", ", &next_token);
	}	

	return OK;
}

static int FindHotkeyByLParam(LPARAM lParam, Hotkey_t** hotkey)
{
	for (int i = 0; i < sizeof(g_Hotkeys); ++i)
	{
		if (g_Hotkeys[i].EventLParam == lParam)
		{
			*hotkey = &g_Hotkeys[i];
			return OK;
		}
	}

	return ERR;
}

static int RegisterHotkeys(int hotkeys)
{
	for (int i = 0; i < hotkeys; ++i)
	{
		UINT modifier, vk;
		if (ParseHotkey(&g_Hotkeys[i], &modifier, &vk) == ERR)
			return ERR; // TODO: Show error

		if (!RegisterHotKey(NULL, i, modifier, vk))
			return ERR;

		g_Hotkeys[i].EventLParam = (modifier & 0xFFFF) | (vk << 16);
	}

	return OK;
}

int main()
{
	if (InitializeSettingsPath() == ERR)
		return ERR;

	int hotkeys = ParseHotkeysFromSettings();
	if (hotkeys == ERR)
		return ERR;

	if (RegisterHotkeys(hotkeys) == ERR)
		return ERR;

	Hotkey_t* hotkey = NULL;
	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (msg.message != WM_HOTKEY)
			continue;

		if (FindHotkeyByLParam(msg.lParam, &hotkey) == ERR)
		{
			printf("Hotkey event received, but didn't match any hotkeys :O");
			continue;
		}

		char commandLine[BUFSIZE];
		snprintf(commandLine, sizeof(commandLine), "lensert.exe %.32s", hotkey->Name);

		STARTUPINFO startupInfo = {0};
		startupInfo.cb = sizeof(startupInfo);
		PROCESS_INFORMATION processInformation = { 0 };

		if (!CreateProcess(
			NULL, 
			commandLine, 
			NULL, 
			NULL, 
			FALSE, 
			NORMAL_PRIORITY_CLASS, 
			NULL, 
			NULL, 
			&startupInfo, 
			&processInformation
		))
			return ERR;

		printf("Launched Lensert %.32s\n", hotkey->Name);
	}

	return 0;
}
