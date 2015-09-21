using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shortcut;

namespace Lensert
{
    public partial class MainForm : Form
    {

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private HotkeyBinder _hotkeyBinder;
        public MainForm()
        {
            InitializeComponent();
            InitializeHotkeys();

            Task.Run(async () =>
            {
                var client = new LensertClient("joell", "123qwe");
                var result = await client.Login();

                var image = Image.FromFile(@"C:\Users\joell\Desktop\screenshot.png");

                var json = await client.UploadImage(image);

                image.Dispose();

                int i = 0;
            });
        }

        private void InitializeHotkeys()
        {
            _hotkeyBinder = new HotkeyBinder();
            _hotkeyBinder.Bind(Settings.Default.HotkeySelectFullscreen).To(TakeFullscreen);
            _hotkeyBinder.Bind(Settings.Default.HotkeySelectRegion).To(TakeRegion);
            _hotkeyBinder.Bind(Settings.Default.HotkeySelectWindow).To(TakeWindow);
        }
        
        private void TakeFullscreen()
        {
            throw new NotImplementedException();
        }

        private void TakeRegion()
        {
            throw new NotImplementedException();
        }

        private void TakeWindow()
        {
            throw new NotImplementedException();
        }
    }
}
