namespace Xilium.CefGlue;

public sealed unsafe partial class CefCookieManager
{
    /// <summary>
    /// Returns the global cookie manager. By default data will be stored at
    /// cef_settings_t.cache_path if specified or in memory otherwise. If
    /// |callback| is non-NULL it will be executed asnychronously on the UI thread
    /// after the manager's storage has been initialized. Using this method is
    /// equivalent to calling
    /// CefRequestContext::GetGlobalContext()->GetDefaultCookieManager().
    /// </summary>
    public static CefCookieManager GetGlobal(CefCompletionCallback callback)
        => GetGlobalManager(callback);
}
