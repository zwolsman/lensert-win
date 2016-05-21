using Lensert.Screenshot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Shortcut;

namespace Lensert
{
    internal static class Program
    {
        private static readonly ILog _log = LogManager.GetLogger("Startup");
        private static HotkeyBinder _binder;
        
        [STAThread]
        public static void Main()
        {
            if (IsInstanceRunning())
                return;

            CreateStartupLink();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SecretForm());
        }

        private static void BindHotkeys()
        {
            _binder = new HotkeyBinder();
            
            var hotkeySettings = new Dictionary<Type, SettingType>
            {
                [typeof(UserSelectionTemplate)] = SettingType.SelectAreaHotkey,
                [typeof(SelectWindowTemplate)] = SettingType.SelectWindowHotkey,
                [typeof(FullScreenTemplate)] = SettingType.FullscreenHotkey
            };

            foreach (var hotkeySetting in hotkeySettings)
            {
                var hotkey = Settings.GetSetting<Hotkey>(hotkeySetting.Value);
                if (hotkey == default(Hotkey))
                {
                    //_log.Warn($"Hotkey {hotkeySetting.Key} couldn't not be fetched from settings. Therefor the hotkey will not be set.");
                    continue;
                }
                if (_binder.IsHotkeyAlreadyBound(hotkey))
                {
                    //_log.Warn($"Hotkey {hotkeySetting.Key} is already bound. Therefor the hotkey will not be set.");
                    continue;
                }

                _binder.Bind(hotkey, args => HandleHotkey(hotkeySetting.Key));
            }
        }

        private static async void HandleHotkey(Type template)
        {
            var screenshot = ScreenshotFactory.Create(template);    
            if (screenshot == null || screenshot.Size.Width <= 1 || screenshot.Size.Height <= 1)
                return;

            try
            {
                var link = await LensertClient.UploadImageAsync(screenshot);

                Console.WriteLine($"Got link '{link}'");

                NotificationProvider.Show(
                    "Upload complete",
                    link,
                    () => Process.Start(link));

                Clipboard.SetText(link);
            }
            catch (HttpRequestException)
            {
                NotificationProvider.Show(
                    "Upload failed :(",
                    "Your machine seems to be offline. Don't worry your screenshot was saved localy and will be uploaded when you re-connect.");
            }
            finally
            {
                screenshot.Dispose();
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

        private static bool IsInstanceRunning()
        {
            try
            {
                var mutex = new Mutex(false, MutexName());
                return !mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch
            {
                return true;
            }
        }

        private static string MutexName() => $"Global\\{{{ResolveAssemblyGuid()}}}";

        private static string ResolveAssemblyGuid()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyAttributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
                var guidAttribute = (GuidAttribute)assemblyAttributes.GetValue(0);
                return guidAttribute.Value;
            }
            catch
            {
                throw new InvalidOperationException(
                    "Ensure there is a Guid attribute defined for this assembly.");
            }
        }

        private sealed class SecretForm : Form
        {
            public SecretForm()
            {
                BindHotkeys();
                NotificationProvider.Show();
                _log.Info("Lensert started");
            }

            protected override void SetVisibleCore(bool value)
            {
                base.SetVisibleCore(false);
            }
        }
    }
}
