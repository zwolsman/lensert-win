using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Lensert.Core;
using Lensert.DependencyInjection;
using Lensert.Helpers;
using NLog;
using Timer = System.Threading.Timer;
using System.IO.Compression;
using System.Linq;

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
            MainImpl();
#else
            try
            {
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                MainImpl();
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Unhandled exception");
                Environment.Exit(-1);
            }
#endif
        }

        private static void MainImpl()
        {
#if DEBUG
            {
                var consoleRule = LogManager.Configuration.LoggingRules.First(r => r.Targets.Any(t => t.Name == "console"));
                consoleRule.EnableLoggingForLevel(LogLevel.Debug);
            }
#endif

            _logger.Info("Lensert started");

            if (IsAlreadyRunning())
            {
                _logger.Warn("Lensert instance already running, exiting..");
                return;
            }

#if !DEBUG
            if (Settings.GetSetting<bool>(SettingType.CheckForUpdates))
                new Timer(UpdateRoutine, null, TimeSpan.Zero, TimeSpan.FromHours(1));
#endif

            if (Settings.GetSetting<bool>(SettingType.StartupOnLogon))
                CreateStartupLink();

            var uploader = KernelFactory.Resolve<IHotkeyHandler>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HotkeyForm(uploader));
        }

        private static async void UpdateRoutine(object state)
        {
            var file = await DownloadFileToTemp(LENSERT_URL);
            var updateDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert-installer");

            if (Directory.Exists(updateDirectory))
                Directory.Delete(updateDirectory, true);

            ZipFile.ExtractToDirectory(file, updateDirectory);

            file = Path.Combine(updateDirectory, "lensert-installer.exe");
            if (File.Exists(file))
                Process.Start(file);
            else
                _logger.Error("Extracted updater not found!");
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

        private static bool IsAlreadyRunning()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyAttributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
                var guidAttribute = (GuidAttribute) assemblyAttributes.GetValue(0);
                var mutexName = $"Global\\{{{guidAttribute.Value}}}";

                var mutex = new Mutex(false, mutexName);
                return !mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch
            {
                return true;
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
    }
}
