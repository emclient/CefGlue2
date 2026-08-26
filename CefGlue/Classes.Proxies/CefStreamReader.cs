using System;
using System.IO;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefStreamReader
{
    /// <summary>
    /// Create a new CefStreamReader object from a file.
    /// </summary>
    public static CefStreamReader Create(string fileName) => CreateForFile(fileName);

    /// <summary>
    /// Create a new CefStreamReader object from data.
    /// </summary>
    public static CefStreamReader Create(void* data, long size)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(size);
        return CreateForData((IntPtr)data, checked((nuint)size));
    }

    /// <summary>
    /// Create a new CefStreamReader object from a custom handler.
    /// </summary>
    public static CefStreamReader Create(CefReadHandler handler) => CreateForHandler(handler);

    /// <summary>
    /// Read raw binary data.
    /// </summary>
    public int Read(byte[] buffer, int offset, int length)
    {
        if (offset < 0 || length < 0 || buffer.Length - offset < length)
            throw new ArgumentOutOfRangeException();

        fixed (byte* ptr = buffer)
            return (int)Read((IntPtr)(ptr + offset), 1, (nuint)length);
    }

    /// <summary>
    /// Seek to the specified offset position. |whence| may be any one of
    /// SEEK_CUR, SEEK_END or SEEK_SET. Returns zero on success and non-zero on
    /// failure.
    /// </summary>
    public bool Seek(long offset, SeekOrigin whence) => Seek(offset, (int)whence) == 0;
}
