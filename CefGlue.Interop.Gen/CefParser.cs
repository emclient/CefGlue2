using System.Text;
using System.Text.RegularExpressions;

namespace CefParser
{
    public partial class CefParser
    {
        List<TypeDef> typeDefs = new();
        Dictionary<string, string> globalTypeDefMap = new();
        Dictionary<string, string> versionProperties = new();
        List<EnumDef> enums = new();
        List<Function> globalFunctions = new();
        List<Class> classes = new();
        HashSet<string> enumNames = new();

        public IReadOnlyList<Function> GlobalFunctions => globalFunctions;
        public IReadOnlyList<Class> Classes => classes;
        public IReadOnlyList<EnumDef> Enums => enums;
        public IReadOnlyDictionary<string, string> VersionProperties => versionProperties;
        public int StableApiVersion { get; private set; }
        public Dictionary<int, string> ApiHashWindows { get; private set; } = new();
        public Dictionary<int, string> ApiHashMacOS { get; private set; } = new();
        public Dictionary<int, string> ApiHashLinux { get; private set; } = new();

        public CefParser(string[] files)
        {
            foreach (var file in files)
                ParseFile(file);
        }

        private void ParseFile(string filePath)
        {
            var fileData = new StreamReader(filePath).ReadToEnd();
            ParseFileData(fileData, Path.GetFileName(filePath));
        }

