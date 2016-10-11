using System;
using System.Windows;
using MahApps.Metro;

namespace DHaven.MicroMvvm.Wpf
{
    /// <summary>
    /// Convenience base class for Micro Mvvm application.
    /// While it's not strictly necessary to use, it will make
    /// setting up much easier.
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
            {
                await WindowManager.OpenWindow(StartingViewModel);
            }
            else
            {
                base.OnStartup(e);
            }
        }

        private void EnsureResourcesAreConfigured()
        {
            if (Locator == null)
            {
                Locator = new Locator();
            }

            if (WindowManager == null)
            {                
                WindowManager = new WindowManager
                {
                    AppName = GetType().Assembly.GetName().Name
                };
            }

            if (!Resources.Contains(nameof(Locator)))
            {
                Resources.Add(nameof(Locator), Locator);
            }

            if (!Resources.Contains(nameof(WindowManager)))
            {
                Resources.Add(nameof(WindowManager), WindowManager);
            }

            if (Resources.MergedDictionaries.Count == 0)
            {
                Resources.MergedDictionaries.Add(new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
                Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") });
                Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml") });
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
