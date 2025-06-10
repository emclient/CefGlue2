namespace CefParser
{
    public partial class CefParser
    {
        public class Class
        {
            public string Attrib { get; }
            public string Name { get; }
            public string ParentName { get; }
            public string[] Comment { get; }
            public IReadOnlyList<StaticFunction> StaticFunctions { get; }
            public IReadOnlyList<VirtualFunction> VirtualFunctions { get; }

            public Class(
                string attrib,
                string name,
                string parentName,
                string[] comment,
                IReadOnlyList<StaticFunction>? staticFunctions = null,
                IReadOnlyList<VirtualFunction>? virtualFunctions = null)
            {
                this.Attrib = attrib;
                this.Name = name;
                this.ParentName = parentName;
                this.Comment = comment;
                this.StaticFunctions = staticFunctions ?? Array.Empty<StaticFunction>();
                this.VirtualFunctions = virtualFunctions ?? Array.Empty<VirtualFunction>();
            }
        }
    }
}
