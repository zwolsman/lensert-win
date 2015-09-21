using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lensert.Properties;

namespace Lensert.Classes
{
    static class NotificationProvider
    {
        private static readonly NotifyIcon _notifyIcon;

        private static string _link;
        
        static NotificationProvider()
        {
            _notifyIcon = new NotifyIcon
            {
                BalloonTipText = "Your image has been uploaded to Lensert.\r\nClick here to open",
                BalloonTipTitle = "Lensert",
                BalloonTipIcon = ToolTipIcon.Info,
                Visible = true,
                Icon = Resources.lensert_icon_fresh,
                Text = "Lensert"
            };

            _notifyIcon.BalloonTipClicked += OnBalloonClicked;
        }

        private static void OnBalloonClicked(object sender, EventArgs eventArgs)
        {
            Process.Start(_link);
        }

        private static int index = 0;
        public static void Show(string link)
        {
           // _notifyIcon.BalloonTipText = $"{++index}";
            _link = link;
            _notifyIcon.ShowBalloonTip(500);
        }
    }
}
