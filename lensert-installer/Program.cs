using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Lensert.Installer
{
    internal class Program
    {
        private const string URL_LENSERT_ZIP = "https://lensert.com/download?type=win&installer=false";
        private static readonly string _lensertDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert");
        private static readonly string _traceFileName = Path.Combine(_lensertDirectory, "logs", "lensert-installer.log");

        private static void Main(string[] args)
        {
#if DEBUG
            MainImpl(args).Wait();
#else
            try
            {
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                MainImpl(args).Wait();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
#endif
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                Trace.TraceError(exception?.ToString() ?? $"ExceptionObject is not an exception: {e.ExceptionObject}");
            }
            catch { }

            Environment.Exit(-1);
        }

        private static async Task MainImpl(string[] args)
        {
            var directory = Path.GetDirectoryName(_traceFileName);
            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new TextWriterTraceListener(_traceFileName));
            Trace.AutoFlush = true;

            if (IsAlreadyRunning())
            {
                Trace.TraceWarning("lensert-installer is already running, exiting..");
                return;
            }

            var forceCheckForUpdates = args.Length == 1 && args[0] == "--force-update";
            if (!forceCheckForUpdates && !ShouldCheckForUpdates())
            {
                Trace.TraceInformation("already checked for updates in last hour, exiting..");
                if (!StartDaemon())
                    Trace.TraceError("failed to start lensert-daemon");

                return;
            }
            
            Trace.TraceInformation("lensert-installer started");
            Trace.TraceInformation("downloading lensert-win.zip..");
            var file = await DownloadFileToTemp(URL_LENSERT_ZIP);
            Trace.TraceInformation($"downloaded new zip file to {file}");

            // we weren't able to shutdown lensert..
            if (!await StopDaemon())
            {
                Trace.TraceError("unable to stop running lensert-daemon");
                return;
            }

            var directoryInfo = new DirectoryInfo(_lensertDirectory);
            if (directoryInfo.Exists)
            {
                Trace.TraceInformation("deleting previous lensert");

                var extensionsToDelete = new[] {".exe", ".dll", ".config"};
                var filesToDelete = extensionsToDelete.SelectMany(e => directoryInfo.GetFiles($"*{e}").Where(f => f.Extension.Equals(e, StringComparison.InvariantCultureIgnoreCase)));
                foreach (var f in filesToDelete)
                {
                    try
                    {
                        // make sure to delete readonly
                        f.Attributes = FileAttributes.Normal;
                        f.Delete();
                    }
                    catch {}
                }
            }

            Trace.TraceInformation("extracting lensert..");
            ZipFile.ExtractToDirectory(file, _lensertDirectory);
            
            StartDaemon();

            Trace.TraceInformation("lensert-installer complete");
        }

        private static bool StartDaemon()
        {
            Trace.TraceInformation("starting lensert-daemon..");
            if (Process.GetProcessesByName("lensert-daemon").Any())
                return true;

            var file = Path.Combine(_lensertDirectory, "lensert-daemon.exe");
            if (!File.Exists(file))
            {
                Trace.TraceError($"lensert-daemon not found at {file}");
                return false;
            }

            Process.Start(file);
            return true;
        }

        private static async Task<bool> StopDaemon()
        {
            for (var i = 5; i > 0; --i)
            {
                var processes = Process.GetProcessesByName("lensert-daemon");
                if (!processes.Any())
                    return true;

                foreach (var process in processes)
                    process.Kill();

                await Task.Delay(100);
            }

            return !Process.GetProcessesByName("lensert-daemon").Any();
        }

        private static bool ShouldCheckForUpdates()
        {
            var logFileInfo = new FileInfo(_traceFileName);
            if (!logFileInfo.Exists)
                return true;

            // should check if last update time has been more than one hour
            return (DateTime.UtcNow - logFileInfo.LastWriteTimeUtc).Hours > 1;
        }
        
        private static async Task<string> DownloadString(string url)
        {
            using (var httpClient = new HttpClient())
                return await httpClient.GetStringAsync(url);
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
