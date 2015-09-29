using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shortcut;
using Lensert.Classes;

namespace Lensert.Forms
{
    public partial class MainForm : Form
    {
        public static HotkeyBinder HotkeyBinder;
        public static LensertClient Client;

        private readonly bool _hotkeyEnabled;
        public MainForm()
        {
            HotkeyBinder = new HotkeyBinder();


            Client = new LensertClient(Preferences.Default.Username, Preferences.Default.Password);
            Client.StateHandler += (sender, args) => myImagesToolStripMenuItem.Visible = Client.IsAuthorized;
            Application.ApplicationExit += (sender, e) => notifyIcon.Visible = false;


            _hotkeyEnabled = true;

            InitializeComponent();
            InitializeHotkeys();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        void InitializeHotkeys()
        { 
            if (!HotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectFullscreen))
                HotkeyBinder.Bind(Preferences.Default.HotkeySelectFullscreen).To(() => HotkeyHandler(ScreenshotType.Fullscreen));

            if (!HotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectArea))
                HotkeyBinder.Bind(Preferences.Default.HotkeySelectArea).To(() => HotkeyHandler(ScreenshotType.Area));

            if (!HotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectCurrentWindow))
                HotkeyBinder.Bind(Preferences.Default.HotkeySelectCurrentWindow).To(() => HotkeyHandler(ScreenshotType.CurrentWindow));

            if (!HotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectWindow))
                HotkeyBinder.Bind(Preferences.Default.HotkeySelectWindow).To(() => HotkeyHandler(ScreenshotType.SelectWindow));

            if (!HotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeyClipboard))
                HotkeyBinder.Bind(Preferences.Default.HotkeyClipboard).To(() => HotkeyHandler(ScreenshotType.Clipboard));


            captureImageToolStripMenuItem.ShortcutKeyDisplayString = Preferences.Default.HotkeySelectArea.ToString().Replace(",", " +");
        }

        private async void HotkeyHandler(ScreenshotType type)
        {
            if (!_hotkeyEnabled)
                return;

            var screenshot = ScreenshotProvider.GetScreenshot(type);
            if (screenshot == null)
                return;

            var link = await Client.UploadImageAsync(screenshot);

            Console.WriteLine($"Got link '{link}'");
            notifyIcon.ShowBalloonTip(500);
            /*NotificationProvider.Show(link);*/

            if (Preferences.Default.CopyToClipboard)                                                                                         
                Clipboard.SetText(link);                                                                                                     
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreferences();
        }

        private void myImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Client.IsAuthorized)
                Process.Start($"http://lnsrt.me/user/{Client.Username}");
        }

        private void captureImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HotkeyHandler(ScreenshotType.Area);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowPreferences();
        }


        private void ShowPreferences()
        {
             using (var frm = new PreferencesForm())
            {
                frm.ShowDialog();
            }

            InitializeHotkeys();
        }
    }
}
