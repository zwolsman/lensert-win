using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Shortcut;

namespace Lensert
{
    internal static class Settings
    {
        private static readonly string _iniPath;

        static Settings()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lensert");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _iniPath = Path.Combine(directory, "Settings.ini");
            if (File.Exists(_iniPath))
                return;

            File.Create(_iniPath).Dispose();

            foreach (var setting in Enum.GetValues(typeof (SettingType)).Cast<SettingType>())
            {
                var value = DefaultSetting(setting);
                NativeHelper.WriteValueToIni(_iniPath, setting.ToString(), value, "Settings", true);
            }
        }

        public static T GetSetting<T>(SettingType type)
        {
            var value = NativeHelper.ParseValueFromIni<T>(_iniPath, type.ToString(), "Settings");
            return value == null || value.Equals(default(T))
                ? (T) DefaultSetting(type)
                : value;
        }

        private static object DefaultSetting(SettingType type)
        {
            switch (type)
            {
                case SettingType.SelectAreaHotkey:
                    return new Hotkey(Modifiers.Control | Modifiers.Shift, Keys.A);
                case SettingType.SelectWindowHotkey:
                    return new Hotkey(Modifiers.Control | Modifiers.Shift, Keys.W);
                case SettingType.FullscreenHotkey:
                    return new Hotkey(Modifiers.Control | Modifiers.Shift, Keys.F);
                default:
                    throw new ArgumentException("Invalid setting type", nameof(type));
            }
        }
    }

    internal enum SettingType
    {
        SelectAreaHotkey,
        SelectWindowHotkey,
        FullscreenHotkey
    }
}