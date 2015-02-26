namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Drawing;

    public class LinearRegressionObject : LineGroupObject
    {
        private double A;
        private double B;
        private int Bar1;
        private int Bar2;
        private bool centerLine = true;
        private bool downLine = true;
        private FormulaData fd;
        private bool openEnd = false;
        private bool openStart = false;
        private double percentage = 1.0;
        private RegressionType regressionType;
        private SegmentCollection scLines = new SegmentCollection();
        private bool showAuxLine = false;
        private bool upLine = true;

        private void AddLine(ObjectPoint op1, ObjectPoint op2, float Delta)
        {
            op1.Y -= Delta * this.percentage;
            op2.Y -= Delta * this.percentage;
            this.scLines.Add(op1, op2);
        }

        public override void CalcPoint()
        {
            float num3;
            this.fd = base.Area.FormulaDataArray[0];
            double[] dd = base.Manager.Canvas.BackChart.DataProvider["DATE"];
            int index = 0;
            int num2 = 1;
            if (base.ControlPoints[0].X > base.ControlPoints[1].X)
            {
                index = 1;
                num2 = 0;
            }
            this.Bar1 = FormulaChart.FindIndex(dd, base.ControlPoints[index].X);
            this.Bar2 = FormulaChart.FindIndex(dd, base.ControlPoints[num2].X);
            if (this.regressionType != RegressionType.UpDownTrend)
            {
                FormulaBase.CalcLinearRegression(this.fd, this.Bar2, this.Bar2 - this.Bar1, out this.A, out this.B);
            }
            else
            {
                this.A = base.ControlPoints[index].Y;
                this.B = (base.ControlPoints[num2].Y - base.ControlPoints[index].Y) / ((double) (this.Bar2 - this.Bar1));
            }
            ObjectPoint point = base.ControlPoints[index];
            ObjectPoint point2 = base.ControlPoints[num2];
            point.Y = this.A;
            point2.Y = this.A + (this.B * (this.Bar2 - this.Bar1));
            this.scLines.Clear();
            if (this.centerLine)
            {
                this.scLines.Add(point, point2);
            }
            if (((this.regressionType == RegressionType.Channel) || (this.regressionType == RegressionType.AsynChannel)) || (this.regressionType == RegressionType.UpDownTrend))
            {
                float num4 = base.CalcDelta(this.fd, this.A, this.B, this.Bar1, this.Bar2, "H", true);
                float num5 = base.CalcDelta(this.fd, this.A, this.B, this.Bar1, this.Bar2, "L", false);
                if (this.regressionType == RegressionType.Channel)
                {
                    num3 = Math.Max(Math.Abs(num4), Math.Abs(num5));
                }
                else
                {
                    num3 = -num4;
                }
                if (this.upLine)
                {
                    this.AddLine(point, point2, num3);
                }
                if (this.regressionType != RegressionType.Channel)
                {
                    num3 = num5;
                }
                if (this.downLine)
                {
                    this.AddLine(point, point2, -num3);
                }
            }
            else if ((this.regressionType == RegressionType.StdChannel) || (this.regressionType == RegressionType.StdErrorChannel))
            {
                num3 = this.Std(this.regressionType == RegressionType.StdErrorChannel);
                this.AddLine(point, point2, num3);
                this.AddLine(point, point2, -num3);
            }
            base.pfStart = new PointF[this.scLines.Count];
            base.pfEnd = new PointF[this.scLines.Count];
            for (int i = 0; i < this.scLines.Count; i++)
            {
                ObjectSegment segment = this.scLines[i];
                if (!double.IsNaN(segment.op1.Y) && !double.IsNaN(segment.op2.Y))
                {
                    base.pfStart[i] = base.ToPointF(segment.op1);
                    base.pfEnd[i] = base.ToPointF(segment.op2);
                    if (this.openStart)
                    {
                        base.ExpandLine(ref base.pfEnd[i], ref base.pfStart[i]);
                    }
                    if (this.openEnd)
                    {
                        base.ExpandLine(ref base.pfStart[i], ref base.pfEnd[i]);
                    }
                }
                else
                {
                    base.pfStart[i] = PointF.Empty;
                    base.pfEnd[i] = PointF.Empty;
                }
            }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            Pen pen = base.LinePen.GetPen();
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            Rectangle rect = base.Area.Canvas.Rect;
            if ((base.InMove || this.showAuxLine) && ((tf != PointF.Empty) && (tf2 != PointF.Empty)))
            {
                g.DrawLine(pen, tf.X, (float) rect.Y, tf.X, (float) rect.Bottom);
                g.DrawLine(pen, tf2.X, (float) rect.Y, tf2.X, (float) rect.Bottom);
            }
        }

        public override RectangleF GetMaxRect()
        {
            PointF tf = base.ToPointF(base.ControlPoints[0]);
            PointF tf2 = base.ToPointF(base.ControlPoints[1]);
            Rectangle rect = base.Area.Canvas.Rect;
            tf.Y = rect.Y;
            tf2.Y = rect.Bottom;
            if (this.openStart)
            {
                tf.X = rect.Left;
            }
            if (this.openEnd)
            {
                tf2.X = rect.Right;
            }
            PointF[] pfs = new PointF[] { tf, tf2 };
            return base.GetMaxRect(pfs);
        }

        public void InitAsynChannel()
        {
            this.regressionType = RegressionType.AsynChannel;
        }

        public void InitChannel()
        {
            this.regressionType = RegressionType.Channel;
        }

        public void InitDownTrend()
        {
            this.regressionType = RegressionType.UpDownTrend;
            this.upLine = false;
        }

        public void InitOpenChannel()
        {
            this.regressionType = RegressionType.Channel;
            this.openEnd = true;
        }

        public void InitSingle()
        {
            this.regressionType = RegressionType.Channel;
            this.upLine = false;
            this.downLine = false;
        }

        public void InitStdChannel()
        {
            this.regressionType = RegressionType.StdChannel;
        }

        public void InitStdErrorChannel()
        {
            this.regressionType = RegressionType.StdErrorChannel;
        }

        public void InitUpDownTrend()
        {
            this.regressionType = RegressionType.UpDownTrend;
        }

        public void InitUpTrend()
        {
            this.regressionType = RegressionType.UpDownTrend;
            this.downLine = false;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Linear Regression", typeof(LinearRegressionObject), "InitSingle", "LinearReg", "LRSingle", 200), new ObjectInit("Channel Linear Regression", typeof(LinearRegressionObject), "InitChannel", "LinearReg", "LRChannel"), new ObjectInit("Open Channel Linear Regression", typeof(LinearRegressionObject), "InitOpenChannel", "LinearReg", "LROChannel"), new ObjectInit("Asyn Channel Linear Regression", typeof(LinearRegressionObject), "InitAsynChannel", "LinearReg", "LRAChannel"), new ObjectInit("Std Channel", typeof(LinearRegressionObject), "InitStdChannel", "LinearReg", "LRSTD"), new ObjectInit("Std Error Channel", typeof(LinearRegressionObject), "InitStdErrorChannel", "LinearReg", "LRError"), new ObjectInit("Up Trend Channel", typeof(LinearRegressionObject), "InitUpTrend", "LinearReg", "LRUp"), new ObjectInit("Down Trend Channel", typeof(LinearRegressionObject), "InitDownTrend", "LinearReg", "LRDown"), new ObjectInit("Up/Down Trend Channel", typeof(LinearRegressionObject), "InitUpDownTrend", "LinearReg", "LRUpDown") };
        }

        private float Std(bool Error)
        {
            double[] data = this.fd.Data;
            double num = 0.0;
            for (int i = this.Bar1; i < this.Bar2; i++)
            {
                num += data[i];
            }
            num /= (double) (this.Bar2 - this.Bar1);
            double d = 0.0;
            for (int j = this.Bar1; j < this.Bar2; j++)
            {
                d += (data[j] - num) * (data[j] - num);
            }
            d /= (double) ((this.Bar2 - this.Bar1) - (Error ? 0 : 1));
            return (float) Math.Sqrt(d);
        }

        public bool CenterLine
        {
            get
            {
                return this.centerLine;
            }
            set
            {
                this.centerLine = value;
            }
        }

        public bool DownLine
        {
            get
            {
                return this.downLine;
            }
            set
            {
                this.downLine = value;
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

        public double Percentage
        {
            get
            {
                return this.percentage;
            }
            set
            {
                this.percentage = value;
            }
        }

        public RegressionType RegressionType
        {
            get
            {
                return this.regressionType;
            }
            set
            {
                this.regressionType = value;
            }
        }

        public bool ShowAuxLine
        {
            get
            {
                return this.showAuxLine;
            }
            set
            {
                this.showAuxLine = value;
            }
        }

        public bool UpLine
        {
            get
            {
                return this.upLine;
            }
            set
            {
                this.upLine = value;
            }
        }
    }
}

