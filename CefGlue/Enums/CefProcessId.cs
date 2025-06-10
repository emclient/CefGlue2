//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_process_id_t.
//
global using cef_process_id_t = Xilium.CefGlue.CefProcessId;

namespace Xilium.CefGlue;

/// <summary>
/// Existing process IDs.
/// </summary>
public enum CefProcessId
{
    Browser,
    Renderer,
}
