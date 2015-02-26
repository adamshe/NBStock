namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;

    public class ObjectPenEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            ObjectPen pen = (ObjectPen) e.Value;
            Rectangle bounds = e.Bounds;
            Region clip = e.Graphics.Clip;
            e.Graphics.SetClip(bounds);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawLine(pen.GetPen(), e.Bounds.X, e.Bounds.Y, e.Bounds.Right - 1, e.Bounds.Bottom - 1);
            e.Graphics.SmoothingMode = SmoothingMode.Default;
            e.Graphics.Clip = clip;
            base.PaintValue(e);
        }
    }
}

