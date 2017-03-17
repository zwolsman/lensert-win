// std libraries
#include <stdio.h>
#include <stdlib.h>

// platform libraries
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

int main()
{
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
