using System;
using System.Collections.Concurrent;
using System.Windows.Forms;
using Lensert.Properties;

namespace Lensert.Core
{
    internal static class NotificationProvider
    {
        private static NotifyIcon _notifyIcon;
        private static readonly ConcurrentQueue<Notification> _backlog;
        private static Notification _currentNotification;

        static NotificationProvider()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Resources.lensert_icon_fresh,
                Text = "Lensert"
            };

            _backlog = new ConcurrentQueue<Notification>();

            _notifyIcon.BalloonTipClicked += OnBalloonClicked;
            _notifyIcon.BalloonTipClosed += OnBalloonClosed;
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

        private static void Show(Notification notification)
        {
            if (_currentNotification != null && _currentNotification.Priority != -1 && _currentNotification.Priority >= notification.Priority)
                _backlog.Enqueue(notification);
            else
                ShowNotification(notification);
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
            {
                _notifyIcon.Visible = false;
                return;
            }

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

        public static bool IsVisible()
        {
            return _currentNotification != null;
        }

        private class Notification
        {
            public string Title { get; set; }
            public string Text { get; set; }
            public Action Clicked { get; set; }
            public int Priority { get; set; }
        }

        public static void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.BalloonTipClosed -= OnBalloonClosed;
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }

            _currentNotification = null;
        }
    }
}
