using System;

namespace Shortcut
{
    public class HotkeyPressedEventArgs : EventArgs
    {
        public Hotkey Hotkey { get; }

        internal HotkeyPressedEventArgs(Hotkey hotkey)
        {
            Hotkey = hotkey;
        }
    }
}