//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_button_state_t.
//
global using cef_button_state_t = Xilium.CefGlue.CefButtonState;

namespace Xilium.CefGlue;

/// <summary>
/// Specifies the button display state.
/// </summary>
public enum CefButtonState
{
    Normal,
    Hovered,
    Pressed,
    Disabled,
}
