namespace Xilium.CefGlue;

public sealed unsafe partial class CefXmlReader
{
    /// <summary>
    /// Returns the node type.
    /// </summary>
    public CefXmlNodeType NodeType => Type;

    /// <summary>
    /// Returns the URI defining the namespace associated with the node. See
    /// http://www.w3.org/TR/REC-xml-names/ for additional details.
    /// </summary>
    public string NamespaceUri => NamespaceURI;

    /// <summary>
    /// Returns the base URI of the node. See http://www.w3.org/TR/xmlbase/ for
    /// additional details.
    /// </summary>
    public string BaseUri => BaseURI;

    /// <summary>
    /// Returns an XML representation of the current node's children.
    /// </summary>
    public string GetInnerXml() => InnerXml;

    /// <summary>
    /// Returns an XML representation of the current node including its children.
    /// </summary>
    public string GetOuterXml() => OuterXml;
}
