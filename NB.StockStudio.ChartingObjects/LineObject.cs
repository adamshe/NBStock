using System;
using System.Drawing;
using NB.StockStudio.Foundation;

namespace NB.StockStudio.ChartingObjects
{
    [Serializable]
    public class LineObject : BaseObject
    {
        private int Num = 2;
        private bool openEnd;
        private bool openStart;

        public LineObject()
        {
            this.Num = 2;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            PointF[] points = this.ToPoints();
            g.DrawLines(base.LinePen.GetPen(), points);
        }

        public override RectangleF GetMaxRect()
        {
            return base.GetMaxRect(this.ToPoints());
        }

        public void InitArrowCap()
        {
            base.LinePen.EndCap = new ArrowCap(10, 10, false);
        }

        public void InitLine()
        {
            this.openStart = true;
            this.openEnd = true;
        }

        public void InitLine3()
        {
            this.Num = 4;
            base.InitPoints = new PointF[] { new PointF(0f, 0f), new PointF(1f, 3f), new PointF(2f, 1f), new PointF(3f, 5f) };
            base.ControlPoints = new ObjectPoint[this.Num];
        }

        public void InitLine4()
        {
            this.Num = 5;
            base.InitPoints = new PointF[] { new PointF(0f, 0f), new PointF(1f, 2f), new PointF(2f, 0f), new PointF(3f, 2f), new PointF(4f, 0f) };
            base.ControlPoints = new ObjectPoint[this.Num];
        }

        public void InitLine5()
        {
            this.Num = 6;
            base.InitPoints = new PointF[] { new PointF(0f, 0f), new PointF(1f, 3f), new PointF(2f, 1f), new PointF(3f, 5f), new PointF(4f, 4f), new PointF(5f, 7f) };
            base.ControlPoints = new ObjectPoint[this.Num];
        }

        public void InitLine8()
        {
            this.Num = 9;
            base.InitPoints = new PointF[] { new PointF(0f, 0f), new PointF(1f, 3f), new PointF(2f, 1f), new PointF(3f, 5f), new PointF(4f, 4f), new PointF(5f, 7f), new PointF(6f, 5f), new PointF(7f, 6f), new PointF(8f, 4f) };
            base.ControlPoints = new ObjectPoint[this.Num];
        }

        public void InitOpen2()
        {
            this.openStart = true;
            this.openEnd = true;
        }

        public void InitOpenEnd()
        {
            this.openEnd = true;
        }

        public override bool InObject(int X, int Y)
        {
            PointF[] tfArray = this.ToPoints();
            for (int i = 0; i < (tfArray.Length - 1); i++)
            {
                bool flag = base.InLineSegment(X, Y, tfArray[i], tfArray[i + 1], base.LinePen.Width + 1);
                if (flag)
                {
                    return flag;
                }
            }
            return false;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Segment", typeof(LineObject), null, "Line", "Segment", 100), new ObjectInit("Open end segment", typeof(LineObject), "InitOpenEnd", "Line", "Line1"), new ObjectInit("Line", typeof(LineObject), "InitOpen2", "Line", "Line2"), new ObjectInit("Arrow Line", typeof(LineObject), "InitArrowCap", "Line", "ArrowLine"), new ObjectInit("3 Segments Line", typeof(LineObject), "InitLine3", "Line", "Line3"), new ObjectInit("5 Segments Line", typeof(LineObject), "InitLine5", "Line", "Line5"), new ObjectInit("8 Segments Line", typeof(LineObject), "InitLine8", "Line", "Line8"), new ObjectInit("4 Segments Line", typeof(LineObject), "InitLine4", "Line", "Line4") };
        }

        public PointF[] ToPoints()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            if (tfArray.Length > 1)
            {
                if (this.openStart)
                {
                    base.ExpandLine(ref tfArray[1], ref tfArray[0]);
                }
                if (this.openEnd)
                {
                    base.ExpandLine(ref tfArray[tfArray.Length - 2], ref tfArray[tfArray.Length - 1]);
                }
            }
            return tfArray;
        }

        public override int ControlPointNum
        {
            get
            {
                return this.Num;
            }
        }

        public bool OpenEnd
        {
            get
            {
                return this.openEnd;
            }
            set
            {
                this.openEnd = value;
            }
        }

        public bool OpenStart
        {
            get
            {
                return this.openStart;
            }
            set
            {
                this.openStart = value;
            }
        }
    }
}

