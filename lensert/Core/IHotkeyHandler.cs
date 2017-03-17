using Shortcut;

namespace Lensert.Core
{
    internal interface IHotkeyHandler
    {
        void HandleHotkey(HotkeyPressedEventArgs eventArgs);
    }
}
