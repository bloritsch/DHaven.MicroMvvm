#region Copyright 2016 D-Haven.org

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

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using DHaven.MicroMvvm.Notice;
using DHaven.MicroMvvm.Wpf.Notice;

namespace DHaven.MicroMvvm.Wpf
{
    /// <summary>
    ///     Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow
    {
        private readonly TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();

        public ViewWindow()
        {
            InitializeComponent();
        }

        public ObservableCollection<Notification> ShownNotifications { get; } = new ObservableCollection<Notification>();

        public ObservableCollection<Notification> OpenNotifications { get; } = new ObservableCollection<Notification>();

        public Task Task => completionSource.Task;

        public INotificationControl Publish(Notification notification)
        {
            OpenNotifications.Add(notification);
            ShownNotifications.Add(notification);

            return new NotificationControl(notification, this);
        }

        #region Overrides of Window

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closed" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            completionSource.SetResult(true);

            base.OnClosed(e);
        }

        #endregion

        private void HideNotificationClick(object sender, RoutedEventArgs e)
        {
            var control = sender as FrameworkElement;
            var notice = control?.DataContext as Notification;

            if (notice == null)
            {
                return;
            }

            ShownNotifications.Remove(notice);
        }

        private void CloseNotificationClick(object sender, RoutedEventArgs e)
        {
            var control = sender as FrameworkElement;
            var notice = control?.DataContext as Notification;

            if (notice == null)
            {
                return;
            }

            ShownNotifications.Remove(notice);
            OpenNotifications.Remove(notice);
        }

        private void CloseAllNotificationsClick(object sender, RoutedEventArgs e)
        {
            ShownNotifications.Clear();
            OpenNotifications.Clear();
        }
    }
}