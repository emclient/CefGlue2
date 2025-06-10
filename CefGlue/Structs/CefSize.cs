namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using Xilium.CefGlue.Interop;

    public struct CefSize
    {
        private int _width;
        private int _height;

        public CefSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }


        #region Interop

        internal static unsafe CefSize FromNative(cef_size_t* ptr)
        {
            if (ptr == null) throw new ArgumentNullException("ptr");

            return new CefSize
            {
                Width = ptr->width,
                Height = ptr->height,
            };
        }

        internal unsafe cef_size_t ToNative()
        {
            return new cef_size_t
            {
                width = Width,
                height = Height
            };
        }

        #endregion
    }
}
