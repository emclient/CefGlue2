using System;
using System.Globalization;

namespace Xilium.CefGlue;

internal static class ExceptionBuilder
{
    public static CefVersionMismatchException RuntimeVersionApiHashMismatch(string actual, string expected, string expectedVersion)
    {
        return new CefVersionMismatchException(string.Format(CultureInfo.InvariantCulture,
            "CEF runtime version mismatch: loaded version API hash \"{0}\", but supported \"{1}\" ({2}).",
            actual,
            expected,
            expectedVersion));
    }

    public static Exception CefRuntimeAlreadyInitialized()
    {
        return new InvalidOperationException("CEF runtime already initialized.");
    }

    public static Exception CefRuntimeFailedToInitialize()
    {
        return new InvalidOperationException("Failed to initialize CEF runtime.");
    }

    public static Exception UnsupportedPlatform()
    {
        return new InvalidOperationException("Unsupported platform.");
    }

    public static Exception FailedToCreateBrowser(int errorCode)
    {
        return new InvalidOperationException($"Failed to create browser with error code ({errorCode})");
    }

    public static Exception ObjectDisposed()
    {
        return new InvalidOperationException("Object disposed.");
    }
}
