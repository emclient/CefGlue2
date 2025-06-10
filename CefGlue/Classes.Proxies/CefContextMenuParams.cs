namespace Xilium.CefGlue;

public sealed unsafe partial class CefContextMenuParams
{
    /// <summary>
    /// Returns the X coordinate of the mouse where the context menu was invoked.
    /// Coords are relative to the associated RenderView's origin.
    /// </summary>
    public int X => XCoord;

    /// <summary>
    /// Returns the Y coordinate of the mouse where the context menu was invoked.
    /// Coords are relative to the associated RenderView's origin.
    /// </summary>
    public int Y => YCoord;

    /// <summary>
    /// Returns flags representing the type of node that the context menu was
    /// invoked on.
    /// </summary>
    public CefContextMenuTypeFlags ContextMenuType => TypeFlags;

    /// <summary>
    /// Returns flags representing the actions supported by the media element, if
    /// any, that the context menu was invoked on.
    /// </summary>
    public CefContextMenuMediaStateFlags MediaState => MediaStateFlags;

    /// <summary>
    /// Returns the text of the misspelled word, if any, that the context menu was
    /// invoked on.
    /// </summary>
    public string GetMisspelledWord() => MisspelledWord;

    /// <summary>
    /// Returns flags representing the actions supported by the editable node, if
    /// any, that the context menu was invoked on.
    /// </summary>
    public CefContextMenuEditStateFlags EditState => EditStateFlags;
}
