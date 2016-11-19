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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DHaven.MicroMvvm.Wpf
{
    /// <summary>
    ///     Supports ViewModel first development.  Bind the DataContext to your
    ///     ViewModel, and the framework will create a DynamicView instance for your ViewModel.
    /// </summary>
    public sealed class DynamicView : ContentControl
    {
        public DynamicView()
        {
            Locator = Application.Current.Resources["Locator"] as Locator ?? new Locator();

            DataContextChanged += View_DataContextChanged;
        }

        private Locator Locator { get; }

        private void View_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Just in case the old view disposable, we'll want to preserve the expected behavior
            var view = Content as IDisposable;
            view?.Dispose();

            Content = Locator[DataContext];
        }
    }
}