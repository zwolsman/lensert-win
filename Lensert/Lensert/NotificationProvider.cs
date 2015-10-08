using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lensert.Properties;

namespace Lensert
{
    static class NotificationProvider
    {
        public static NotifyIcon NotifyIcon { get; }

        public static PreferencesForm PreferencesForm { get; set; }

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
            NotifyIcon.DoubleClick += showPreferencesForm;


            var trayIconContextMenu = new ContextMenuStrip();
            var closeMenuItem = new ToolStripMenuItem();
            var preferencesMenuItem = new ToolStripMenuItem();
            trayIconContextMenu.SuspendLayout();

            // 
            // trayIconContextMenu
            // 
            trayIconContextMenu.Items.AddRange(new ToolStripItem[] {
            closeMenuItem});
            trayIconContextMenu.Name = "trayIconContextMenu";
            trayIconContextMenu.Size = new Size(153, 70);
            // 
            // closeMenuItem
            // 
            closeMenuItem.Name = "closeMenuItem";
            closeMenuItem.Size = new Size(152, 22);
            closeMenuItem.Text = "Close";
            closeMenuItem.Click += new EventHandler(CloseMenuItem_Click);

            // 
            // preferencesMenuItem
            // 
            preferencesMenuItem.Name = "preferencesMenuItem";
            preferencesMenuItem.Size = new Size(152, 22);
            preferencesMenuItem.Text = "Preferences";
            preferencesMenuItem.Click += new EventHandler(showPreferencesForm);

            trayIconContextMenu.ResumeLayout(false);
            NotifyIcon.ContextMenuStrip = trayIconContextMenu;
        }

        private static void CloseMenuItem_Click(object sender, EventArgs e)
        {
            NotifyIcon.Visible = false;
            Application.Exit();
        }

        private static void showPreferencesForm(object sender, EventArgs e)
        {
            PreferencesForm?.ShowDialog();
        }

        private static void OnBalloonClicked(object sender, EventArgs eventArgs)
        {
            _clicked();
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
