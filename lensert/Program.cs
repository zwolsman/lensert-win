using System;
using System.Windows.Forms;
using Lensert.Core;
using Lensert.DependencyInjection;
using Ninject;
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

            if (!AssemblyManager.HandleStartup())
            {
                _logger.Warn("Handle startup says not to start this instance.");
                return;
            }

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
    }
}
