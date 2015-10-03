using Shortcut;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert
{
    public partial class MainForm : Form
    {
        private readonly PreferencesForm _preferencesForm;
        
        private readonly HotkeyBinder _hotkeyBinder;
        private LensertClient _client;

        public MainForm()
        {
            InitializeComponent();

            components.Add(NotificationProvider.NotifyIcon);

            _preferencesForm = new PreferencesForm();
            _preferencesForm.AccountChanged += PreferencesForm_AccountChanged;
            _preferencesForm.HotkeyChanged += PreferencesForm_HotkeyChanged;

            NotificationProvider.PreferencesForm = _preferencesForm;

            _hotkeyBinder = new HotkeyBinder();
            if (Preferences.Default.RememberMe)
                Login();

            InitializeHotkeys();
        }

        private void Login()
        {
            _preferencesForm.Login(Preferences.Default.Username, Preferences.Default.Password);
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

        private void PreferencesForm_AccountChanged(object sender, AccountEventArgs e)
        {
            _client = e.LensertClient;

            if (_client != null)
                myImagesToolStripMenuItem.Visible = _client.LoggedIn;
        }

        private async void OnHotkeyPressed(HotkeyPressedEventArgs hotkeyEventArgs)
        {
            var type = hotkeyEventArgs.Hotkey.GetScreenshotType();
            await ScreenshotHandler(type);
        }

        private async Task<string> ScreenshotHandler(ScreenshotType type)
        {
            var screenshot = ScreenshotProvider.GetScreenshot(type);
            if (screenshot == null)
                return null;

            var link = await _client.UploadImageAsync(screenshot);

            Console.WriteLine($"Got link '{link}'");

            if (Preferences.Default.ShowNotification)
                NotificationProvider.Show(link);

            if (Preferences.Default.CopyToClipboard)                                                                                         
                Clipboard.SetText(link);

            screenshot?.Dispose();
            return link;
        }

        void InitializeHotkeys()
        {
            var hotkeys = Utils.SettingsOfType(typeof(Hotkey))
                .Select(setting => (Hotkey)setting.PropertyValue)
                .Where(hotkey => !_hotkeyBinder.IsHotkeyAlreadyBound(hotkey));

            foreach (var hotkey in hotkeys)
                _hotkeyBinder.Bind(hotkey, OnHotkeyPressed);
        }

        private void myImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_client.LoggedIn)
                Process.Start($"http://lnsrt.me/user/{_client.Username}");
        }

        private async void captureImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ScreenshotHandler(ScreenshotType.Area);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenPreferences_UI(object sender, EventArgs e)
        {
            if (!_preferencesForm.Visible)
            _preferencesForm.ShowDialog();
        }

    }
}
