using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Lensert.Helpers;
using NLog;

namespace Lensert.Core
{
    internal static class AssemblyManager
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        static AssemblyManager()
        {
            AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lensert");
            if (!Directory.Exists(AppData)) // I believe log4net already make this folder -> sanity check
                Directory.CreateDirectory(AppData);

#if DEBUG
            FirstLaunch = false;
#else
            FirstLaunch = !Assembly.GetExecutingAssembly().Location.EndsWith("lensert-win.exe");       // terrible check :$
#endif

            _logger.Info($"First launch: {FirstLaunch}");
        }

        public static string AppData { get; }
        public static bool FirstLaunch { get; }

        public static bool HandleStartup()
        {
            if (!FirstLaunch && !Settings.GetSetting<bool>(SettingType.StartupOnLogon))
            {
                _logger.Info("StartupOnLogon Disabled");
                return false;
            }

            if (FirstLaunch)
            {
                Settings.Reset();

                var path = Path.Combine(AppData, "lensert-win.exe");
                File.Copy(Assembly.GetExecutingAssembly().Location, path, true);
                Process.Start(path);

                _logger.Info("Installed Lensert :)");
                new Timer(x => Environment.Exit(0), null, 200, 0);

                return true;
            }

            if (IsAlreadyRunning())
            {
                _logger.Info("Lensert is already running");
                return false;
            }

            CreateStartupLink();
            Task.Run(UpdateHandler);
            return true;
        }

        private static Task UpdateHandler()
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            using (var client = new HttpClient()) {}

            return Task.FromResult(0);
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
                var guidAttribute = (GuidAttribute) assemblyAttributes.GetValue(0);
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
