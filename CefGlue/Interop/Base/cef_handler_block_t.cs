using System;
using System.Runtime.InteropServices;

namespace Xilium.CefGlue.Interop;

internal unsafe struct cef_handler_block_t
{
    internal int _refct;
    internal IntPtr _gcHandle;
}
