using Lensert.Screenshot;
using Shortcut;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Clipboard = System.Windows.Forms.Clipboard;

namespace Lensert
{
    public partial class MainForm : Form
    {
        private readonly HotkeyBinder _hotkeyBinder;

        private LensertClient _client;

        public MainForm()
        {
            InitializeComponent();
            
            _hotkeyBinder = new HotkeyBinder();

            components.Add(NotificationProvider.NotifyIcon);

            InitializeHotkeys();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void PreferencesForm_HotkeyChanged(object sender, HotkeyEventArgs e)
        {
            Console.WriteLine($"Removing {e.OldHotkey}");
            _hotkeyBinder.Unbind(e.OldHotkey);

            InitializeHotkeys();
        }

        private async void OnHotkeyPressed(HotkeyPressedEventArgs hotkeyEventArgs)
        {
            var type = hotkeyEventArgs.Hotkey.GetScreenshotType();
            await ScreenshotHandler(type);
        }

        private async Task ScreenshotHandler(Type type)
        {
            var screenshot = ScreenshotProvider.Create(type);
            if (screenshot == null)
                return;

            try
            {
                var link = await _client.UploadImageAsync(screenshot);

                Console.WriteLine($"Got link '{link}'");

                if (Preferences.Default.ShowNotification)   // TODO: new settings
                {
                    NotificationProvider.Show(
                        "Succesful Upload!",
                        "Your image was uploaded. Click here to open it.",
                        () => Process.Start(link));
                }

                if (Preferences.Default.CopyToClipboard)
                    Clipboard.SetText(link);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Probably offline..");

                NotificationProvider.Show(
                    "Upload failed :(",
                    "Your machine seems to be offline. Don't worry your screenshot was saved localy and will be uploaded when you re-connect.");
            }
            finally
            {
                screenshot?.Dispose();
            }
        }

        void InitializeHotkeys()
        {
            var hotkeys = Utils.SettingsOfType(typeof(Hotkey))
                .Select(setting => (Hotkey)setting.PropertyValue)
                .Where(hotkey => !_hotkeyBinder.IsHotkeyAlreadyBound(hotkey));

            foreach (var hotkey in hotkeys)
                _hotkeyBinder.Bind(hotkey, OnHotkeyPressed);
        }
        
        private async void captureImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ScreenshotHandler(typeof(SelectArea));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
