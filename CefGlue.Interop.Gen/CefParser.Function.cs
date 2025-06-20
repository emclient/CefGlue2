using System.Diagnostics.CodeAnalysis;

namespace CefParser
{
    public partial class CefParser
    {
        public class Function
        {
            string retVal;
            string argVal;
            Func<string, (TypeClass, string)> resolveType;

            string? capiName;
            ObjAnalysis? parsedRetVal;
            IReadOnlyList<ObjArgument>? parsedArguments;
            HashSet<string>? optionalParams = null;

            public string Attrib { get; }
            public ObjAnalysis ReturnType { get { if (parsedRetVal is null) Resolve(); return parsedRetVal.Value; } }
            public IReadOnlyList<ObjArgument> Arguments { get { if (parsedArguments is null) Resolve(); return parsedArguments; } }
            public string[] Comment { get; }
            public string Name { get; }

            public int VersionAdded { get; }
            public int VersionRemoved { get; }
            public string? DefaultReturnValue { get; }

            public virtual string GetCApiName(string prefix = "")
            {
                if (parsedRetVal is null) Resolve();
                return CefParser.GetCApiName(capiName ?? Name, false, prefix);
            }

            [MemberNotNull(nameof(parsedRetVal), nameof(parsedArguments))]
            void Resolve()
            {
                parsedRetVal = new ObjAnalysis(retVal, resolveType);

                if (!string.IsNullOrEmpty(argVal))
                {
                    List<ObjArgument> args = new();
                    var argList = argVal.Split(',');
                    int argIndex = 0;
                    while (argIndex < argList.Length)
                    {
                        var arg = argList[argIndex];
                        if (arg.IndexOf('<') >= 0 && arg.IndexOf('>') < 0)
                        {
                            // We've split inside of a template type declaration. Join the
                            // next argument with this argument.
                            argIndex++;
                            arg += "," + argList[argIndex];
                        }

                        arg = arg.Trim();
                        if (arg.Length > 0)
                        {
                            int nameSeparatorPos = arg.LastIndexOfAny([' ', '\n']);
                            if (nameSeparatorPos < 0)
                                throw new FormatException($"Invalid argument: {arg}");

                            string argType = arg[..nameSeparatorPos];
                            string argName = arg[(nameSeparatorPos + 1)..];

                            args.Add(new ObjArgument(argType, argName, resolveType, isOptional: optionalParams?.Contains(argName) == true));
                        }

                        argIndex++;
                    }
                    parsedArguments = args;
                }
                else
                {
                    parsedArguments = Array.Empty<ObjArgument>();
                }
            }

            public IEnumerable<(string Type, string Name)> CInteropArgs
            {
                get
                {
                    foreach (var arg in Arguments)
                    {
                        if (arg.Type.IsMultiArg)
                            yield return (arg.Type.IsConst ? "nuint" : "nuint*", $"{arg.Name}_count");
                        yield return (arg.Type.CInteropType, NameConverter.QuoteName(arg.Name));
                    }
                }
            }

            public string CInteropArgsPrototype
            {
                get
                {
                    return string.Join(", ", CInteropArgs.Select(a => $"{a.Type} {a.Name}"));
                }
            }

            public virtual string CInteropName
            {
                get
                {
                    return GetCApiName();
                }
            }

            public Function(string attrib, string retVal, string argVal, string[] comment, Func<string, (TypeClass, string)> resolveType)
            {
                this.Attrib = attrib;
                this.Comment = comment;
                this.argVal = argVal;
                this.resolveType = resolveType;

                // Split retVal into the return value type and the function name
                retVal = retVal.Trim();
                int nameSeparatorPos = retVal.LastIndexOfAny([' ', '\n']);
                if (nameSeparatorPos < 0)
                    throw new FormatException($"Invalid return value: {retVal}");
                this.retVal = retVal[..nameSeparatorPos].Trim();
                this.Name = retVal[(nameSeparatorPos + 1)..];

                // Parse attributes
                foreach (var attribPair in Attrib.Split(','))
                {
                    var ap = attribPair.Trim();
                    if (ap.StartsWith("capi_name="))
                        capiName = ap[10..];
                    if (ap.StartsWith("optional_param="))
                        (optionalParams ?? (optionalParams = new())).Add(ap[15..]);
                    if (ap.StartsWith("added="))
                        VersionAdded = CefParser.VersionNumberFromString(ap[6..]);
                    if (ap.StartsWith("removed="))
                        VersionRemoved = CefParser.VersionNumberFromString(ap[6..]);
                    if (ap.StartsWith("default_retval="))
                        DefaultReturnValue = ap[15..];
                }
            }
        }

        public class VirtualFunction : Function
        {
            public bool IsConst { get; }
            public bool IsAbstract { get; }

            public VirtualFunction(string attrib, string retVal, string argVal, string[] comment, Func<string, (TypeClass, string)> resolveType, string virtualMods, bool isAbstract)
                : base(attrib, retVal, argVal, comment, resolveType)
            {
                IsConst = virtualMods == "const";
                IsAbstract = isAbstract;
            }
        }

        public class StaticFunction : Function
        {
            string classCApiName;

            public StaticFunction(string attrib, string retVal, string argVal, string[] comment, Func<string, (TypeClass, string)> resolveType, string clsName)
                : base(attrib, retVal, argVal, comment, resolveType)
            {
                classCApiName = CefParser.GetCApiName(clsName, false);
            }

            public override string GetCApiName(string prefix = "")
            {
                if (string.IsNullOrEmpty(prefix))
                    prefix = classCApiName;
                return base.GetCApiName(prefix);
            }

            public override string CInteropName
            {
                get
                {
                    string csnName = CefParser.GetCApiName(this.Name, false);
                    var prefix = classCApiName;
                    if (prefix.StartsWith("cef"))
                    {
                        int pos = csnName.IndexOf(prefix[3..]);
                        if (pos > 0)
                            csnName = csnName[0..pos];
                    }
                    return csnName;
                }
            }
        }
    }
}
