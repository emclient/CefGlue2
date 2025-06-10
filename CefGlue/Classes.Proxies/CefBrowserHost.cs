using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefBrowserHost
{
    /// <summary>
    /// Create a new browser using the window parameters specified by
    /// |windowInfo|. All values will be copied internally and the actual window
    /// (if any) will be created on the UI thread. If |request_context| is empty
    /// the global request context will be used. This method can be called on any
    /// browser process thread and will not block. The optional |extra_info|
    /// parameter provides an opportunity to specify extra information specific to
    /// the created browser that will be passed to
    /// CefRenderProcessHandler::OnBrowserCreated() in the render process.
    /// </summary>
    public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url, CefDictionaryValue extraInfo = null, CefRequestContext requestContext = null)
        => CreateBrowser(windowInfo, client, url, settings, extraInfo, requestContext);

    /// <summary>
    /// Create a new browser using the window parameters specified by
    /// |windowInfo|. If |request_context| is empty the global request context
    /// will be used. This method can only be called on the browser process UI
    /// thread. The optional |extra_info| parameter provides an opportunity to
    /// specify extra information specific to the created browser that will be
    /// passed to CefRenderProcessHandler::OnBrowserCreated() in the render
    /// process.
    /// </summary>
    public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url, CefDictionaryValue extraInfo = null, CefRequestContext requestContext = null)
        => CreateBrowserSync(windowInfo, client, url, settings, extraInfo, requestContext);

    /// <summary>
    /// Retrieve the window handle (if any) for this browser. If this browser is
    /// wrapped in a CefBrowserView this method should be called on the browser
    /// process UI thread and it will return the handle for the top-level native
    /// window.
    /// </summary>
    public IntPtr GetWindowHandle() => WindowHandle;

    /// <summary>
    /// Retrieve the window handle (if any) of the browser that opened this
    /// browser. Will return NULL for non-popup browsers or if this browser is
    /// wrapped in a CefBrowserView. This method can be used in combination with
    /// custom handling of modal windows.
    /// </summary>
    public IntPtr GetOpenerWindowHandle() => OpenerWindowHandle;

    /// <summary>
    /// Get the default zoom level. This value will be 0.0 by default but can be
    /// configured with the Chrome runtime. This method can only be called on the
    /// UI thread.
    /// </summary>
    public double GetDefaultZoomLevel() => DefaultZoomLevel;

    /// <summary>
    /// Get the current zoom level. The default zoom level is 0.0. This method can
    /// only be called on the UI thread.
    /// </summary>
    public double GetZoomLevel() => ZoomLevel;

    /// <summary>
    /// Send a method call message over the DevTools protocol. |message| must be a
    /// UTF8-encoded JSON dictionary that contains "id" (int), "method" (string)
    /// and "params" (dictionary, optional) values. See the DevTools protocol
    /// documentation at https://chromedevtools.github.io/devtools-protocol/ for
    /// details of supported methods and the expected "params" dictionary
    /// contents. |message| will be copied if necessary. This method will return
    /// true if called on the UI thread and the message was successfully submitted
    /// for validation, otherwise false. Validation will be applied asynchronously
    /// and any messages that fail due to formatting errors or missing parameters
    /// may be discarded without notification. Prefer ExecuteDevToolsMethod if a
    /// more structured approach to message formatting is desired.
    /// Every valid method call will result in an asynchronous method result or
    /// error message that references the sent message "id". Event messages are
    /// received while notifications are enabled (for example, between method
    /// calls for "Page.enable" and "Page.disable"). All received messages will be
    /// delivered to the observer(s) registered with AddDevToolsMessageObserver.
    /// See CefDevToolsMessageObserver::OnDevToolsMessage documentation for
    /// details of received message contents.
    /// Usage of the SendDevToolsMessage, ExecuteDevToolsMethod and
    /// AddDevToolsMessageObserver methods does not require an active DevTools
    /// front-end or remote-debugging session. Other active DevTools sessions will
    /// continue to function independently. However, any modification of global
    /// browser state by one session may not be reflected in the UI of other
    /// sessions.
    /// Communication with the DevTools front-end (when displayed) can be logged
    /// for development purposes by passing the
    /// `--devtools-protocol-log-file=&lt;path&gt;` command-line flag.
    /// </summary>
    public bool SendDevToolsMessage(IntPtr message, int messageSize)
    {
        return cef_browser_host_t.send_dev_tools_message(
            _self, (void*)message, checked((UIntPtr)messageSize)) != 0;
    }

    /// <summary>
    /// Returns the maximum rate in frames per second (fps) that
    /// CefRenderHandler::OnPaint will be called for a windowless browser. The
    /// actual fps may be lower if the browser cannot generate frames at the
    /// requested rate. The minimum value is 1 and the maximum value is 60
    /// (default 30). This method can only be called on the UI thread.
    /// </summary>
    public int GetWindowlessFrameRate() => WindowlessFrameRate;

    /// <summary>
    /// Begins a new composition or updates the existing composition. Blink has a
    /// special node (a composition node) that allows the input method to change
    /// text without affecting other DOM nodes. |text| is the optional text that
    /// will be inserted into the composition node. |underlines| is an optional
    /// set of ranges that will be underlined in the resulting text.
    /// |replacement_range| is an optional range of the existing text that will be
    /// replaced. |selection_range| is an optional range of the resulting text
    /// that will be selected after insertion or replacement. The
    /// |replacement_range| value is only used on OS X.
    /// This method may be called multiple times as the composition changes. When
    /// the client is done making changes the composition should either be
    /// canceled or completed. To cancel the composition call
    /// ImeCancelComposition. To complete the composition call either
    /// ImeCommitText or ImeFinishComposingText. Completion is usually signaled
    /// when:
    /// 1. The client receives a WM_IME_COMPOSITION message with a GCS_RESULTSTR
    /// flag (on Windows), or;
    /// 2. The client receives a "commit" signal of GtkIMContext (on Linux), or;
    /// 3. insertText of NSTextInput is called (on Mac).
    /// This method is only used when window rendering is disabled.
    /// </summary>
    public void ImeSetComposition(string text,
        int underlinesCount,
        CefCompositionUnderline underlines,
        CefRange replacementRange,
        CefRange selectionRange)
    {
        fixed (char* text_ptr = text)
        {
            cef_string_t n_text = new cef_string_t(text_ptr, text != null ? text.Length : 0);
            UIntPtr n_underlinesCount = checked((UIntPtr)underlinesCount);
            var n_underlines = underlines.ToNative();
            cef_range_t n_replacementRange = new cef_range_t(replacementRange.From, replacementRange.To);
            cef_range_t n_selectionRange = new cef_range_t(selectionRange.From, selectionRange.To);

            cef_browser_host_t.ime_set_composition(_self, &n_text, n_underlinesCount, &n_underlines, &n_replacementRange, &n_selectionRange);
        }
    }
}
