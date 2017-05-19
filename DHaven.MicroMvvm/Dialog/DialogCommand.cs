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

namespace DHaven.MicroMvvm.Dialog
{
    public class DialogCommand : ObservableObject, IDialogCommand
    {
        public static readonly DialogCommand Ok = new DialogCommand("OK", true);
        public static readonly DialogCommand Cancel = new DialogCommand("Cancel");
        public static readonly DialogCommand Yes = new DialogCommand("Yes", true);
        public static readonly DialogCommand No = new DialogCommand("No");

        /// <summary>
        ///     Make a mutable version of the DialogCommand.
        /// </summary>
        public DialogCommand()
        {
        }

        /// <summary>
        ///     Make a readonly version of the DialogCommand.  All data supplied.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isPrimary"></param>
        /// <param name="callback"></param>
        public DialogCommand(string label, bool isPrimary = false, Action<IDialogCommand> callback = null)
        {
            Label = label;
            IsPrimary = isPrimary;
            Invoked = callback;

            MakeReadonly();
        }

        #region Implementation of IDialogCommand

        public string Label
        {
            get => GetValue<string>(nameof(Label));
            set => SetValue(nameof(Label), value);
        }

        public Action<IDialogCommand> Invoked
        {
            get => GetValue<Action<IDialogCommand>>(nameof(Invoked));
            set => SetValue(nameof(Invoked), value);
        }

        public bool IsPrimary
        {
            get => GetValue<bool>(nameof(IsPrimary));
            set => SetValue(nameof(IsPrimary), value);
        }

        #endregion
    }
}