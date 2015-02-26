namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Xml.Serialization;

    public class LabelObject : BaseObject
    {
        private ObjectBrush backBrush = new ObjectBrush(Color.Yellow);
        private ObjectFont labelFont = new ObjectFont();
        private string NewText;
        private RectangleF Rect;
        private int roundWidth = 2;
        private int shadowWidth = 2;
        private StickAlignment stickAlignment;
        private int stickHeight = 6;
        private string text = "Label";

        public LabelObject()
        {
            this.LabelFont.Alignment = StringAlignment.Center;
            this.LabelFont.LineAlignment = StringAlignment.Center;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            PointF[] points = this.ToPoints();
            if (this.ShadowWidth > 0)
            {
                PointF[] pfs = (PointF[]) points.Clone();
                this.OffsetPoint(pfs, (float) this.ShadowWidth, (float) this.ShadowWidth);
                g.FillPolygon(new SolidBrush(Color.FromArgb(0x40, Color.Black)), pfs);
            }
            if (this.BackBrush.Color != Color.Empty)
            {
                g.FillPolygon(this.BackBrush.GetBrush(this.GetMaxRect()), points);
            }
            if (base.LinePen.Color != Color.Empty)
            {
                g.DrawLines(base.LinePen.GetPen(), points);
            }
            TextRenderingHint textRenderingHint = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            this.LabelFont.DrawString(this.NewText, g, this.Rect);
            g.TextRenderingHint = textRenderingHint;
        }

        public override RectangleF GetMaxRect()
        {
            return base.GetMaxRect(this.ToPoints());
        }

        public void InitPriceDateLabel()
        {
            this.text = "Label\r\n{0}\r\n{D}";
        }

        public void InitPriceLabel()
        {
            this.LabelFont.Alignment = StringAlignment.Near;
            this.text = "Date = {D}\r\nOpen = {O}\r\nHigh = {H}\r\nLow = {L}\r\nClose = {0}";
        }

        public void InitTransparentText()
        {
            this.stickHeight = 0;
            this.backBrush.BrushStyle = BrushStyle.Empty;
            this.shadowWidth = 0;
            this.roundWidth = 0;
            base.LinePen.Alpha = 0;
            this.LabelFont.TextBrush.Alpha = 100;
            this.LabelFont.TextFont = new Font(this.LabelFont.TextFont.FontFamily, 40f);
        }

        public override bool InObject(int X, int Y)
        {
            return this.GetMaxRect().Contains((float) X, (float) Y);
        }

        public void OffsetPoint(PointF[] pfs, float OffsetX, float OffsetY)
        {
            for (int i = 0; i < pfs.Length; i++)
            {
                pfs[i].X += OffsetX;
                pfs[i].Y += OffsetY;
            }
        }

        public ArrayList OffsetPoint(ArrayList al, float OffsetX, float OffsetY)
        {
            PointF[] pfs = (PointF[]) al.ToArray(typeof(PointF));
            this.OffsetPoint(pfs, OffsetX, OffsetY);
            ArrayList list = new ArrayList();
            list.AddRange(pfs);
            return list;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Label", typeof(LabelObject), null, "Text", "TextL", 800), new ObjectInit("Price Label", typeof(LabelObject), "InitPriceLabel", "Text", "TextLP"), new ObjectInit("Price Date Label", typeof(LabelObject), "InitPriceDateLabel", "Text", "TextLPD"), new ObjectInit("Transparent Text", typeof(LabelObject), "InitTransparentText", "Text", "Text") };
        }

        public PointF[] ToPoints()
        {
            ArrayList al = new ArrayList();
            if (base.CurrentGraphics != null)
            {
                this.NewText = base.ReplaceTag(base.ControlPoints[0], this.Text);
                SizeF ef = this.LabelFont.Measure(base.CurrentGraphics, this.NewText);
                PointF tf = base.ToPointF(base.ControlPoints[0]);
                this.Rect = new RectangleF(tf.X, tf.Y, ef.Width + 4f, ef.Height + 4f);
                al.Add(new PointF(this.Rect.Left, this.Rect.Top + this.RoundWidth));
                al.Add(new PointF(this.Rect.Left + this.RoundWidth, this.Rect.Top));
                al.Add(new PointF(this.Rect.Right - this.RoundWidth, this.Rect.Top));
                al.Add(new PointF(this.Rect.Right, this.Rect.Top + this.RoundWidth));
                al.Add(new PointF(this.Rect.Right, this.Rect.Bottom - this.RoundWidth));
                al.Add(new PointF(this.Rect.Right - this.RoundWidth, this.Rect.Bottom));
                al.Add(new PointF(this.Rect.Left + this.RoundWidth, this.Rect.Bottom));
                al.Add(new PointF(this.Rect.Left, this.Rect.Bottom - this.RoundWidth));
                al.Add(al[0]);
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
                            offsetY = -this.Rect.Height - this.StickHeight;
                            index = 7;
                            break;

                        case StickAlignment.RightTop:
                            offsetX = -this.Rect.Width - this.StickHeight;
                            offsetY = this.StickHeight;
                            index = 3;
                            break;

                        case StickAlignment.RightBottom:
                            offsetX = -this.Rect.Width - this.StickHeight;
                            offsetY = -this.Rect.Height - this.StickHeight;
                            index = 5;
                            break;
                    }
                    al = this.OffsetPoint(al, offsetX, offsetY);
                    al.Insert(index, tf);
                    this.Rect.Offset(offsetX, offsetY);
                }
            }
            return (PointF[]) al.ToArray(typeof(PointF));
        }

        public ObjectBrush BackBrush
        {
            get
            {
                return this.backBrush;
            }
            set
            {
                this.backBrush = value;
            }
        }

        public override int ControlPointNum
        {
            get
            {
                return 1;
            }
        }

        public override int InitNum
        {
            get
            {
                return 1;
            }
        }

        public ObjectFont LabelFont
        {
            get
            {
                return this.labelFont;
            }
            set
            {
                this.labelFont = value;
            }
        }

        [XmlIgnore]
        public string[] Lines
        {
            get
            {
                string[] strArray = this.text.Split(new char[] { '\r' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Trim();
                }
                return strArray;
            }
            set
            {
                this.text = string.Join("\r\n", value);
            }
        }

        public int RoundWidth
        {
            get
            {
                return this.roundWidth;
            }
            set
            {
                this.roundWidth = value;
            }
        }

        public int ShadowWidth
        {
            get
            {
                return this.shadowWidth;
            }
            set
            {
                this.shadowWidth = value;
            }
        }

        public StickAlignment StickAlignment
        {
            get
            {
                return this.stickAlignment;
            }
            set
            {
                this.stickAlignment = value;
            }
        }

        public int StickHeight
        {
            get
            {
                return this.stickHeight;
            }
            set
            {
                this.stickHeight = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

