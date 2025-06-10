//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_com_init_mode_t.
//
global using cef_com_init_mode_t = Xilium.CefGlue.CefComInitMode;

namespace Xilium.CefGlue;

/// <summary>
/// Windows COM initialization mode. Specifies how COM will be initialized for a
/// new thread.
/// </summary>
public enum CefComInitMode
{
    None,
    STA,
    MTA,
}
