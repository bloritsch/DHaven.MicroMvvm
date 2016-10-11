using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHaven.MicroMvvm.Dialog
{
    public class DialogCommand : ObservableObject, IDialogCommand
    {
        public static readonly DialogCommand Ok = new DialogCommand("OK", true);
        public static readonly DialogCommand Cancel = new DialogCommand("Cancel");
        public static readonly DialogCommand Yes = new DialogCommand("Yes", true);
        public static readonly DialogCommand No = new DialogCommand("No");

        /// <summary>
        /// Make a mutable version of the DialogCommand.
        /// </summary>
        public DialogCommand() { }

        /// <summary>
        /// Make a readonly version of the DialogCommand.  All data supplied.
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
            get { return GetValue<string>(nameof(Label)); }
            set { SetValue(nameof(Label), value);}
        }

        public Action<IDialogCommand> Invoked
        {
            get { return GetValue<Action<IDialogCommand>>(nameof(Invoked)); }
            set { SetValue(nameof(Invoked), value); }
        }

        public bool IsPrimary
        {
            get { return GetValue<bool>(nameof(IsPrimary)); }
            set { SetValue(nameof(IsPrimary), value); }
        }

        #endregion
    }
}
