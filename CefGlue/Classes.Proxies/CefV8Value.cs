using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefV8Value
{
    /// <summary>
    /// Return a bool value.
    /// </summary>
    public bool GetBoolValue() => BoolValue;

    /// <summary>
    /// Return an int value.
    /// </summary>
    public int GetIntValue() => IntValue;

    /// <summary>
    /// Return an unsigned int value.
    /// </summary>
    public uint GetUIntValue() => UIntValue;

    /// <summary>
    /// Return a double value.
    /// </summary>
    public double GetDoubleValue() => DoubleValue;

    /// <summary>
    /// Return a string value.
    /// </summary>
    public string GetStringValue() => StringValue;

    /// <summary>
    /// Read the keys for the object's values into the specified vector. Integer-
    /// based keys will also be returned as strings.
    /// </summary>
    public bool TryGetKeys(out string[] keys)
    {
        var list = libcef.string_list_alloc();
        var result = cef_v8_value_t.get_keys(_self, list) != 0;
        if (result) keys = cef_string_list.ToArray(list);
        else keys = null;
        libcef.string_list_free(list);
        return result;
    }

    /// <summary>
    /// Returns the amount of externally allocated memory registered for the
    /// object.
    /// </summary>
    public int GetExternallyAllocatedMemory() => ExternallyAllocatedMemory;

    // ARRAY METHODS - These methods are only available on arrays.

    /// <summary>
    /// Returns the number of elements in the array.
    /// </summary>
    public int GetArrayLength() => ArrayLength;

    // ARRAY BUFFER METHODS - These methods are only available on ArrayBuffers.

    /// <summary>
    /// Returns the length (in bytes) of the ArrayBuffer.
    /// </summary>
    public UIntPtr GetArrayBufferByteLength() => ArrayBufferByteLength;

    /// <summary>
    /// Returns a pointer to the beginning of the memory block for this
    /// ArrayBuffer backing store. The returned pointer is valid as long as the
    /// CefV8Value is alive.
    /// </summary>
    public IntPtr GetArrayBufferData() => ArrayBufferData;

    // FUNCTION METHODS - These methods are only available on functions.

    /// <summary>
    /// Returns the function name.
    /// </summary>
    public string GetFunctionName() => FunctionName;

    /// <summary>
    /// Execute the function using the current V8 context. This method should only
    /// be called from within the scope of a CefV8Handler or CefV8Accessor
    /// callback, or in combination with calling Enter() and Exit() on a stored
    /// CefV8Context reference. |object| is the receiver ('this' object) of the
    /// function. If |object| is empty the current context's global object will be
    /// used. |arguments| is the list of arguments that will be passed to the
    /// function. Returns the function return value on success. Returns NULL if
    /// this method is called incorrectly or an exception is thrown.
    /// </summary>
    public CefV8Value ExecuteFunction(CefV8Value obj, CefV8Value[] arguments)
    {
        var n_arguments = CreateArguments(arguments);
        cef_v8_value_t* n_retval;

        fixed (cef_v8_value_t** n_arguments_ptr = n_arguments)
        {
            n_retval = cef_v8_value_t.execute_function(
                _self,
                obj != null ? obj.ToNative() : null,
                n_arguments != null ? (UIntPtr)n_arguments.Length : UIntPtr.Zero,
                n_arguments_ptr
                );
        }

        return CefV8Value.FromNativeOrNull(n_retval);
    }

    /// <summary>
    /// Execute the function using the specified V8 context. |object| is the
    /// receiver ('this' object) of the function. If |object| is empty the
    /// specified context's global object will be used. |arguments| is the list of
    /// arguments that will be passed to the function. Returns the function return
    /// value on success. Returns NULL if this method is called incorrectly or an
    /// exception is thrown.
    /// </summary>
    public CefV8Value ExecuteFunctionWithContext(CefV8Context context, CefV8Value obj, CefV8Value[] arguments)
    {
        var n_arguments = CreateArguments(arguments);
        cef_v8_value_t* n_retval;

        fixed (cef_v8_value_t** n_arguments_ptr = n_arguments)
        {
            n_retval = cef_v8_value_t.execute_function_with_context(
                _self,
                context.ToNative(),
                obj != null ? obj.ToNative() : null,
                n_arguments != null ? (UIntPtr)n_arguments.Length : UIntPtr.Zero,
                n_arguments_ptr
                );
        }

        return CefV8Value.FromNativeOrNull(n_retval);
    }

    private static cef_v8_value_t*[] CreateArguments(CefV8Value[] arguments)
    {
        if (arguments == null) return null;

        var length = arguments.Length;
        if (length == 0) return null;

        var result = new cef_v8_value_t*[arguments.Length];

        for (var i = 0; i < length; i++)
        {
            result[i] = arguments[i].ToNative();
        }

        return result;
    }

    /// <summary>
    /// Associates a value with the specified identifier and returns true on
    /// success. Returns false if this method is called incorrectly or an
    /// exception is thrown. For read-only values this method will return true
    /// even though assignment failed.
    /// </summary>
    public bool SetValue(string key, CefV8Value value) => SetValue(key, value, CefV8PropertyAttribute.None);
}
