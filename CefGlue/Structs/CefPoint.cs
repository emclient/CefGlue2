﻿namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue.Interop;

    public struct CefPoint
    {
        private int _x;
        private int _y;

        public CefPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #region Interop

        internal static unsafe CefPoint FromNative(cef_point_t* ptr)
        {
            if (ptr == null) throw new ArgumentNullException("ptr");

            return new CefPoint
            {
                X = ptr->x,
                Y = ptr->y,
            };
        }

        internal cef_point_t ToNative()
        {
            return new cef_point_t { x = X, y = Y };
        }

        #endregion
    }
}
