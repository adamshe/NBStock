namespace NB.StockStudio.Foundation
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FormulaTick : DeepClone
    {
        public int Count = 4;
        public DataCycle DataCycle;
        public IFormatProvider DateFormatProvider;
        public string Format;
        public bool FullTick = false;
        public bool Inside = false;
        public Pen LinePen = new Pen(Color.DarkGray);
        public int MinimumPixel;
        public bool ShowLine;
        public bool ShowText = true;
        public bool ShowTick = true;
        public Pen TickPen = new Pen(Color.DarkGray);
        public int TickWidth = 5;
        public bool Visible = true;

        public FormulaTick()
        {
            this.LinePen.DashStyle = DashStyle.Dot;
        }

        public void DrawXAxisTick(FormulaCanvas Canvas, double[] Date, FormulaData fdDate, PointF[] pfs, FormulaAxisX fax, ExchangeIntraday ei)
        {
            if (this.DataCycle != null)
            {
                int num = 0;
                int sequence = 0;
                string str = "";
                int num3 = -10000;
                int num4 = -1;
                Graphics currentGraph = Canvas.CurrentGraph;
                double num5 = 0.0;
                if (Date.Length > 0)
                {
                    num5 = Date[Date.Length - 1] - Date[0];
                }
                double[] numArray = Date;
                if ((ei != null) && ei.NativeCycle)
                {
                    numArray = new double[Date.Length];
                    for (int j = 0; j < numArray.Length; j++)
                    {
                        numArray[j] = ((int) Date[j]) + ei.OneDayTime(Date[j]);
                    }
                }
                for (int i = pfs.Length - 1; i >= 0; i--)
                {
                    int index = ((Date.Length - 1) - Canvas.Start) - i;
                    double d = Date[index];
                    DateTime time = DateTime.FromOADate(d);
                    sequence = this.DataCycle.GetSequence(numArray[index]);
                    if (sequence != num)
                    {
                        PointF tf = pfs[i];
                        num = sequence;
                        if (this.ShowLine)
                        {
                            int bottom = Canvas.Rect.Bottom;
                            if (!fax.Visible)
                            {
                                bottom = Canvas.FrameRect.Bottom;
                            }
                            currentGraph.DrawLine(this.LinePen, tf.X, (float) Canvas.FrameRect.Top, tf.X, (float) bottom);
                        }
                        if (fax.Visible && this.ShowTick)
                        {
                            int tickWidth = this.TickWidth;
                            if (this.FullTick)
                            {
                                tickWidth = fax.Rect.Height;
                            }
                            if (this.Inside)
                            {
                                tickWidth = -tickWidth;
                            }
                            currentGraph.DrawLine(this.TickPen, tf.X, (float) fax.Rect.Top, tf.X, (float) (tickWidth + fax.Rect.Top));
                        }
                        string text = time.ToString(this.Format, this.DateFormatProvider);
                        int startIndex = text.IndexOf('{');
                        int num13 = text.IndexOf('}');
                        if (num13 > startIndex)
                        {
                            string str2 = text.Substring(startIndex + 1, (num13 - startIndex) - 1);
                            if (str2 != str)
                            {
                                text = text.Remove(startIndex, 1).Remove(num13 - 1, 1);
                            }
                            else
                            {
                                text = text.Substring(0, startIndex) + text.Substring(num13 + 1);
                            }
                            str = str2;
                        }
                        Font labelFont = fax.LabelFont;
                        float width = currentGraph.MeasureString(text, labelFont).Width;
                        float x = tf.X;
                        switch (fax.AxisLabelAlign)
                        {
                            case AxisLabelAlign.TickCenter:
                                x -= width / 2f;
                                break;

                            case AxisLabelAlign.TickLeft:
                                x -= width;
                                break;
                        }
                        if (x < fax.Rect.Left)
                        {
                            x = fax.Rect.Left;
                        }
                        if ((fax.Visible && this.ShowText) && ((num3 + this.MinimumPixel) < x))
                        {
                            if (((ei == null) || ei.ShowFirstXLabel) || ((i < (pfs.Length - 1)) || (num5 > 1.0)))
                            {
                                currentGraph.DrawString(text, labelFont, fax.LabelBrush, x, (float) fax.Rect.Top);
                                num3 = (int) (x + width);
                            }
                            num4 = i;
                        }
                    }
                }
            }
        }
    }
}

