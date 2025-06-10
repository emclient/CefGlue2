namespace Xilium.CefGlue;

public sealed unsafe partial class CefProcessMessage
{
    /// <summary>
    /// Returns the list of arguments.
    /// Returns nullptr when message contains a shared memory region.
    /// </summary>
    public CefListValue? Arguments => GetArgumentList();
}
