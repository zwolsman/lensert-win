using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using log4net;
using Shortcut;

namespace Lensert
{
    internal static class Settings
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Settings));
        private static readonly string _lensertAppData;
        private static readonly string _iniPath;

        static Settings()
        {
            _lensertAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lensert");
            if (!Directory.Exists(_lensertAppData))
                Directory.CreateDirectory(_lensertAppData);

            _iniPath = Path.Combine(_lensertAppData, "Settings.ini");
            if (!File.Exists(_iniPath))
                File.Create(_iniPath).Dispose();

            SetupSettingsFile();
        }

        public static T GetSetting<T>(SettingType type)
        {
            var value = NativeHelper.ParseValueFromIni<T>(_iniPath, type.ToString(), "Settings");
            if (value != null && !value.Equals(default(T)))
                return value;

            var defaultValue = (T)DefaultSetting(type);
            _log.Warn($"Failed to parse '{type}' to '{typeof(T)}', restoring default value '{defaultValue}'");

            return defaultValue;
        }

        private static void SetupSettingsFile()
        {
            var missingSettings = Enum.GetValues(typeof (SettingType))
                .Cast<SettingType>()
                .Where(s => string.IsNullOrEmpty(NativeHelper.ReadValueFromIni(_iniPath, s.ToString(), "Settings")));

            foreach (var setting in missingSettings)
            {
                var value = DefaultSetting(setting);
                NativeHelper.WriteValueToIni(_iniPath, setting.ToString(), value, "Settings");
                _log.Info($"Restored missing '{setting}' setting to default value");
            }
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