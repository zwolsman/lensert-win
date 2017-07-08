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
using Lensert.Core.ScreenshotHandler;
using Lensert.Helpers;
using NLog;

namespace Lensert.Core
{
    internal sealed class HotkeyHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string _backupDirectory = Path.Combine(Settings.InstallationDirectory, "backup");

        private static readonly IScreenshotHandler _clipboardHandler = new ClipboardHandler();
        private static readonly IScreenshotHandler _lensertUploader = new LensertUploader();

        private static readonly IDictionary<SettingType, Type> _factoryTypeDictionary = new Dictionary<SettingType, Type>
        {
            [SettingType.FullscreenHotkey] = typeof(FullScreenshot),
            [SettingType.SelectAreaHotkey] = typeof(UserSelectionScreenshot),
            [SettingType.SelectWindowHotkey] = typeof(SelectWindowScreenshot),
            [SettingType.ClipboardHotkey] = typeof(ClipboardScreenshot),
            [SettingType.FullscreenClipboardHotkey] = typeof(FullScreenshot),
            [SettingType.SelectAreaClipboardHotkey] = typeof(UserSelectionScreenshot),
            [SettingType.SelectWindowClipboardHotkey] = typeof(SelectWindowScreenshot),
        };

        private static readonly IDictionary<SettingType, IScreenshotHandler> _hotkeyHandlerDictionary = new Dictionary<SettingType, IScreenshotHandler>
        {
            [SettingType.FullscreenHotkey] = _lensertUploader,
            [SettingType.SelectAreaHotkey] = _lensertUploader,
            [SettingType.SelectWindowHotkey] = _lensertUploader,
            [SettingType.ClipboardHotkey] = _lensertUploader,
            [SettingType.FullscreenClipboardHotkey] = _clipboardHandler,
            [SettingType.SelectAreaClipboardHotkey] = _clipboardHandler,
            [SettingType.SelectWindowClipboardHotkey] = _clipboardHandler,
        };
        
        public HotkeyHandler()
        {
            if (!Directory.Exists(_backupDirectory))
                Directory.CreateDirectory(_backupDirectory);
        }

        public async Task HandleHotkey(SettingType settingType)
        {
            Type factoryType;
            if (!_factoryTypeDictionary.TryGetValue(settingType, out factoryType))
                throw new ArgumentException($"Hotkey '{settingType}' failed to resolve to a factory type", nameof(settingType));

            IScreenshotHandler handler;
            if (!_hotkeyHandlerDictionary.TryGetValue(settingType, out handler))
                throw new ArgumentException($"Hotkey '{settingType}' failed to resolve to a screenshot handler", nameof(settingType));

            _logger.Info($"Processing : {settingType}");

            using (var screenshot = ScreenshotFactory.Create(factoryType))
            {
                if (screenshot == null || screenshot.Size.Width <= 1 || screenshot.Size.Height <= 1)
                    return;

                var identifier = await handler.HandleAsync(screenshot);
                
                if (!Settings.GetSetting<bool>(SettingType.SaveBackup))
                    return;

                try
                {
                    var filename = Path.Combine(_backupDirectory, $"{DateTime.Now:ddMMyyHHmmssfff}-{identifier ?? "failed"}.png");
                    screenshot.Save(filename, ImageFormat.Png);

                    if (identifier == null)
                    {
                        NotificationProvider.Show(
                            "Upload failed",
                            "Lensert was unable to upload the image to the server, but it has been saved locally. (click to show)",
                            () => Process.Start("explorer.exe", $"/select, \"{filename}\""));
                    }
                }
                catch (Exception)
                {
                    if (identifier == null)
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
