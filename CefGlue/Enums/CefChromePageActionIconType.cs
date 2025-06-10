//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_chrome_page_action_icon_type_t.
//
global using cef_chrome_page_action_icon_type_t = Xilium.CefGlue.CefChromePageActionIconType;

namespace Xilium.CefGlue;

/// <summary>
/// Chrome page action icon types. Should be kept in sync with Chromium's
/// PageActionIconType type.
/// </summary>
public enum CefChromePageActionIconType
{
    BookmarkStar,
    ClickToCall,
    CookieControls,
    FileSystemAccess,
    Find,
    MemorySaver,
    IntentPicker,
    LocalCardMigration,
    ManagePasswords,
    PaymentsOfferNotification,
    PriceTracking,
    PwaInstall,
    QrCodeGeneratorDeprecated,
    ReaderModeDeprecated,
    SaveAutofillAddress,
    SaveCard,
    SendTabToSelfDeprecated,
    SharingHub,
    SideSearchDeprecated,
    SmsRemoteFetcher,
    Translate,
    VirtualCardEnroll,
    VirtualCardInformation,
    Zoom,
    SaveIban,
    MandatoryReauth,
    PriceInsights,
    ReadAnythingDeprecated,
    ProductSpecifications,
    LensOverlay,
    Discounts,
    OptimizationGuide,
    CollaborationMessaging,
    ChangePassword,
}
