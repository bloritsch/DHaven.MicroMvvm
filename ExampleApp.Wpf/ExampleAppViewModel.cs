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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DHaven.MicroMvvm;
using DHaven.MicroMvvm.Dialog;
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
                return AvailableAccents.FirstOrDefault(info => info.Model.Name == style.Item2.Name);
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
            var response = await Model.WindowManager.ShowDialog(new DialogViewModel("You clicked me!",
                "Do you like what you are seeing?", DialogCommand.Yes, DialogCommand.No));

            var title = response.Label == DialogCommand.Yes.Label ? "Hooray!" : "Oh well...";
            var message = response.Label == DialogCommand.Yes.Label
                ? "I'm glad you like it."
                : "I wonder how I can improve?";

            var control = Model.WindowManager.Notify(new Message(title, message));

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
            get => GetValue<Brush>(nameof(AccentColor));
            set => SetValue(nameof(AccentColor), value);
        }
    }
}