//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_value_type_t.
//
global using cef_value_type_t = Xilium.CefGlue.CefValueType;

namespace Xilium.CefGlue;

/// <summary>
/// Supported value types.
/// </summary>
public enum CefValueType
{
    Invalid = 0,
    Null,
    Bool,
    Int,
    Double,
    String,
    Binary,
    Dictionary,
    List,
}
