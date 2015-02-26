namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class SingleLineObject : LineGroupTextObject
    {
        private string dataFormat = "f2";
        private SingleLineType lineType;

        public SingleLineObject()
        {
            base.ObjectFont.TextFont = new Font(base.ObjectFont.TextFont, FontStyle.Italic | FontStyle.Bold);
            base.ObjectFont.TextBrush.Color = Color.Red;
            base.SmoothingMode = ObjectSmoothingMode.Default;
        }

        public override void CalcPoint()
        {
            base.pfStart = new PointF[1];
            base.pfEnd = new PointF[1];
            base.pfStart[0] = base.ToPointF(base.ControlPoints[0]);
            base.pfEnd[0] = base.pfStart[0];
            if (this.lineType == SingleLineType.Horizontal)
            {
                base.pfEnd[0].X++;
            }
            else
            {
                base.pfEnd[0].Y++;
            }
            base.ExpandLine2(ref base.pfStart[0], ref base.pfEnd[0]);
        }

        public override string GetStr()
        {
            if (this.lineType == SingleLineType.Horizontal)
            {
                PointF tf = base.ToPointF(base.ControlPoints[0]);
                return base.Area.AxisY.GetValueFromY(tf.Y).ToString(this.DataFormat);
            }
            double[] dd = base.Manager.Canvas.BackChart.DataProvider["DATE"];
            double d = dd[FormulaChart.FindIndex(dd, base.ControlPoints[0].X)];
            return DateTime.FromOADate(d).ToString(this.DataFormat);
        }

        public override RectangleF GetTextRect()
        {
            return base.GetTextRect(0, this.lineType == SingleLineType.Horizontal);
        }

        public void Horizontal()
        {
            this.LineType = SingleLineType.Horizontal;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Vertical line", typeof(SingleLineObject), "Vertical", "Line", "VLine"), new ObjectInit("Horizontal line", typeof(SingleLineObject), "Horizontal", "Line", "HLine") };
        }

        public void Vertical()
        {
            this.LineType = SingleLineType.Vertical;
        }

        public override int ControlPointNum
        {
            get
            {
                return 1;
            }
        }

        public string DataFormat
        {
            get
            {
                return this.dataFormat;
            }
            set
            {
                this.dataFormat = value;
            }
        }

        public override int InitNum
        {
            get
            {
                return 1;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public SingleLineType LineType
        {
            get
            {
                return this.lineType;
            }
            set
            {
                this.lineType = value;
                if (value == SingleLineType.Horizontal)
                {
                    this.dataFormat = "f2";
                    base.ObjectFont.Alignment = StringAlignment.Near;
                }
                else
                {
                    this.dataFormat = "yyyy-MM-dd";
                    base.ObjectFont.Alignment = StringAlignment.Far;
                }
            }
        }
    }
}

