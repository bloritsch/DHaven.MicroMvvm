using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHaven.MicroMvvm.Dialog;

namespace DHaven.MicroMvvm.Notice
{
    public interface INotificationControl
    {
        bool IsHidden { get; }
        bool IsClosed { get; }

        Message Content { get; }

        void Show();
        void Hide();
        void Close();
        void AutoHideAfter(TimeSpan time);
        void AutoCloseAfter(TimeSpan time);
    }
}
