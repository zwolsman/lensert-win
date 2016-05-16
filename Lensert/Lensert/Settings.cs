using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;

namespace Lensert
{
    internal sealed class Settings
    {
        public static Settings Instance { get; }
        private readonly string _iniPath;

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

        public T GetValue<T>(string key, string section = null)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof (T));
                var value = NativeHelper.ReadFromIni(_iniPath, key, section);
            
                return string.IsNullOrEmpty(value)
                    ? default(T)
                    : (T) converter.ConvertFromString(value);
            }
            catch
            {
                return default(T);
            }
        }

        //public void SetValue<T>(string key, T value, string section = null)
        //{
        //    var converter = TypeDescriptor.GetConverter(typeof (T));
        //    var strValue = converter.ConvertToString(value);

        //    NativeHelper.WriteToIni(_iniPath, key, strValue, section);
        //}
    }
}