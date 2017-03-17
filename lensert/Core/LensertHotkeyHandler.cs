using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lensert.Core.Screenshot;
using Lensert.Core.Screenshot.Factories;
using Lensert.Helpers;
using NLog;

namespace Lensert.Core
{
    internal sealed class LensertHotkeyHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string _backupDirectory = Path.Combine(Settings.InstallationDirectory, "backup");

        private static readonly IDictionary<SettingType, Type> _hotkeyDictionary = new Dictionary<SettingType, Type>
        {
            [SettingType.FullscreenHotkey] = typeof(FullScreenshot),
            [SettingType.SelectAreaHotkey] = typeof(UserSelectionScreenshot),
            [SettingType.SelectWindowHotkey] = typeof(SelectWindowScreenshot),
            [SettingType.CurrentWindowHotkey] = typeof(CurrentWindowScreenshot),
            [SettingType.ClipboardHotkey] = typeof(ClipboardScreenshot)
        };

        private readonly IImageUploader _imageUploader;

        public LensertHotkeyHandler(IImageUploader imageUploader)
        {
            _imageUploader = imageUploader;
            if (!Directory.Exists(_backupDirectory))
                Directory.CreateDirectory(_backupDirectory);
        }

        public async Task HandleHotkey(SettingType settingType)
        {
            var type = _hotkeyDictionary[settingType];

            _logger.Info($"Processing : {settingType}");

            using (var screenshot = ScreenshotFactory.Create(type))
            {
                if (screenshot == null || screenshot.Size.Width <= 1 || screenshot.Size.Height <= 1)
                    return;

                string link = null;
                var failedOnUpload = false;

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
                    failedOnUpload = true;
                }
                catch (ExternalException)
                {
                    if (!string.IsNullOrEmpty(link))
                    {
                        NotificationProvider.Show(
                            "Clipboard Error",
                            "Lensert failed to copy the link to the clipboard",
                            () => Process.Start(link),
                            -1);
                    }
                }

                if (!Settings.GetSetting<bool>(SettingType.SaveBackup) && !failedOnUpload)
                    return;

                try
                {
                    var lensertId = link != null
                        ? link.Split('/').Last()
                        : "failed";

                    var filename = Path.Combine(_backupDirectory, $"{DateTime.Now:ddMMyyHHmmssfff}-{lensertId}.png");
                    screenshot.Save(filename, ImageFormat.Png);

                    if (failedOnUpload)
                    {
                        NotificationProvider.Show(
                            "Upload failed",
                            "Lensert was unable to upload the image to the server, but it has been saved locally. (click to show)",
                            () => Process.Start("explorer.exe", $"/select, \"{filename}\""));
                    }
                }
                catch (Exception)
                {
                    if (failedOnUpload)
                    {
                        NotificationProvider.Show(
                            "Lensert Error",
                            "Lensert failed both on uploading and creating a local backup. The last screenshot is lost for eternity..");
                    }
                    else
                    {
                        NotificationProvider.Show(
                            "Backup Error",
                            "Lensert failed to save the last screenshot");
                    }
                }
            }
        }
    }
}
