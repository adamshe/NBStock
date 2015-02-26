namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Drawing.Text;

    public class ObjectFontEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            ObjectFont font = (ObjectFont) e.Value;
            TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            font.DrawString("A", e.Graphics, e.Bounds);
            e.Graphics.TextRenderingHint = textRenderingHint;
            base.PaintValue(e);
        }
    }
}

