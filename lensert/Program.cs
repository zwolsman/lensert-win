using Lensert.Screenshot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using NLog;
using Shortcut;

namespace Lensert
{
    internal static class Program
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();
        private static HotkeyBinder _binder;
        
        [STAThread]
        public static void Main()
        {
            _log.Info("Lensert started");

            if (!AssemblyManager.HandleStartup())
            {
                _log.Info("Handle startup says not to start this instance.");
                return;
            }


#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
#endif

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
            _binder.HotkeysEnabled = false;

            var screenshot = ScreenshotFactory.Create(template);
            _binder.HotkeysEnabled = true;
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
        
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Fatal($"Unhandled exception: {e.ExceptionObject}");
            NotificationProvider.Show("Lensert Closing", "An unhandled error occured please send the log file to the dev.\r\n");
        }

        private sealed class SecretForm : Form
        {
            public SecretForm()
            {
                if (AssemblyManager.FirstLaunch)            // we secretly know we get killed
                    NotificationProvider.Show("Lensert", "Lensert now starts with Windows!");
                else
                    BindHotkeys();

                NotificationProvider.ShowIcon();
            }

            protected override void SetVisibleCore(bool value)
            {
                base.SetVisibleCore(false);
            }
        }
    }
}
