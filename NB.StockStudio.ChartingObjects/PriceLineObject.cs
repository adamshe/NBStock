namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class PriceLineObject : LineGroupTextObject
    {
        public PriceLineObject()
        {
            base.SmoothingMode = ObjectSmoothingMode.Default;
            base.ObjectFont.TextFont = new Font(base.ObjectFont.TextFont, FontStyle.Italic);
        }

        public override void CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            base.pfStart = new PointF[3];
            base.pfEnd = new PointF[3];
            Rectangle rect = base.Area.Canvas.Rect;
            base.pfStart[0] = tfArray[0];
            base.pfEnd[0] = new PointF(tfArray[1].X, tfArray[0].Y);
            base.pfStart[1] = new PointF(tfArray[0].X, tfArray[1].Y);
            base.pfEnd[1] = tfArray[1];
            base.pfStart[2] = new PointF((tfArray[0].X + tfArray[1].X) / 2f, tfArray[0].Y);
            base.pfEnd[2] = new PointF((tfArray[0].X + tfArray[1].X) / 2f, tfArray[1].Y);
        }

        public override string GetStr()
        {
            double num = base.ControlPoints[1].Y - base.ControlPoints[0].Y;
            double num2 = num / base.ControlPoints[0].Y;
            return (num.ToString("f2") + "\r\n" + num2.ToString("p2"));
        }

        public override RectangleF GetTextRect()
        {
            return base.GetTextRect(2, false);
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Price Line", typeof(PriceLineObject), null, "LineGroup", "PriceLine") };
        }
    }
}

