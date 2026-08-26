using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue.Platform;

internal sealed unsafe class CefAcceleratedPaintInfoLinuxImpl : CefAcceleratedPaintInfo
{
    private cef_accelerated_paint_info_t_linux* _self;

    internal CefAcceleratedPaintInfoLinuxImpl(cef_accelerated_paint_info_t* ptr)
    {
        _self = (cef_accelerated_paint_info_t_linux*)ptr;

        Modifier = _self->modifier;
        PlaneCount = Math.Clamp(_self->plane_count, 0, 4);
        Planes = new CefAcceleratedPaintNativePixmapPlane[PlaneCount];
        for (var i = 0; i < PlaneCount; i++)
            Planes[i] = CefAcceleratedPaintNativePixmapPlane.FromNative(_self->planes[i]);
        Format = _self->format;
        Extra = CefAcceleratedPaintInfoCommon.FromNative(_self->extra);
    }

    public override CefColorType Format { get; }
    public override CefAcceleratedPaintInfoCommon Extra { get; }

    public override ulong Modifier { get; }
    public override int PlaneCount { get; }
    public override CefAcceleratedPaintNativePixmapPlane[] Planes { get; }

    public override IntPtr SharedTexture => throw new PlatformNotSupportedException();
}
