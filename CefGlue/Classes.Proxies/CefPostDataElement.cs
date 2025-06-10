using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefPostDataElement
{
    /// <summary>
    /// The post data element will represent bytes.  The bytes passed
    /// in will be copied.
    /// </summary>
    public void SetToBytes(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(nameof(bytes));
        fixed (byte* bytes_ptr = bytes)
            SetToBytes((nuint)bytes.Length, (IntPtr)bytes_ptr);
    }

    /// <summary>
    /// Return the type of this post data element.
    /// </summary>
    public CefPostDataElementType ElementType => Type;

    /// <summary>
    /// Return the file name.
    /// </summary>
    [Obsolete]
    public string GetFile() => File;

    /// <summary>
    /// Read up to |size| bytes into |bytes| and return the number of bytes
    /// actually read.
    /// </summary>
    public byte[] GetBytes()
    {
        var size = BytesCount;
        if (size == 0)
            return Array.Empty<byte>();

        var bytes = new byte[size];
        fixed (byte* bytes_ptr = bytes)
        {
            var written = cef_post_data_element_t.get_bytes(_self, (UIntPtr)size, bytes_ptr);
            if (written != size) throw new InvalidOperationException();
        }

        return bytes;
    }
}
