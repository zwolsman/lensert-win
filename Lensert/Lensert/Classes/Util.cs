using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lensert
{
    static class Util
    {
        private static Dictionary<string, string> _descriptions = new Dictionary<string, string>
        {
            [nameof(Preferences.HotkeyClipboard)] = "Upload clipboard",
            [nameof(Preferences.HotkeySelectArea)] = "Take screenshot of selected area",
            [nameof(Preferences.HotkeySelectCurrentWindow)] = "Take screenshot of current window",
            [nameof(Preferences.HotkeySelectFullscreen)] = "Take screenshot of all screens",
            [nameof(Preferences.HotkeySelectWindow)] = "Take screenshot of current window"
        };

        public static string GetDescription(this SettingsProperty setting)              //maybe use ContainsKey and [] ? (performance wise)
            => _descriptions.FirstOrDefault(des => des.Key == setting.Name).Value;
        
        public static string GetPropertySettingName(string description) 
            => _descriptions.FirstOrDefault(des => des.Value == description).Key;
    }
}
