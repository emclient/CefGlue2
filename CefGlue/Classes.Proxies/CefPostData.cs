using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefPostData
{
    /// <summary>
    /// Returns the number of existing post data elements.
    /// </summary>
    public int Count => (int)ElementCount;

    /// <summary>
    /// Retrieve the post data elements.
    /// </summary>
    public CefPostDataElement[] GetElements()
    {
        // FIXME: CefPostDataElement.GetElements(): check CEF C API impl
        var count = Count;
        if (count == 0) return new CefPostDataElement[0];

        UIntPtr n_elementsCount = (UIntPtr)count;
        var n_elements = new cef_post_data_element_t*[count];
        fixed (cef_post_data_element_t** n_elements_ptr = n_elements)
        {
            cef_post_data_t.get_elements(_self, &n_elementsCount, n_elements_ptr);
            if ((int)n_elementsCount > count) throw new InvalidOperationException();
        }

        count = (int)n_elementsCount;
        var elements = new CefPostDataElement[count];
        for (var i = 0; i < count; i++)
        {
            elements[i] = CefPostDataElement.FromNative(n_elements[i]);
        }

        return elements;
    }

    /// <summary>
    /// Remove the specified post data element.  Returns true if the removal
    /// succeeds.
    /// </summary>
    public bool Remove(CefPostDataElement element) => RemoveElement(element);

    /// <summary>
    /// Add the specified post data element.  Returns true if the add succeeds.
    /// </summary>
    public bool Add(CefPostDataElement element) => AddElement(element);

    /// <summary>
    /// Remove all existing post data elements.
    /// </summary>
    public void RemoveAll() => RemoveElements();
}
