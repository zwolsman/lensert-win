using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;
using Lensert.Properties;

namespace Lensert.Core
{
    internal static class NotificationProvider
    {
        private static readonly NotifyIcon _notifyIcon;
        private static readonly ConcurrentQueue<Notification> _backlog;
        private static Notification _currentNotification;

        static NotificationProvider()
        {
            _notifyIcon = new NotifyIcon
            {
                //BalloonTipIcon = ToolTipIcon.Info,
                Visible = true,
                Icon = Resources.lensert_icon_fresh,
                Text = "Lensert"
            };

            _backlog = new ConcurrentQueue<Notification>();

            _notifyIcon.BalloonTipClicked += OnBalloonClicked;
            _notifyIcon.BalloonTipClosed += OnBalloonClosed;

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

        public static void ShowIcon() {}

        public static void Show(Notification notification)
        {
            if (_currentNotification != null && _currentNotification.Priority != -1 && _currentNotification.Priority >= notification.Priority)
                _backlog.Enqueue(notification);
            else
                ShowNotification(notification);
        }

        public static void Show(string title, string text, Action clicked = null, int priority = 0)
        {
            var notification = new Notification
            {
                Title = title,
                Text = text,
                Clicked = clicked,
                Priority = priority
            };

            Show(notification);
        }

        private static void CloseMenuItem_Click(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private static void OnBalloonClicked(object sender, EventArgs eventArgs)
        {
            _currentNotification?.Clicked?.Invoke();
            OnBalloonClosed(null, EventArgs.Empty);
        }

        private static void OnBalloonClosed(object sender, EventArgs eventArgs)
        {
            _currentNotification = null;

            if (_backlog.IsEmpty)
                return;

            Notification notification;
            if (!_backlog.TryDequeue(out notification))
                return;

            ShowNotification(notification);
        }

        private static void ShowNotification(Notification notification)
        {
            _notifyIcon.Visible = false;
            Notification peeked;
            if (_backlog.TryPeek(out peeked) && peeked == notification)
                _backlog.TryDequeue(out peeked);

            _currentNotification = notification;

            _notifyIcon.Visible = true;
            _notifyIcon.BalloonTipTitle = notification.Title;
            _notifyIcon.BalloonTipText = notification.Text;
            _notifyIcon.ShowBalloonTip(500);
        }
    }

    internal class Notification
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Action Clicked { get; set; }
        public int Priority { get; set; }
    }
}
