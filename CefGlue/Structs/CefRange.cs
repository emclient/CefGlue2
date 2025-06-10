namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue.Interop;

    public struct CefRange
    {
        private int _from;
        private int _to;

        public CefRange(int from, int to)
        {
            _from = from;
            _to = to;
        }

        internal unsafe static CefRange FromNative(cef_range_t* range)
            => new CefRange(range->from, range->to);

        internal cef_range_t ToNative()
            => new cef_range_t { from = _from, to = _to };

        public int From
        {
            get { return _from; }
            set { _from = value; }
        }

        public int To
        {
            get { return _to; }
            set { _to = value; }
        }
    }
}
