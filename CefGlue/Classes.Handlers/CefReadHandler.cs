using System;
using System.IO;

namespace Xilium.CefGlue;

public abstract unsafe partial class CefReadHandler
{
    private partial nuint Read(IntPtr ptr, nuint size, nuint n)
    {
        if (size == 0 || n == 0 || size > unchecked((nuint)long.MaxValue) / n)
            return 0;

        var length = (long)(size * n);
        using var stream = new UnmanagedMemoryStream((byte*)ptr, length, length, FileAccess.Write);
        var bytesRead = Read(stream, length);
        if (bytesRead < 0 || bytesRead > length)
            return 0;

        return (nuint)bytesRead / size;
    }

    /// <summary>
    /// Read raw binary data.
    /// </summary>
    protected abstract long Read(Stream stream, long length);
}
