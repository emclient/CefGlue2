using System;
using System.IO;

namespace Xilium.CefGlue;

public abstract unsafe partial class CefWriteHandler
{
    private partial nuint Write(IntPtr ptr, nuint size, nuint n)
    {
        if (size == 0 || n == 0 || size > unchecked((nuint)long.MaxValue) / n)
            return 0;

        var length = (long)(size * n);
        using var stream = new UnmanagedMemoryStream((byte*)ptr, length, length, FileAccess.Read);
        var bytesWritten = Write(stream, length);
        if (bytesWritten < 0 || bytesWritten > length)
            return 0;

        return (nuint)bytesWritten / size;
    }
    
    /// <summary>
    /// Write raw binary data.
    /// </summary>
    protected abstract long Write(Stream stream, long length);
}
