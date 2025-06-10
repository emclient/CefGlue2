using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue
{
    public class CefLinuxWindowProperties
    {
        internal static unsafe CefLinuxWindowProperties FromNative(cef_linux_window_properties_t* ptr)
            => throw new NotImplementedException();
    }
}
