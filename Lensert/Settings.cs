﻿using System;
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
        private static readonly string _iniPath;

        static Settings()
        {
            _iniPath = Path.Combine(AssemblyManager.AppData, "Settings.ini");
            if (!File.Exists(_iniPath))
                File.Create(_iniPath).Dispose();

            Reset();
        }

        public static void SetSetting<T>(SettingType type, T value)
        {
            NativeHelper.WriteValueToIni(_iniPath, type.ToString(), value, "Settings");
        }

        public static T GetSetting<T>(SettingType type)
        {
            var value = NativeHelper.ParseValueFromIni<T>(_iniPath, type.ToString(), "Settings");
            if (value != null && !value.Equals(default(T)))
                return value;

            var defaultValue = (T)DefaultSetting(type);
            _log.Warn($"Failed to parse '{value}' to '{typeof(T)}', restored to default value '{defaultValue}'");
            NativeHelper.WriteValueToIni(_iniPath, type.ToString(), defaultValue, "Settings");

            NotificationProvider.Show("Error", "Settings file was corrupted and has been auto-fixed.", Util.OpenLog);

            return defaultValue;
        }

        public static void Reset()
        {
            var missingSettings = Enum.GetValues(typeof (SettingType))
                .Cast<SettingType>()
                .Where(s => string.IsNullOrEmpty(NativeHelper.ReadValueFromIni(_iniPath, s.ToString(), "Settings")))
                .ToList();

            if (!missingSettings.Any())
                return;

            if (missingSettings.Count != Enum.GetValues(typeof(SettingType)).Length)
                NotificationProvider.Show("Error", "Settings file was corrupted and has been auto-fixed.", Util.OpenLog);

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