using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Shortcut;

namespace Lensert
{
    internal sealed class Settings
    {
        private readonly string _iniPath;

        public static Settings Instance { get; }

        static Settings()
        {
            Instance = new Settings();
        }

        private Settings()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lensert");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _iniPath = Path.Combine(directory, "Settings.ini");
            if (!File.Exists(_iniPath))
                File.Create(_iniPath).Dispose();
        }

        public T GetSetting<T>(SettingType type)
        {
            var value = NativeHelper.ParseValueFromIni<T>(_iniPath, type.ToString(), "Settings");
            return value == null || value.Equals(default(T))
                ? (T) DefaultSetting(type)
                : value;
        }

        private object DefaultSetting(SettingType type)
        {
            switch (type)
            {
                case SettingType.SelectAreaHotkey:
                    return new Hotkey(Modifiers.Control | Modifiers.Shift, Keys.A);
                case SettingType.SelectWindowHotkey:
                    return new Hotkey(Modifiers.Control | Modifiers.Shift, Keys.W);
                case SettingType.FullscreenHotkey:
                    return new Hotkey(Modifiers.Control | Modifiers.Shift, Keys.F);
                case SettingType.BackgroundColor:
                    return Color.White;
                case SettingType.ForegroundColor:
                    return Color.Red;
                default:
                    throw new ArgumentException("Invalid setting type", nameof(type));
            }
        }
    }

    internal enum SettingType
    {
        SelectAreaHotkey,
        SelectWindowHotkey,
        FullscreenHotkey,
        BackgroundColor,
        ForegroundColor
    }
}