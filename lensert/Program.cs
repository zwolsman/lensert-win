using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Lensert.Core;
using Lensert.Helpers;
using Nito.AsyncEx;
using NLog;

namespace Lensert
{
    internal static class Program
    {
        private const string LENSERT_URL = "https://lensert.com/download?type=win";
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        private static void Main(string[] args)
        {
#if DEBUG
            AsyncContext.Run(() => MainImpl(args));
#else
            try
            {
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                AsyncContext.Run(() => MainImpl(args));
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Unhandled exception");
                Environment.Exit(-1);
            }
#endif
        }

        private static async Task MainImpl(string[] args)
        {
#if DEBUG
            var consoleRule = LogManager.Configuration.LoggingRules.First(r => r.Targets.Any(t => t.Name == "console"));
            consoleRule.EnableLoggingForLevel(LogLevel.Debug);
#endif

            var profilePath = Path.Combine(Settings.InstallationDirectory, "profileroot");
            if (!Directory.Exists(profilePath))
                Directory.CreateDirectory(profilePath);

            ProfileOptimization.SetProfileRoot(profilePath);
            ProfileOptimization.StartProfile("Start.Profile");

            _logger.Info($"'{Environment.CommandLine}' started");
            if (args.Length == 0)
            {
                _logger.Warn("Lensert must be invoked with argument");
                if (!Process.GetProcessesByName("lensert-daemon").Any())
                {
                    _logger.Info("Daemon not running, started checking for updates..");
                    await UpdateRoutine();
                }

                await Task.Yield();
                return;
            }

            if (args.Length != 1)
            {
                _logger.Error($"Lensert must be started with one argument, but {args.Length} where given");

                await Task.Yield();
                return;
            }

            var hotkeySettings = Settings.GetSettings<string>().Where(keyValue => keyValue.Key.ToString().EndsWith("Hotkey"));
            var settingType = hotkeySettings.Single(keyValue => keyValue.Key.ToString().StartsWith(args[0])).Key;

            var hotkeyHandler = new LensertHotkeyHandler(new LensertClient());
            await hotkeyHandler.HandleHotkey(settingType);

#if !DEBUG
            if (Settings.GetSetting<bool>(SettingType.CheckForUpdates))
                await UpdateRoutine();
#endif

            if (Settings.GetSetting<bool>(SettingType.StartupOnLogon))
                CreateStartupLink();

            while (NotificationProvider.IsVisible())
                await Task.Delay(100);
        }

        private static async Task UpdateRoutine()
        {
            var file = await DownloadFileToTemp(LENSERT_URL);
            var updateDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert-installer");

            if (Directory.Exists(updateDirectory))
                Directory.Delete(updateDirectory, true);

            ZipFile.ExtractToDirectory(file, updateDirectory);

            file = Path.Combine(updateDirectory, "lensert-installer.exe");
            if (!File.Exists(file))
                _logger.Error("Extracted updater not found!");

            var timeout = 5;
            var process = Process.Start(file);
            while (!process.HasExited && --timeout > 0)
            {
                _logger.Info($"Waiting on installer ({timeout} remaining..)");
                await Task.Delay(1000);
                process.Refresh();
            }

            // installer still runs :O
            process = Process.GetProcessesByName("lensert-installer").FirstOrDefault();
            process?.Kill();

            // cleanup after installer
            if (!Directory.Exists(updateDirectory))
                throw new InvalidOperationException("Dafuck happend.");

            Directory.Delete(updateDirectory, true);
            _logger.Info("Installer completed.");
        }

        private static void CreateStartupLink()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var location = Path.Combine(Settings.InstallationDirectory, "lensert-daemon.exe");

            // delete old shortcut
            if (File.Exists(Path.Combine(directory, "Lensert.url")))
                File.Delete(Path.Combine(directory, "Lensert.url"));

            using (var streamWriter = new StreamWriter(Path.Combine(directory, "lensert-daemon.url")))
            {
                streamWriter.WriteLine("[InternetShortcut]");
                streamWriter.WriteLine("URL=file:///" + location);
                streamWriter.WriteLine("IconIndex=0");
                streamWriter.WriteLine("IconFile=" + location);
            }
        }

        private static async Task<string> DownloadFileToTemp(string url)
        {
            var tempFile = Path.GetTempFileName();

            using (var fileStream = File.OpenWrite(tempFile))
            using (var httpClient = new HttpClient())
            {
                var httpStream = await httpClient.GetStreamAsync(url);
                await httpStream.CopyToAsync(fileStream);
            }

            return tempFile;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                NotificationProvider.Show("Lensert Closing", "An unhandled error occured please send the log file to the dev.\r\n");

                var exception = e.ExceptionObject as Exception;
                if (exception == null)
                    _logger.Fatal($"ExceptionObject is not an exception: {e.ExceptionObject}");
                else
                    _logger.Fatal(exception, "Unhandled exception");
            }
            catch {}

            Environment.Exit(-1);
        }
    }
}
