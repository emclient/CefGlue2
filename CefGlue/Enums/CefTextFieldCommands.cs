//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_text_field_commands_t.
//
namespace Xilium.CefGlue
{
    /// <summary>
    /// Represents commands available to TextField. Should be kept in sync with
    /// Chromium's views::TextField::MenuCommands type.
    /// </summary>
    public enum CefTextFieldCommands
    {
        Unknown,
        Cut,
        Copy,
        Paste,
        SelectAll,
        SelectWord,
        Undo,
        Delete,
    }
}
