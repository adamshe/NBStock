namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class GridObject : LineGroupObject
    {
        private int gridCount = 10;

        public override void CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            float num = tfArray[1].X - tfArray[0].X;
            float num2 = tfArray[1].Y - tfArray[0].Y;
            base.pfStart = new PointF[(this.gridCount * 4) + 2];
            base.pfEnd = new PointF[(this.gridCount * 4) + 2];
            for (int i = -this.gridCount; i <= this.gridCount; i++)
            {
                int index = (i + this.gridCount) * 2;
                PointF tf = new PointF(tfArray[0].X + (num * i), tfArray[0].Y - (num2 * i));
                PointF tf2 = new PointF(tfArray[1].X + (num * i), tfArray[1].Y - (num2 * i));
                base.ExpandLine(ref tf, ref tf2);
                base.ExpandLine(ref tf2, ref tf);
                base.pfStart[index] = tf;
                base.pfEnd[index] = tf2;
                tf = new PointF(tfArray[0].X + (num * i), tfArray[0].Y + (num2 * i));
                tf2 = new PointF((tfArray[0].X + num) + (num * i), (tfArray[0].Y - num2) + (num2 * i));
                base.ExpandLine(ref tf, ref tf2);
                base.ExpandLine(ref tf2, ref tf);
                base.pfStart[index + 1] = tf;
                base.pfEnd[index + 1] = tf2;
            }
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Grid", typeof(GridObject), null, "Channel", "ChannelGrid") };
        }

        public int GridCount
        {
            get
            {
                return this.gridCount;
            }
            set
            {
                this.gridCount = value;
            }
        }
    }
}

