using System;
using System.IO;

namespace Xilium.CefGlue;

public abstract unsafe partial class CefResourceHandler
{
    private partial bool Read(IntPtr dataOut, int bytesToRead, ref int bytesRead, CefResourceReadCallback callback)
    {

        using var m_stream = new UnmanagedMemoryStream((byte*)dataOut, bytesToRead, bytesToRead, FileAccess.Write);
        var m_result = Read(m_stream, bytesToRead, out var m_bytesRead, callback);
        bytesRead = m_bytesRead;
        return m_result;
    }

    /// <summary>
    /// Read response data. If data is available immediately copy up to
    /// |bytes_to_read| bytes into |response|, set |bytes_read| to the number of
    /// bytes copied, and return true. To read the data at a later time keep a
    /// pointer to |data_out|, set |bytes_read| to 0, return true and execute
    /// |callback| when the data is available (|response| will remain valid until
    /// the callback is executed). To indicate response completion set
    /// |bytes_read| to 0 and return false. To indicate failure set |bytes_read|
    /// to &lt; 0 (e.g. -2 for ERR_FAILED) and return false. This method will be
    /// called in sequence but not from a dedicated thread. For backwards
    /// compatibility set |bytes_read| to -1 and return false and the ReadResponse
    /// method will be called.
    /// </summary>
    protected virtual bool Read(Stream response, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
    {
        bytesRead = -2;
        return false;
    }

    private partial bool ReadResponse(IntPtr dataOut, int bytesToRead, ref int bytesRead, CefCallback callback)
    {
        using var m_stream = new UnmanagedMemoryStream((byte*)dataOut, bytesToRead, bytesToRead, FileAccess.Write);
#pragma warning disable CS0618 // Type or member is obsolete
        var m_result = ReadResponse(m_stream, bytesToRead, out var m_bytesRead, callback);
#pragma warning restore CS0618 // Type or member is obsolete
        bytesRead = m_bytesRead;
        return m_result;
    }

    /// <summary>
    /// Read response data. If data is available immediately copy up to
    /// |bytes_to_read| bytes into |data_out|, set |bytes_read| to the number of
    /// bytes copied, and return true. To read the data at a later time set
    /// |bytes_read| to 0, return true and call CefCallback::Continue() when the
    /// data is available. To indicate response completion return false.
    /// WARNING: This method is deprecated. Use Skip and Read instead.
    /// </summary>
    [Obsolete("This method is deprecated. Use Skip and Read instead.")]
    protected virtual bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
    {
        bytesRead = 0;
        return false;
    }
}
