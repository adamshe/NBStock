namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class Circle2Object : BaseObject
    {
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            double num = base.Dist(tf, tf2);
            g.DrawEllipse(base.LinePen.GetPen(), (int) (tf.X - num), (int) (tf.Y - num), (int) (num * 2.0), (int) (num * 2.0));
        }

        public override Region GetRegion()
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = (float) base.Dist(tf, tf2);
            int num2 = base.LinePen.Width + 6;
            RectangleF rect = new RectangleF((tf.X - num) - num2, (tf.Y - num) - num2, (tf.X + num) + (2 * num2), (tf.Y + num) + (2 * num2));
            if (rect.X < 0f)
            {
                rect.X = 0f;
            }
            if (rect.Y < 0f)
            {
                rect.Y = 0f;
            }
            return new Region(rect);
        }

        public override bool InObject(int X, int Y)
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            double num = base.Dist(tf, tf2);
            return (Math.Abs((double) (base.Dist(new PointF((float) X, (float) Y), tf) - num)) <= (base.LinePen.Width + 1));
        }
    }
}

