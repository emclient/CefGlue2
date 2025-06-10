namespace Xilium.CefGlue;

public sealed unsafe partial class CefNavigationEntry
{
    /// <summary>
    /// Returns the time for the last known successful navigation completion. A
    /// navigation may be completed more than once if the page is reloaded. May be
    /// 0 if the navigation has not yet completed.
    /// </summary>
    public CefBaseTime CompletionTime => GetCompletionTime();
}
