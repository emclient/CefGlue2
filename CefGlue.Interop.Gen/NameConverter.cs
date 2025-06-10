using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string ToCSharpClassName(string name, CefParser.TypeClass typeClass)
        {
            if (typeClass == CefParser.TypeClass.Enum)
                return ToCInteropClassName(name, typeClass);
            if (name is "CefRect")
                return "CefRectangle";
            return name.Replace("DOM", "Dom").Replace("SSL", "Ssl").Replace("URL", "Url").Replace("PDF", "Pdf");
        }

        public static string ToCSharpMethodName(string name)
        {
            return name.Replace("DOM", "Dom").Replace("SSL", "Ssl").Replace("URL", "Url").Replace("PDF", "Pdf").Replace("PNG", "Png").Replace("JPEG", "Jpeg");
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
