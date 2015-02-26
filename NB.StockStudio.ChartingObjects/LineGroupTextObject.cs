namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class LineGroupTextObject : LineGroupObject
    {
        private ObjectFont objectFont = new ObjectFont();

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            string s = this.GetStr();
            if (s != null)
            {
                this.ObjectFont.DrawString(s, g, this.GetTextRect());
            }
        }

        public override Region GetRegion()
        {
            Region region = base.GetRegion();
            if (base.CurrentGraphics != null)
            {
                region.Union(this.GetTextRect());
            }
            return region;
        }

        public virtual string GetStr()
        {
            return null;
        }

        public virtual RectangleF GetTextRect()
        {
            return RectangleF.Empty;
        }

        public virtual RectangleF GetTextRect(int LineNumber, bool Horizon)
        {
            if (base.CurrentGraphics != null)
            {
                SizeF ef = this.ObjectFont.Measure(base.CurrentGraphics, this.GetStr());
                RectangleF empty = RectangleF.Empty;
                if (Horizon)
                {
                    empty = new RectangleF(base.pfStart[LineNumber].X, base.pfStart[LineNumber].Y - ef.Height, base.pfEnd[LineNumber].X - base.pfStart[LineNumber].X, ef.Height * 2f);
                }
                else
                {
                    empty = new RectangleF(base.pfStart[LineNumber].X - ef.Width, base.pfStart[LineNumber].Y, ef.Width * 2f, base.pfEnd[LineNumber].Y - base.pfStart[LineNumber].Y);
                }
                if (empty.Width < 0f)
                {
                    empty.X += empty.Width;
                    empty.Width = -empty.Width;
                }
                if (empty.Height < 0f)
                {
                    empty.Y += empty.Height;
                    empty.Height = -empty.Height;
                }
                empty.Width = Math.Max(empty.Width, ef.Width);
                empty.Height = Math.Max(empty.Height, ef.Height);
                return empty;
            }
            return RectangleF.Empty;
        }

        public ObjectFont ObjectFont
        {
            get
            {
                return this.objectFont;
            }
            set
            {
                this.objectFont = value;
            }
        }
    }
}

