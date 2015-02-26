namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    public class ObjectBrushEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            ObjectBrush brush = (ObjectBrush) e.Value;
            e.Graphics.FillRectangle(brush.GetBrush(e.Bounds), e.Bounds);
            base.PaintValue(e);
        }
    }
}

