﻿using System;
using System.Runtime.InteropServices;

namespace Xilium.CefGlue.Interop;

[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_base_ref_counted_t
{
    internal UIntPtr _size;
    internal delegate* unmanaged<cef_base_ref_counted_t*, void> _add_ref;
    internal delegate* unmanaged<cef_base_ref_counted_t*, int> _release;
    internal delegate* unmanaged<cef_base_ref_counted_t*, int> _has_one_ref;
    internal delegate* unmanaged<cef_base_ref_counted_t*, int> _has_at_least_one_ref;
}
