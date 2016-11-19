using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DHaven.MicroMVVM.Universal
{
    class DynamicView : ContentControl
    {
        public DynamicView()
        {
            Locator = Application.Current.Resources["Locator"] as Locator ?? new Locator();

            DataContextChanged += DynamicView_DataContextChanged;
        }

        private void DynamicView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            // Just in case the old view disposable, we'll want to preserve the expected behavior
            var view = Content as IDisposable;
            view?.Dispose();

            Content = Locator[DataContext];
        }

        private Locator Locator { get; }
    }
}
