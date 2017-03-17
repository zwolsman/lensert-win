using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Lensert.Core.Screenshot;
using Lensert.Core.Screenshot.Factories;
using Lensert.Helpers;
using NLog;
using Shortcut;

namespace Lensert.Core
{
    internal sealed class LensertHotkeyHandler : IHotkeyHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string _backupDirectory = Path.Combine(Settings.InstallationDirectory, "backup");

        private static readonly IDictionary<SettingType, Type> _hotkeyDictionary = new Dictionary<SettingType, Type>
        {
            [SettingType.FullscreenHotkey] = typeof(FullScreenshot),
            [SettingType.SelectAreaHotkey] = typeof(UserSelectionScreenshot),
            [SettingType.SelectWindowHotkey] = typeof(SelectWindowScreenshot),
            [SettingType.CurrentWindowHotkey] = typeof(CurrentWindowScreenshot)
        };

        private readonly IImageUploader _imageUploader;

        public LensertHotkeyHandler(IImageUploader imageUploader)
        {
            _imageUploader = imageUploader;
            if (!Directory.Exists(_backupDirectory))
                Directory.CreateDirectory(_backupDirectory);
        }

        public async void HandleHotkey(HotkeyPressedEventArgs eventArgs)
        {
            var settingType = Settings.GetSettingType(eventArgs.Hotkey);
            var type = _hotkeyDictionary[settingType];

            _logger.Info($"Hotkey pressed: {eventArgs.Hotkey} ({settingType})");
            //            _binder.HotkeysEnabled = false;

            using (var screenshot = ScreenshotFactory.Create(type))
            {
                //_binder.HotkeysEnabled = true;
                if ((screenshot == null) || (screenshot.Size.Width <= 1) || (screenshot.Size.Height <= 1))
                    return;

                string link = null;

                try
                {
                    // upload to the server
                    link = await _imageUploader.UploadImageAsync(screenshot);
                    if (string.IsNullOrEmpty(link))
                    {
                        _logger.Error("UploadImageAsync did not return a valid link");
                        NotificationProvider.Show("Upload failed", "Uploading the screenshot failed", LogFile.Open);
                    }
                    else
                    {
                        _logger.Info($"Image uploaded {link}");
                        NotificationProvider.Show("Upload complete", link, () => Process.Start(link), -1); // priority: -1 -> always get overwritten even by itself (spamming lensert e.g.)
                        Clipboard.SetDataObject(link, false, 10, 200);
                    }
                }
                catch (HttpRequestException)
                {
                    NotificationProvider.Show(
                        "Upload failed :(",
                        "Your machine seems to be offline. Don't worry your screenshot was saved localy and will be uploaded when you re-connect.");
                }
                catch (ExternalException)
                {
                    NotificationProvider.Show(
                        "Clipboard Error",
                        "Lensert failed to copy the link to the clipboard");
                }

                if (!Settings.GetSetting<bool>(SettingType.SaveBackup))
                    return;

                try
                {
                    var lensertId = link != null
                        ? link.Split('/').Last()
                        : "failed";

                    var filename = Path.Combine(_backupDirectory, $"{DateTime.Now:ddMMyyHHmmssfff}-{lensertId}.png");
                    screenshot.Save(filename, ImageFormat.Png);
                }
                catch (Exception)
                {
                    NotificationProvider.Show(
                           "Backup Error",
                           "Lensert failed to save the last screenshot");
                }
            }
        }
    }
}