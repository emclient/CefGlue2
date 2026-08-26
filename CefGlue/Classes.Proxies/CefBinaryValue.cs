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
        ArgumentNullException.ThrowIfNull(data);
        fixed (byte* data_ptr = data)
            return Create((IntPtr)data_ptr, (nuint)data.LongLength);
    }
    
    /// <summary>
    /// Read up to |buffer_size| number of bytes into |buffer|. Reading begins at
    /// the specified byte |data_offset|. Returns the number of bytes read.
    /// </summary>
    public long GetData(byte[] buffer, long bufferSize, long dataOffset)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(bufferSize);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bufferSize, buffer.LongLength);
        ArgumentOutOfRangeException.ThrowIfNegative(dataOffset);

        fixed (byte* buffer_ptr = buffer)
            return (long)GetData(
                (IntPtr)buffer_ptr,
                checked((nuint)bufferSize),
                checked((nuint)dataOffset));
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
