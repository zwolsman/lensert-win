using Lensert.Screenshot;
using Shortcut;
using Shortcut.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Threading.Timer;

namespace Lensert
{
    internal static class Utils
    {
        private static readonly HotkeyConverter _hotkeyConverter = new HotkeyConverter();
        
        private static IEnumerable<SettingsPropertyValue> GetSettings() 
            => Preferences.Default.PropertyValues.Cast<SettingsPropertyValue>();

        private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>
        {
            [nameof(Preferences.HotkeyClipboard)] = "Upload clipboard",
            [nameof(Preferences.HotkeySelectArea)] = "Take screenshot of selected area",
            [nameof(Preferences.HotkeySelectCurrentWindow)] = "Take screenshot of current window",
            [nameof(Preferences.HotkeySelectFullscreen)] = "Take screenshot of all screens",
            [nameof(Preferences.HotkeySelectWindow)] = "Take screenshot of a specific window"
        };
        
        private static readonly Dictionary<Hotkey, Type> _screenshotTypes = new Dictionary<Hotkey, Type>
        {
           // [Preferences.Default.HotkeyClipboard] = typeof(Screenshot.Clipboard),
            [Preferences.Default.HotkeySelectArea] = typeof(SelectArea),
            [Preferences.Default.HotkeySelectCurrentWindow] = typeof(CurrentWindow),
            [Preferences.Default.HotkeySelectFullscreen] = typeof(FullScreen),
            [Preferences.Default.HotkeySelectWindow] = typeof(SelectWindow)
        };
        
        public static SettingsPropertyValue FindSettingByValue(object defaultValue)
            => GetSettings().FirstOrDefault(setting => setting.PropertyValue.Equals(defaultValue));
        
        public static IEnumerable<SettingsPropertyValue> SettingsOfType(Type type)
            => GetSettings().Where(setting => setting.Property.PropertyType == type); 
        
        public static Type GetScreenshotType(this Hotkey hotkey)
            => _screenshotTypes[hotkey];

        public static Hotkey GetHotkey(Type type) =>
            _screenshotTypes.First(t => t.Value == type).Key;
        
        public static string GetSettingDescription(this SettingsPropertyValue setting)              //maybe use ContainsKey and [] ? (performance wise)
            => _descriptions.FirstOrDefault(des => des.Key == setting.Name).Value;
        
        public static Hotkey ConvertToHotkey(string hotkeyString)
            => (Hotkey)_hotkeyConverter.ConvertFromString(hotkeyString);
        
        public static void EnableControls(this Form form, bool enable, params Control[] controls)
        {
            foreach (var control in controls)
                control.Enabled = enable;
        }
    }
}
