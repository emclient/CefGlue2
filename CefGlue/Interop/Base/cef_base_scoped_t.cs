using System;
using System.Runtime.InteropServices;

namespace Xilium.CefGlue.Interop;

[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_base_scoped_t
{
    internal UIntPtr _size;
    internal delegate* unmanaged<cef_base_scoped_t*, void> _del;
}
