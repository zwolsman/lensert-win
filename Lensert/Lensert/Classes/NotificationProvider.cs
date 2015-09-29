﻿/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lensert.Forms;
using Lensert.Properties;

namespace Lensert
{
    static class NotificationProvider
    {
        private static readonly NotifyIcon _notifyIcon;

        private static string _link;

        static NotificationProvider()
        {
            _notifyIcon = new NotifyIcon
            {
                BalloonTipText = "Click here to open uploaded screenshot.",
                BalloonTipTitle = "Upload succesful!",
                //BalloonTipIcon = ToolTipIcon.Info,
                Visible = true,
                Icon = Resources.lensert_icon_fresh,
                Text = "Lensert"
            };


            _notifyIcon.BalloonTipClicked += OnBalloonClicked;
            _notifyIcon.DoubleClick += showPreferencesForm;


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
            _notifyIcon.ContextMenuStrip = trayIconContextMenu;
        }

        private static void CloseMenuItem_Click(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private static void showPreferencesForm(object sender, EventArgs e)
        {
            var preferencesForm = new PreferencesForm();
            preferencesForm.ShowDialog();
            preferencesForm.Dispose();
        }

        private static void OnBalloonClicked(object sender, EventArgs eventArgs)
        {
            Process.Start(_link);
        }

        public static void Show(string link)
        {
            _link = link;
            _notifyIcon.ShowBalloonTip(500);
        }
    }
}
*/
