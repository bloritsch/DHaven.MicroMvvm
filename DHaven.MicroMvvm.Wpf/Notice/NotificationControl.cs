#region Copyright 2017 D-Haven.org

// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using DHaven.MicroMvvm.Dialog;
using DHaven.MicroMvvm.Notice;
using System;
using System.Threading.Tasks;

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
                viewWindow.ShownNotifications.Add(notification);

            RaisePropertyChanged(nameof(IsHidden));
        }

        public void Hide()
        {
            if (!IsHidden)
                viewWindow.ShownNotifications.Remove(notification);

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