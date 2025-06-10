using System;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefRequestContext
{
    /// <summary>
    /// Returns the current value for |content_type| that applies for the
    /// specified URLs. If both URLs are NULL the default value will be returned.
    /// Returns nullptr if no value is configured. Must be called on the browser
    /// process UI thread.
    /// </summary>
    [Obsolete]
    public CefValue GetWebsiteSettings(
        string requestingUrl,
        string topLevelUrl,
        CefContentSettingType contentType) => GetWebsiteSetting(requestingUrl, topLevelUrl, contentType);

    /// <summary>
    /// Sets the current value for |content_type| for the specified URLs in the
    /// default scope. If both URLs are NULL, and the context is not incognito,
    /// the default value will be set. Pass nullptr for |value| to remove the
    /// default value for this content type.
    ///
    /// WARNING: Incorrect usage of this function may cause instability or
    /// security issues in Chromium. Make sure that you first understand the
    /// potential impact of any changes to |content_type| by reviewing the related
    /// source code in Chromium. For example, if you plan to modify
    /// CEF_CONTENT_SETTING_TYPE_POPUPS, first review and understand the usage of
    /// ContentSettingsType::POPUPS in Chromium:
    /// https://source.chromium.org/search?q=ContentSettingsType::POPUPS
    /// </summary>
    [Obsolete]
    public void SetWebsiteSettings(
        string requestingUrl,
        string topLevelUrl,
        CefContentSettingType contentType,
        CefValue value) => SetWebsiteSetting(requestingUrl, topLevelUrl, contentType, value);
}
