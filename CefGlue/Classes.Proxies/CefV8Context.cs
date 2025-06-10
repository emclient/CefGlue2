#nullable enable

namespace Xilium.CefGlue;

public sealed unsafe partial class CefV8Context
{
    /// <summary>
    /// Execute a string of JavaScript code in this V8 context. The |script_url|
    /// parameter is the URL where the script in question can be found, if any.
    /// The |start_line| parameter is the base line number to use for error
    /// reporting. On success |retval| will be set to the return value, if any,
    /// and the function will return true. On failure |exception| will be set to
    /// the exception, if any, and the function will return false.
    /// </summary>
    public bool TryEval(string code, string scriptUrl, int startLine, out CefV8Value? returnValue, out CefV8Exception? exception)
    {
        CefV8Value? _returnValue = null;
        CefV8Exception? _exception = null;
        bool result = Eval(code, scriptUrl, startLine, ref _returnValue, ref _exception);
        returnValue = _returnValue;
        exception = _exception;
        return result;
    }
}
