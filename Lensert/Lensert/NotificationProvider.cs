using System;
using System.Drawing;
using System.Windows.Forms;
using Lensert.Properties;

namespace Lensert
{
    internal static class NotificationProvider
    {
        private static readonly NotifyIcon _notifyIcon;

        private static Action _clicked;

        static NotificationProvider()
        {
            _notifyIcon = new NotifyIcon
            {
                //BalloonTipIcon = ToolTipIcon.Info,
                Visible = true,
                Icon = Resources.lensert_icon_fresh,
                Text = "Lensert"
            };
            
            _notifyIcon.BalloonTipClicked += OnBalloonClicked;

            var trayIconContextMenu = new ContextMenuStrip();
            var closeMenuItem = new ToolStripMenuItem();
            trayIconContextMenu.SuspendLayout();

            trayIconContextMenu.Items.AddRange(new ToolStripItem[] {closeMenuItem});
            trayIconContextMenu.Name = "trayIconContextMenu";
            trayIconContextMenu.Size = new Size(153, 70);

            closeMenuItem.Name = "closeMenuItem";
            closeMenuItem.Size = new Size(152, 22);
            closeMenuItem.Text = "Close";
            closeMenuItem.Click += CloseMenuItem_Click;

            trayIconContextMenu.ResumeLayout(false);
            _notifyIcon.ContextMenuStrip = trayIconContextMenu;
        }

        private static void CloseMenuItem_Click(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }
        
        private static void OnBalloonClicked(object sender, EventArgs eventArgs)
        {
            _clicked?.Invoke();
        }

        public static void Show()
        {
            _notifyIcon.Visible = true;
        }

        public static void Show(string title, string text, Action clicked = null)
        {
            _clicked = clicked;

            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.ShowBalloonTip(500);
        }
    }
}
