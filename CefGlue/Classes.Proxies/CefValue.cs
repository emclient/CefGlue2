namespace Xilium.CefGlue;

public sealed unsafe partial class CefValue
{
    /// <summary>
    /// Returns the underlying value type.
    /// </summary>
    public CefValueType GetValueType() => Type;

    /// <summary>
    /// Returns the underlying value as type bool.
    /// </summary>
    public bool GetBool() => Bool;

    /// <summary>
    /// Returns the underlying value as type int.
    /// </summary>
    public int GetInt() => Int;

    /// <summary>
    /// Returns the underlying value as type double.
    /// </summary>
    public double GetDouble() => Double;

    /// <summary>
    /// Returns the underlying value as type string.
    /// </summary>
    public string GetString() => String;
}
