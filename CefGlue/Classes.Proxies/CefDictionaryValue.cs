namespace Xilium.CefGlue;

public sealed unsafe partial class CefDictionaryValue
{
    /// <summary>
    /// Returns the number of values.
    /// </summary>
    public int Count => (int)Size;

    /// <summary>
    /// Returns the value type for the specified key.
    /// </summary>
    public CefValueType GetValueType(string key) => GetType(key);
}
