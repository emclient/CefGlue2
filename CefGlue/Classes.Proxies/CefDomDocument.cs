using System;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefDomDocument
{
    /// <summary>
    /// Returns the document type.
    /// </summary>
    public CefDomDocumentType DocumentType => this.Type;

    /// <summary>
    /// Returns the root document node.
    /// </summary>
    public CefDomNode Root => GetDocument();

    /// <summary>
    /// Returns the BODY node of an HTML document.
    /// </summary>
    public CefDomNode Body => GetBody();

    /// <summary>
    /// Returns the HEAD node of an HTML document.
    /// </summary>
    public CefDomNode Head => GetHead();

    /// <summary>
    /// Returns the node that currently has keyboard focus.
    /// </summary>
    public CefDomNode FocusedNode => GetFocusedNode();

    /// <summary>
    /// Returns the selection offset within the end node.
    /// </summary>
    [Obsolete]
    public int GetSelectionEndOffset => SelectionEndOffset;

    /// <summary>
    /// Returns the contents of this selection as text.
    /// </summary>
    [Obsolete]
    public string GetSelectionAsText => SelectionAsText;
}
