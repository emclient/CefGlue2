using System;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefRequest
{
    /// <summary>
    /// Get the referrer URL.
    /// </summary>
    [Obsolete]
    public string ReferrerURL => ReferrerUrl;

    /// <summary>
    /// Get the post data.
    /// </summary>
    public CefPostData PostData
    {
        get => GetPostData();
        set => SetPostData(value);
    }

    /// <summary>
    /// Get the options used in combination with CefUrlRequest.
    /// </summary>
    public CefUrlRequestOptions Options
    {
        get { return (CefUrlRequestOptions)Flags; }
        set { SetFlags((int)value); }
    }
}
