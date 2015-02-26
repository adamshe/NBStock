namespace NB.StockStudio.Foundation
{
    using System;
    using System.Drawing;

    public class FormulaAxisY
    {
        public bool AutoFormat = true;
        public bool AutoMultiply = true;
        public bool AutoScale = true;
        public AxisPos AxisPos = AxisPos.Right;
        public FormulaBack Back;
        private string format;
        public Rectangle FrameRect;
        public Brush LabelBrush = Brushes.Black;
        public Font LabelFont = new Font("verdana", 8f);
        private float[] LabelPos;
        private double[] LabelValues;
        public RectangleF LastCursorRect;
        public FormulaTick MajorTick = new FormulaTick();
        public double MaxY = double.MaxValue;
        public FormulaTick MinorTick;
        public double MinY = double.MinValue;
        public FormulaBack MultiplyBack;
        public double MultiplyFactor = 1.0;
        public Rectangle Rect;
        public double RefValue;
        public TransformFunc RevertTransform;
        private ScaleType scale;
        public bool ShowAsPercent;
        public TransformFunc Transform;
        public bool Visible = true;
        public int Width = 80;

        public FormulaAxisY()
        {
            this.MajorTick.ShowLine = true;
            this.MinorTick = new FormulaTick();
            this.MinorTick.TickWidth = 3;
            this.MinorTick.MinimumPixel = 10;
            this.Back = new FormulaBack();
            this.MultiplyBack = new FormulaBack();
            this.MultiplyBack.BackGround = new SolidBrush(Color.Yellow);
        }

        public int CalcLabelWidth(FormulaCanvas Canvas)
        {
            Graphics currentGraph = Canvas.CurrentGraph;
            this.CalcLableLine(Canvas);
            int num = -2147483648;
            if (this.AutoFormat)
            {
                if ((this.MaxY <= 10.0) && (this.MinY < 1.0))
                {
                    this.format = "f3";
                }
                else
                {
                    this.format = "f2";
                }
            }
            for (int i = -1; i < this.LabelValues.Length; i++)
            {
                string str;
                if (i < 0)
                {
                    str = this.MultiplyFactorToString();
                }
                else
                {
                    str = (this.LabelValues[i] / this.MultiplyFactor).ToString(this.Format);
                }
                num = Math.Max(num, (int) currentGraph.MeasureString(str, this.LabelFont).Width);
            }
            return num;
        }

        private void CalcLableLine(FormulaCanvas Canvas)
        {
            double minY = this.MinY;
            double maxY = this.MaxY;
            if (this.ShowAsPercent)
            {
                minY = (minY / this.RefValue) - 1.0;
                maxY = (maxY / this.RefValue) - 1.0;
            }
            double d = maxY - minY;
            double num4 = Math.Pow(10.0, Math.Floor(Math.Log10(d)) + 1.0);
            double[] numArray = new double[] { 2.0, 2.5, 2.0 };
            for (int i = 0; (((d / num4) * Canvas.LabelHeight) * 3.0) < Canvas.Rect.Height; i++)
            {
                num4 /= numArray[i % numArray.Length];
            }
            double num6 = Math.Floor((double) (minY / num4)) * num4;
            double num7 = Math.Ceiling((double) (maxY / num4)) * num4;
            int num8 = (int) ((num7 - num6) / num4);
            this.LabelPos = new float[num8 + 1];
            this.LabelValues = new double[num8 + 1];
            for (int j = 0; j <= num8; j++)
            {
                this.LabelValues[j] = num6 + (num4 * j);
                if (this.ShowAsPercent)
                {
                    this.LabelPos[j] = this.CalcY((this.LabelValues[j] + 1.0) * this.RefValue);
                }
                else
                {
                    this.LabelPos[j] = this.CalcY(this.LabelValues[j]);
                }
            }
            if (this.AutoMultiply && !this.ShowAsPercent)
            {
                this.MultiplyFactor = 1.0;
                int[] numArray2 = new int[] { this.ZeroLog10(minY), this.ZeroLog10(maxY) };
                if (Math.Sign(numArray2[1]) != Math.Sign(numArray2[0]))
                {
                    numArray2[0] = 0;
                }
                foreach (int num10 in numArray2)
                {
                    if (num10 > 3)
                    {
                        this.MultiplyFactor = Math.Max(this.MultiplyFactor, Math.Pow(10.0, (double) (num10 - 3)));
                    }
                    else if (num10 < -1)
                    {
                        this.MultiplyFactor = Math.Min(this.MultiplyFactor, Math.Pow(10.0, (double) (num10 + 1)));
                    }
                }
            }
        }

        public float CalcY(double d)
        {
            if (this.MinY == this.MaxY)
            {
                return 0f;
            }
            d /= this.MultiplyFactor;
            double num = this.MaxY / this.MultiplyFactor;
            double num2 = this.MinY / this.MultiplyFactor;
            if (this.Transform != null)
            {
                d = this.Transform(d);
                num = this.Transform(num);
                num2 = this.Transform(num2);
            }
            return this.CalcY(d, num, num2);
        }

        public float CalcY(double d, double Max, double Min)
        {
            return (float) (this.Rect.Bottom - ((this.Rect.Height * (d - Min)) / (Max - Min)));
        }

        public void CopyFrom(FormulaAxisY fay)
        {
            this.Visible = fay.Visible;
            this.AutoMultiply = fay.AutoMultiply;
            this.Width = fay.Width;
            this.AutoScale = fay.AutoScale;
            this.MajorTick = (FormulaTick) fay.MajorTick.Clone();
            this.MinorTick = (FormulaTick) fay.MinorTick.Clone();
            this.Back = (FormulaBack) fay.Back.Clone();
            this.AutoFormat = fay.AutoFormat;
            this.Format = fay.Format;
            this.MultiplyBack = (FormulaBack) fay.MultiplyBack.Clone();
            this.LabelFont = (Font) fay.LabelFont.Clone();
            this.LabelBrush = (Brush) fay.LabelBrush.Clone();
            this.AxisPos = fay.AxisPos;
        }

        public void DrawCursor(Graphics g, FormulaChart fc, FormulaArea Area, float Y)
        {
            this.DrawCursor(g, fc, Area, Y, this.GetValueFromY(Y - fc.Rect.Y));
        }

        public void DrawCursor(Graphics g, FormulaChart fc, FormulaArea Area, float Y, double d)
        {
            if (!this.LastCursorRect.IsEmpty)
            {
                fc.RestoreMemBmp(g, this.LastCursorRect);
            }
            FormulaLabel label = Area.Labels[2];
            string text = d.ToString(this.Format);
            SizeF ef = g.MeasureString(text, this.LabelFont);
            RectangleF rect = new RectangleF((float) this.Rect.Left, Y - fc.Rect.Y, (this.Rect.Width - 1) - this.Back.RightPen.Width, ef.Height);
            this.LastCursorRect = rect;
            this.LastCursorRect.Inflate(2f, 1f);
            rect.Offset((PointF) fc.Rect.Location);
            label.DrawString(g, text, this.LabelFont, label.TextBrush, VerticalAlign.Bottom, FormulaAlign.Left, rect, false);
        }

        public double GetValueFromY(float Y)
        {
            if (this.MinY == this.MaxY)
            {
                return this.MinY;
            }
            double d = this.MaxY / this.MultiplyFactor;
            double num2 = this.MinY / this.MultiplyFactor;
            if (this.Transform != null)
            {
                d = this.Transform(d);
                num2 = this.Transform(num2);
            }
            double num3 = this.GetValueFromY(Y, d, num2);
            if (this.RevertTransform != null)
            {
                num3 = this.RevertTransform(num3);
            }
            return num3;
        }

        public double GetValueFromY(float Y, double Max, double Min)
        {
            return ((((this.Rect.Bottom - Y) / ((float) this.Rect.Height)) * (Max - Min)) + Min);
        }

        private string MultiplyFactorToString()
        {
            if (this.MultiplyFactor >= 1000000.0)
            {
                return ("x" + (this.MultiplyFactor / 1000000.0) + "M");
            }
            if (this.MultiplyFactor >= 1000.0)
            {
                return ("x" + (this.MultiplyFactor / 1000.0) + "K");
            }
            return ("x" + this.MultiplyFactor.ToString());
        }

        public void Render(FormulaCanvas Canvas, FormulaArea Area)
        {
            this.CalcLableLine(Canvas);
            Rectangle frameRect = this.FrameRect;
            Graphics currentGraph = Canvas.CurrentGraph;
            int left = frameRect.Left;
            if (this.AxisPos == AxisPos.Left)
            {
                left = frameRect.Right;
            }
            int tickWidth = this.MajorTick.TickWidth;
            if (this.MajorTick.FullTick)
            {
                tickWidth = frameRect.Width;
            }
            if (this.MajorTick.Inside)
            {
                tickWidth = -tickWidth;
            }
            int width = this.MinorTick.TickWidth;
            if (this.MinorTick.FullTick)
            {
                width = frameRect.Width;
            }
            if (this.MinorTick.Inside)
            {
                width = -width;
            }
            if (this.AxisPos == AxisPos.Left)
            {
                tickWidth = -tickWidth;
                width = -width;
            }
            this.Back.Render(currentGraph, frameRect);
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.LabelPos.Length; i++)
            {
                if ((this.LabelPos[i] >= Canvas.Rect.Top) && (this.LabelPos[i] <= Canvas.Rect.Bottom))
                {
                    int num6 = left;
                    string text = (this.LabelValues[i] / this.MultiplyFactor).ToString(this.Format);
                    SizeF ef = currentGraph.MeasureString(text, this.LabelFont);
                    if (this.AxisPos == AxisPos.Left)
                    {
                        num6 -= (int) ef.Width;
                        if (tickWidth < 0)
                        {
                            num6 += tickWidth;
                        }
                    }
                    else if (tickWidth > 0)
                    {
                        num6 += tickWidth;
                    }
                    float y = this.LabelPos[i] - (Canvas.LabelHeight / 2f);
                    if ((maxValue - y) > ef.Height)
                    {
                        currentGraph.DrawString(text, this.LabelFont, this.LabelBrush, (float) num6, y);
                        maxValue = y;
                    }
                    if (this.MajorTick.ShowLine)
                    {
                        currentGraph.DrawLine(this.MajorTick.LinePen, (float) Canvas.Rect.Left, this.LabelPos[i], (float) Canvas.Rect.Right, this.LabelPos[i]);
                    }
                    if (this.MajorTick.ShowTick)
                    {
                        currentGraph.DrawLine(this.MajorTick.TickPen, (float) left, this.LabelPos[i], (float) (left + tickWidth), this.LabelPos[i]);
                    }
                }
                if ((this.MinorTick.Visible && !double.IsInfinity((double) this.LabelPos[i])) && (i != (this.LabelPos.Length - 1)))
                {
                    int count = this.MinorTick.Count;
                    if (this.MinorTick.MinimumPixel != 0)
                    {
                        count = (int) ((this.LabelPos[i] - this.LabelPos[i + 1]) / ((float) this.MinorTick.MinimumPixel));
                    }
                    for (float j = this.LabelPos[i]; j > this.LabelPos[i + 1]; j += (this.LabelPos[i + 1] - this.LabelPos[i]) / ((float) count))
                    {
                        if ((j >= frameRect.Top) && (j <= frameRect.Bottom))
                        {
                            currentGraph.DrawLine(this.MinorTick.TickPen, (float) left, j, (float) (left + width), j);
                        }
                    }
                }
            }
            if (this.MultiplyFactor != 1.0)
            {
                string str2 = this.MultiplyFactorToString();
                Rectangle r = frameRect;
                r.Y = (int) ((r.Bottom - Canvas.LabelHeight) - 2f);
                if (Area.AxisX.Visible)
                {
                    r.Y -= (int) ((Canvas.LabelHeight / 2f) + 1f);
                }
                r.Height = (int) Canvas.LabelHeight;
                r.Width = ((int) currentGraph.MeasureString(str2, this.LabelFont).Width) + 1;
                if (this.AxisPos == AxisPos.Left)
                {
                    r.Offset((frameRect.Width - r.Width) - 2, 0);
                }
                this.MultiplyBack.Render(currentGraph, r);
                currentGraph.DrawString(str2, this.LabelFont, this.LabelBrush, r);
            }
            if (Area.Selected && (Area.SelectedPen != null))
            {
                Rectangle rect = frameRect;
                rect.Inflate(-1, -1);
                currentGraph.DrawRectangle(Area.SelectedPen, rect);
            }
            LatestValueType latestValueType = Area.Parent.LatestValueType;
            if (latestValueType != LatestValueType.None)
            {
                for (int k = Area.FormulaDataArray.Count-1; k >=0; k--)
                    //(int k = 0; k < Area.FormulaDataArray.Count; k++)
                {
                    FormulaData data = Area.FormulaDataArray[k];
                    if ((!data.TextInvisible || (data.RenderType == FormulaRenderType.STOCK)) && (((latestValueType == LatestValueType.All) || ((latestValueType == LatestValueType.StockOnly) && (Area.AxisYs[data.AxisYIndex] == this))) || ((latestValueType == LatestValueType.Custom) && data.LastValueInAxis)))
                    {
                        FormulaLabel label = Area.Labels[2];
                        if (latestValueType != LatestValueType.StockOnly)
                        {
                            label = (FormulaLabel) label.Clone();
                            Pen currentPen = Area.GetCurrentPen(k);
                            label.BGColor = currentPen.Color;
                            if (label.BGColor == Color.Empty)
                            {
                                label.BGColor = Color.White;
                            }
                            label.SetProperTextColor();
                        }
                        if (data.Length > Canvas.Start)
                        {
                            double d = data[(data.Length - 1) - Canvas.Start];
                            if (((latestValueType == LatestValueType.StockOnly) && (data.Length > (Canvas.Start + 1))) && (data[(data.Length - 2) - Canvas.Start] > d))
                            {
                                label = Area.Labels[1];
                            }
                            string str3 = " " + ((d / this.MultiplyFactor)).ToString(this.Format);
                            FormulaAlign right = FormulaAlign.Left;
                            if (this.AxisPos == AxisPos.Left)
                            {
                                right = FormulaAlign.Right;
                            }
                            label.DrawString(currentGraph, str3, this.LabelFont, label.TextBrush, VerticalAlign.Bottom, right, new PointF((float) left, this.CalcY(d)), false);
                        }
                    }
                }
            }
        }

        private double Sqr(double d)
        {
            return (d * d);
        }

        private double ZeroLog(double d)
        {
            if (d <= 0.0)
            {
                return double.NaN;
            }
            return Math.Log(Math.Abs(d));
        }

        private int ZeroLog10(double d)
        {
            if (d == 0.0)
            {
                return 0;
            }
            return (int) Math.Log10(Math.Abs(d));
        }

        private double ZeroSqrt(double d)
        {
            if (d < 0.0)
            {
                return 0.0;
            }
            return Math.Sqrt(d);
        }

        public string Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
                this.AutoFormat = (value == "") || (value == null);
            }
        }

        public ScaleType Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                if (value == ScaleType.Normal)
                {
                    this.Transform = null;
                    this.RevertTransform = null;
                }
                else if (value == ScaleType.Log)
                {
                    this.Transform = new TransformFunc(this.ZeroLog);
                    this.RevertTransform = new TransformFunc(Math.Exp);
                }
                else if (value == ScaleType.SquareRoot)
                {
                    this.Transform = new TransformFunc(this.ZeroSqrt);
                    this.RevertTransform = new TransformFunc(this.Sqr);
                }
            }
        }
    }
}

