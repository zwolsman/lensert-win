// std libraries
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>

// platform libraries
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <ShlObj.h>
#include <Shellapi.h>

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
	const char relativeIniPath[] = "\\lensert\\Settings.ini";
	strncat_s(g_SettingsFile, sizeof(g_SettingsFile), relativeIniPath, sizeof(relativeIniPath) - 1);
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
		{"Alt", 0x0001},
		{"Control", 0x0002},
		//{ "Alt", 0x0001 }, TODO: Norepeat?
		{"Shift", 0x0004},
		{"Win", 0x0008},
		{NULL, 0}
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
	strncpy_s(buf, sizeof(buf), hotkey->HotkeyPattern, sizeof(hotkey->HotkeyPattern) - 1);

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
		{
			char buf[MAX_PATH];
			snprintf(buf, sizeof(buf), "Lensert failed to register %s", g_Hotkeys[i].Name);

			ShowNotification("Hotkey", buf);
		}
		// return ERR;

		g_Hotkeys[i].EventLParam = (modifier & 0xFFFF) | (vk << 16);
	}

	return OK;
}

static int StartLensert(const char* arguments, size_t len)
{
	assert(arguments);

	STARTUPINFO startupInfo = {0};
	startupInfo.cb = sizeof(startupInfo);
	PROCESS_INFORMATION processInformation = {0};

	char buf[MAX_PATH] = "lensert.exe ";
	strncat_s(buf, sizeof(buf), arguments, len);

	return CreateProcess(
		                    NULL,
		                    buf,
		                    NULL,
		                    NULL,
		                    FALSE,
		                    NORMAL_PRIORITY_CLASS,
		                    NULL,
		                    NULL,
		                    &startupInfo,
		                    &processInformation
	                    ) ? OK : ERR;
}

static int ShowNotification(const char* title, const char* text)
{
	assert(title);
	assert(text);

	size_t titleLength = strnlen_s(title, MAX_PATH / 2);
	size_t textLength = strnlen_s(text, MAX_PATH / 2);

	// check if it fits our buffer
	if (titleLength + textLength + sizeof("--show-notification " + 3) > MAX_PATH)
		return ERR;

	char arguments[MAX_PATH];
	snprintf(arguments, sizeof(arguments), "--show-notification \"%s\" \"%s\"", title, text);

	return StartLensert(arguments, strnlen_s(arguments, sizeof(arguments)));
}

#ifdef _DEBUG
int main()
#else
int CALLBACK WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
#endif
{
	const char resetSettingsArgument[] = "--reset-settings";

	if (InitializeSettingsPath() == ERR)
		return ERR;

	int timeout = 10;

	int hotkeys = ParseHotkeysFromSettings();
	if (hotkeys == ERR)
	{
		if (StartLensert(resetSettingsArgument, sizeof(resetSettingsArgument) - 1) == ERR)
			return ERR;

		while ((hotkeys = ParseHotkeysFromSettings()) == ERR)
		{
			if (--timeout <= 0)
				return ERR;

			Sleep(100);
		}
	}
	
	if (RegisterHotkeys(hotkeys) == ERR)
	{
		ShowNotification("Hotkey", "Lensert failed to register its hotkeys.");
		return ERR;
	}

	int argc;
	LPWSTR* argv = CommandLineToArgvW(GetCommandLineW(), &argc);
	
	if (argv != NULL && argc == 2)
	{
		if (wcscmp(argv[1], L"--updated") == 0)
			ShowNotification("Lensert", "Lensert has been updated to latest version!");
		else if (wcscmp(argv[1], L"--fresh") == 0)
			ShowNotification("Lensert", "Lensert has been installed!");
	}


	Hotkey_t* hotkey = NULL;
	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0) != 0)
	{
		if (msg.message != WM_HOTKEY)
			continue;

		if (FindHotkeyByLParam(msg.lParam, &hotkey) == ERR)
		{
			printf("Hotkey event received, but didn't match any hotkeys :O");
			continue;
		}

		StartLensert(hotkey->Name, strnlen_s(hotkey->Name, sizeof(hotkey->Name)));
	}

	return 0;
}
