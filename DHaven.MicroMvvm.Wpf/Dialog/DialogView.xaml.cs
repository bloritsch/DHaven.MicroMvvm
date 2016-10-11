using System.Windows;
using DHaven.MicroMvvm.Dialog;

namespace DHaven.MicroMvvm.Wpf.Dialog
{
    /// <summary>
    /// Interaction logic for DialogView.xaml
    /// </summary>
    public partial class DialogView
    {
        public DialogView(DialogViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void DialogButtonClicked(object sender, RoutedEventArgs e)
        {
            var clickSource = sender as FrameworkElement;
            var command = clickSource?.DataContext as IDialogCommand;            

            var dialogViewModel = DataContext as DialogViewModel;
            dialogViewModel?.ChooseCommand(command);
        }
    }
}
