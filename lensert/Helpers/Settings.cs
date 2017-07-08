using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lensert.Core;
using NLog;

namespace Lensert.Helpers
{
    internal static class Settings
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string _iniPath;

        private static readonly Dictionary<SettingType, object> _defaultSettings = new Dictionary<SettingType, object>
        {
            [SettingType.SelectAreaHotkey] = "Control, Shift, A",
            [SettingType.SelectWindowHotkey] = "Control, Shift, W", 
            [SettingType.FullscreenHotkey] = "Control, Shift, F",
            [SettingType.ClipboardHotkey] = "Control, Shift, C",
            [SettingType.SelectAreaClipboardHotkey] = "Alt, Shift, A",
            [SettingType.SelectWindowClipboardHotkey] = "Alt, Shift, W",
            [SettingType.FullscreenClipboardHotkey] = "Alt, Shift, F",
            [SettingType.StartupOnLogon] = true,
            [SettingType.CheckForUpdates] = true,
            [SettingType.SaveBackup] = true
        };

        static Settings()
        {
            InstallationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert");
            _iniPath = Path.Combine(InstallationDirectory, "Settings.ini");

            Restore();
        }

        public static string InstallationDirectory { get; }

        public static void SetSetting<T>(SettingType type, T value)
        {
            Native.WriteValueToIni(_iniPath, type.ToString(), value, "Settings");
        }

        public static T GetSetting<T>(SettingType type)
        {
            var value = Native.ParseValueFromIni<T>(_iniPath, type.ToString(), "Settings");
            if (value != null && !value.Equals(default(T)))
                return value;

            var defaultValue = (T) _defaultSettings[type];
            _logger.Warn($"Failed to parse '{value}' to '{typeof(T)}', restored to default value '{defaultValue}'");
            Native.WriteValueToIni(_iniPath, type.ToString(), defaultValue, "Settings");

            NotificationProvider.Show("Error", "Settings file was corrupted and has been auto-fixed.", LogFile.Open);

            return defaultValue;
        }

        public static IEnumerable<KeyValuePair<SettingType, T>> GetSettings<T>()
        {
            var settings = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();

            return settings.Where(s => _defaultSettings[s].GetType().IsAssignableFrom(typeof(T))).ToDictionary(k => k, GetSetting<T>);
        }

        public static SettingType GetSettingType<T>(T t)
        {
            var settings = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();
            return settings.First(s => EqualityComparer<T>.Default.Equals(t, GetSetting<T>(s)));
        }

        public static void Restore()
        {
            if (!File.Exists(_iniPath))
            {
                var directory = Path.GetDirectoryName(_iniPath);
                if (directory != null && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.Create(_iniPath).Dispose();
            }

            var missingSettings = Enum.GetValues(typeof(SettingType))
                .Cast<SettingType>()
                .Where(s => string.IsNullOrEmpty(Native.ReadValueFromIni(_iniPath, s.ToString(), "Settings")))
                .ToList();

            if (!missingSettings.Any())
                return;

            foreach (var setting in missingSettings)
            {
                var value = _defaultSettings[setting];
                Native.WriteValueToIni(_iniPath, setting.ToString(), value, "Settings");
                _logger.Info($"Restored missing '{setting}' setting to default value");
            }
        }

        public static void Reset()
        {
            if (File.Exists(_iniPath))
                File.Decrypt(_iniPath);

            Restore();
        }
    }

    internal enum SettingType
    {
        SelectAreaHotkey,
        SelectWindowHotkey,
        FullscreenHotkey,
        ClipboardHotkey,
        SelectAreaClipboardHotkey,
        SelectWindowClipboardHotkey,
        FullscreenClipboardHotkey,
        StartupOnLogon,
        CheckForUpdates,
        SaveBackup
    }
}
