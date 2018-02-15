﻿#region Copyright 2017 D-Haven.org

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

using DHaven.MicroMvvm.Dialog;
using System.Windows;

namespace DHaven.MicroMvvm.Wpf.Dialog
{
    /// <summary>
    ///     Interaction logic for DialogView.xaml
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