// std libraries
#include <stdio.h>
#include <stdlib.h>

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

// globals
const char g_SettingsFile[MAX_PATH];
static Hotkey_t g_Hotkeys[8];

static int GetHotkeys()
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
	
	return 0;
}

int main()
{
	// set the settings file
	if (!SHGetFolderPath(NULL, CSIDL_LOCAL_APPDATA, NULL, 0, g_SettingsFile) == S_OK)
		return 1;

	// very safe catenation
	const char* const relativeIniPath = "\\lensert\\Settings.ini";
	strncat_s(g_SettingsFile, sizeof(g_SettingsFile), relativeIniPath, sizeof(g_SettingsFile)-strnlen_s(g_SettingsFile, sizeof(g_SettingsFile))-sizeof(relativeIniPath)-1);

	GetHotkeys();

	if (!RegisterHotKey(NULL, 1, MOD_CONTROL | MOD_SHIFT, 0x41))
		return 1;

	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (msg.message != WM_HOTKEY)
			continue;

		printf("%04X %04X\n", msg.lParam & 0xFFFF, msg.lParam >> 16);
	}

	return 0;
}
