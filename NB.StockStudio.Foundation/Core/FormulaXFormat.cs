namespace NB.StockStudio.Foundation
{
    using System;

    public class FormulaXFormat
    {
        private AxisLabelAlign axisLabelAlign;
        private double cycleDivide;
        private double days100Pixel;
        private DataCycle interval;
        private bool[] showMajorLine;
        private bool[] showMinorLine;
        private bool[] visible;
        private string xCursorFormat;
        private string xFormat;

        public FormulaXFormat(double Days100Pixel, string Interval, string XFormat)
        {
            this.xCursorFormat = "yyyy-MM-dd dddd";
            this.days100Pixel = Days100Pixel;
            this.interval = DataCycle.Parse(Interval);
            this.xFormat = XFormat;
        }

        public FormulaXFormat(double Days100Pixel, string Interval, string XFormat, double CycleDivide) : this(Days100Pixel, Interval, XFormat)
        {
            this.cycleDivide = CycleDivide;
        }

        public FormulaXFormat(double Days100Pixel, string Interval, string XFormat, double CycleDivide, string XCursorFormat) : this(Days100Pixel, Interval, XFormat, CycleDivide)
        {
            this.xCursorFormat = XCursorFormat;
        }

        public FormulaXFormat(double Days100Pixel, string Interval, string XFormat, double CycleDivide, string XCursorFormat, AxisLabelAlign AxisLabelAlign) : this(Days100Pixel, Interval, XFormat, CycleDivide, XCursorFormat)
        {
            this.axisLabelAlign = AxisLabelAlign;
        }

        public FormulaXFormat(double Days100Pixel, string Interval, string XFormat, double CycleDivide, bool[] Visible, bool[] ShowMajorLine, bool[] ShowMinorLine) : this(Days100Pixel, Interval, XFormat, CycleDivide)
        {
            this.visible = Visible;
            this.showMajorLine = ShowMajorLine;
            this.showMinorLine = ShowMinorLine;
        }

        public void SetMajorLine(FormulaChart fc)
        {
            for (int i = 0; i < this.ShowMajorLine.Length; i++)
            {
                fc.SetAxisXShowMajorLine(i, this.ShowMajorLine[i]);
            }
        }

        public void SetMinorLine(FormulaChart fc)
        {
            for (int i = 0; i < this.ShowMinorLine.Length; i++)
            {
                fc.SetAxisXShowMinorLine(i, this.ShowMinorLine[i]);
            }
        }

        public void SetVisible(FormulaChart fc)
        {
            for (int i = 0; i < this.Visible.Length; i++)
            {
                fc.SetAxisXVisible(i, this.Visible[i]);
            }
        }

        public AxisLabelAlign AxisLabelAlign
        {
            get
            {
                return this.axisLabelAlign;
            }
            set
            {
                this.axisLabelAlign = value;
            }
        }

        public double CycleDivide
        {
            get
            {
                return this.cycleDivide;
            }
            set
            {
                this.cycleDivide = value;
            }
        }

        public double Days100Pixel
        {
            get
            {
                return this.days100Pixel;
            }
            set
            {
                this.days100Pixel = value;
            }
        }

        public DataCycle Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        public bool[] ShowMajorLine
        {
            get
            {
                return this.showMajorLine;
            }
            set
            {
                this.showMajorLine = value;
            }
        }

        public bool[] ShowMinorLine
        {
            get
            {
                return this.showMinorLine;
            }
            set
            {
                this.showMinorLine = value;
            }
        }

        public bool[] Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }

        public string XCursorFormat
        {
            get
            {
                return this.xCursorFormat;
            }
            set
            {
                this.xCursorFormat = value;
            }
        }

        public string XFormat
        {
            get
            {
                return this.xFormat;
            }
            set
            {
                this.xFormat = value;
            }
        }
    }
}

