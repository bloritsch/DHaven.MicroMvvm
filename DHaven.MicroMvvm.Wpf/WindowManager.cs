using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DHaven.MicroMvvm.Dialog;
using DHaven.MicroMvvm.Wpf.Dialog;
using MahApps.Metro.Controls;
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
            get { return GetValue<ResizeMode>(nameof(ResizeMode)); }
            set { SetValue(nameof(ResizeMode), value); }
        }

        public ImageSource AppIcon
        {
            get { return GetValue<ImageSource>(nameof(AppIcon)); }
            set { SetValue(nameof(AppIcon), value); }
        }

        public string AppName
        {
            get { return GetValue<string>(nameof(AppName)); }
            set { SetValue(nameof(AppName), value); }
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

        private static MetroWindow GetViewModelWindow(IViewModel parent = null)
        {
            MetroWindow window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(w => ReferenceEquals(parent, w.DataContext))
                                 ?? Application.Current.MainWindow as MetroWindow;

            return window;
        }

        public Task OpenWindow(IViewModel viewModel, bool showAppIcon)
        {
            ViewWindow window = new ViewWindow
            {
                DataContext = viewModel
            };

            if (showAppIcon)
            {
                BindingOperations.SetBinding(window, Window.IconProperty, new Binding
                {
                    Source = this,
                    Path = new PropertyPath(nameof(AppIcon)),
                    IsAsync = true
                });
            }

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

        #endregion
    }
}