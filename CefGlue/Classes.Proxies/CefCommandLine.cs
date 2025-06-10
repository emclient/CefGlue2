namespace Xilium.CefGlue;

public sealed unsafe partial class CefCommandLine
{
    /// <summary>
    /// Create a new CefCommandLine instance.
    /// </summary>
    public static CefCommandLine Create()
        => CreateCommandLine();

    /// <summary>
    /// Returns the singleton global CefCommandLine object. The returned object
    /// will be read-only.
    /// </summary>
    public static CefCommandLine Global
        => GetGlobalCommandLine();

    /// <summary>
    /// Constructs and returns the represented command line string. Use this
    /// method cautiously because quoting behavior is unclear.
    /// </summary>
    public override string ToString() => CommandLineString;

    /// <summary>
    /// Get the program part of the command line string (the first item).
    /// </summary>
    public string GetProgram() => Program;

    /// <summary>
    /// Add a switch with the specified value to the end of the command line.
    /// If the switch has no value pass an empty value string.
    /// </summary>
    public void AppendSwitch(string name, string value) => AppendSwitchWithValue(name, value);

    /// <summary>
    /// Insert an argument to the beginning of the command line.
    /// Unlike PrependWrapper this method doesn't strip argument by spaces.
    /// </summary>
    public void PrependArgument(string argument)
    {
        if (argument.IndexOf(' ') >= 0)
        {
            // When argument contains spaces, we just prepend command line with dummy wrapper
            // and then replace it with actual argument.
            PrependWrapper(".");
            SetProgram(argument);
        }
        else PrependWrapper(argument);
    }
}
