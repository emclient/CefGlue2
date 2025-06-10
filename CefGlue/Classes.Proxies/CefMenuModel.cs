namespace Xilium.CefGlue;

public sealed unsafe partial class CefMenuModel
{
    /// <summary>
    /// Create a new MenuModel with the specified |delegate|.
    /// </summary>
    public static CefMenuModel Create(CefMenuModelDelegate handler) => CreateMenuModel(handler);

    /// <summary>
    /// Returns the item type at the specified |index|.
    /// </summary>
    public CefMenuItemType GetItemTypeAt(nuint index) => GetTypeAt(index);
}
