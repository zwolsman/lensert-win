using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime;
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
            if (args.Length == 1)
            {
                if (args[0] == "--restore-settings")
                {
                    Settings.Restore();
                }
                else if (args[0] == "--reset-settings")
                {
                    Settings.Reset();
                }
                else
                {
                    var hotkeySettings = Settings.GetSettings<string>().Where(keyValue => keyValue.Key.ToString().EndsWith("Hotkey"));
                    var settingType = hotkeySettings.Single(keyValue => keyValue.Key.ToString().StartsWith(args[0])).Key;

                    var hotkeyHandler = new LensertHotkeyHandler(new LensertClient());
                    await hotkeyHandler.HandleHotkey(settingType);
                }
            }
            else if (args.Length == 3 && args[0] == "--show-notification")
            {
                NotificationProvider.Show(args[1], args[2]);
            }
            else
                _logger.Warn("Lensert should be invoked by lensert-daemon");

            // still check for updates and startup
            await UpdateRoutine();

#if !DEBUG
            if (Settings.GetSetting<bool>(SettingType.CheckForUpdates))
                await UpdateRoutine();
#endif

            if (Settings.GetSetting<bool>(SettingType.StartupOnLogon))
                CreateStartupLink();

            var timeout = 10;
            while (--timeout > 0 && NotificationProvider.IsVisible())
                await Task.Delay(100);
        }

        private static async Task UpdateRoutine()
        {
            if (!ShouldCheckForUpdates())
                return;

            var file = await DownloadFileToTemp(LENSERT_URL);
            var updateDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert", "installer");

            if (Directory.Exists(updateDirectory))
                Directory.Delete(updateDirectory, true);

            ZipFile.ExtractToDirectory(file, updateDirectory);

            file = Path.Combine(updateDirectory, "lensert-installer.exe");
            if (!File.Exists(file))
                _logger.Error("Extracted updater not found!");

            var timeout = 5;
            var processInfo = new ProcessStartInfo(file)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            var process = Process.Start(processInfo);
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

        private static bool ShouldCheckForUpdates()
        {
            var installationTraceFile = Path.Combine(Settings.InstallationDirectory, "logs", "lensert-installer.log");
            var logFileInfo = new FileInfo(installationTraceFile);
            if (!logFileInfo.Exists)
                return true;

            // should check if last update time has been more than one hour
            return (DateTime.UtcNow - logFileInfo.LastWriteTimeUtc).Hours > 1;
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
