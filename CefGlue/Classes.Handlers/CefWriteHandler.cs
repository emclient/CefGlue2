using System;
using System.IO;

namespace Xilium.CefGlue;

public abstract unsafe partial class CefWriteHandler
{
    private partial nuint Write(IntPtr ptr, nuint size, nuint n)
    {
        var length = (long)size * (long)n;
        using var stream = new UnmanagedMemoryStream((byte*)ptr, length, length, FileAccess.Write);
        return (UIntPtr)Write(stream, length);
    }
    
    /// <summary>
    /// Write raw binary data.
    /// </summary>
    protected abstract long Write(Stream stream, long length);
}
