namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class MultiArcObject : BaseObject
    {
        private float[] split = new float[] { 0.3333333f, 0.375f, 0.5f, 0.625f, 0.6666667f };

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = (float) base.Dist(tf, tf2);
            Pen pen = base.LinePen.GetPen();
            float startAngle = 0f;
            if (tf2.Y < tf.Y)
            {
                startAngle = 180f;
            }
            if (num > 0f)
            {
                foreach (float num3 in this.split)
                {
                    g.DrawArc(pen, new RectangleF(tf.X - (num * num3), tf.Y - (num * num3), (num * 2f) * num3, (num * 2f) * num3), startAngle, 180f);
                }
            }
            g.DrawLine(pen, tf, tf2);
        }

        public override RectangleF GetMaxRect()
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            float num = (float) base.Dist(tf, tf2);
            int num2 = base.LinePen.Width + 6;
            RectangleF ef = new RectangleF((tf.X - num) - num2, (tf.Y - num) - num2, (tf.X + num) + (2 * num2), (tf.Y + num) + (2 * num2));
            if (ef.X < 0f)
            {
                ef.X = 0f;
            }
            if (ef.Y < 0f)
            {
                ef.Y = 0f;
            }
            return ef;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Multi Arc", typeof(MultiArcObject), null, "Circle", "CircleM") };
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

