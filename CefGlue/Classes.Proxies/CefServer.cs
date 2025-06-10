using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefServer
{
    /// <summary>
    /// Create a new server that binds to |address| and |port|. |address| must be
    /// a valid IPv4 or IPv6 address (e.g. 127.0.0.1 or ::1) and |port| must be a
    /// port number outside of the reserved range (e.g. between 1025 and 65535 on
    /// most platforms). |backlog| is the maximum number of pending connections.
    /// A new thread will be created for each CreateServer call (the "dedicated
    /// server thread"). It is therefore recommended to use a different
    /// CefServerHandler instance for each CreateServer call to avoid thread
    /// safety issues in the CefServerHandler implementation. The
    /// CefServerHandler::OnServerCreated method will be called on the dedicated
    /// server thread to report success or failure. See
    /// CefServerHandler::OnServerCreated documentation for a description of
    /// server lifespan.
    /// </summary>
    public static void Create(string address, ushort port, int backlog, CefServerHandler handler)
    {
        fixed (char* address_str = address)
        {
            var n_address = new cef_string_t(address_str, address.Length);
            cef_server_t.create(&n_address, port, backlog, handler.ToNative());
        }
    }
}
