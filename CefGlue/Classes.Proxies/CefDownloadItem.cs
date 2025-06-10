namespace Xilium.CefGlue;

public sealed unsafe partial class CefDownloadItem
{
    /// <summary>
    /// Returns the time that the download started.
    /// </summary>
    public CefBaseTime StartTime => GetStartTime();

    /// <summary>
    /// Returns the time that the download ended.
    /// </summary>
    public CefBaseTime EndTime => GetEndTime();
}
