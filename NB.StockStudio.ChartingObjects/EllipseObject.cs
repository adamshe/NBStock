namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class EllipseObject : BaseObject
    {
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = tf2.X - tf.X;
            float num2 = tf.Y - tf2.Y;
            g.DrawEllipse(base.LinePen.GetPen(), tf.X, tf2.Y, num * 2f, num2 * 2f);
        }

        public override RectangleF GetMaxRect()
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = tf2.X - tf.X;
            float num2 = tf.Y - tf2.Y;
            float x = tf.X;
            float y = tf2.Y;
            if (num < 0f)
            {
                x += num * 2f;
                num = -num;
            }
            if (num2 < 0f)
            {
                y += num2 * 2f;
                num2 = -num2;
            }
            int num5 = base.LinePen.Width + 6;
            return new RectangleF(x - num5, y - num5, (num * 2f) + (num5 * 2), (num2 * 2f) + (num5 * 2));
        }

        public override bool InObject(int X, int Y)
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = tf2.X - tf.X;
            float num2 = tf.Y - tf2.Y;
            return base.PointInEllipse(X, Y, tf2.X, tf.Y, num, num2, base.LinePen.Width + 1);
        }
    }
}

