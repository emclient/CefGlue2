namespace CefParser
{
    public partial class CefParser
    {
        // Derived from https://github.com/chromiumembedded/cef/blob/master/tools/translator.README.txt
        // (RefPtr "same" and "diff" are joined into the same constants)
        public enum ArgClass
        {
            SimpleByVal, // int, uint
            SimpleByRef, // int&
            SimpleByRefConst, // const int &
            SimpleByAddr, // int*
            BoolByVal, // bool
            BoolByRef, // bool&
            BoolByAddr, // bool*
            StructByRefConst, // const CefPopupFeatures&
            StructByRef, // CefWindowInfo& value
            StructByVal, // return value
            StringByRefConst, // const CefString&
            StringByRef, // CefString& value
            StringUserFree, // return value
            RefPtr,
            RefPtrByRef,
            StringVecByRef,
            StringVecByRefConst,
            StringMapSingleByRef,
            StringMapSingleByRefConst,
            StringMapMultiByRef,
            StringMapMultiByRefConst,
            SimpleVecByRef,
            SimpleVecByRefConst,
            BoolVecByRef,
            BoolVecByRefConst,
            RefPtrVecByRef,
            RefPtrVecByRefConst,
            StructVecByRef,
            StructVecByRefConst,
        }
    }
}
