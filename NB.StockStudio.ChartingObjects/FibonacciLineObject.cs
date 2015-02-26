namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Drawing;

    [Serializable]
    public class FibonacciLineObject : LineGroupTextObject
    {
        private SizeF sf;
        private SnapType snap;
        private float[] split;

        public FibonacciLineObject()
        {
            base.SmoothingMode = ObjectSmoothingMode.Default;
            base.ObjectFont.TextFont = new Font(base.ObjectFont.TextFont, FontStyle.Italic);
        }

        public override void CalcPoint()
        {
            base.SetSnapPrice(this.snap);
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            double y = base.ControlPoints[0].Y;
            double num2 = base.ControlPoints[1].Y;
            base.pfStart = new PointF[this.split.Length];
            base.pfEnd = new PointF[this.split.Length];
            ObjectPoint op = new ObjectPoint(base.ControlPoints[0].X, 0.0);
            for (int i = 0; i < this.split.Length; i++)
            {
                op.Y = ((num2 - y) * this.split[i]) + y;
                PointF tf3 = base.ToPointF(op);
                base.pfStart[i] = new PointF(tf.X, tf3.Y);
                base.pfEnd[i] = new PointF(tf2.X, tf3.Y);
            }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            this.sf = SizeF.Empty;
            for (int i = 0; i < this.split.Length; i++)
            {
                try
                {
                    base.ObjectFont.DrawString(this.GetStr(i), g, this.GetRect(i));
                }
                catch
                {
                }
            }
        }

        private RectangleF GetRect(int i)
        {
            SizeF ef = base.ObjectFont.Measure(base.CurrentGraphics, this.GetStr(i));
            float x = Math.Min(base.pfStart[i].X, base.pfEnd[i].X);
            float y = base.pfStart[i].Y;
            float width = Math.Max(Math.Abs((float) (base.pfStart[i].X - base.pfEnd[i].X)), ef.Width);
            return new RectangleF(x, y, width, ef.Height);
        }

        public override Region GetRegion()
        {
            Region region = base.GetRegion();
            if (base.CurrentGraphics != null)
            {
                for (int i = 0; i < this.split.Length; i++)
                {
                    region.Union(this.GetRect(i));
                }
            }
            return region;
        }

        private string GetStr(int i)
        {
            return this.split[i].ToString("p1");
        }

        public void InitFibonacci()
        {
            this.split = new float[] { 0f, 0.382f, 0.5f, 0.618f, 1f };
        }

        public void InitFibonacciA()
        {
            this.split = new float[] { 
                0f, 0.236f, 0.382f, 0.5f, 0.618f, 1f, 1.382f, 1.5f, 1.618f, 2f, 2.382f, 2.618f, 4.236f, 6.853f, 11.088f, 17.941f, 
                29.029f
             };
        }

        public void InitPercent()
        {
            this.split = new float[] { 0f, 0.125f, 0.25f, 0.3333333f, 0.375f, 0.5f, 0.625f, 0.6666667f, 0.75f, 0.875f, 1f };
        }

        public void InitPercentA()
        {
            this.split = new float[] { -3f, -2f, -1f, 0f, 1f, 2f, 3f };
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Fibonacci Line", typeof(FibonacciLineObject), "InitFibonacci", "LineGroup", "FibLine", 300), new ObjectInit("Percentage Line", typeof(FibonacciLineObject), "InitPercent", "LineGroup", "PercentLine"), new ObjectInit("Fibonacci Line A", typeof(FibonacciLineObject), "InitFibonacciA", "LineGroup", "FibLineA"), new ObjectInit("Percentage Line A", typeof(FibonacciLineObject), "InitPercentA", "LineGroup", "PercentLineA") };
        }

        public SnapType Snap
        {
            get
            {
                return this.snap;
            }
            set
            {
                this.snap = value;
            }
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

