using System;
using System.Collections.Generic;
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

                StartLensert();
            }
#endif
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                Trace.TraceError(exception?.ToString() ?? $"ExceptionObject is not an exception: {e.ExceptionObject}");

                StartLensert();
            }
            catch { }

            Environment.Exit(-1);
        }

        private static async Task MainImpl(string[] args)
        {
            var logDirectory = Path.GetDirectoryName(_traceFileName);
            if (logDirectory != null && !Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            
            Trace.Listeners.Add(new TextWriterTraceListener(_traceFileName));
            Trace.AutoFlush = true;

            Trace.TraceInformation($"'{Environment.CommandLine}' started");

            if (IsAlreadyRunning())
            {
                Trace.TraceWarning("lensert-installer is already running, exiting..");
                return;
            }

            var forceCheckForUpdates = args.Length == 1 && args[0] == "--force-update";
            if (!forceCheckForUpdates && !ShouldCheckForUpdates())
            {
                Trace.TraceInformation("already checked for updates in last hour, exiting..");
                if (!StartLensert())
                    Trace.TraceError("failed to start lensert-daemon");

                return;
            }
            
            Trace.TraceInformation("downloading lensert-win.zip..");
            var file = await DownloadFileToTemp(URL_LENSERT_ZIP);
            Trace.TraceInformation($"downloaded new zip file to {file}");

            var unpacked = Path.Combine(_lensertDirectory, "installer", "unpacked");
            if (Directory.Exists(unpacked))
                Directory.Delete(unpacked, true);
            ZipFile.ExtractToDirectory(file, unpacked);

            var unpackedDirectoryInfo = new DirectoryInfo(unpacked);
            var installDirectoryInfo = new DirectoryInfo(_lensertDirectory);

            if (!unpackedDirectoryInfo.Exists)
            {
                Trace.TraceError($"Unpacked directory '{unpackedDirectoryInfo} does not exist");
                return;
            }

            if (!installDirectoryInfo.Exists)
            {
                Trace.TraceError($"Install directory '{installDirectoryInfo} does not exist");
                return;
            }

            var extensions = new[] { ".exe", ".dll", ".config" };
            var newFiles = GetFilesWithExtensions(extensions, unpackedDirectoryInfo).ToArray();
            var oldFiles = GetFilesWithExtensions(extensions, installDirectoryInfo).ToArray();
            var freshInstall = oldFiles.Length == 0;

            var shouldUpdate = newFiles.Length != oldFiles.Length || !newFiles.SequenceEqual(oldFiles);
            if (!shouldUpdate)
            {
                Trace.TraceInformation("all local files are up to date with server files");
                Directory.Delete(unpacked, true);
                return;
            }

            var oldExecutables = oldFiles.Where(f => f.Extension == ".exe").Select(f => f.Name.Replace(".exe", ""));
            var tasks = oldExecutables.Select(StopProcess);
            var results = await Task.WhenAll(tasks.ToArray());

            // if any failed to stop
            if (results.Any(r => r == false))
            {
                Trace.TraceError("unable to stop lensert");
                return;
            }

            foreach (var f in oldFiles)
            {
                try
                {
                    // make sure to delete readonly
                    f.Attributes = FileAttributes.Normal;
                    f.Delete();
                }
                catch { }
            }

            foreach (var f in newFiles)
                f.MoveTo(Path.Combine(_lensertDirectory, f.Name));
            
            StartLensert(true, freshInstall);

            Trace.TraceInformation("lensert-installer complete");
        }

        private static IEnumerable<FileInfo> GetFilesWithExtensions(IEnumerable<string> extensions, DirectoryInfo directoryInfo)
            => extensions.SelectMany(e => directoryInfo.GetFiles($"*{e}").Where(f => f.Extension.Equals(e, StringComparison.InvariantCultureIgnoreCase)));

        private static bool StartLensert(bool updated = false, bool freshInstall = false)
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

            var args = freshInstall
                ? "--fresh"
                : updated
                    ? "--updated"
                    : "";

            var startInfo = new ProcessStartInfo(file, args);
            Process.Start(startInfo);

            return true;
        }
        
        private static async Task<bool> StopProcess(string processName)
        {
            for (var i = 5; i > 0; --i)
            {
                var processes = Process.GetProcessesByName(processName);
                if (!processes.Any())
                    return true;

                foreach (var process in processes)
                    process.Kill();

                await Task.Delay(100);
            }

            return !Process.GetProcessesByName(processName).Any();
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
