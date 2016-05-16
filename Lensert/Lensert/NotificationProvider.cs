using System;
using System.Drawing;
using System.Windows.Forms;
using Lensert.Properties;

namespace Lensert
{
    internal static class NotificationProvider
    {
        public static NotifyIcon NotifyIcon { get; }

        private static Action _clicked;

        static NotificationProvider()
        {
            NotifyIcon = new NotifyIcon
            {
                //BalloonTipIcon = ToolTipIcon.Info,
                Visible = true,
                Icon = Resources.lensert_icon_fresh,
                Text = "Lensert"
            };
            
            NotifyIcon.BalloonTipClicked += OnBalloonClicked;

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
            NotifyIcon.ContextMenuStrip = trayIconContextMenu;
        }

        private static void CloseMenuItem_Click(object sender, EventArgs e)
        {
            NotifyIcon.Visible = false;
            Application.Exit();
        }
        
        private static void OnBalloonClicked(object sender, EventArgs eventArgs)
        {
            _clicked?.Invoke();
        }

        public static void Show()
        {
            NotifyIcon.Visible = true;
        }

        public static void Show(string title, string text, Action clicked = null)
        {
            _clicked = clicked;

            NotifyIcon.BalloonTipTitle = title;
            NotifyIcon.BalloonTipText = text;
            NotifyIcon.ShowBalloonTip(500);
        }
    }
}
