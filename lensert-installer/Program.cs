using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lensert.Installer
{
    internal class Program
    {
        private const string URL_LENSERT_ZIP = "https://lensert.com/download?type=win&installer=false";
        private const string URL_LENSERT_VERSION = "https://lensert.com/version?type=win";
        private static readonly string _installationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lensert");

        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "-v")
                Trace.Listeners.Add(new ConsoleTraceListener());

#if DEBUG
            MainImpl(args).Wait();
#else
            try
            {
                MainImpl(args).Wait();
            }
            catch
            {
            }
#endif
        }

        private static async Task MainImpl(string[] args)
        {
            Trace.TraceInformation("lensert-updater started");

            var version = new Version(await DownloadString(URL_LENSERT_VERSION));
            Trace.TraceInformation($"server version: {version}");

            var file = Path.Combine(_installationDirectory, "lensert.exe");
            if (File.Exists(file))
            {
                var localVersion = new Version(FileVersionInfo.GetVersionInfo(file).FileVersion);
                Trace.TraceInformation($"local lensert version: {localVersion}");

                if (localVersion >= version)
                {
                    Trace.TraceInformation("latest version, bye");
                    return;
                }
            }

            Trace.TraceInformation("downloading lensert..");
            file = await DownloadFileToTemp(URL_LENSERT_ZIP);
            Trace.TraceInformation($"downloaded new zip file to {file}");

            // we weren't able to shutdown lensert..
            if (!await KillLensert())
            {
                Trace.TraceError("unable to kill running lensert");
                return;
            }

            var directoryInfo = new DirectoryInfo(_installationDirectory);
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
            ZipFile.ExtractToDirectory(file, _installationDirectory);

            Trace.TraceInformation("starting lensert-daemon..");
            file = Path.Combine(_installationDirectory, "lensert-daemon.exe");
            Process.Start(file);

            Trace.TraceInformation("lensert-updater complete :)");
        }

        private static async Task<bool> KillLensert()
        {
            for (var i = 5; i > 0; --i)
            {
                var processes = Process.GetProcessesByName("lensert");
                if (!processes.Any())
                    return true;

                foreach (var process in processes)
                    process.Kill();

                await Task.Delay(100);
            }

            return !Process.GetProcessesByName("lensert").Any();
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
    }
}
