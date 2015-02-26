namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class FormulaLabel : ICloneable
    {
        public Color BGColor;
        public Color BorderColor;
        public int StickHeight;
        public Brush TextBrush;

        public FormulaLabel(Color BorderColor, Color BGColor) : this(BorderColor, BGColor, new SolidBrush(Color.White))
        {
        }

        public FormulaLabel(Color BorderColor, Color BGColor, Brush TextBrush)
        {
            this.StickHeight = 6;
            this.BorderColor = BorderColor;
            this.BGColor = BGColor;
            this.TextBrush = TextBrush;
        }

        public object Clone()
        {
            return new FormulaLabel(this.BorderColor, this.BGColor, (Brush) this.TextBrush.Clone());
        }

        public void DrawString(Graphics g, string Text, Font TextFont, Brush TextBrush, VerticalAlign VAlign, FormulaAlign Align, PointF Pos, bool ShowArrow)
        {
            this.DrawString(g, Text, TextFont, TextBrush, VAlign, Align, new RectangleF(Pos, (SizeF) Size.Empty), ShowArrow);
        }

        public void DrawString(Graphics g, string Text, Font TextFont, Brush TextBrush, VerticalAlign VAlign, FormulaAlign Align, RectangleF Rect, bool ShowArrow)
        {
            if (!double.IsNaN((double) Rect.Y) && !double.IsInfinity((double) Rect.Y))
            {
                PointF location = Rect.Location;
                SizeF size = Rect.Size;
                SizeF ef2 = g.MeasureString(Text, TextFont);
                if (size.Width == 0f)
                {
                    size.Width = ef2.Width;
                }
                if (size.Height < ef2.Height)
                {
                    size.Height = ef2.Height;
                }
                ArrayList list = new ArrayList();
                int num = 3;
                int stickHeight = 0;
                if (ShowArrow)
                {
                    stickHeight = this.StickHeight;
                }
                int height = (int) size.Height;
                if (VAlign == VerticalAlign.Bottom)
                {
                    stickHeight = -stickHeight;
                    height = -height;
                }
                if (VAlign == VerticalAlign.VCenter)
                {
                    location.Y += size.Height / 2f;
                }
                float num4 = 0f;
                if (Align == FormulaAlign.Center)
                {
                    num4 = size.Width / 2f;
                }
                else if (Align == FormulaAlign.Right)
                {
                    num4 = size.Width + 2f;
                }
                if (ShowArrow)
                {
                    list.Add(location);
                    list.Add(new PointF(location.X - num, location.Y - stickHeight));
                }
                list.Add(new PointF(location.X - num4, location.Y - stickHeight));
                list.Add(new PointF(location.X - num4, (location.Y - stickHeight) - height));
                list.Add(new PointF(((location.X - num4) + 1f) + size.Width, (location.Y - stickHeight) - height));
                list.Add(new PointF(((location.X - num4) + 1f) + size.Width, location.Y - stickHeight));
                if (ShowArrow)
                {
                    list.Add(new PointF(location.X + num, location.Y - stickHeight));
                }
                list.Add(list[0]);
                if (this.BGColor != Color.Empty)
                {
                    g.FillPolygon(new SolidBrush(this.BGColor), (PointF[]) list.ToArray(typeof(PointF)));
                }
                if (this.BorderColor != Color.Empty)
                {
                    g.DrawLines(new Pen(this.BorderColor), (PointF[]) list.ToArray(typeof(PointF)));
                }
                RectangleF layoutRectangle = new RectangleF(new PointF(location.X - num4, location.Y - stickHeight), size);
                if (height > 0)
                {
                    layoutRectangle.Y -= size.Height;
                }
                g.DrawString(Text, TextFont, TextBrush, layoutRectangle);
            }
        }

        public void SetProperTextColor()
        {
            Color black = Color.Black;
            if (((this.BGColor.R < 160) || (this.BGColor.G < 160)) || (this.BGColor.B < 160))
            {
                black = Color.White;
            }
            if (this.TextBrush is SolidBrush)
            {
                (this.TextBrush as SolidBrush).Color = black;
            }
        }

        public static FormulaLabel EmptyLabel
        {
            get
            {
                return new FormulaLabel(Color.Empty, Color.Empty, null);
            }
        }

        public static FormulaLabel GreenLabel
        {
            get
            {
                return new FormulaLabel(Color.Black, Color.FromArgb(180, 0xff, 180), Brushes.Black);
            }
        }

        public static FormulaLabel RedLabel
        {
            get
            {
                return new FormulaLabel(Color.Black, Color.FromArgb(0xff, 180, 180), Brushes.Black);
            }
        }

        public static FormulaLabel WhiteLabel
        {
            get
            {
                return new FormulaLabel(Color.DarkGreen, Color.WhiteSmoke, Brushes.Black);
            }
        }
    }
}

