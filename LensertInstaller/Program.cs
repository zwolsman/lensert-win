using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LensertInstaller
{
    class Program
    {
        private readonly string[] _files;
        private readonly string _appdata;   

        public Program()
        {
            _files = new[] {"Shortcut.dll", "Lensert.exe", "log4net.dll", "log.txt", "lensert-win.exe"};

            _appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lensert");
            if (!Directory.Exists(_appdata))         // I believe log4net already make this folder -> sanity check
                Directory.CreateDirectory(_appdata);

            Uninstall(false);
            ZipFile.ExtractToDirectory(@"C:\Users\joell\Dropbox\Programming\Lensert\lensert-win\Lensert\bin\Debug\lensert-win2.zip", _appdata);
        }

        private void Uninstall(bool keepSettings)
        {
            foreach (var file in _files)
                TryDelete(Path.Combine(_appdata, file));

            if (!keepSettings)
                TryDelete(Path.Combine(_appdata, "Settings.ini"));
        }

        private async Task<string> DownloadFileToTemp(string url)
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

        private bool TryDelete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            var program = new Program();

        }
    }
}
