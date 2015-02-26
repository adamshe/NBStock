namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class ChannelObject : LineGroupObject
    {
        private float[] split;

        public override void CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            base.pfStart = new PointF[this.split.Length];
            base.pfEnd = new PointF[this.split.Length];
            if (tfArray.Length == 3)
            {
                float num = tfArray[2].X - tfArray[0].X;
                float num2 = tfArray[2].Y - tfArray[0].Y;
                float num3 = tfArray[2].X - tfArray[1].X;
                float num4 = tfArray[2].Y - tfArray[1].Y;
                for (int i = 0; i < this.split.Length; i++)
                {
                    base.pfStart[i] = new PointF(tfArray[0].X + (num * this.split[i]), tfArray[0].Y + (num2 * this.split[i]));
                    if (this.split[i] == 1f)
                    {
                        base.pfEnd[i] = new PointF((tfArray[0].X - tfArray[1].X) + tfArray[2].X, (tfArray[0].Y - tfArray[1].Y) + tfArray[2].Y);
                    }
                    else
                    {
                        base.pfEnd[i] = new PointF(tfArray[1].X + (num3 * this.split[i]), tfArray[1].Y + (num4 * this.split[i]));
                    }
                    base.ExpandLine(ref base.pfStart[i], ref base.pfEnd[i]);
                    base.ExpandLine(ref base.pfEnd[i], ref base.pfStart[i]);
                }
            }
        }

        public void Equal()
        {
            this.split = new float[] { 0f, 0.25f, 0.5f, 0.75f, 1f };
        }

        public void Fibonacci()
        {
            this.split = new float[] { 0f, 0.3333333f, 0.375f, 0.5f, 0.625f, 0.6666667f, 1f };
        }

        public void Multi()
        {
            int num = 10;
            this.split = new float[(num * 2) + 1];
            for (int i = -num; i <= num; i++)
            {
                this.split[i + num] = i;
            }
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Equal Channel", typeof(ChannelObject), "Equal", "Channel", "ChannelE", 600), new ObjectInit("Fibonacci Channel", typeof(ChannelObject), "Fibonacci", "Channel", "ChannelF"), new ObjectInit("Channels", typeof(ChannelObject), "Multi", "Channel", "ChannelM") };
        }

        public override int ControlPointNum
        {
            get
            {
                return 3;
            }
        }

        public override int InitNum
        {
            get
            {
                return 3;
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

