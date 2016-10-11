using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DHaven.MicroMvvm;
using DHaven.MicroMvvm.Dialog;
using DHaven.MicroMvvm.Notice;
using DHaven.MicroMvvm.Wpf;
using MahApps.Metro;

namespace ExampleApp.Wpf
{
    public class ExampleAppViewModel : ViewModel<MvvmApplication>
    {
        public ExampleAppViewModel() : base(Application.Current as MvvmApplication)
        {
            AvailableAccents = ThemeManager.Accents.Select(a => new AccentViewModel(a)).ToList();
            ClickCommand = new RelayCommand(Clicked);
        }

        public ICollection<AccentViewModel> AvailableAccents { get; }

        public AccentViewModel SelectedAccent
        {
            get
            {
                var style = ThemeManager.DetectAppStyle(Model);
                return AvailableAccents.FirstOrDefault(info=>info.Model.Name == style.Item2.Name);
            }
            set
            {
                var style = ThemeManager.DetectAppStyle(Model);
                if (!Equals(style.Item2.Name, value.Model.Name))
                {
                    ThemeManager.ChangeAppStyle(Model, value.Model, style.Item1);
                    RaisePropertyChanged(nameof(SelectedAccent));
                }
            }
        }

        public bool IsLightTheme
        {
            get
            {
                var style = ThemeManager.DetectAppStyle(Model);
                return style.Item1.Name.EndsWith("light", StringComparison.InvariantCultureIgnoreCase);
            }
            set
            {
                var style = ThemeManager.DetectAppStyle(Model);
                if (value != style.Item1.Name.EndsWith("light", StringComparison.InvariantCultureIgnoreCase))
                {
                    var newAppTheme = ThemeManager.GetInverseAppTheme(style.Item1);
                    ThemeManager.ChangeAppStyle(Model, style.Item2, newAppTheme);
                    RaisePropertyChanged(nameof(IsLightTheme));
                }
            }
        }

        public RelayCommand ClickCommand { get; }

        private async void Clicked()
        {
            IDialogCommand response = await Model.WindowManager.ShowDialog(new DialogViewModel("You clicked me!", "Do you like what you are seeing?", DialogCommand.Yes, DialogCommand.No));

            string title = response.Label == DialogCommand.Yes.Label ? "Hooray!" : "Oh well...";
            string message = response.Label == DialogCommand.Yes.Label
                ? "I'm glad you like it."
                : "I wonder how I can improve?";

            INotificationControl control = Model.WindowManager.Notify(new Message(title, message));

            control.AutoHideAfter(TimeSpan.FromSeconds(5));
            control.AutoCloseAfter(TimeSpan.FromSeconds(15));
        }
    }

    public class AccentViewModel : ViewModel<Accent>
    {
        public AccentViewModel(Accent accent) : base(accent)
        {
            AccentColor = accent.Resources["AccentColorBrush"] as Brush;
        }

        public Brush AccentColor
        {
            get { return GetValue<Brush>(nameof(AccentColor)); }
            set { SetValue(nameof(AccentColor), value); }
        }
    }
}
