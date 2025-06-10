using System;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefDragData
{
    /// <summary>
    /// Set the link URL that is being dragged.
    /// </summary>
    [Obsolete]
    public void SetLinkURL(string url) => SetLinkUrl(url);

    /// <summary>
    /// Set the base URL that the fragment came from.
    /// </summary>
    [Obsolete]
    public void SetFragmentBaseURL(string baseUrl) => SetFragmentBaseUrl(baseUrl);
}
