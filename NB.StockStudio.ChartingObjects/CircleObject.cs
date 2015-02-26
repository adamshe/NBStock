namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class CircleObject : FillPolygonObject
    {
        public override PointF[] CalcPoint()
        {
            PointF[] pathPoints = null;
            GraphicsPath path = new GraphicsPath();
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            float num = (float) base.Dist(tfArray[0], tfArray[1]);
            try
            {
                path.AddArc((float) (tfArray[0].X - num), (float) (tfArray[0].Y - num), (float) (num * 2f), (float) (num * 2f), 0f, 360f);
                path.Flatten();
                pathPoints = path.PathPoints;
            }
            catch
            {
            }
            return pathPoints;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Circle", typeof(CircleObject), null, "Circle", "Circle"), new ObjectInit("Fill Circle", typeof(CircleObject), "Fill", "Circle", "CircleF") };
        }
    }
}

