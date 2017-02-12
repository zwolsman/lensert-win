using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Lensert.Core;
using NLog;
using Shortcut;

namespace Lensert.Helpers
{
    internal static class Settings
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string _iniPath;

        static Settings()
        {
            InstallationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert");
            _iniPath = Path.Combine(InstallationDirectory, "Settings.ini");

            Reset();
        }

        public static string InstallationDirectory { get; }

        public static void SetSetting<T>(SettingType type, T value)
        {
            Native.WriteValueToIni(_iniPath, type.ToString(), value, "Settings");
        }

        public static T GetSetting<T>(SettingType type)
        {
            var value = Native.ParseValueFromIni<T>(_iniPath, type.ToString(), "Settings");
            if ((value != null) && !value.Equals(default(T)))
                return value;

            var defaultValue = (T)DefaultSetting(type);
            _logger.Warn($"Failed to parse '{value}' to '{typeof(T)}', restored to default value '{defaultValue}'");
            Native.WriteValueToIni(_iniPath, type.ToString(), defaultValue, "Settings");

            NotificationProvider.Show("Error", "Settings file was corrupted and has been auto-fixed.", LogFile.Open);

            return defaultValue;
        }

        public static IEnumerable<KeyValuePair<SettingType, T>> GetSettings<T>()
        {
            var settings = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();

            return settings.Where(s => DefaultSetting(s).GetType().IsAssignableFrom(typeof(T))).ToDictionary(k => k, GetSetting<T>);
        }

        public static SettingType GetSettingType<T>(T t)
        {
            var settings = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();
            return settings.First(s => EqualityComparer<T>.Default.Equals(t, GetSetting<T>(s)));
        } 

        public static void Reset()
        {
            if (!File.Exists(_iniPath))
                File.Create(_iniPath).Dispose();

            var missingSettings = Enum.GetValues(typeof(SettingType))
                .Cast<SettingType>()
                .Where(s => string.IsNullOrEmpty(Native.ReadValueFromIni(_iniPath, s.ToString(), "Settings")))
                .ToList();

            if (!missingSettings.Any())
                return;

            if (missingSettings.Count != Enum.GetValues(typeof(SettingType)).Length)
                NotificationProvider.Show("Error", "Settings file was corrupted and has been auto-fixed.", LogFile.Open);

            foreach (var setting in missingSettings)
            {
                var value = DefaultSetting(setting);
                Native.WriteValueToIni(_iniPath, setting.ToString(), value, "Settings");
                _logger.Info($"Restored missing '{setting}' setting to default value");
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
            case SettingType.StartupOnLogon:
                return true;
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
        StartupOnLogon
    }
}
