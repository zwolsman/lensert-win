using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Lensert.Core;
using Lensert.DependencyInjection;
using Lensert.Helpers;
using NLog;

namespace Lensert
{
    internal static class Program
    {
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
            _logger.Info("Lensert started");

            if (IsAlreadyRunning())
            {
                _logger.Warn("Lensert instance already running, exiting..");
                return;
            }

            if (Settings.GetSetting<bool>(SettingType.StartupOnLogon))
                CreateStartupLink();

            var uploader = KernelFactory.Resolve<IHotkeyHandler>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HotkeyForm(uploader));
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
    }
}
