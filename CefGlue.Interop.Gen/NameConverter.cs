using System.Text;

namespace CefParser
{
    public static class NameConverter
    {
        public static string CInteropToCSharp(string name, bool startSmall = false)
        {
            StringBuilder result = new();
            char lastChar = startSmall ? '\0' : '_';

            foreach (var c in name)
            {
                if (c != '_')
                {
                    if (lastChar == '_')
                        result.Append(char.ToUpper(c));
                    else
                        result.Append(c);
                }
                lastChar = c;
            }

            return result.ToString()
                .Replace("Jsdialog", "JSDialog")
                .Replace("OnFaviconUrlchange", "OnFaviconUrlChange")
                .Replace("GetByname", "GetByName")
                .Replace("GetByindex", "GetByIndex");
        }


        public static string ToCInteropClassName(string name, CefParser.TypeClass typeClass)
        {
            if (typeClass == CefParser.TypeClass.Enum)
            {
                if (name is "cef_uri_unescape_rule_t")
                    return "CefUriUnescapeRules";
                else if (name is "cef_postdataelement_type_t")
                    return "CefPostDataElementType";
                else if (name is "cef_content_setting_types_t")
                    return "CefContentSettingType";
                else if (name is "cef_content_setting_values_t")
                    return "CefContentSettingValue";
                else if (name is "cef_urlrequest_flags_t")
                    return "CefUrlRequestOptions";
                else if (name is "cef_urlrequest_status_t")
                    return "CefUrlRequestStatus";
                else if (name is "cef_errorcode_t")
                    return "CefErrorCode";
                else if (name is "cef_v8_propertyattribute_t")
                    return "CefV8PropertyAttribute";
                else if (name is "cef_xml_encoding_type_t")
                    return "CefXmlEncoding";
                else if (name is "cef_jsdialog_type_t")
                    return "CefJSDialogType";
                else if (name is "cef_resultcode_t")
                    return "CefResultCode";

                if (name.EndsWith("_t"))
                    name = name[..^2];
                StringBuilder result = new();
                char lastChar = '_';

                foreach (var c in name)
                {
                    if (c != '_')
                    {
                        if (lastChar == '_')
                            result.Append(char.ToUpper(c));
                        else
                            result.Append(c);
                    }
                    lastChar = c;
                }

                return result.ToString();
            }
            if (name.EndsWith("_t"))
                return name;
            return CefParser.GetCApiName(name, true);
        }

        /// <summary>
        /// Convert the name by changing the sequence of 3+ letter acronyms to only
        /// capatalize the first letter.
        /// </summary>
        private static string CSharpifyAcronyms(string name)
        {
            StringBuilder result = new();
            StringBuilder upperCase = new();

            foreach (var c in name)
            {
                if (char.IsUpper(c))
                {
                    upperCase.Append(c);
                    continue;
                }

                if (upperCase.Length > 0)
                {
                    if (upperCase.Length > 3)
                        result.Append($"{char.ToUpper(upperCase[0])}{upperCase.ToString(1, upperCase.Length - 2).ToLowerInvariant()}{char.ToUpper(upperCase[^1])}");
                    else
                        result.Append(upperCase.ToString());
                    upperCase.Clear();
                }

                result.Append(c);
            }

            if (upperCase.Length > 0)
            {
                if (upperCase.Length >= 3)
                    result.Append($"{char.ToUpper(upperCase[0])}{upperCase.ToString(1, upperCase.Length - 1).ToLowerInvariant()}");
                else
                    result.Append(upperCase.ToString());
            }

            return result.ToString();
        }

        public static string ToCSharpClassName(string name, CefParser.TypeClass typeClass)
        {
            if (typeClass == CefParser.TypeClass.Enum)
                return ToCInteropClassName(name, typeClass);
            if (name is "CefRect")
                return "CefRectangle";
            return CSharpifyAcronyms(name);
        }

        public static string ToCSharpMethodName(string name)
        {
            return CSharpifyAcronyms(name);
        }

        private static Dictionary<string, string> enumValuesSpecialCases = new()
        {
            { "TOPLEFT", "TopLeft" },
            { "TOPRIGHT", "TopRight" },
            { "BOTTOMCENTER", "BottomCenter" },
            { "XMLHTTPREQUEST_PROGRESS", "XmlHttpRequestProgress" },
            { "RELOAD_NOCACHE", "ReloadNoCache" },
            { "STOPLOAD", "StopLoad" },
            { "LOCALSTORAGE", "LocalStorage" },
            { "SESSIONSTORAGE", "SessionStorage" },
            { "DONTENUM", "DontEnum" },
            { "DONTDELETE", "DontDelete" },
            { "UTF16LE", "Utf16LE" },
            { "UTF16BE", "Utf16BE" },
        };

        public static string ToCSharpEnumValueName(string name)
        {
            if (enumValuesSpecialCases.TryGetValue(name, out var specialCase))
                return specialCase;

            var parts = name.Split('_');
            StringBuilder result = new();
            bool lastWasNumber = false;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (i == 0 && part is "FLAG")
                    continue;
                if (lastWasNumber)
                    result.Append('_');
                if (part.Length > 2 ||
                    part is "TO" or "NO" or "IS" or "IN" or "AT" or "ON" or "OK")
                {
                    result.Append(char.ToUpperInvariant(part[0]));
                    result.Append(part[1..].ToLowerInvariant());
                    lastWasNumber = false;
                }
                else if (char.IsDigit(part[0]) && int.TryParse(part, out _))
                {
                    result.Append(part);
                    lastWasNumber = true;
                }
                else
                {
                    result.Append(part);
                    lastWasNumber = false;
                }
            }

            return result.ToString();
        }

        static HashSet<string> csharpKeywords = ["object", "string", "checked", "event", "params", "delegate"];

        public static string QuoteName(string name)
        {
            if (csharpKeywords.Contains(name))
                return $"@{name}";
            return name;
        }
    }
}
