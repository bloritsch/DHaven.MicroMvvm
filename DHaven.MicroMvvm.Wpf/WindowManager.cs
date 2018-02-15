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

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DHaven.MicroMvvm.Dialog;
using DHaven.MicroMvvm.Notice;
using DHaven.MicroMvvm.Wpf.Dialog;
using DHaven.MicroMvvm.Wpf.Notice;
using MahApps.Metro.Controls.Dialogs;

namespace DHaven.MicroMvvm.Wpf
{
    public class WindowManager : ObservableObject, IWindowManager
    {
        public WindowManager()
        {
            ResizeMode = ResizeMode.CanResizeWithGrip;
        }

        public ResizeMode ResizeMode
        {
            get => GetValue<ResizeMode>(nameof(ResizeMode));
            set => SetValue(nameof(ResizeMode), value);
        }

        public ImageSource AppIcon
        {
            get => GetValue<ImageSource>(nameof(AppIcon));
            set => SetValue(nameof(AppIcon), value);
        }

        public string AppName
        {
            get => GetValue<string>(nameof(AppName));
            set => SetValue(nameof(AppName), value);
        }

        #region Implementation of IWindowManager

        public async Task<IDialogCommand> ShowDialog(DialogViewModel viewModel)
        {
            var window = GetViewModelWindow();
            Debug.Assert(window != null, "There are no parent windows visible");

            var dialog = new DialogView(viewModel);

            await window.ShowMetroDialogAsync(dialog);
            var command = await viewModel.Result.Task;
            await window.HideMetroDialogAsync(dialog);

            return command;
        }

        private static ViewWindow GetViewModelWindow(IViewModel parent = null)
        {
            var window =
                Application.Current.Windows.OfType<ViewWindow>()
                    .FirstOrDefault(w => ReferenceEquals(parent, w.DataContext))
                ?? Application.Current.MainWindow as ViewWindow;

            return window;
        }

        public Task OpenWindow(IViewModel viewModel, bool showAppIcon)
        {
            var window = new ViewWindow
            {
                DataContext = viewModel
            };

            if (showAppIcon)
                BindingOperations.SetBinding(window, Window.IconProperty, new Binding
                {
                    Source = this,
                    Path = new PropertyPath(nameof(AppIcon)),
                    IsAsync = true
                });

            BindingOperations.SetBinding(window, Window.TitleProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(AppName))
            });
            BindingOperations.SetBinding(window, Window.ResizeModeProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(ResizeMode))
            });

            window.Show();

            return window.Task;
        }

        public INotificationControl Notify(Message message, IViewModel parentViewModel = null)
        {
            var window = GetViewModelWindow(parentViewModel);

            return window.Publish(new Notification(message));
        }

        #endregion
    }
}