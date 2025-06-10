//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_color_variant_t.
//
using cef_color_variant_t = Xilium.CefGlue.CefColorVariant;

namespace Xilium.CefGlue;

/// <summary>
/// Specifies the color variants supported by
/// CefRequestContext::SetChromeThemeColor.
/// </summary>
public enum CefColorVariant
{
    System,
    Light,
    Dark,
    TonalSpot,
    Neutral,
    Vibrant,
    Expressive,
}
