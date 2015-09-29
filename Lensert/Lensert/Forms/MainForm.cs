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

            _preferencesForm = new PreferencesForm();
            _preferencesForm.AccountChanged += PreferencesForm_AccountChanged;
            _preferencesForm.HotkeyChanged += PreferencesForm_HotkeyChanged;

            _hotkeyBinder = new HotkeyBinder();
            _client = new LensertClient(Preferences.Default.Username, Preferences.Default.Password);

            if (Preferences.Default.RememberMe)
                _client.Login();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void PreferencesForm_HotkeyChanged(object sender, HotkeyEventArgs e)
        {
            _hotkeyBinder.Unbind(e.OldHotkey);

            InitializeHotkeys();
        }

        private void PreferencesForm_AccountChanged(object sender, AccountEventArgs e)
        {
            _client = e.LensertClient;

            myImagesToolStripMenuItem.Visible = _client.LoggedIn;
        }

        private async void HotkeyHandler(ScreenshotType type)
        {
            //if (!_hotkeyEnabled)
            //    return;

            var screenshot = ScreenshotProvider.GetScreenshot(type);
            if (screenshot == null)
                return;

            var link = await _client.UploadImageAsync(screenshot);

            Console.WriteLine($"Got link '{link}'");

            if(Preferences.Default.ShowNotification)
                NotificationProvider.Show(link);

            if (Preferences.Default.CopyToClipboard)                                                                                         
                Clipboard.SetText(link);                                                                                                     
        }

        void InitializeHotkeys()
        {
           
            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectFullscreen))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectFullscreen).To(() => HotkeyHandler(ScreenshotType.Fullscreen));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectArea))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectArea).To(() => HotkeyHandler(ScreenshotType.Area));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectCurrentWindow))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectCurrentWindow).To(() => HotkeyHandler(ScreenshotType.CurrentWindow));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectWindow))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectWindow).To(() => HotkeyHandler(ScreenshotType.SelectWindow));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeyClipboard))
                _hotkeyBinder.Bind(Preferences.Default.HotkeyClipboard).To(() => HotkeyHandler(ScreenshotType.Clipboard));
        }

        private void myImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_client.LoggedIn)
                Process.Start($"http://lnsrt.me/user/{_client.Username}");
        }

        private void captureImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HotkeyHandler(ScreenshotType.Area);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenPreferences_UI(object sender, EventArgs e)
        {
            _preferencesForm.ShowDialog();
        }
    }
}