        public void ParseFileData(string fileData, string fileName)
        {
            if (fileData.Contains("THIS FILE IS FOR TESTING PURPOSES ONLY."))
                return;
            //if (fileName == "cef_logging.h" || fileName == "cef_string.h")
            //    return;

            // Remove space from between template definition end brackets
            fileData = fileData.Replace("> >", ">>").Replace("\r\n", "\n");

            if (fileName is "cef_types.h" || fileName.StartsWith("cef_types_"))
            {
                // Find out global enum names
                var enumMatch = EnumRegex().Match(fileData);
                while (enumMatch.Success)
                {
                    var enumBody = enumMatch.Groups[1].Value;
                    var enumName = enumMatch.Groups[2].Value;
                    enumNames.Add(enumName);
                    ParseEnumBody(enumName, enumBody);
                    enumMatch = enumMatch.NextMatch();
                }
            }
            else if (fileName is "cef_version.h")
            {
                var defineMatch = DefineRegex().Match(fileData);
                while (defineMatch.Success)
                {
                    var defineName = defineMatch.Groups[1].Value;
                    var defineValue = defineMatch.Groups[2].Value;
                    versionProperties.Add(defineName, defineValue);
                    defineMatch = defineMatch.NextMatch();
                }
            }
            else if (fileName is "cef_api_versions.h")
            {
                var apiVersionMatch = ApiVersionLastRegex().Match(fileData);
                int stableApiVersion;
                if (!apiVersionMatch.Success || !int.TryParse(apiVersionMatch.Groups[1].Value, out stableApiVersion))
                    throw new FormatException("Could not determine API version");
                StableApiVersion = stableApiVersion;

                // Figure out the platform hashes
                var apiHashMatch = ApiHashRegex().Match(fileData);
                while (apiHashMatch.Success)
                {
                    var platformDictionary = apiHashMatch.Groups[1].Value switch
                    {
                        "WIN" => ApiHashWindows,
                        "MAC" => ApiHashMacOS,
                        "LINUX" => ApiHashLinux,
                        _ => throw new FormatException($"Unknown platform {apiHashMatch.Groups[1].Value}")
                    };
                    var apiVersion = int.Parse(apiHashMatch.Groups[2].Value);
                    var apiHash = apiHashMatch.Groups[3].Value;
                    platformDictionary.Add(apiVersion, apiHash);
                    apiHashMatch = apiHashMatch.NextMatch();
                }
            }

            // Extract global typedefs
            var typeDefMatch = GlobalTypeDefRegex().Match(fileData);
            while (typeDefMatch.Success)
            {
                var value = typeDefMatch.Groups[1].Value;
                var pos = value.LastIndexOf(' ');
                if (pos < 0)
                    throw new FormatException($"Invalid typedef: {value}");
                var alias = value[(pos + 1)..];
                value = value[0..pos];

                typeDefs.Add(new TypeDef(fileName, value, alias));

                typeDefMatch = typeDefMatch.NextMatch();
            }

            // Extract global functions
            var functionMatch = GlobalFunctionRegex().Match(fileData);
            while (functionMatch.Success)
            {
                var attrib = functionMatch.Groups[1].Value;
                var retval = functionMatch.Groups[2].Value;
                var argval = functionMatch.Groups[3].Value;
                var comment = GetComment(fileData, $"{retval}({argval});");
                ValidateComment(fileName, retval.ToString(), comment);

                globalFunctions.Add(new Function(attrib, retval, argval, comment, ResolveType));

                functionMatch = functionMatch.NextMatch();
            }

            // TODO: We don't need includes or forward_declares, so not
            // parsing that for now.

            // Extract classes
            var classMatch = ClassRegex().Match(fileData);
            while (classMatch.Success)
            {
                var classAttrib = classMatch.Groups[1].Value;
                var name = classMatch.Groups[2].Value;
                var parentName = classMatch.Groups[3].Value;

                var classBodyStart = classMatch.Index + classMatch.Length;
                var classBodyEnd = fileData.IndexOf("};", classBodyStart);
                var bodySpan = fileData.AsSpan(classBodyStart, classBodyEnd - classBodyStart);

                // Style may place the ':' on the next line.
                var classComment = GetComment(fileData, $"{name} :");
                if (classComment.Length == 0)
                    classComment = GetComment(fileData, $"{name}\n");
                ValidateComment(fileName, name, classComment);

                if (bodySpan.Length == 0)
                {
                    // Empty class
                    classes.Add(new(classAttrib, name, parentName, classComment));
                }
                else
                {
                    var body = bodySpan.ToString();

                    // Extract typedefs
                    List<TypeDef> classTypeDefs = new();
                    typeDefMatch = ClassTypeDefRegex().Match(body);
                    while (typeDefMatch.Success)
                    {
                        var value = typeDefMatch.Groups[1].Value;
                        var pos = value.LastIndexOf(' ');
                        if (pos < 0)
                            throw new FormatException($"Invalid typedef: {value}");
                        var alias = value[(pos + 1)..];
                        value = value[0..pos];
                        classTypeDefs.Add(new TypeDef(fileName, value.ToString(), alias.ToString()));
                        typeDefMatch = typeDefMatch.NextMatch();
                    }

                    var localTypeDefMap = classTypeDefs.ToDictionary(t => t.Alias, t => t.Value);
                    (TypeClass, string) LocalResolveType(string typeName) =>
                        ResolveType(localTypeDefMap.TryGetValue(typeName, out var v) ? v : typeName);

                    // Extract static functions
                    List<StaticFunction> classStaticFuncs = new();
                    functionMatch = ClassStaticFunctionRegex().Match(body);
                    while (functionMatch.Success)
                    {
                        var attrib = functionMatch.Groups[1].Value;
                        var retval = functionMatch.Groups[2].Value;
                        var argval = functionMatch.Groups[3].Value;
                        var comment = GetComment(body, $"{retval}({argval});");
                        ValidateComment(fileName, retval.ToString(), comment);

                        classStaticFuncs.Add(new StaticFunction(attrib, retval, argval, comment, LocalResolveType, name));

                        functionMatch = functionMatch.NextMatch();
                    }

                    // Extract virtual functions
                    List<VirtualFunction> classVirtualFuncs = new();
                    functionMatch = ClassVirtualFunctionRegex().Match(body);
                    while (functionMatch.Success)
                    {
                        var attrib = functionMatch.Groups[1].Value;
                        var retval = functionMatch.Groups[2].Value;
                        var argval = functionMatch.Groups[3].Value;
                        var vfmod = functionMatch.Groups[4].Value;
                        var isAbstract = body.AsSpan(functionMatch.Index + functionMatch.Length).TrimStart().StartsWith("= 0".AsSpan());
                        var comment = GetComment(body, $"{retval}({argval})");
                        ValidateComment(fileName, retval.ToString(), comment);

                        classVirtualFuncs.Add(new VirtualFunction(attrib, retval, argval, comment, LocalResolveType, vfmod, isAbstract));

                        functionMatch = functionMatch.NextMatch();
                    }

                    classes.Add(new(
                        classAttrib, name, parentName, classComment,
                        classStaticFuncs, classVirtualFuncs));
                }

                classMatch = classMatch.NextMatch();
            }
        }

