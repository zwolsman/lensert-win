using Shortcut;
using System;

namespace Lensert
{
    public class HotkeyEventArgs : EventArgs
    {
        public Hotkey OldHotkey { get; }
        public Hotkey NewHotkey { get; }

        public HotkeyEventArgs(Hotkey oldHotkey, Hotkey newHotkey)
        {
            OldHotkey = oldHotkey;
            NewHotkey = newHotkey;
        }
    }
}
