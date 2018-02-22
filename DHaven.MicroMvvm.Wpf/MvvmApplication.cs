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

using MahApps.Metro;
using System;
using System.Windows;

namespace DHaven.MicroMvvm.Wpf
{
    /// <summary>
    ///     Convenience base class for Micro Mvvm application.
    ///     While it's not strictly necessary to use, it will make
    ///     setting up much easier.
    /// </summary>
    public class MvvmApplication : Application
    {
        public IViewModel StartingViewModel { get; set; }

        public Locator Locator { get; set; }

        public IWindowManager WindowManager { get; set; }

        #region Overrides of Application

        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            EnsureResourcesAreConfigured();

            if (StartingViewModel != null)
                await WindowManager.OpenWindow(StartingViewModel);
            else
                base.OnStartup(e);
        }

        private void EnsureResourcesAreConfigured()
        {
            if (Locator == null)
                Locator = new Locator();

            if (WindowManager == null)
                WindowManager = new WindowManager
                {
                    AppName = GetType().Assembly.GetName().Name
                };

            if (!Resources.Contains(nameof(Locator)))
                Resources.Add(nameof(Locator), Locator);

            if (!Resources.Contains(nameof(WindowManager)))
                Resources.Add(nameof(WindowManager), WindowManager);

            if (Resources.MergedDictionaries.Count == 0)
            {
                Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")
                });
                Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")
                });
                Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml")
                });
            }

            if (ThemeManager.DetectAppStyle(this) == null)
            {
                Resources.MergedDictionaries.Add(ThemeManager.GetAccent("Blue").Resources);
                Resources.MergedDictionaries.Add(ThemeManager.GetAppTheme("BaseLight").Resources);
            }
        }

        #endregion
    }
}