        private void ParseEnumBody(string enumName, string enumBody)
        {
            bool isUint = enumName is "cef_transition_type_t" or "cef_drag_operations_mask_t"; // HACK: Special case
            bool isFlags = isUint;
            List<(string Name, string Value)> values = new();
            var enumValueMatch = EnumValueRegex().Match(enumBody);

            while (enumValueMatch.Success)
            {
                var enumValueName = enumValueMatch.Groups[1].Value;
                var enumValueValue = enumValueMatch.Groups[3].Value;

                if (enumValueValue.Contains("<<"))
                    isFlags = true;

                if (enumValueName is not "CEF_CONTENT_SETTING_TOP_LEVEL_TPCD_ORIGIN_TRIAL" && // HACK: Fixed name under #if
                    !enumValueName.EndsWith("_NUM_VALUES"))
                    values.Add((enumValueName, enumValueValue));

                enumValueMatch = enumValueMatch.NextMatch();
            }

            // Find common prefix
            int longestPrefixLength = 0;
            if (values.Count > 0)
            {
                string longestPrefix = values[0].Name;
                for (int i = 1; i < values.Count; i++)
                {
                    var currentName = values[i].Name;
                    int commonPrefixLength = 0;
                    while (commonPrefixLength < currentName.Length && commonPrefixLength < longestPrefix.Length && currentName[commonPrefixLength] == longestPrefix[commonPrefixLength])
                        commonPrefixLength++;
                    if (commonPrefixLength < longestPrefix.Length)
                        longestPrefix = currentName[..commonPrefixLength];
                }

                // Cut the common prefix at last _
                longestPrefixLength = longestPrefix.LastIndexOf('_') + 1;
            }

            List<EnumValue> translatedValues = new();
            foreach (var (name, value) in values)
            {
                string csharpName = NameConverter.ToCSharpEnumValueName(name[longestPrefixLength..]);
                string csharpValue =
                    value is "" || !char.IsLetter(value[0]) ? value :
                    value is "UINT_MAX" ? "uint.MaxValue" :
                    value.Length > longestPrefixLength ? NameConverter.ToCSharpEnumValueName(value[longestPrefixLength..]) :
                    value;
                translatedValues.Add(new EnumValue(name, csharpName, value, csharpValue));
            }

            enums.Add(new EnumDef(enumName, translatedValues, isFlags, isUint));
        }

        (TypeClass, string) ResolveType(string typeName)
        {
            if (globalTypeDefMap.Count == 0)
            {
                foreach (var typeDef in typeDefs)
                {
                    if (!globalTypeDefMap.ContainsKey(typeDef.Alias))
                        globalTypeDefMap[typeDef.Alias] = typeDef.Value;
                }
            }

            string resolvedName = globalTypeDefMap.TryGetValue(typeName, out var v) ? v : typeName;
            TypeClass typeClass = TypeClass.Unknown;
            if (enumNames.Contains(resolvedName))
                typeClass = TypeClass.Enum;
            return (typeClass, resolvedName);
        }


        /// <summary>
        /// Retrieve the start and end positions and value for the line immediately
        /// before the line containing the specified position.
        /// </summary>
        (int Start, int Length) GetPreviousLine(string body, int pos)
        {
            int end = body.LastIndexOf('\n', pos);
            if (end <= 0)
                return (0, 0);
            int start = body.LastIndexOf("\n", end - 1) + 1;
            return (start, end - start);
        }

