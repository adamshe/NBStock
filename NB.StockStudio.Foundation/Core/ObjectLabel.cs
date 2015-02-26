namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Text;

    public class ObjectLabel
    {
        public Color BackColor;
        public Color BorderColor;
        public StringFormat format = new StringFormat(StringFormat.GenericDefault);
        public int Left;
        public int RoundWidth = 4;
        public int ShadowWidth = 2;
        public StickAlignment StickAlignment;
        public int StickHeight = 6;
        public string Text;
        public Brush TextBrush = Brushes.Black;
        public Font TextFont = new Font("Verdana", 8f);
        public int Top;

        public ObjectLabel()
        {
            this.format.Alignment = StringAlignment.Center;
            this.format.LineAlignment = StringAlignment.Center;
        }

        public void Draw(Graphics g)
        {
            SizeF ef = g.MeasureString(this.Text, this.TextFont, 0x3e8, this.format);
            RectangleF layoutRectangle = new RectangleF((float) this.Left, (float) this.Top, ef.Width + 4f, ef.Height + 4f);
            ArrayList al = new ArrayList();
            al.Add(new PointF(layoutRectangle.Left, layoutRectangle.Top + this.RoundWidth));
            al.Add(new PointF(layoutRectangle.Left + this.RoundWidth, layoutRectangle.Top));
            al.Add(new PointF(layoutRectangle.Right - this.RoundWidth, layoutRectangle.Top));
            al.Add(new PointF(layoutRectangle.Right, layoutRectangle.Top + this.RoundWidth));
            al.Add(new PointF(layoutRectangle.Right, layoutRectangle.Bottom - this.RoundWidth));
            al.Add(new PointF(layoutRectangle.Right - this.RoundWidth, layoutRectangle.Bottom));
            al.Add(new PointF(layoutRectangle.Left + this.RoundWidth, layoutRectangle.Bottom));
            al.Add(new PointF(layoutRectangle.Left, layoutRectangle.Bottom - this.RoundWidth));
            al.Add(al[0]);
            PointF tf = new PointF((float) this.Left, (float) this.Top);
            if (this.StickHeight > 0)
            {
                float offsetX = 0f;
                float offsetY = 0f;
                int index = 1;
                switch (this.StickAlignment)
                {
                    case StickAlignment.LeftTop:
                        offsetX = this.StickHeight;
                        offsetY = this.StickHeight;
                        break;

                    case StickAlignment.LeftBottom:
                        offsetX = this.StickHeight;
                        offsetY = -layoutRectangle.Height - this.StickHeight;
                        index = 7;
                        break;

                    case StickAlignment.RightTop:
                        offsetX = -layoutRectangle.Width - this.StickHeight;
                        offsetY = this.StickHeight;
                        index = 3;
                        break;

                    case StickAlignment.RightBottom:
                        offsetX = -layoutRectangle.Width - this.StickHeight;
                        offsetY = -layoutRectangle.Height - this.StickHeight;
                        index = 5;
                        break;
                }
                al = this.OffsetPoint(al, offsetX, offsetY);
                al.Insert(index, tf);
                layoutRectangle.Offset(offsetX, offsetY);
            }
            PointF[] points = (PointF[]) al.ToArray(typeof(PointF));
            if (this.ShadowWidth > 0)
            {
                PointF[] pfs = (PointF[]) points.Clone();
                this.OffsetPoint(pfs, (float) this.ShadowWidth, (float) this.ShadowWidth);
                g.FillPolygon(new SolidBrush(Color.FromArgb(0x40, Color.Black)), pfs);
            }
            if (this.BackColor != Color.Empty)
            {
                g.FillPolygon(new SolidBrush(this.BackColor), points);
            }
            if (this.BorderColor != Color.Empty)
            {
                g.DrawLines(new Pen(this.BorderColor), points);
            }
            TextRenderingHint textRenderingHint = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.DrawString(this.Text, this.TextFont, this.TextBrush, layoutRectangle, this.format);
            g.TextRenderingHint = textRenderingHint;
        }

        public ArrayList OffsetPoint(ArrayList al, float OffsetX, float OffsetY)
        {
            PointF[] pfs = (PointF[]) al.ToArray(typeof(PointF));
            this.OffsetPoint(pfs, OffsetX, OffsetY);
            ArrayList list = new ArrayList();
            list.AddRange(pfs);
            return list;
        }

        public void OffsetPoint(PointF[] pfs, float OffsetX, float OffsetY)
        {
            for (int i = 0; i < pfs.Length; i++)
            {
                pfs[i].X += OffsetX;
                pfs[i].Y += OffsetY;
            }
        }
    }
}

