namespace NB.StockStudio.Foundation
{
    using System;
    using System.Drawing;

    public class NativePaintArgs
    {
        public System.Drawing.Graphics Graphics;
        public Bitmap NativeBitmap;
        public Bitmap NewBitmap;
        public Rectangle Rect;

        public NativePaintArgs(System.Drawing.Graphics graphics, Rectangle Rect, Bitmap NativeBitmap)
        {
            this.Graphics = graphics;
            this.Rect = Rect;
            this.NativeBitmap = NativeBitmap;
        }
    }
}

