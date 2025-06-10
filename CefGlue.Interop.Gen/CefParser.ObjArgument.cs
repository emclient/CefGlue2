namespace CefParser
{
    public partial class CefParser
    {
        public class ObjArgument
        {
            public ObjAnalysis Type { get; }
            public string Name { get; }
            public bool IsOptional { get; }

            public ObjArgument(string argType, string argName, Func<string, (TypeClass, string)> resolveType, bool isOptional = false)
            {
                Type = new ObjAnalysis(argType, resolveType);
                Name = argName;
                IsOptional = isOptional;
            }

            /// <summary>
            /// Returns `?` for reference types if IsOptional is true.
            /// </summary>
            public string CSharpOptionalSuffix
            {
                get
                {
                    if (Type.ArgumentClass
                        is ArgClass.RefPtr
                        or ArgClass.StringByRefConst
                        or ArgClass.StringVecByRefConst
                        or ArgClass.StructVecByRefConst &&
                        IsOptional)
                        return "?";
                    if (Type.ArgumentClass
                        is ArgClass.RefPtrByRef
                        or ArgClass.StringByRef)
                        return "?";
                    return "";
                }
            }
        }
    }
}
