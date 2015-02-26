namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class XFormatCollection : CollectionBase
    {
        public virtual void Add(FormulaXFormat fxf)
        {
            base.List.Add(fxf);
        }

        public static XFormatCollection Default
        {
            get
            {
                XFormatCollection formats = new XFormatCollection();
                formats.Add(new FormulaXFormat(0.001, "MINUTE", "{MMMdd }H:mm", double.NaN, "yyyy-MM-dd HH:mm:ss", AxisLabelAlign.TickCenter));
                formats.Add(new FormulaXFormat(0.005, "MINUTE5", "{MMMdd }H:mm", double.NaN, "yyyy-MM-dd HH:mm:ss", AxisLabelAlign.TickCenter));
                formats.Add(new FormulaXFormat(0.01, "MINUTE10", "{MMMdd }H:mm", double.NaN, "yyyy-MM-dd HH:mm:ss", AxisLabelAlign.TickCenter));
                formats.Add(new FormulaXFormat(0.03, "MINUTE30", "{MMMdd }H:mm", double.NaN, "yyyy-MM-dd HH:mm:ss", AxisLabelAlign.TickCenter));
                formats.Add(new FormulaXFormat(0.06, "HOUR", "{MMMdd }H:mm", double.NaN, "yyyy-MM-dd HH:mm:ss", AxisLabelAlign.TickCenter));
                formats.Add(new FormulaXFormat(0.2, "HOUR2", "HH:mm", double.NaN, "yyyy-MM-dd HH:mm:ss", AxisLabelAlign.TickCenter));
                formats.Add(new FormulaXFormat(0.8, "DAY", "MMM-dd ddd"));
                formats.Add(new FormulaXFormat(3.0, "DAY", "{MMM}dd"));
                formats.Add(new FormulaXFormat(9.0, "DAY", "{MMM}dd", 0.6));
                formats.Add(new FormulaXFormat(14.0, "DAY20", "{yyyy}MMMdd"));
                formats.Add(new FormulaXFormat(40.0, "MONTH", "{yyyy}MMM"));
                formats.Add(new FormulaXFormat(80.0, "MONTH2", "{yyyy}MMM"));
                formats.Add(new FormulaXFormat(160.0, "MONTH4", "{yyyy}MMM"));
                formats.Add(new FormulaXFormat(570.0, "YEAR1", "yyyy"));
                formats.Add(new FormulaXFormat(1100.0, "YEAR2", "yyyy"));
                formats.Add(new FormulaXFormat(2147483647.0, "YEAR", "yyyy", 500.0));
                return formats;
            }
        }

        public virtual FormulaXFormat this[int Index]
        {
            get
            {
                return (FormulaXFormat) base.List[Index];
            }
        }

        public static XFormatCollection TwoAxisX
        {
            get
            {
                XFormatCollection formats = new XFormatCollection();
                bool[] visible = new bool[2];
                visible[0] = true;
                bool[] showMajorLine = new bool[2];
                showMajorLine[0] = true;
                bool[] showMinorLine = new bool[2];
                formats.Add(new FormulaXFormat(0.001, "MINUTE", "{MMMdd }H:mm", double.NaN, visible, showMajorLine, showMinorLine));
                bool[] flagArray4 = new bool[2];
                flagArray4[0] = true;
                bool[] flagArray5 = new bool[2];
                flagArray5[0] = true;
                bool[] flagArray6 = new bool[2];
                formats.Add(new FormulaXFormat(0.005, "MINUTE5", "{MMMdd }H:mm", double.NaN, flagArray4, flagArray5, flagArray6));
                bool[] flagArray7 = new bool[2];
                flagArray7[0] = true;
                bool[] flagArray8 = new bool[2];
                flagArray8[0] = true;
                bool[] flagArray9 = new bool[2];
                formats.Add(new FormulaXFormat(0.01, "MINUTE10", "{MMMdd }H:mm", double.NaN, flagArray7, flagArray8, flagArray9));
                bool[] flagArray10 = new bool[2];
                flagArray10[0] = true;
                bool[] flagArray11 = new bool[2];
                flagArray11[0] = true;
                bool[] flagArray12 = new bool[2];
                formats.Add(new FormulaXFormat(0.03, "MINUTE30", "{MMMdd }H:mm", double.NaN, flagArray10, flagArray11, flagArray12));
                bool[] flagArray13 = new bool[2];
                flagArray13[0] = true;
                bool[] flagArray14 = new bool[2];
                flagArray14[0] = true;
                bool[] flagArray15 = new bool[2];
                formats.Add(new FormulaXFormat(0.06, "HOUR", "{MMMdd }H:mm", double.NaN, flagArray13, flagArray14, flagArray15));
                bool[] flagArray16 = new bool[2];
                flagArray16[0] = true;
                bool[] flagArray17 = new bool[2];
                flagArray17[0] = true;
                bool[] flagArray18 = new bool[2];
                formats.Add(new FormulaXFormat(0.3, "HOUR2", "h:mm", double.NaN, flagArray16, flagArray17, flagArray18));
                bool[] flagArray21 = new bool[2];
                flagArray21[0] = true;
                formats.Add(new FormulaXFormat(5.0, "DAY", "%d", double.NaN, new bool[] { true, true }, new bool[] { true, true }, flagArray21));
                bool[] flagArray24 = new bool[2];
                flagArray24[0] = true;
                formats.Add(new FormulaXFormat(10.0, "WEEK", "%d", double.NaN, new bool[] { true, true }, new bool[] { true, true }, flagArray24));
                bool[] flagArray27 = new bool[2];
                flagArray27[0] = true;
                formats.Add(new FormulaXFormat(40.0, "WEEK2", "%d", double.NaN, new bool[] { true, true }, new bool[] { true, true }, flagArray27));
                bool[] flagArray28 = new bool[2];
                flagArray28[0] = true;
                bool[] flagArray29 = new bool[2];
                flagArray29[0] = true;
                bool[] flagArray30 = new bool[2];
                formats.Add(new FormulaXFormat(80.0, "MONTH2", "{yyyy}MMM", double.NaN, flagArray28, flagArray29, flagArray30));
                bool[] flagArray31 = new bool[2];
                flagArray31[0] = true;
                bool[] flagArray32 = new bool[2];
                flagArray32[0] = true;
                bool[] flagArray33 = new bool[2];
                formats.Add(new FormulaXFormat(160.0, "MONTH4", "{yyyy}MMM", double.NaN, flagArray31, flagArray32, flagArray33));
                bool[] flagArray34 = new bool[2];
                flagArray34[0] = true;
                bool[] flagArray35 = new bool[2];
                flagArray35[0] = true;
                bool[] flagArray36 = new bool[2];
                formats.Add(new FormulaXFormat(570.0, "YEAR1", "yyyy", double.NaN, flagArray34, flagArray35, flagArray36));
                bool[] flagArray37 = new bool[2];
                flagArray37[0] = true;
                bool[] flagArray38 = new bool[2];
                flagArray38[0] = true;
                bool[] flagArray39 = new bool[2];
                formats.Add(new FormulaXFormat(1100.0, "YEAR2", "yyyy", double.NaN, flagArray37, flagArray38, flagArray39));
                bool[] flagArray40 = new bool[2];
                flagArray40[0] = true;
                bool[] flagArray41 = new bool[2];
                flagArray41[0] = true;
                bool[] flagArray42 = new bool[2];
                formats.Add(new FormulaXFormat(2147483647.0, "YEAR", "yyyy", 500.0, flagArray40, flagArray41, flagArray42));
                return formats;
            }
        }
    }
}

