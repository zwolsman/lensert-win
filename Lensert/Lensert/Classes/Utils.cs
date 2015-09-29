using Shortcut;
using Shortcut.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lensert
{
    static class Utils
    {
        private static readonly HotkeyConverter _hotkeyConverter = new HotkeyConverter();

        private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>
        {
            [nameof(Preferences.HotkeyClipboard)] = "Upload clipboard",
            [nameof(Preferences.HotkeySelectArea)] = "Take screenshot of selected area",
            [nameof(Preferences.HotkeySelectCurrentWindow)] = "Take screenshot of current window",
            [nameof(Preferences.HotkeySelectFullscreen)] = "Take screenshot of all screens",
            [nameof(Preferences.HotkeySelectWindow)] = "Take screenshot of a specific window"
        };

        private static readonly Dictionary<Hotkey, ScreenshotType> _screenshotTypes = new Dictionary<Hotkey, ScreenshotType>
        {
            [Preferences.Default.HotkeyClipboard] = ScreenshotType.Clipboard,
            [Preferences.Default.HotkeySelectArea] = ScreenshotType.Area,
            [Preferences.Default.HotkeySelectCurrentWindow] = ScreenshotType.CurrentWindow,
            [Preferences.Default.HotkeySelectFullscreen] = ScreenshotType.Fullscreen,
            [Preferences.Default.HotkeySelectWindow] = ScreenshotType.SelectWindow
        };

        public static ScreenshotType GetScreenshotType(this Hotkey hotkey) =>
            _screenshotTypes[hotkey];

        public static Hotkey GetHotkey(this ScreenshotType screenshotType) =>
            _screenshotTypes.First(type => type.Value == screenshotType).Key;

        public static string GetDescription(this SettingsProperty setting)              //maybe use ContainsKey and [] ? (performance wise)
            => _descriptions.FirstOrDefault(des => des.Key == setting.Name).Value;

        public static string GetPropertySettingName(string description)
            => _descriptions.FirstOrDefault(des => des.Value == description).Key;

        public static Hotkey ConvertToHotkey(string hotkeyString)
            => (Hotkey)_hotkeyConverter.ConvertFromString(hotkeyString);
    }
}
