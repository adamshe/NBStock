namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Drawing;

    public class CycleCounterObject : LineGroupTextObject
    {
        public CycleCounterObject()
        {
            base.ObjectFont.TextFont = new Font(base.ObjectFont.TextFont, FontStyle.Italic | FontStyle.Bold);
            base.ObjectFont.Alignment = StringAlignment.Center;
            base.SmoothingMode = ObjectSmoothingMode.Default;
        }

        public override void CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            base.pfStart = new PointF[3];
            base.pfEnd = new PointF[3];
            Rectangle rect = base.Area.Canvas.Rect;
            base.pfStart[0] = new PointF(tfArray[0].X, (float) rect.Top);
            base.pfEnd[0] = new PointF(tfArray[0].X, (float) rect.Bottom);
            base.pfStart[1] = new PointF(tfArray[1].X, (float) rect.Top);
            base.pfEnd[1] = new PointF(tfArray[1].X, (float) rect.Bottom);
            base.pfStart[2] = new PointF(tfArray[0].X, tfArray[1].Y);
            base.pfEnd[2] = new PointF(tfArray[1].X, tfArray[1].Y);
        }

        public override string GetStr()
        {
            double[] dd = base.Manager.Canvas.BackChart.DataProvider["DATE"];
            int num = FormulaChart.FindIndex(dd, base.ControlPoints[0].X);
            return ((FormulaChart.FindIndex(dd, base.ControlPoints[1].X) - num) + "(T)");
        }

        public override RectangleF GetTextRect()
        {
            return base.GetTextRect(2, true);
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("CycleCounterObject", typeof(CycleCounterObject), null, "Cycle", "CycleC") };
        }
    }
}

