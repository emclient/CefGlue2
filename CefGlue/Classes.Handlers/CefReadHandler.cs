using System;
using System.IO;

namespace Xilium.CefGlue;

public abstract unsafe partial class CefReadHandler
{
    private partial nuint Read(IntPtr ptr, nuint size, nuint n)
    {
        var length = (long)size * (long)n;
        using var stream = new UnmanagedMemoryStream((byte*)ptr, length, length, FileAccess.Write);
        return (UIntPtr)Read(stream, length);
    }

    /// <summary>
    /// Read raw binary data.
    /// </summary>
    protected abstract long Read(Stream stream, long length);
}
