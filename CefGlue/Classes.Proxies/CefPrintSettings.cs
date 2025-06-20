using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefPrintSettings
{
    /// <summary>
    /// Get the DPI (dots per inch).
    /// </summary>
    public int GetDpi() => Dpi;

    /// <summary>
    /// Returns the number of page ranges that currently exist.
    /// </summary>
    public int GetPageRangesCount() => (int)PageRangesCount;

    /// <summary>
    /// Retrieve the page ranges.
    /// </summary>
    public CefRange[] GetPageRanges()
    {
        var count = GetPageRangesCount();
        if (count == 0) return new CefRange[0];

        var n_ranges = new cef_range_t[count];
        UIntPtr n_count = (UIntPtr)count;
        fixed (cef_range_t* n_ranges_ptr = n_ranges)
        {
            cef_print_settings_t.get_page_ranges(_self, &n_count, n_ranges_ptr);
        }

        count = (int)n_count;
        if (count == 0) return new CefRange[0];

        var ranges = new CefRange[count];

        for (var i = 0; i < count; i++)
        {
            ranges[i].From = n_ranges[i].from;
            ranges[i].To = n_ranges[i].to;
        }

        return ranges;
    }

    /// <summary>
    /// Get the color model.
    /// </summary>
    public CefColorModel GetColorModel() => ColorModel;

    /// <summary>
    /// Get the number of copies.
    /// </summary>
    public int GetCopies() => Copies;

    /// <summary>
    /// Get the duplex mode.
    /// </summary>
    public CefDuplexMode GetDuplexMode() => DuplexMode;
}
