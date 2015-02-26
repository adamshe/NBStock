namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;

    public class CycleObject : LineGroupTextObject
    {
        private ArrayList alEnd = new ArrayList();
        private ArrayList alStart = new ArrayList();
        private double CurrentCycle;
        private CycleStyle cycleStyle;
        private int maxLines = 20;
        private bool showCycleText = true;

        public CycleObject()
        {
            base.SmoothingMode = ObjectSmoothingMode.Default;
            base.ObjectFont.TextFont = new Font(base.ObjectFont.TextFont, FontStyle.Italic);
        }

        public override void CalcPoint()
        {
            int num = 1;
            int num2 = 1;
            int num3 = 1;
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            double columnWidth = base.Manager.Canvas.BackChart.ColumnWidth;
            if (this.ControlPointNum > 1)
            {
                num3 = (int) Math.Round((double) (((double) (base.ToPointF(base.ControlPoints[1]).X - tf.X)) / columnWidth));
                if (num3 == 0)
                {
                    num3 = 1;
                }
            }
            this.CurrentCycle = 0.0;
            Rectangle rect = base.Area.Canvas.Rect;
            float x = 1f;
            this.alStart.Clear();
            this.alEnd.Clear();
            for (int i = 0; ((x > 0f) && (x < rect.Right)) && (i < this.maxLines); i++)
            {
                x = tf.X + ((float) (columnWidth * this.CurrentCycle));
                this.alStart.Add(new PointF(x, (float) rect.Top));
                this.alEnd.Add(new PointF(x, (float) rect.Bottom));
                switch (this.cycleStyle)
                {
                    case CycleStyle.Equal:
                    {
                        this.CurrentCycle += num3;
                        continue;
                    }
                    case CycleStyle.FabonacciCycle:
                    {
                        if ((i % 2) != 0)
                        {
                            break;
                        }
                        this.CurrentCycle = num2;
                        num += num2;
                        continue;
                    }
                    case CycleStyle.Sqr:
                    {
                        this.CurrentCycle = (i + 1) * (i + 1);
                        continue;
                    }
                    case CycleStyle.Symmetry:
                    {
                        if (i != 0)
                        {
                            goto Label_018E;
                        }
                        this.CurrentCycle = 0.0;
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                this.CurrentCycle = num;
                num2 += num;
                continue;
            Label_018E:
                switch (i)
                {
                    case 1:
                    {
                        this.CurrentCycle = num3;
                        continue;
                    }
                    case 2:
                    {
                        this.CurrentCycle = -num3;
                        continue;
                    }
                    default:
                    {
                        x = 0f;
                        continue;
                    }
                }
            }
            base.pfStart = (PointF[]) this.alStart.ToArray(typeof(PointF));
            base.pfEnd = (PointF[]) this.alEnd.ToArray(typeof(PointF));
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            if (this.showCycleText)
            {
                for (int i = 0; i < base.pfStart.Length; i++)
                {
                    string s = this.GetStr(i);
                    base.ObjectFont.DrawString(s, g, this.GetRect(s, i));
                }
            }
        }

        public void Equal()
        {
            this.CycleStyle = CycleStyle.Equal;
        }

        public void FabonacciCycle()
        {
            this.CycleStyle = CycleStyle.FabonacciCycle;
        }

        private RectangleF GetRect(int i)
        {
            return this.GetRect(this.GetStr(i), i);
        }

        private RectangleF GetRect(string s, int i)
        {
            SizeF ef = base.ObjectFont.Measure(base.CurrentGraphics, s);
            Rectangle rect = base.Area.Canvas.Rect;
            return new RectangleF(base.pfStart[i].X, (float) rect.Top, ef.Width, (float) rect.Height);
        }

        public override Region GetRegion()
        {
            Region region = base.GetRegion();
            if (this.showCycleText)
            {
                for (int i = 0; i < base.pfStart.Length; i++)
                {
                    region.Union(this.GetRect(i));
                }
            }
            return region;
        }

        private string GetStr(int i)
        {
            double columnWidth = base.Manager.Canvas.BackChart.ColumnWidth;
            int num2 = (int) (Math.Round((double) (base.pfStart[i].X - base.pfStart[0].X)) / columnWidth);
            return num2.ToString();
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Equal cycle line", typeof(CycleObject), "Equal", "Cycle", "CycleE", 500), new ObjectInit("Fabonacci cycle line", typeof(CycleObject), "FabonacciCycle", "Cycle", "CycleF"), new ObjectInit("Sqr cycle line", typeof(CycleObject), "Sqr", "Cycle", "CycleSqr"), new ObjectInit("Symmetry line", typeof(CycleObject), "Symmetry", "Cycle", "CycleSym") };
        }

        public void Sqr()
        {
            this.CycleStyle = CycleStyle.Sqr;
        }

        public void Symmetry()
        {
            this.CycleStyle = CycleStyle.Symmetry;
        }

        public override int ControlPointNum
        {
            get
            {
                return this.InitNum;
            }
        }

        [ReadOnly(true)]
        public CycleStyle CycleStyle
        {
            get
            {
                return this.cycleStyle;
            }
            set
            {
                this.cycleStyle = value;
            }
        }

        public override int InitNum
        {
            get
            {
                return (1 + (((this.cycleStyle == CycleStyle.Equal) | (this.cycleStyle == CycleStyle.Symmetry)) ? 1 : 0));
            }
        }

        public int MaxLines
        {
            get
            {
                return this.maxLines;
            }
            set
            {
                this.maxLines = value;
            }
        }

        public bool ShowCycleText
        {
            get
            {
                return this.showCycleText;
            }
            set
            {
                this.showCycleText = value;
            }
        }
    }
}

