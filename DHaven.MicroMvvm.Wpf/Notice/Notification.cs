using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHaven.MicroMvvm.Dialog;

namespace DHaven.MicroMvvm.Wpf.Notice
{
    public class Notification : ViewModel<Message>
    {
        public Notification(Message modelIn) : base(modelIn)
        {
            Created = DateTime.Now;
        }

        public DateTime Created
        {
            get { return GetValue<DateTime>(nameof(Created)); }
            set { SetValue(nameof(Created), value); }
        }
    }
}
