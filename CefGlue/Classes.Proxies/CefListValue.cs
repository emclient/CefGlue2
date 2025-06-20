using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

#nullable enable

public sealed unsafe partial class CefListValue
{
    /// <summary>
    /// Returns the number of values.
    /// </summary>
    public int Count => (int)Size;

    /// <summary>
    /// Removes the value at the specified index.
    /// </summary>
    public bool Remove(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return Remove((nuint)index);
    }

    /// <summary>
    /// Returns the value type at the specified index.
    /// </summary>
    public CefValueType GetValueType(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetType((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index. For simple types the returned
    /// value will copy existing data and modifications to the value will not
    /// modify this object. For complex types (binary, dictionary and list) the
    /// returned value will reference existing data and modifications to the value
    /// will modify this object.
    /// </summary>
    public CefValue? GetValue(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetValue((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type bool.
    /// </summary>
    public bool GetBool(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetBool((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type int.
    /// </summary>
    public int GetInt(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetInt((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type double.
    /// </summary>
    public double GetDouble(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetDouble((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type string.
    /// </summary>
    public string? GetString(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetString((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type binary. The returned
    /// value will reference existing data.
    /// </summary>
    public CefBinaryValue? GetBinary(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetBinary((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type dictionary. The returned
    /// value will reference existing data and modifications to the value will
    /// modify this object.
    /// </summary>
    public CefDictionaryValue? GetDictionary(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetDictionary((nuint)index);
    }

    /// <summary>
    /// Returns the value at the specified index as type list. The returned
    /// value will reference existing data and modifications to the value will
    /// modify this object.
    /// </summary>
    public CefListValue? GetList(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return GetList((nuint)index);
    }

    /// <summary>
    /// Sets the value at the specified index. Returns true if the value was set
    /// successfully. If |value| represents simple data then the underlying data
    /// will be copied and modifications to |value| will not modify this object.
    /// If |value| represents complex data (binary, dictionary or list) then the
    /// underlying data will be referenced and modifications to |value| will
    /// modify this object.
    /// </summary>
    public bool SetValue(int index, CefValue value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetValue((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type null. Returns true if the
    /// value was set successfully.
    /// </summary>
    public bool SetNull(int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetNull((nuint)index);
    }

    /// <summary>
    /// Sets the value at the specified index as type bool. Returns true if the
    /// value was set successfully.
    /// </summary>
    public bool SetBool(int index, bool value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetBool((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type int. Returns true if the
    /// value was set successfully.
    /// </summary>
    public bool SetInt(int index, int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetInt((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type double. Returns true if the
    /// value was set successfully.
    /// </summary>
    public bool SetDouble(int index, double value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetDouble((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type string. Returns true if the
    /// value was set successfully.
    /// </summary>
    public bool SetString(int index, string? value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetString((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type binary. Returns true if the
    /// value was set successfully. If |value| is currently owned by another
    /// object then the value will be copied and the |value| reference will not
    /// change. Otherwise, ownership will be transferred to this object and the
    /// |value| reference will be invalidated.
    /// </summary>
    public bool SetBinary(int index, CefBinaryValue value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetBinary((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type dict. Returns true if the
    /// value was set successfully. If |value| is currently owned by another
    /// object then the value will be copied and the |value| reference will not
    /// change. Otherwise, ownership will be transferred to this object and the
    /// |value| reference will be invalidated.
    /// </summary>
    public bool SetDictionary(int index, CefDictionaryValue value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetDictionary((nuint)index, value);
    }

    /// <summary>
    /// Sets the value at the specified index as type list. Returns true if the
    /// value was set successfully. If |value| is currently owned by another
    /// object then the value will be copied and the |value| reference will not
    /// change. Otherwise, ownership will be transferred to this object and the
    /// |value| reference will be invalidated.
    /// </summary>
    public bool SetList(int index, CefListValue value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        return SetList((nuint)index, value);
    }
}
