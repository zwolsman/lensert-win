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
using Lensert.Properties;
using Shortcut;

namespace Lensert
{
    public partial class MainForm : Form
    {

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private HotkeyBinder _hotkeyBinder;
        private LensertClient _client;
        public MainForm()
        {
            InitializeComponent();
            InitializeHotkeys();

            Task.Run(async () =>
            {
                _client = new LensertClient(Preferences.Default.username, Preferences.Default.password);
                var result = await _client.Login();
                Console.WriteLine($"Logged in as {_client.Username}");
            });
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
            var jsonResponse = await _client.UploadImageAsync(screenshot);
            string link = jsonResponse["link"]; 
            Console.WriteLine($"Got link '{link}'");
                NotificationProvider.Show(link);
           //kan je hier iets mee?:D //totaal niet :P maar miss ligt het daar niet aan
            if (Preferences.Default.CopyToClipboard) //f googlen
                Clipboard.SetText(link);
        }
        
    }
}
