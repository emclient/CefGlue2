using System;
using System.Collections.Generic;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefDomNode
{
    /// <summary>
    /// Returns the type for this node.
    /// </summary>
    public CefDomNodeType NodeType => Type;

    /// <summary>
    /// Returns the contents of this node as markup.
    /// </summary>
    public string GetAsMarkup() => AsMarkup;

    /// <summary>
    /// Returns the document associated with this node.
    /// </summary>
    public CefDomDocument Document => GetDocument();

    /// <summary>
    /// Returns the parent node.
    /// </summary>
    public CefDomNode Parent => GetParent();

    /// <summary>
    /// Returns the previous sibling node.
    /// </summary>
    public CefDomNode PreviousSibling => GetPreviousSibling();

    /// <summary>
    /// Returns the next sibling node.
    /// </summary>
    public CefDomNode NextSibling => GetNextSibling();

    /// <summary>
    /// Return the first child node.
    /// </summary>
    public CefDomNode FirstChild => GetFirstChild();

    /// <summary>
    /// Returns the last child node.
    /// </summary>
    public CefDomNode LastChild => GetLastChild();

    /// <summary>
    /// Returns true if this element has attributes.
    /// </summary>
    [Obsolete]
    public bool HasAttributes => HasElementAttributes;

    /// <summary>
    /// Returns true if this element has an attribute named |attrName|.
    /// </summary>
    [Obsolete]
    public bool HasAttribute(string attrName) => HasElementAttribute(attrName);

    /// <summary>
    /// Returns the element attribute named |attrName|.
    /// </summary>
    [Obsolete]
    public string GetAttribute(string attrName) => GetElementAttribute(attrName);

    /// <summary>
    /// Returns a map of all element attributes.
    /// </summary>
    [Obsolete]
    public IReadOnlyDictionary<string, string> GetAttributes() => GetElementAttributes();

    /// <summary>
    /// Set the value for the element attribute named |attrName|. Returns true on
    /// success.
    /// </summary>
    [Obsolete]
    public bool SetAttribute(string attrName, string value) => SetElementAttribute(attrName, value);

    /// <summary>
    /// Returns the inner text of the element.
    /// </summary>
    [Obsolete]
    public string InnerText => ElementInnerText;
}
