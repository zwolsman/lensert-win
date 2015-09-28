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
        public static string GetDescription(this SettingsProperty setting)
        {
            switch (setting.Name)
            {
                case nameof(Preferences.HotkeyClipboard):
                    return "Upload clipboard";
                case nameof(Preferences.HotkeySelectArea):
                    return "Take screenshot of selected area";
                case nameof(Preferences.HotkeySelectCurrentWindow):
                    return "Take screenshot of current window";
                case nameof(Preferences.HotkeySelectFullscreen):
                    return "Take screenshot of all screens";
                case nameof(Preferences.HotkeySelectWindow):
                    return "Take screenshot of current window";
            }

            return "";
        }
    }
}
