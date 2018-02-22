using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHaven.MicroMvvm.Wpf
{
    public class DHavenInitializationException : ApplicationException
    {
        public DHavenInitializationException(string message, Exception originException)
            : base(message, originException)
        { }
    }
}
