namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class LineGroupObject : BaseObject
    {
        protected PointF[] pfEnd;
        protected PointF[] pfStart;

        public virtual void CalcPoint()
        {
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            this.CalcPoint();
            if ((this.pfStart != null) && (this.pfEnd != null))
            {
                for (int i = 0; i < this.pfStart.Length; i++)
                {
                    try
                    {
                        g.DrawLine(base.LinePen.GetPen(), this.pfStart[i], this.pfEnd[i]);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public override RectangleF GetMaxRect()
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            this.CalcPoint();
            if ((this.pfStart != null) && (this.pfEnd != null))
            {
                ArrayList list = new ArrayList();
                list.AddRange(this.pfStart);
                list.AddRange(this.pfEnd);
                return base.GetMaxRect((PointF[]) list.ToArray(typeof(PointF)));
            }
            return base.GetMaxRect();
        }

        public override bool InObject(int X, int Y)
        {
            this.CalcPoint();
            if ((this.pfStart != null) && (this.pfEnd != null))
            {
                for (int i = 0; i < this.pfStart.Length; i++)
                {
                    if (this.pfStart[i] != PointF.Empty)
                    {
                        bool flag = base.InLineSegment(X, Y, this.pfStart[i], this.pfEnd[i], base.LinePen.Width);
                        if (flag)
                        {
                            return flag;
                        }
                    }
                }
            }
            return false;
        }
    }
}

