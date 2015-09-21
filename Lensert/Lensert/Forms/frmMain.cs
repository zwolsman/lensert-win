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
                //var image = Image.FromFile(@"C:\Users\joell\Desktop\screenshot.png");

                //var json = await client.UploadImage(image);

                //image.Dispose();

                //int i = 0;
            });
        }

        private void InitializeHotkeys()
        {
            _hotkeyBinder = new HotkeyBinder();
            _hotkeyBinder.Bind(Preferences.Default.HotkeySelectFullscreen).To(() => HotkeyHandler(ScreenshotType.Fullscreen));
            _hotkeyBinder.Bind(Preferences.Default.HotkeySelectArea).To(() => HotkeyHandler(ScreenshotType.Area));
            _hotkeyBinder.Bind(Preferences.Default.HotkeySelectCurrentWindow).To(() => HotkeyHandler(ScreenshotType.CurrentWindow));
        }

        private void HotkeyHandler(ScreenshotType type)
        {
            var screenshot = ScreenshotProvider.GetScreenshot(type);
            _client.UploadImage(screenshot).ContinueWith((data) =>
            {
                string link = data.Result.ToString();
                Console.WriteLine($"Got link '{link}'");
                notifyIcon.BalloonTipClicked += (sender, args) => Process.Start(link);
                notifyIcon.ShowBalloonTip(500, "Lensert", "Your image has been uploaded to Lensert.\r\nClick here to open", ToolTipIcon.Info);
                if (Preferences.Default.CopyToClipboard)
                {
                    Clipboard.SetText(link);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
