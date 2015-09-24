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
using Lensert.Classes;
using Lensert.Forms;
using Lensert.Properties;
using Shortcut;

namespace Lensert
{
    public partial class MainForm : Form
    {
        private HotkeyBinder _hotkeyBinder;
        private readonly LensertClient _client;

        public MainForm()
        {
            InitializeComponent();
            InitializeHotkeys();

            label1.Text = $"Select Area: {Preferences.Default.HotkeySelectArea}\n\n";
            label1.Text += $"Fullscreen: {Preferences.Default.HotkeySelectFullscreen}\n\n";
            label1.Text += $"Current window: {Preferences.Default.HotkeySelectCurrentWindow}\n\n";

            _client = new LensertClient(Preferences.Default.username, Preferences.Default.password);
        }

        private void InitializeHotkeys()
        {
            _hotkeyBinder = new HotkeyBinder();
            _hotkeyBinder.Bind(Preferences.Default.HotkeySelectFullscreen).To(() => HotkeyHandler(ScreenshotType.Fullscreen));
            _hotkeyBinder.Bind(Preferences.Default.HotkeySelectArea).To(() => HotkeyHandler(ScreenshotType.Area));
            _hotkeyBinder.Bind(Preferences.Default.HotkeySelectCurrentWindow).To(() => HotkeyHandler(ScreenshotType.CurrentWindow));
        }

        private async void HotkeyHandler(ScreenshotType type)
        {
            var screenshot = ScreenshotProvider.GetScreenshot(type);
            if (screenshot == null)
                return;

            var jsonResponse = await _client.UploadImageAsync(screenshot);
            string link = jsonResponse["link"];

            Console.WriteLine($"Got link '{link}'");
            NotificationProvider.Show(link);

            //if (Preferences.Default.CopyToClipboard)
            //    Clipboard.SetText(link);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            var loggedIn = await _client.Login();
            if (!loggedIn)
                throw new Exception("Invalid credentials");

            Console.WriteLine($"Logged in as {_client.Username}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new PreferencesForm().Show();
        }
    }
}
