//
// This file manually written from cef/include/internal/cef_types.h.
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_linux_window_properties_t
    {
        public UIntPtr size;
        public cef_string_t wayland_app_id;
        public cef_string_t wm_class_class;
        public cef_string_t wm_class_name;
        public cef_string_t wm_role_name;
    }
}
