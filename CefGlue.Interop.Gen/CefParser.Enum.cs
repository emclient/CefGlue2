namespace CefParser
{
    public partial class CefParser
    {
        public record EnumValue(string Name, string CSharpName, string Value, string CSharpValue);

        public class EnumDef
        {
            string name;
            IReadOnlyList<EnumValue> values;
            bool isFlags;
            bool isUint;

            public EnumDef(string name, IReadOnlyList<EnumValue> values, bool isFlags, bool isUint)
            {
                this.name = name;
                this.values = values;
                this.isFlags = isFlags;
                this.isUint = isUint;
            }

            public string Name => name;
            public IReadOnlyList<EnumValue> Values => values;
            public bool IsFlags => isFlags;
            public bool IsUint => isUint;
        }
    }
}
