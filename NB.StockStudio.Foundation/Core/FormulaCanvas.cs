namespace NB.StockStudio.Foundation
{
    using System;
    using System.Drawing;

    public class FormulaCanvas
    {
        public FormulaAxisX AxisX;
        public double ColumnPercent;
        public double ColumnWidth;
        public int Count;
        public Graphics CurrentGraph;
        public double[] DATE;
        public Rectangle FrameRect;
        public float LabelHeight;
        public Rectangle Rect;
        public int Start;
        public int Stop;

        public float GetX(int i)
        {
            return (this.Rect.X + ((float) (((i - this.Stop) * this.ColumnWidth) + (this.ColumnWidth / 2.0))));
        }
    }
}

