using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Lensert
{
    internal static class AssemblyManager
    {
        public static string AppData { get; }
        private static readonly bool _firstLaunch;

        static AssemblyManager()
        {
            AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lensert");
            if (!Directory.Exists(AppData))
            {
                Directory.CreateDirectory(AppData);
                _firstLaunch = true;
            }
        }

        public static bool HandleStartup()
        {
            if (IsAlreadyRunning())
                return false;

            if (_firstLaunch)
            {
                var result = MessageBox.Show("Start Lensert when Windows start?", "Lensert", MessageBoxButtons.YesNo);
            }
        }
        
        private static bool IsAlreadyRunning()
        {
            try
            {
                var mutex = new Mutex(false, GetMutexName());
                return !mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch
            {
                return true;
            }
        }

        private static void CreateStartupLink()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var location = Assembly.GetExecutingAssembly().Location;

            using (var streamWriter = new StreamWriter(Path.Combine(directory, "Lensert.url")))
            {
                streamWriter.WriteLine("[InternetShortcut]");
                streamWriter.WriteLine("URL=file:///" + location);
                streamWriter.WriteLine("IconIndex=0");
                streamWriter.WriteLine("IconFile=" + location);
            }
        }

        private static string GetMutexName()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyAttributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
                var guidAttribute = (GuidAttribute)assemblyAttributes.GetValue(0);
                return $"Global\\{{{guidAttribute.Value}}}";
            }
            catch
            {
                throw new InvalidOperationException(
                    "Ensure there is a Guid attribute defined for this assembly.");
            }
        }
    }
}