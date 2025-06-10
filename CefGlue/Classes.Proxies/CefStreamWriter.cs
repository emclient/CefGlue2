using System;
using System.IO;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefStreamWriter
{
    /// <summary>
    /// Create a new CefStreamWriter object for a file.
    /// </summary>
    public static CefStreamWriter Create(string fileName) => CreateForFile(fileName);

    /// <summary>
    /// Create a new CefStreamWriter object for a custom handler.
    /// </summary>
    public static CefStreamWriter Create(CefWriteHandler handler) => CreateForHandler(handler);

    /// <summary>
    /// Write raw binary data.
    /// </summary>
    public int Write(byte[] buffer, int offset, int length)
    {
        if (offset < 0 || length < 0 || buffer.Length - offset < length)
            throw new ArgumentOutOfRangeException();

        fixed (byte* ptr = &buffer[offset])
            return (int)Write((IntPtr)ptr, (nuint)offset, (nuint)length);
    }

    /// <summary>
    /// Seek to the specified offset position. |whence| may be any one of
    /// SEEK_CUR, SEEK_END or SEEK_SET. Returns zero on success and non-zero on
    /// failure.
    /// </summary>
    public bool Seek(long offset, SeekOrigin whence) => Seek(offset, (int)whence) == 0;
}
