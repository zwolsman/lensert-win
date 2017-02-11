using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Lensert.Core.Screenshot;
using Lensert.Helpers;
using NLog;
using Shortcut;

namespace Lensert.Core
{
    internal sealed class HotkeyForm : Form
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly HotkeyBinder _binder;
        private readonly IHotkeyHandler _hotkeyHandler;

        public HotkeyForm(IHotkeyHandler hotkeyHandler)
        {
            _binder = new HotkeyBinder();
            _hotkeyHandler = hotkeyHandler;

            BindHotkeys();
            NotificationProvider.ShowIcon();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void BindHotkeys()
        {
            var hotkeySettings = Settings.GetSettings<Hotkey>().ToArray();
            
            var failedHotkeys = new List<SettingType>();
            foreach (var hotkeySetting in hotkeySettings)
            {
                if (_binder.IsHotkeyAlreadyBound(hotkeySetting.Value))
                {
                    failedHotkeys.Add(hotkeySetting.Key);
                    _logger.Warn($"Hotkey {hotkeySetting.Value} is already bound. Therefor the hotkey will not be set.");
                }
                else
                {
                    _binder.Bind(hotkeySetting.Value, _hotkeyHandler.HandleHotkey);
                }
            }

            if (!failedHotkeys.Any())
                return;

            if (failedHotkeys.SequenceEqual(hotkeySettings.Select(s => s.Key)))
            {
                _logger.Fatal("All hotkeys failed to bind. Exiting..");
                NotificationProvider.Show("Lensert Closing", "All hotkeys failed to bind");
                Environment.Exit(0);
            }
            else
            {
                var message = $"Failed to bind: {string.Join(", ", failedHotkeys)}";
                NotificationProvider.Show("Error", message, LogFile.Open);
            }
        }
    }
}
