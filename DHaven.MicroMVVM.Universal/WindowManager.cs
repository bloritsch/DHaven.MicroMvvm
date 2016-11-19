using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using DHaven.MicroMvvm;
using DHaven.MicroMvvm.Dialog;
using DHaven.MicroMvvm.Notice;

namespace DHaven.MicroMVVM.Universal
{
    public class WindowManager : IWindowManager
    {
        public Locator Locator { get; }

        #region Implementation of IWindowManager

        public async Task<IDialogCommand> ShowDialog(DialogViewModel dialogViewModel)
        {
            IDialogCommand primaryCommand = dialogViewModel.Commands.FirstOrDefault(cmd => cmd.IsPrimary) ?? dialogViewModel.Commands.FirstOrDefault();
            IDialogCommand secondaryCommand = dialogViewModel.Commands.FirstOrDefault(cmd => !cmd.IsPrimary) ?? dialogViewModel.Commands.LastOrDefault();

            if (dialogViewModel.Commands.Count == 1)
            {
                secondaryCommand = null;
            }

            ContentDialog dialog = new ContentDialog();

            if (primaryCommand != null)
            {
                dialog.PrimaryButtonText = primaryCommand.Label;
                dialog.IsPrimaryButtonEnabled = true;
            }

            if (secondaryCommand != null)
            {
                dialog.SecondaryButtonText = secondaryCommand.Label;
                dialog.IsSecondaryButtonEnabled = true;
            }

            BindingOperations.SetBinding(dialog, ContentDialog.TitleProperty, new Binding
            {
                Source = dialogViewModel.Model,
                Path = new PropertyPath("Title")
            });

            dialog.Content = CreateAndBindView(dialogViewModel);

            IDialogCommand command = null;

            switch (await dialog.ShowAsync())
            {
                case ContentDialogResult.Primary:
                    command = primaryCommand;
                    break;

                case ContentDialogResult.Secondary:
                    command = secondaryCommand;
                    break;
            }

            return command;
        }

        public Task OpenWindow(IViewModel viewModel, bool showAppIcon = true)
        {
            var view = CreateAndBindView(viewModel);

            Window.Current.Content = view;
            Window.Current.Activate();

            // Kind of a noop here.  Universal apps are full screen.
            var completion = new TaskCompletionSource<bool>();
            completion.SetResult(true);

            return completion.Task;
        }

        private FrameworkElement CreateAndBindView(IViewModel viewModel)
        {
            var view = Locator[viewModel] as FrameworkElement;

            if (view != null)
            {
                view.DataContext = viewModel;
            }
            return view;
        }

        public INotificationControl Notify(Message message, IViewModel parentViewModel = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
