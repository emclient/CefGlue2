using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefZipReader
{
    /// <summary>
    /// Returns the name of the file.
    /// </summary>
    public string GetFileName() => FileName;

    /// <summary>
    /// Returns the uncompressed size of the file.
    /// </summary>
    public long GetFileSize() => FileSize;

    /// <summary>
    /// Read uncompressed file contents into the specified buffer. Returns &lt; 0 if
    /// an error occurred, 0 if at the end of file, or the number of bytes read.
    /// </summary>
    public int ReadFile(byte[] buffer, int offset, int length)
    {
        if (offset < 0 || length < 0 || buffer.Length - offset < length) throw new ArgumentOutOfRangeException();

        fixed (byte* buffer_ptr = buffer)
        {
            return cef_zip_reader_t.read_file(_self, buffer_ptr + offset, (UIntPtr)length);
        }
    }
}
