using System;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefBinaryValue
{
    /// <summary>
    /// Creates a new object that is not owned by any other object. The specified
    /// |data| will be copied.
    /// </summary>
    public static CefBinaryValue Create(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(nameof(data));
        fixed (byte* data_ptr = data)
            return Create((IntPtr)data_ptr, (nuint)data.LongLength);
    }
    
    /// <summary>
    /// Read up to |buffer_size| number of bytes into |buffer|. Reading begins at
    /// the specified byte |data_offset|. Returns the number of bytes read.
    /// </summary>
    public long GetData(byte[] buffer, long bufferSize, long dataOffset)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(buffer.LongLength, dataOffset + bufferSize, nameof(dataOffset));
        fixed (byte* buffer_ptr = buffer)
            return (long)GetData((IntPtr)buffer_ptr, (nuint)bufferSize, (nuint)dataOffset);
    }

    public byte[] ToArray()
    {
        var value = new byte[Size];
        var read = GetData(value, value.Length, 0);
        if (read != value.Length)
            throw new InvalidOperationException();
        return value;
    }
}
