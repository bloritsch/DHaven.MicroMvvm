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
using System.Threading.Tasks;

namespace DHaven.MicroMvvm.Wpf
{
    /// <summary>
    ///     Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow
    {
        private readonly TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();

        public ViewWindow()
        {
            InitializeComponent();
        }

        public Task Task => completionSource.Task;

        #region Overrides of Window

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closed" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            completionSource.SetResult(true);

            base.OnClosed(e);
        }

        #endregion
    }
}