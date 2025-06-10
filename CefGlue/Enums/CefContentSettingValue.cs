//
// This file manually written from cef/include/internal/cef_types_content_settings.h.
// C API name: cef_content_setting_values_t.
//
global using cef_content_setting_values_t = Xilium.CefGlue.CefContentSettingValue;

namespace Xilium.CefGlue;

public enum CefContentSettingValue
{
    Default,
    Allow,
    Block,
    Ask,
    SessionOnly,
    DetectImportantContent,
    NumValues,
}
