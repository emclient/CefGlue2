namespace Xilium.CefGlue;

public sealed unsafe partial class CefXmlReader
{
    /// <summary>
    /// Returns the node type.
    /// </summary>
    public CefXmlNodeType NodeType => Type;

    /// <summary>
    /// Returns an XML representation of the current node's children.
    /// </summary>
    public string GetInnerXml() => InnerXml;

    /// <summary>
    /// Returns an XML representation of the current node including its children.
    /// </summary>
    public string GetOuterXml() => OuterXml;
}
