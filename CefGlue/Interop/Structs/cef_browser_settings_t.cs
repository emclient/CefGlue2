//
// This file manually written from cef/include/internal/cef_types.h.
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_browser_settings_t
    {
        public UIntPtr size;

        public int windowless_frame_rate;

        public cef_string_t standard_font_family;
        public cef_string_t fixed_font_family;
        public cef_string_t serif_font_family;
        public cef_string_t sans_serif_font_family;
        public cef_string_t cursive_font_family;
        public cef_string_t fantasy_font_family;
        public int default_font_size;
        public int default_fixed_font_size;
        public int minimum_font_size;
        public int minimum_logical_font_size;

        public cef_string_t default_encoding;

        public CefState remote_fonts;
        public CefState javascript;
        public CefState javascript_close_windows;
        public CefState javascript_access_clipboard;
        public CefState javascript_dom_paste;
        public CefState image_loading;
        public CefState image_shrink_standalone_to_fit;
        public CefState text_area_resize;
        public CefState tab_to_links;
        public CefState local_storage;
        public CefState databases;
        public CefState webgl;

        public uint background_color;
        public CefState chrome_status_bubble;
        public CefState chrome_zoom_bubble;

        #region Alloc & Free
        private static readonly int _sizeof;

        static cef_browser_settings_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_browser_settings_t));
        }

        public static cef_browser_settings_t* Alloc()
        {
            var ptr = (cef_browser_settings_t*)NativeMemory.AllocZeroed((nuint)_sizeof);
            *ptr = new cef_browser_settings_t();
            ptr->size = (UIntPtr)_sizeof;
            return ptr;
        }

        public static cef_browser_settings_t* Clone(cef_browser_settings_t* source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var ptr = Alloc();
            *ptr = *source;
            ptr->size = (UIntPtr)_sizeof;

            // Reset shallow-copied strings before allocating independent copies.
            ptr->standard_font_family = default;
            ptr->fixed_font_family = default;
            ptr->serif_font_family = default;
            ptr->sans_serif_font_family = default;
            ptr->cursive_font_family = default;
            ptr->fantasy_font_family = default;
            ptr->default_encoding = default;

            cef_string_t.Copy(&source->standard_font_family, &ptr->standard_font_family);
            cef_string_t.Copy(&source->fixed_font_family, &ptr->fixed_font_family);
            cef_string_t.Copy(&source->serif_font_family, &ptr->serif_font_family);
            cef_string_t.Copy(&source->sans_serif_font_family, &ptr->sans_serif_font_family);
            cef_string_t.Copy(&source->cursive_font_family, &ptr->cursive_font_family);
            cef_string_t.Copy(&source->fantasy_font_family, &ptr->fantasy_font_family);
            cef_string_t.Copy(&source->default_encoding, &ptr->default_encoding);
            return ptr;
        }

        public static void Free(cef_browser_settings_t* ptr)
        {
            if (ptr is null) return;

            libcef.string_clear(&ptr->standard_font_family);
            libcef.string_clear(&ptr->fixed_font_family);
            libcef.string_clear(&ptr->serif_font_family);
            libcef.string_clear(&ptr->sans_serif_font_family);
            libcef.string_clear(&ptr->cursive_font_family);
            libcef.string_clear(&ptr->fantasy_font_family);
            libcef.string_clear(&ptr->default_encoding);
            NativeMemory.Free(ptr);
        }
        #endregion
    }
}
