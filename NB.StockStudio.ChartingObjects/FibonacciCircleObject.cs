namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class FibonacciCircleObject : BaseObject
    {
        private float[] split = new float[] { ((float) (1.5 - (Math.Sqrt(5.0) / 2.0))), ((float) ((Math.Sqrt(5.0) / 2.0) - 0.5)), 1f, ((float) (2.5 - (Math.Sqrt(5.0) / 2.0))), ((float) ((Math.Sqrt(5.0) / 2.0) + 0.5)), 2f, ((float) (3.5 - (Math.Sqrt(5.0) / 2.0))), ((float) ((Math.Sqrt(5.0) / 2.0) + 1.5)), ((float) (Math.Sqrt(5.0) + 2.0)), ((float) (5.5 + (Math.Sqrt(5.0) / 2.0))), ((float) (7.5 + ((Math.Sqrt(5.0) * 3.0) / 2.0))) };

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = tf2.X - tf.X;
            float num2 = tf.Y - tf2.Y;
            Pen pen = base.LinePen.GetPen();
            foreach (float num3 in this.split)
            {
                g.DrawEllipse(pen, (float) (tf.X - (num * num3)), (float) (tf2.Y - (num2 * num3)), (float) ((num * 2f) * num3), (float) ((num2 * 2f) * num3));
            }
            g.DrawLine(pen, tf.X, tf.Y, tf.X, tf2.Y);
            g.DrawLine(pen, tf.X, tf2.Y, tf2.X, tf2.Y);
        }

        public override RectangleF GetMaxRect()
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = this.split[this.split.Length - 1];
            float num2 = Math.Abs((float) ((tf2.X - tf.X) * num));
            float num3 = Math.Abs((float) ((tf.Y - tf2.Y) * num));
            float x = tf.X;
            float y = tf2.Y;
            int num6 = base.LinePen.Width + 6;
            return new RectangleF((x - num2) - num6, (y - num3) - num6, (num2 * 2f) + (num6 * 2), (num3 * 2f) + (num6 * 2));
        }

        public override bool InObject(int X, int Y)
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = tf2.X - tf.X;
            float num2 = tf.Y - tf2.Y;
            foreach (float num3 in this.split)
            {
                bool flag = base.PointInEllipse(X, Y, tf.X, tf2.Y, num * num3, num2 * num3, base.LinePen.Width + 1);
                if (flag)
                {
                    return flag;
                }
            }
            return false;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Fibonacci Circle", typeof(FibonacciCircleObject), null, "Circle", "CircleFib") };
        }

        public float[] Split
        {
            get
            {
                return this.split;
            }
            set
            {
                this.split = value;
            }
        }
    }
}

