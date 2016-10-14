using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHaven.MicroMvvm.Dialog;
using DHaven.MicroMvvm.Notice;

namespace DHaven.MicroMvvm.Wpf.Notice
{
    public class NotificationControl : ObservableObject, INotificationControl
    {
        private readonly Notification notification;
        private readonly ViewWindow viewWindow;

        public NotificationControl(Notification notification, ViewWindow viewWindow)
        {
            this.notification = notification;
            this.viewWindow = viewWindow;
        }
        #region Implementation of INotificationControl

        public bool IsHidden => !viewWindow.ShownNotifications.Contains(notification);

        public bool IsClosed => !viewWindow.OpenNotifications.Contains(notification);

        public Message Content => notification.Model;

        public void Show()
        {
            // Only show the notification if it is still open.
            if (IsHidden && !IsClosed)
            {
                viewWindow.ShownNotifications.Add(notification);
            }

            RaisePropertyChanged(nameof(IsHidden));
        }

        public void Hide()
        {
            if (!IsHidden)
            {
                viewWindow.ShownNotifications.Remove(notification);
            }

            RaisePropertyChanged(nameof(IsHidden));
        }

        public void Close()
        {
            viewWindow.ShownNotifications.Remove(notification);
            viewWindow.OpenNotifications.Remove(notification);

            RaisePropertyChanged();
        }

        public async void AutoHideAfter(TimeSpan time)
        {
            await Task.Delay(time);
            Hide();
        }

        public async void AutoCloseAfter(TimeSpan time)
        {
            await Task.Delay(time);
            Close();
        }

        #endregion
    }
}