        /// <summary>
        /// Retrieve the comment for a class or function.
        /// </summary>
        private string[] GetComment(string fileData, string name)
        {
            List<string> comment = new();
            bool inBlockComment = false;

            int pos = fileData.IndexOf(name);
            while (pos > 0)
            {
                var data = GetPreviousLine(fileData, pos);
                var line = fileData.AsSpan(data.Start, data.Length).Trim().ToString();
                pos = data.Start;
                if (line.Length == 0)
                    break;
                // Single line /*--cef()--*/
                if (line.StartsWith("/*") && line.EndsWith("*/"))
                    continue;
                // Start of multi line /*--cef()--*/
                if (inBlockComment && line.StartsWith("/*"))
                {
                    inBlockComment = false;
                    continue;
                }
                // End of multi line /*--cef()--*/
                if (!inBlockComment && line.EndsWith("*/"))
                {
                    inBlockComment = true;
                    continue;
                }
                if (inBlockComment)
                    continue;
                if (line.StartsWith("///"))
                {
                    // Keep the comment line including any leading spaces
                    comment.Add(line[3..]);
                }
            }

            comment.Reverse();
            return comment.ToArray();
        }

        /// <summary>
        /// Validate the comment array returned by GetComment.
        /// </summary>
        /// <remarks>
        /// Verify that the comment contains beginning and ending '///' as required by
        /// Doxygen (the leading '///' from each line will already have been removed by
        /// the GetComment logic).
        /// </remarks>
        private void ValidateComment(string fileName, string name, string[] comment)
        {
            if (comment.Length < 3 || comment[0].Length != 0 || comment[^1].Length != 0)
                throw new FormatException($"Missing or incorrect comment in {fileName} for {name}");
        }

        /// <summary>Convert a C++ CamelCaps name to a C API underscore name.</summary>
        public static string GetCApiName(string cppName, bool isClassName, string prefix = "")
        {
            StringBuilder result = new();
            char lastChar = '\0';

            foreach (var c in cppName)
            {
                // Add an underscore if the current character is an upper case letter
                // and the last character was a lower case letter.
                if (result.Length > 0 && !char.IsDigit(c) && char.IsUpper(c) && !char.IsUpper(lastChar))
                {
                    result.Append('_');
                }
                result.Append(char.ToLower(c));
                lastChar = c;
            }

            if (isClassName)
                result.Append("_t");

            string resultStr = result.ToString();

            if (!string.IsNullOrEmpty(prefix))
            {
                if (prefix.StartsWith("cef"))
                {
                    // If the prefix name is duplicated in the function name
                    // remove that portion of the function name.
                    var subprefix = prefix.Substring(3);
                    int pos = resultStr.IndexOf(subprefix);
                    if (pos >= 0)
                    {
                        return resultStr.Substring(0, pos) + resultStr.Substring(pos + subprefix.Length);
                    }
                }

                return prefix + "_" + resultStr;
            }

            return resultStr;
        }

        public static int VersionNumberFromString(string version)
        {
            if (version is "next")
                return 999998;
            if (version is "experimental")
                return 999999;
            return int.Parse(version);
        }

