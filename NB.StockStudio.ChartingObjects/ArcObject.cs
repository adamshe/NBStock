namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class ArcObject : FillPolygonObject
    {
        public override PointF[] CalcPoint()
        {
            PointF[] pathPoints = null;
            GraphicsPath path = new GraphicsPath();
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            try
            {
                float x;
                float y;
                float width = Math.Abs((float) (tfArray[1].X - tfArray[0].X)) * 2f;
                float height = Math.Abs((float) (tfArray[1].Y - tfArray[0].Y)) * 2f;
                if (tfArray[0].X < tfArray[1].X)
                {
                    x = tfArray[0].X;
                }
                else
                {
                    x = (tfArray[1].X * 2f) - tfArray[0].X;
                }
                if (tfArray[0].Y < tfArray[1].Y)
                {
                    y = (tfArray[0].Y * 2f) - tfArray[1].Y;
                }
                else
                {
                    y = tfArray[1].Y;
                }
                path.AddArc(x, y, width, height, 0f, 360f);
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
            return new ObjectInit[] { new ObjectInit("Ellpise", typeof(ArcObject), null, "Circle", "Ellipse"), new ObjectInit("Fill Ellpise", typeof(ArcObject), "Fill", "Circle", "EllipseF") };
        }
    }
}

