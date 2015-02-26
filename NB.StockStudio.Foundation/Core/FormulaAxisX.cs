namespace NB.StockStudio.Foundation
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FormulaAxisX
    {
        public bool AutoScale;
        public AxisLabelAlign AxisLabelAlign;
        public FormulaBack Back = new FormulaBack();
        public string CursorFormat = "dd-MMM-yyyy dddd";
        public DataCycle DataCycle = DataCycle.Month();
        public IFormatProvider DateFormatProvider;
        public DateTime EndTime = DateTime.MaxValue;
        private FormulaData fdDate;
        public string Format = "yyMMM";
        public int Height = 0x10;
        public ExchangeIntraday IntradayInfo;
        public Brush LabelBrush = Brushes.Black;
        public Font LabelFont = new Font("verdana", 8f);
        public RectangleF LastCursorRect;
        public FormulaTick MajorTick = new FormulaTick();
        public FormulaTick MinorTick;
        private double OneDay = -1.0;
        public Rectangle Rect;
        public DateTime StartTime = DateTime.MinValue;
        private double Total = -1.0;
        public bool Visible = true;

        public FormulaAxisX()
        {
            this.MajorTick.ShowLine = true;
            this.MajorTick.Inside = true;
            this.MinorTick = new FormulaTick();
            this.MinorTick.Count = 1;
            this.MinorTick.Inside = true;
            this.MinorTick.TickWidth = 2;
        }

        public void CopyFrom(FormulaAxisX fax)
        {
            this.Visible = fax.Visible;
            this.DateFormatProvider = fax.DateFormatProvider;
            this.AutoScale = fax.AutoScale;
            this.Format = fax.Format;
            this.MajorTick = (FormulaTick) fax.MajorTick.Clone();
            this.MinorTick = (FormulaTick) fax.MinorTick.Clone();
            this.Back = (FormulaBack) fax.Back.Clone();
            this.LabelFont = (Font) fax.LabelFont.Clone();
            this.LabelBrush = (Brush) fax.LabelBrush.Clone();
            this.AxisLabelAlign = fax.AxisLabelAlign;
            this.DataCycle = fax.DataCycle;
        }

        public void DrawCursor(Graphics g, FormulaChart fc, FormulaArea Area, float X)
        {
            if (!this.LastCursorRect.IsEmpty)
            {
                fc.RestoreMemBmp(g, this.LastCursorRect);
            }
            FormulaLabel label = Area.Labels[2];
            int cursorPos = fc.CursorPos;
            if ((Object)fdDate != null && cursorPos >= 0) 
            {
                if (cursorPos < this.fdDate.Length)
                {
                    string text = DateTime.FromOADate(this.fdDate[cursorPos]).ToString(this.CursorFormat,
                                                                                       DateTimeFormatInfo.InvariantInfo);
                    SizeF ef = g.MeasureString(text, this.LabelFont);
                    RectangleF rect = new RectangleF(X - fc.Rect.X, (float) this.Rect.Y, ef.Width,
                                                     (float) (this.Rect.Height - 1));
                    this.LastCursorRect = rect;
                    this.LastCursorRect.Inflate(2f, 1f);
                    rect.Offset((PointF) fc.Rect.Location);
                    label.DrawString(g, text, this.LabelFont, label.TextBrush, VerticalAlign.Bottom, FormulaAlign.Left,
                                     rect, false);
                }
            }
        }

        private double FromStartTime(double D)
        {
            if (this.OneDay == -1.0)
            {
                this.OneDay = this.IntradayInfo.GetOpenTimePerDay();
            }
            double d = this.StartTime.ToOADate();
            if (((int) d) == ((int) D))
            {
                return (this.IntradayInfo.OneDayTime(D) - this.IntradayInfo.OneDayTime(d));
            }
            return (((this.OneDay - this.IntradayInfo.OneDayTime(d)) + this.IntradayInfo.OneDayTime(D)) + (((((int) D) - ((int) d)) - 1) * this.OneDay));
        }

        public float GetX(double D, int x1, int x2)
        {
            if (this.Total == -1.0)
            {
                this.Total = this.FromStartTime(this.EndTime.ToOADate());
            }
            double num = this.FromStartTime(D);
            return (float) (x1 + ((((double) (x2 - x1)) / this.Total) * num));
        }

        public void Render(FormulaCanvas Canvas, FormulaArea fa)
        {
            if (this.Visible)
            {
                if (fa.AxisY.AxisPos == AxisPos.Left)
                {
                    this.Rect.X--;
                }
                Graphics currentGraph = Canvas.CurrentGraph;
                this.Back.Render(currentGraph, this.Rect);
            }
            if (this.MajorTick.Visible || this.MinorTick.Visible)
            {
                double[] data = fa.Parent.DataProvider["DATE"];
                this.fdDate = new FormulaData(data);
                this.fdDate.Canvas = Canvas;
                this.fdDate.AxisY = fa.AxisY;
                PointF[] points = this.fdDate.GetPoints();
                this.MajorTick.DataCycle = this.DataCycle;
                this.MajorTick.Format = this.Format;
                this.MajorTick.DateFormatProvider = this.DateFormatProvider;
                this.MajorTick.DrawXAxisTick(Canvas, data, this.fdDate, points, this, this.IntradayInfo);
                this.MinorTick.DrawXAxisTick(Canvas, data, this.fdDate, points, this, this.IntradayInfo);
            }
        }

        public bool IsFixedTime
        {
            get
            {
                return (this.StartTime > DateTime.MinValue);
            }
        }
    }
}