        // Regex for matching comment-formatted attributes
        private const string _cre_attrib = @"/\*--cef\(([A-Za-z0-9_ ,=:\n]{0,})\)--\*/";
        // Regex for matching class and function names
        private const string _cre_cfname = @"([A-Za-z0-9_]{1,})";
        // Regex for matching typedef value and name combination
        private const string _cre_typedef = @"([A-Za-z0-9_<>:,\*\&\s]{1,})";
        // Regex for matching function return value and name combination
        private const string _cre_func = @"([A-Za-z][A-Za-z0-9_<>:,\*\&\s]{1,})";
        // Regex for matching virtual function modifiers + arbitrary whitespace
        private const string _cre_vfmod = @"([\sA-Za-z0-9_]{0,})";
        // Regex for matching arbitrary whitespace
        private const string _cre_space = @"[\s]{1,}";
        // Regex for matching optional virtual keyword
        private const string _cre_virtual = @"(?:[\s]{1,}virtual){0,1}";

#if NET
        [GeneratedRegex($"\ntypedef{_cre_space}{_cre_typedef};", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex GlobalTypeDefRegex();

        [GeneratedRegex($"\n{_cre_attrib}\n{_cre_func}\\((.*?)\\)", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex GlobalFunctionRegex();

        [GeneratedRegex($"\n{_cre_attrib}\nclass{_cre_space}{_cre_cfname}{_cre_space}:{_cre_space}public{_cre_virtual}{_cre_space}{_cre_cfname}{_cre_space}{{", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex ClassRegex();

        [GeneratedRegex($"\n{_cre_space}typedef{_cre_space}{_cre_typedef};", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex ClassTypeDefRegex();

        [GeneratedRegex($"\n{_cre_space}{_cre_attrib}\n{_cre_space}static{_cre_space}{_cre_func}\\((.*?)\\)", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex ClassStaticFunctionRegex();

        [GeneratedRegex($"\n{_cre_space}{_cre_attrib}\n{_cre_space}virtual{_cre_space}{_cre_func}\\((.*?)\\){_cre_vfmod}", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex ClassVirtualFunctionRegex();

        [GeneratedRegex($"\ntypedef{_cre_space}enum{_cre_space}{{(.*?)}}{_cre_space}{_cre_cfname};", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex EnumRegex();

        [GeneratedRegex($"^#define{_cre_space}{_cre_cfname} (.+)$", RegexOptions.Multiline)]
        private static partial Regex DefineRegex();

        [GeneratedRegex($"^#define{_cre_space}CEF_API_VERSION_LAST{_cre_space}CEF_API_VERSION_(.+)$", RegexOptions.Multiline)]
        private static partial Regex ApiVersionLastRegex();

        [GeneratedRegex($"defined\\(OS_(WIN|MAC|LINUX)\\)\n#define{_cre_space}CEF_API_HASH_([0-9]+){_cre_space}\"([0-9a-f]*)\"", RegexOptions.Multiline)]
        private static partial Regex ApiHashRegex();

        [GeneratedRegex($"^\\s+([A-Z_]+)( = (.+?),?)?")];
        private static partial Regex EnumValueRegex();
#else
        private static Regex globalTypeDefRegex = new Regex($"\ntypedef{_cre_space}{_cre_typedef};", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex GlobalTypeDefRegex() => globalTypeDefRegex;

        private static Regex globalFunctionRegex = new Regex($"\n{_cre_attrib}\n{_cre_func}\\((.*?)\\)", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex GlobalFunctionRegex() => globalFunctionRegex;

        private static Regex classRegex = new Regex($"\n{_cre_attrib}\nclass{_cre_space}{_cre_cfname}{_cre_space}:{_cre_space}public{_cre_virtual}{_cre_space}{_cre_cfname}{_cre_space}{{", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex ClassRegex() => classRegex;

        private static Regex classTypeDefRegex = new Regex($"\n{_cre_space}typedef{_cre_space}{_cre_typedef};", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex ClassTypeDefRegex() => classTypeDefRegex;

        private static Regex classStaticFunctionRegex = new Regex($"\n{_cre_space}{_cre_attrib}\n{_cre_space}static{_cre_space}{_cre_func}\\((.*?)\\)", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex ClassStaticFunctionRegex() => classStaticFunctionRegex;

        private static Regex classVirtualFunctionRegex = new Regex($"\n{_cre_space}{_cre_attrib}\n{_cre_space}virtual{_cre_space}{_cre_func}\\((.*?)\\){_cre_vfmod}", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex ClassVirtualFunctionRegex() => classVirtualFunctionRegex;

        private static Regex enumRegex = new Regex($"\ntypedef{_cre_space}enum{_cre_space}{{(.*?)}}{_cre_space}{_cre_cfname};", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex EnumRegex() => enumRegex;

        private static Regex defineRegex = new Regex($"^#define{_cre_space}{_cre_cfname} (.+)$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static Regex DefineRegex() => defineRegex;

        private static Regex apiVersionLastRegex = new Regex($"^#define{_cre_space}CEF_API_VERSION_LAST{_cre_space}CEF_API_VERSION_(.+)$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static Regex ApiVersionLastRegex() => apiVersionLastRegex;

        private static Regex apiHashRegex = new Regex($"defined\\(OS_(WIN|MAC|LINUX)\\)\n#define{_cre_space}CEF_API_HASH_([0-9]+){_cre_space}\"([0-9a-f]*)\"", RegexOptions.Multiline | RegexOptions.Compiled);
        private static Regex ApiHashRegex() => apiHashRegex;

        private static Regex enumValueRegex = new Regex($"^\\s+([A-Z0-9a-z_]+)({_cre_space}={_cre_space}(.+?)[,\\n])?", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex EnumValueRegex() => enumValueRegex;
#endif
    }
}
