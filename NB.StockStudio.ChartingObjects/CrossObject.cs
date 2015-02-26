namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class CrossObject : LineGroupObject
    {
        private int lineCount = 2;

        public override void CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            base.pfStart = new PointF[3 + (this.lineCount * 2)];
            base.pfEnd = new PointF[3 + (this.lineCount * 2)];
            base.pfStart[0] = tfArray[0];
            base.pfEnd[0] = tfArray[1];
            base.pfStart[2] = tfArray[0];
            base.pfEnd[2] = new PointF((tfArray[1].X + tfArray[2].X) / 2f, (tfArray[1].Y + tfArray[2].Y) / 2f);
            float num = base.pfEnd[2].X - tfArray[0].X;
            float num2 = base.pfEnd[2].Y - tfArray[0].Y;
            float num3 = base.pfEnd[2].X - tfArray[1].X;
            float num4 = base.pfEnd[2].Y - tfArray[1].Y;
            for (int i = 0; i < (this.lineCount * 2); i++)
            {
                int num6 = i - this.lineCount;
                if (num6 >= 0)
                {
                    num6++;
                }
                base.pfStart[3 + i] = new PointF(base.pfEnd[2].X - (num3 * num6), base.pfEnd[2].Y - (num4 * num6));
                base.pfEnd[3 + i] = new PointF(base.pfStart[3 + i].X + (num * Math.Abs(num6)), base.pfStart[3 + i].Y + (num2 * Math.Abs(num6)));
                base.ExpandLine(ref base.pfStart[3 + i], ref base.pfEnd[3 + i]);
            }
            base.pfStart[1] = base.pfStart[3];
            base.pfEnd[1] = base.pfStart[base.pfStart.Length - 1];
            base.ExpandLine(ref base.pfStart[2], ref base.pfEnd[2]);
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Cross Channel", typeof(CrossObject), null, "Channel", "ChannelCross") };
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

        public int LineCount
        {
            get
            {
                return this.lineCount;
            }
            set
            {
                this.lineCount = value;
            }
        }
    }
}

