namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class FanObject : LineGroupObject
    {
        private float[] split;

        public override void CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            base.pfStart = new PointF[this.split.Length];
            base.pfEnd = new PointF[this.split.Length];
            for (int i = 0; i < this.split.Length; i++)
            {
                base.pfStart[i] = tfArray[0];
                base.pfEnd[i] = new PointF(tfArray[1].X, tfArray[0].Y + ((tfArray[1].Y - tfArray[0].Y) * this.split[i]));
                base.ExpandLine(ref base.pfStart[i], ref base.pfEnd[i]);
            }
        }

        public void Equal3()
        {
            this.split = new float[] { 0.3333333f, 0.6666667f, 1f };
        }

        public void Equal4()
        {
            this.split = new float[] { 0.25f, 0.5f, 1f };
        }

        public void Fibonacci()
        {
            this.split = new float[] { 0.3333333f, 0.375f, 0.5f, 0.625f, 0.6666667f, 1f };
        }

        public override Region GetRegion()
        {
            return new Region();
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Speed line", typeof(FanObject), "Equal3", "Fan", "Fan3", 400), new ObjectInit("Speed line 4", typeof(FanObject), "Equal4", "Fan", "Fan4"), new ObjectInit("Fibonacci fan", typeof(FanObject), "Fibonacci", "Fan", "FanFib") };
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

