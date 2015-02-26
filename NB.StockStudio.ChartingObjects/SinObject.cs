namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class SinObject : PolygonObject
    {
        private ArrayList alPoint = new ArrayList();

        public override PointF[] CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            this.alPoint.Clear();
            float num = tfArray[1].X - tfArray[0].X;
            float num2 = (tfArray[1].Y - tfArray[0].Y) / 2f;
            Rectangle rect = base.Area.Canvas.Rect;
            if (num != 0f)
            {
                for (int i = rect.Left; i < rect.Right; i++)
                {
                    this.alPoint.Add(new PointF((float) i, (tfArray[0].Y + num2) + ((float) (num2 * Math.Cos(((i - tfArray[1].X) / num) * 3.1415926535897931)))));
                }
            }
            return (PointF[]) this.alPoint.ToArray(typeof(PointF));
        }

        public override RectangleF GetMaxRect()
        {
            PointF[] pfs = base.ToPoints(base.ControlPoints);
            Rectangle rect = base.Area.Canvas.Rect;
            pfs[0].X = rect.Left;
            pfs[1].X = rect.Right;
            return base.GetMaxRect(pfs);
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Sin Object", typeof(SinObject), null, "Circle", "Sin") };
        }
    }
}

