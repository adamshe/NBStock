namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class PolygonObject : BaseObject
    {
        protected PointF[] AllPoints;

        public virtual PointF[] CalcPoint()
        {
            return new PointF[0];
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            this.AllPoints = this.CalcPoint();
            if (this.AllPoints.Length > 0)
            {
                try
                {
                    if (this.Closed)
                    {
                        g.DrawPolygon(base.LinePen.GetPen(), this.AllPoints);
                    }
                    else
                    {
                        g.DrawLines(base.LinePen.GetPen(), this.AllPoints);
                    }
                }
                catch
                {
                }
            }
        }

        public override RectangleF GetMaxRect()
        {
            this.AllPoints = this.CalcPoint();
            return base.GetMaxRect(this.AllPoints);
        }

        public override bool InObject(int X, int Y)
        {
            return base.InLineSegment(X, Y, this.AllPoints, base.LinePen.Width, this.Closed);
        }

        [XmlIgnore, Browsable(false)]
        public virtual bool Closed
        {
            get
            {
                return false;
            }
        }
    }
}

