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
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
        private static HotkeyBinder _binder;
        
        [STAThread]
        public static void Main()
        {
            if (IsInstanceRunning())
                return;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

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

            var failedHotkeys = new List<SettingType>();
            foreach (var hotkeySetting in hotkeySettings)
            {
                var hotkey = Settings.GetSetting<Hotkey>(hotkeySetting.Value);
                if (_binder.IsHotkeyAlreadyBound(hotkey))
                {
                    failedHotkeys.Add(hotkeySetting.Value);
                    _log.Warn($"Hotkey {hotkeySetting.Value} is already bound. Therefor the hotkey will not be set.");
                    continue;
                }

                _binder.Bind(hotkey, args => HandleHotkey(hotkeySetting.Key));
            }

            if (failedHotkeys.SequenceEqual(hotkeySettings.Select(s => s.Value)))
            {
                _log.Fatal("All hotkeys failed to bind. Exiting..");
                NotificationProvider.Show("Lensert Closing", "All hotkeys failed to bind");
                Environment.Exit(0);
            }
            else if (failedHotkeys.Any())
            {
                var message = $"Failed to bind: {string.Join(", ", failedHotkeys)}";
                NotificationProvider.Show("Error", message, Util.OpenLog);
            }
        }

        private static async void HandleHotkey(Type template)
        {
            _log.Info($"Hotkey Handler: {template}..");
            var screenshot = ScreenshotFactory.Create(template);    
            if (screenshot == null || screenshot.Size.Width <= 1 || screenshot.Size.Height <= 1)
                return;

            try
            {
                var link = await LensertClient.UploadImageAsync(screenshot);
                if (string.IsNullOrEmpty(link))
                {
                    _log.Error("UploadImageAsync did not return a valid link");
                    NotificationProvider.Show("Upload failed", "Uploading the screenshot failed", Util.OpenLog);
                }
                else
                {
                    _log.Info($"Image uploaded {link}");
                    NotificationProvider.Show("Upload complete", link, () => Process.Start(link), -1);      // priority: -1 -> always get overwritten even by itself (spamming lensert e.g.)
                    Clipboard.SetText(link);
                }
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

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Fatal($"Unhandled exception: {e.ExceptionObject}");
            NotificationProvider.Show("Lensert Closing", "An unhandled error occured please send the log file to the dev.\r\n");
        }

        private sealed class SecretForm : Form
        {
            public SecretForm()
            {
                BindHotkeys();
                _log.Info("Lensert started");

            }

            protected override void SetVisibleCore(bool value)
            {
                base.SetVisibleCore(false);
            }
        }
    }
}
