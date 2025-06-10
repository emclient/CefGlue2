namespace Xilium.CefGlue;

public sealed unsafe partial class CefFrame
{
    /// <summary>
    /// Returns the parent of this frame or NULL if this is the main (top-level)
    /// frame.
    /// </summary>
    public CefFrame Parent => GetParent();

    /// <summary>
    /// Returns the browser that this frame belongs to.
    /// </summary>
    public CefBrowser Browser => GetBrowser();

    /// <summary>
    /// Get the V8 context associated with the frame. This method can only be
    /// called from the render process.
    /// </summary>
    public CefV8Context V8Context => GetV8Context();
}
