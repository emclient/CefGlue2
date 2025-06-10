using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CefParser
{
    public partial class CefParser
    {
        public struct ObjAnalysis
        {
            static Dictionary<string, string> basicTypeToCsType = new()
            {
                { "bool", "int" },
                { "void", "void" },
                { "char", "byte" },
                { "int", "int" },
                { "int16", "short" },
                { "int16_t", "short" },
                { "uint16", "ushort" },
                { "uint16_t", "ushort" },
                { "int32", "int" },
                { "int32_t", "int" },
                { "uint32", "uint" },
                { "uint32_t", "uint" },
                { "int64", "long" },
                { "int64_t", "long" },
                { "uint64", "ulong" },
                { "uint64_t", "ulong" },
                { "float", "float" },
                { "double", "double" },
                { "size_t", "nuint" },

                { "void*", "void*"},
                { "float*", "float*"},
                { "char* const", "char*"},

                { "CefWindowHandle", "IntPtr" },
                { "CefEventHandle", "IntPtr" },
                { "CefCursorHandle", "IntPtr" },

                { "cef_color_t", "uint" },

                { "cef_platform_thread_id_t", "nint" },
            };

            private ArgClass argumentClass;
            private string baseType;

            public ObjAnalysis(string value, Func<string, (TypeClass, string)> resolveType)
            {
                bool isConst = false;

                if (value.StartsWith("const ") || value.StartsWith("const\n"))
                {
                    isConst = true;
                    value = value[6..].Trim();
                }

                if (value.Length == 0)
                    throw new FormatException($"Invalid argument value: {value}");

                string type = value;
                bool isByRef = false, isByAddr = false;

                // Extract the last character of the data type
                var endChar = type[^1];

                // Check if the value is passed by reference
                if (endChar == '&')
                {
                    isByRef = true;
                    type = type[..^1];
                }
                // Check if the value is passed by address
                else if (endChar == '*')
                {
                    isByAddr = true;
                    type = type[..^1];
                }

                // Check for basic types (before typedef resolution)
                if (InitSimple(type, isByRef, isByAddr))
                    return;

                // Try to resolve user types
                var (typeClass, resolvedName) = resolveType(type);
                type = resolvedName;

                // Check for basic types
                if (InitSimple(type, isByRef, isByAddr))
                    return;

                // Check for smart pointers
                if (type.EndsWith(">") && (type.StartsWith("CefRefPtr<") || type.StartsWith("CefOwnPtr<") || type.StartsWith("CefRawPtr<")))
                {
                    Debug.Assert(!isByAddr);
                    this.argumentClass = isByRef ? ArgClass.RefPtrByRef : ArgClass.RefPtr;
                    this.baseType = type[10..^1];
                    return;
                }

                // Check for collection types
                if (InitCollection(type, resolveType, isByRef, isConst))
                    return;

                if (type is "CefString" or "CefStringUTF16")
                {
                    Debug.Assert(!isByAddr);
                    this.baseType = "CefString";
                    if (isByRef)
                    {
                        this.argumentClass = isConst ? ArgClass.StringByRefConst : ArgClass.StringByRef;
                    }
                    else
                    {
                        // Return type
                        this.argumentClass = ArgClass.StringUserFree;
                    }
                }
                else if (!isByRef)
                {
                    Debug.Assert(!isByRef && !isByAddr);
                    if (typeClass == TypeClass.Enum)
                    {
                        this.argumentClass = ArgClass.SimpleByVal;
                        this.baseType = NameConverter.ToCInteropClassName(type, TypeClass.Enum);
                    }
                    else
                    {
                        this.argumentClass = ArgClass.StructByVal;
                        this.baseType = type;
                    }
                }
                else
                {
                    Debug.Assert(isByRef);
                    Debug.Assert(!isByAddr);
                    this.argumentClass = isConst ? ArgClass.StructByRefConst : ArgClass.StructByRef;
                    this.baseType = type;
                }
            }

            [MemberNotNullWhen(true, nameof(argumentClass), nameof(baseType))]
            bool InitCollection(string value, Func<string, (TypeClass, string)> resolveType, bool isByRef, bool isConst)
            {
                if (value.StartsWith("std::vector"))
                {
                    if (!isByRef)
                        throw new FormatException("Invalid (vector not byref) type");

                    var val = value[12..^1];
                    if (val is "CefString")
                    {
                        this.argumentClass = isConst ? ArgClass.StringVecByRefConst : ArgClass.StringVecByRef;
                        this.baseType = "CefString";
                    }
                    else if (val.StartsWith("CefRefPtr<") && val.EndsWith(">"))
                    {
                        this.argumentClass = isConst ? ArgClass.RefPtrVecByRefConst : ArgClass.RefPtrVecByRef;
                        this.baseType = val[10..^1];
                    }
                    else if (val is "bool")
                    {
                        this.argumentClass = isConst ? ArgClass.BoolVecByRefConst : ArgClass.BoolVecByRef;
                        this.baseType = "bool";
                    }
                    else if (basicTypeToCsType.TryGetValue(val, out var csType))
                    {
                        this.argumentClass = isConst ? ArgClass.SimpleVecByRefConst : ArgClass.SimpleVecByRef;
                        this.baseType = csType;
                    }
                    else
                    {
                        this.argumentClass = isConst ? ArgClass.StructVecByRefConst : ArgClass.StructVecByRef;
                        this.baseType = val;
                    }

                    return true;
                }

                if (value.StartsWith("std::map"))
                {
                    if (!isByRef)
                        throw new FormatException("Invalid (vector not byref) type");

                    var val = value[9..^1].Split(',');
                    if (val.Length != 2 || val[0].Trim() != "CefString" || val[1].Trim() != "CefString")
                        throw new FormatException($"Unsupported type {value}");

                    this.argumentClass = isConst ? ArgClass.StringMapSingleByRefConst : ArgClass.StringMapSingleByRef;
                    this.baseType = "CefString";

                    return true;
                }

                if (value.StartsWith("std::multimap"))
                {
                    if (!isByRef)
                        throw new FormatException("Invalid (vector not byref) type");

                    var val = value[14..^1].Split(',');
                    if (val.Length != 2 || val[0].Trim() != "CefString" || val[1].Trim() != "CefString")
                        throw new FormatException($"Unsupported type {value}");

                    this.argumentClass = isConst ? ArgClass.StringMapMultiByRefConst : ArgClass.StringMapMultiByRef;
                    this.baseType = "CefString";

                    return true;
                }

                return false;
            }

            [MemberNotNullWhen(true, nameof(argumentClass), nameof(baseType))]
            bool InitSimple(string type, bool isByRef, bool isByAddr)
            {
                if (type is "bool")
                {
                    Debug.Assert(!isByRef || !isByAddr);
                    this.argumentClass =
                        isByRef ? ArgClass.BoolByRef :
                        isByAddr ? ArgClass.BoolByAddr :
                        ArgClass.BoolByVal;
                    this.baseType = "bool";

                    return true;
                }
                else if (basicTypeToCsType.TryGetValue(type, out var csType))
                {
                    Debug.Assert(!isByRef || !isByAddr);
                    this.argumentClass =
                        isByRef ? ArgClass.SimpleByRef :
                        isByAddr ? ArgClass.SimpleByAddr :
                        ArgClass.SimpleByVal;
                    this.baseType = csType;

                    return true;
                }

                return false;
            }

            public ArgClass ArgumentClass => this.argumentClass;

            public string CInteropType
            {
                get
                {
                    return this.argumentClass switch
                    {
                        ArgClass.SimpleByVal => this.baseType,
                        ArgClass.SimpleByRef => $"{this.baseType}*",
                        ArgClass.SimpleByRefConst => $"{this.baseType}*",
                        ArgClass.SimpleByAddr => $"{this.baseType}*",
                        ArgClass.BoolByVal => "int",
                        ArgClass.BoolByRef => "int*",
                        ArgClass.BoolByAddr => "int*",
                        ArgClass.StructByRefConst => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Struct)}*",
                        ArgClass.StructByRef => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Struct)}*",
                        ArgClass.StructByVal => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Struct)}",
                        ArgClass.StringByRefConst => "cef_string_t*",
                        ArgClass.StringByRef => "cef_string_t*",
                        ArgClass.StringUserFree => "cef_string_userfree*",
                        ArgClass.RefPtr => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Class)}*",
                        ArgClass.RefPtrByRef => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Class)}**",
                        ArgClass.StringVecByRef => "cef_string_list*",
                        ArgClass.StringVecByRefConst => "cef_string_list*",
                        ArgClass.StringMapSingleByRef => "cef_string_map*",
                        ArgClass.StringMapSingleByRefConst => "cef_string_map*",
                        ArgClass.StringMapMultiByRef => "cef_string_multimap*",
                        ArgClass.StringMapMultiByRefConst => "cef_string_multimap*",
                        ArgClass.SimpleVecByRef => $"{this.baseType}*", // Multiple args
                        ArgClass.SimpleVecByRefConst => $"{this.baseType}*", // Multiple args
                        ArgClass.BoolVecByRef => $"int*", // Multiple args
                        ArgClass.BoolVecByRefConst => $"int*", // Multiple args
                        ArgClass.RefPtrVecByRef => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Class)}**", // Multiple args
                        ArgClass.RefPtrVecByRefConst => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Class)}**", // Multiple args
                        ArgClass.StructVecByRef => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Struct)}*", // Multiple args
                        ArgClass.StructVecByRefConst => $"{NameConverter.ToCInteropClassName(this.baseType, TypeClass.Struct)}*", // Multiple args
                        _ => throw new NotImplementedException()
                    };
                }
            }

            public string CSharpType
            {
                get
                {
                    return this.argumentClass switch
                    {
                        ArgClass.SimpleByVal => this.baseType,
                        ArgClass.SimpleByRef => $"ref {this.baseType}",
                        ArgClass.SimpleByRefConst => $"{this.baseType}",
                        ArgClass.SimpleByAddr => $"IntPtr",
                        ArgClass.BoolByVal => "bool",
                        ArgClass.BoolByRef => "ref bool",
                        ArgClass.BoolByAddr => "ref bool",
                        ArgClass.StructByRefConst => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Struct)}",
                        ArgClass.StructByRef => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Struct)}",
                        ArgClass.StructByVal => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Struct)}",
                        ArgClass.StringByRefConst => "string",
                        ArgClass.StringByRef => "ref string",
                        ArgClass.StringUserFree => "string",
                        ArgClass.RefPtr => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Class)}",
                        ArgClass.RefPtrByRef => $"ref {NameConverter.ToCSharpClassName(this.baseType, TypeClass.Class)}",
                        ArgClass.StringVecByRef => "ref string[]",
                        ArgClass.StringVecByRefConst => "string[]",
                        ArgClass.StringMapSingleByRef => "IDictionary<string, string>",
                        ArgClass.StringMapSingleByRefConst => "IDictionary<string, string>", // TODO?
                        ArgClass.StringMapMultiByRef => "NameValueCollection",
                        ArgClass.StringMapMultiByRefConst => "NameValueCollection",
                        ArgClass.SimpleVecByRef => $"ref {this.baseType}[]",
                        ArgClass.SimpleVecByRefConst => $"{this.baseType}[]",
                        ArgClass.BoolVecByRef => $"bool[]", // TODO?
                        ArgClass.BoolVecByRefConst => $"bool[]",
                        ArgClass.RefPtrVecByRef => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Class)}[]", // TODO
                        ArgClass.RefPtrVecByRefConst => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Class)}[]",
                        ArgClass.StructVecByRef => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Struct)}[]", // TODO
                        ArgClass.StructVecByRefConst => $"{NameConverter.ToCSharpClassName(this.baseType, TypeClass.Struct)}[]",
                        _ => throw new NotImplementedException()
                    };
                }
            }

            public bool IsMultiArg =>
                this.argumentClass
                is ArgClass.SimpleVecByRef
                or ArgClass.SimpleByRefConst
                or ArgClass.BoolVecByRef
                or ArgClass.BoolVecByRefConst
                or ArgClass.RefPtrVecByRef
                or ArgClass.RefPtrVecByRefConst
                or ArgClass.StructVecByRef
                or ArgClass.StructVecByRefConst;

            public bool IsConst =>
                this.argumentClass
                is ArgClass.SimpleByRefConst
                or ArgClass.StructByRefConst
                or ArgClass.StringByRefConst
                or ArgClass.StringVecByRefConst
                or ArgClass.BoolVecByRefConst
                or ArgClass.RefPtrVecByRefConst
                or ArgClass.StructVecByRefConst;
        }
    }
}
