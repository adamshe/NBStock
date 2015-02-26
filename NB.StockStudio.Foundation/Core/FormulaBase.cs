using System.ComponentModel;

namespace NB.StockStudio.Foundation
{
    using NB.StockStudio.Foundation.DataProvider;
    using FML.NATIVE;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class FormulaBase
    {
        public IDataProvider DataProvider;
        public static int DefaultTestCount = 5;
        public static int DMATestCount = 200;
        protected const bool FALSE = false;
        public static int MaxTestCount = 100;
        public string Name;
        protected const double NAN = double.NaN;
        public ParamCollection Params;
        public string Quote;
        public bool ShowParameter;
        public static SortedList SupportedAssemblies = new SortedList();
        public static bool Testing = false;
        public bool TextInvisible;
        protected const bool TRUE = true;
        public static int ZigTestCount = 200;

        public FormulaBase()
        {
            this.ShowParameter = true;
            this.Params = new ParamCollection();
            this.Name = base.GetType().Name;
        }

        public FormulaBase(IDataProvider DataProvider) : this()
        {
            this.DataProvider = DataProvider;
        }

        public static FormulaData ABS(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Abs(f.Data[i]);
            }
            return data;
        }

        public static double ABS(double d)
        {
            return Math.Abs(d);
        }

        public static FormulaData ACOS(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Acos(f.Data[i]);
            }
            return data;
        }

        public void AddParam(string ParamName, double DefaultValue, double MinValue, double MaxValue)
        {
            this.AddParam(ParamName, DefaultValue.ToString(), MinValue.ToString(), MaxValue.ToString(), FormulaParamType.Double);
        }

        public void AddParam(string ParamName, string DefaultValue, string MinValue, string MaxValue)
        {
            this.AddParam(ParamName, DefaultValue, MinValue, MaxValue, FormulaParamType.String);
        }

        public void AddParam(string ParamName, string DefaultValue, string MinValue, string MaxValue, FormulaParamType ParamType)
        {
            FormulaParam fp = new FormulaParam(ParamName, DefaultValue, MinValue, MaxValue, ParamType);
            this.SetParam(fp, DefaultValue);
            this.Params.Add(fp);
        }

        public FormulaData AdjustDateTime(double[] Date, double[] Data)
        {
            return this.AdjustDateTime(this.DataProvider["DATE"], Date, Data);
        }

        public FormulaData AdjustDateTime(IDataProvider dp, FormulaData f)
        {
            if (((this.DataProvider == null) || (f.FormulaType == FormulaType.Const)) || (((dp != null) && (dp == this.DataProvider)) && (dp.DataCycle.ToString() == this.DataProvider.DataCycle.ToString())))
            {
                return f;
            }
            return this.AdjustDateTime(dp["DATE"], f.Data);
        }

        public FormulaData AdjustDateTime(double[] D1, double[] D2, double[] Data)
        {
            if ((D1 == null) || (D2 == null))
            {
                return Data;
            }
            FormulaData data = new FormulaData(D1.Length);
            int index = 0;
            int num2 = 0;
            while ((index < D1.Length) && (num2 < D2.Length))
            {
                if (D1[index] < D2[num2])
                {
                    if (index > 0)
                    {
                        data[index] = data[index - 1];
                    }
                    else
                    {
                        data[index] = double.NaN;
                    }
                    index++;
                }
                else if (D1[index] > D2[num2])
                {
                    if ((index > 0) && (D1[index - 1] < D2[num2]))
                    {
                        data[index - 1] = Data[num2];
                    }
                    num2++;
                }
                else
                {
                    data[index] = Data[num2];
                    index++;
                    num2++;
                }
            }
            while (index < D1.Length)
            {
                data[index] = double.NaN;
                index++;
            }
            return data;
        }

        public static FormulaData ASIN(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Asin(f.Data[i]);
            }
            return data;
        }

        public FormulaData ASKPRICE(int N)
        {
            return this.DataProvider["ASKPRICE" + N];
        }

        public FormulaData ASKVOL(int N)
        {
            return this.DataProvider["ASKVOL" + N];
        }

        public static FormulaData ATAN(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Atan(f.Data[i]);
            }
            return data;
        }

        public static FormulaData AVEDEV(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            FormulaData data2 = MA(f, N);
            for (int i = 0; (i < (n - 1)) && (i < data.Length); i++)
            {
                data.Data[i] = double.NaN;
            }
            for (int j = n - 1; j < f.Length; j++)
            {
                double num4 = 0.0;
                for (int k = (j - n) + 1; k <= j; k++)
                {
                    num4 += Math.Abs((double) (f.Data[k] - data2.Data[j]));
                }
                data.Data[j] = num4 / ((double) n);
            }
            return data;
        }

        public static FormulaData BACKSET(FormulaData f, double N)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            data.Set(double.NaN);
            for (int i = 0; i < f.Length; i++)
            {
                if (f.Data[i] != 0.0)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if ((i - j) >= 0)
                        {
                            data.Data[i - j] = 1.0;
                        }
                    }
                }
            }
            return data;
        }

        public static FormulaData BARSCOUNT(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(DefaultTestCount, new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = i;
            }
            return data;
        }

        public static FormulaData BARSLAST(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(DefaultTestCount, new FormulaData[] { f });
            }
            int num = -1;
            for (int i = 0; i < f.Length; i++)
            {
                if (f.Data[i] != 0.0)
                {
                    num = i;
                }
                if (num < 0)
                {
                    data.Data[i] = 0.0;
                }
                else
                {
                    data.Data[i] = i - num;
                }
            }
            return data;
        }

        public static FormulaData BARSSINCE(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(DefaultTestCount, new FormulaData[] { f });
            }
            int num = -1;
            for (int i = 0; i < f.Length; i++)
            {
                if ((!double.IsNaN(f.Data[i]) && (f.Data[i] != 0.0)) && (num < 0))
                {
                    num = i;
                }
                if (num < 0)
                {
                    data.Data[i] = 0.0;
                }
                else
                {
                    data.Data[i] = (i - num) + 1;
                }
            }
            return data;
        }

        private bool BasicFormula(string s)
        {
            int index = s.IndexOf('#');
            if (index > 0)
            {
                s = s.Substring(0, index);
            }
            s = s.ToUpper();
            return ((((((s == "C") || (s == "O")) || ((s == "H") || (s == "L"))) || (((s == "V") || (s == "OPEN")) || ((s == "CLOSE") || (s == "HIGH")))) || (((s == "LOW") || (s == "VOL")) || (s == "VOLUME"))) || (s == "DATE"));
        }

        public static FormulaData BETWEEN(FormulaData f1, FormulaData f2, FormulaData f3)
        {
            FormulaData.MakeSameLength(new FormulaData[] { f1, f2, f3 });
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f1, f2, f3 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                double num2 = f2.Data[i];
                double num3 = f3.Data[i];
                if (num2 > num3)
                {
                    double num4 = num2;
                    num3 = num2;
                    num2 = num4;
                }
                if ((f1.Data[i] >= num2) && (f1.Data[i] <= num3))
                {
                    data.Data[i] = 1.0;
                }
            }
            return data;
        }

        public FormulaData BIDPRICE(int N)
        {
            return this.DataProvider["BIDPRICE" + N];
        }

        public FormulaData BIDVOL(int N)
        {
            return this.DataProvider["BIDVOL" + N];
        }

        public void BindFormulaCycle(IDataProvider dp, ref string FormulaName)
        {
            int index = FormulaName.IndexOf('#');
            if ((index > 0) && (dp != null))
            {
                string s = FormulaName.Substring(index + 1);
                dp.DataCycle = DataCycle.Parse(s);
                FormulaName = FormulaName.Substring(0, index);
            }
            else if ((this.DataProvider != null) && (dp != null))
            {
                dp.DataCycle = this.DataProvider.DataCycle;
            }
        }

        public FormulaData BUYVOL(int N)
        {
            return this.DataProvider["BUYVOL" + N];
        }

        public static void CalcLinearRegression(FormulaData f, int Start, int Count, out double a, out double b)
        {
            if (Start < (Count - 1))
            {
                a = double.NaN;
                b = double.NaN;
            }
            else
            {
                double num = (Count - 1.0) / 2.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                for (int i = 0; i < Count; i++)
                {
                    int index = ((Start - Count) + 1) + i;
                    num2 += i * f.Data[index];
                    num3 += i * i;
                    num4 += f.Data[index];
                }
                num4 /= (double) Count;
                b = (num2 - ((Count * num) * num4)) / (num3 - ((Count * num) * num));
                a = num4 - (b * num);
            }
        }

        private static FormulaData CalLine(FormulaData f, double N, bool ForCast)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            if (n <= 1)
            {
                throw new Exception("Invalid parameter");
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = n - 1; i < f.Length; i++)
            {
                double num3;
                double num4;
                CalcLinearRegression(f, i, n, out num4, out num3);
                if (ForCast)
                {
                    data.Data[i] = num4 + (num3 * ((i - n) + 1));
                }
                else
                {
                    data.Data[i] = num3;
                }
            }
            return data;
        }

        public static FormulaData CEILING(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Ceiling(f.Data[i]);
            }
            return data;
        }

        public static FormulaData CORR(FormulaData f1, FormulaData f2, double N)
        {
            int num = (int) N;
            FormulaData.MakeSameLength(f1, f2);
            double avg = f1.Avg;
            double num3 = f2.Avg;
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            for (int i = 0; i < f1.Length; i++)
            {
                num4 += (f1[i] - avg) * (f2[i] - num3);
                num5 += (f1[i] - avg) * (f1[i] - avg);
                num6 += (f2[i] - num3) * (f2[i] - num3);
            }
            return (num4 / Math.Sqrt(num5 * num6));
        }

        public static FormulaData COS(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Cos(f.Data[i]);
            }
            return data;
        }

        public FormulaData COST(double N)
        {
            FormulaData h = this.H;
            FormulaData l = this.L;
            FormulaData v = this.V;
            FormulaData data4 = new FormulaData(this.H.Length);
            double num = 0.0;
            SortedList list = new SortedList();
            for (int i = 0; i < this.H.Length; i++)
            {
                double num3 = (v.Data[i] / ((h.Data[i] - l.Data[i]) + 1.0)) / 10.0;
                num += v.Data[i];
                for (double j = l.Data[i]; j <= h.Data[i]; j += 0.1)
                {
                    double num5 = Math.Round(j, 1);
                    if (list[num5] == null)
                    {
                        list[num5] = num3;
                    }
                    else
                    {
                        list[num5] = ((double) list[num5]) + num3;
                    }
                }
                double num6 = 0.0;
                for (int k = 0; k < list.Count; k++)
                {
                    if ((num6 / num) > (N / 100.0))
                    {
                        data4.Data[i] = (double) list.GetKey(k);
                        break;
                    }
                    num6 += (double) list.GetByIndex(k);
                }
            }
            return data4;
        }

        public static FormulaData COUNT(FormulaData f, double N)
        {
            return SUM((FormulaData) (f > 0.0), N);
        }

        public static FormulaData CROSS(FormulaData f1, FormulaData f2)
        {
            return LONGCROSS(f1, f2, 1.0);
        }

        public int DataCountAtLeast()
        {
            int length;
            IDataProvider dp = new RandomDataProvider(0x3e8);
            Testing = true;
            try
            {
                FormulaPackage package = this.Run(dp);
                FormulaData data = package[package.Count - 1];
                for (int i = 0; i < data.Length; i++)
                {
                    if (!double.IsNaN(data[i]))
                    {
                        return (i + 1);
                    }
                }
                length = data.Length;
            }
            finally
            {
                this.DataProvider = null;
                Testing = false;
            }
            return length;
        }

        public FormulaData DATADIFF(FormulaData f1, FormulaData f2)
        {
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                TimeSpan span = (TimeSpan) (DateTime.FromOADate(f1.Data[i]) - DateTime.FromOADate(f2.Data[i]));
                data.Data[i] = span.Days;
            }
            return data;
        }

        public static FormulaData DEVSQ(FormulaData f, double N)
        {
            return (VARP(f, N) * N);
        }

        public static FormulaData DMA(FormulaData f, double A)
        {
            return DMA(f, A, 0);
        }

        public static FormulaData DMA(FormulaData f, double A, int Start)
        {
            if (Testing)
            {
                return TestData(Start + DMATestCount, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            double num = 0.0;
            for (int i = 0; (i < Start) && (i < f.Length); i++)
            {
                num += f[i];
                data[i] = double.NaN;
            }
            if ((Start > 0) && (Start <= f.Length))
            {
                data[Start - 1] = num / ((double) Start);
            }
            for (int j = Start; j < f.Length; j++)
            {
                if (j > 0)
                {
                    if (!double.IsNaN(f.Data[j]))
                    {
                        if (!double.IsNaN(data.Data[j - 1]))
                        {
                            data.Data[j] = (f.Data[j] * A) + (data.Data[j - 1] * (1.0 - A));
                        }
                        else
                        {
                            data.Data[j] = f.Data[j];
                        }
                    }
                    else
                    {
                        data.Data[j] = double.NaN;
                    }
                }
                else
                {
                    data.Data[j] = f.Data[j];
                }
            }
            return data;
        }

        public FormulaData DRAWAXISY(FormulaData f, double start, double end)
        {
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.AXISY;
            data.OwnerData["START"] = start;
            data.OwnerData["END"] = end;
            return data;
        }

        public FormulaData DRAWICON(FormulaData Cond, FormulaData f, double IconIndex)
        {
            FormulaData.MakeSameLength(Cond, f);
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.ICON;
            data["COND"] = Cond.Data;
            data.OwnerData["ICON"] = IconIndex;
            data.TextInvisible = true;
            return data;
        }

        public FormulaData DRAWLINE(FormulaData f1, FormulaData f2, FormulaData f3, FormulaData f4, double Expand)
        {
            FormulaData.MakeSameLength(new FormulaData[] { f1, f2, f3, f4 });
            FormulaData data = new FormulaData(f2);
            data.RenderType = FormulaRenderType.LINE;
            data["COND"] = f1.Data;
            data["COND2"] = f3.Data;
            data["PRICE2"] = f4.Data;
            data.OwnerData["EXPAND"] = Expand;
            return data;
        }

        public FormulaData DRAWNUMBER(FormulaData Cond, FormulaData f1, FormulaData f2, string Format)
        {
            FormulaData.MakeSameLength(new FormulaData[] { Cond, f1, f2 });
            FormulaData data = new FormulaData(f1);
            data.RenderType = FormulaRenderType.TEXT;
            data["COND"] = Cond.Data;
            data["NUMBER"] = f2.Data;
            data.OwnerData["FORMAT"] = Format;
            data.TextInvisible = true;
            return data;
        }

        public FormulaData DRAWTEXT(FormulaData Cond, FormulaData f, string Text)
        {
            FormulaData.MakeSameLength(Cond, f);
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.TEXT;
            data["COND"] = Cond.Data;
            data.OwnerData["TEXT"] = Text;
            data.TextInvisible = true;
            return data;
        }

        public FormulaData DRAWTEXTAXISY(FormulaData f, double Text, double start)
        {
            return this.DRAWTEXTAXISY(f, Text.ToString(), start);
        }

        public FormulaData DRAWTEXTAXISY(FormulaData f, string Text, double start)
        {
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.AXISYTEXT;
            data.OwnerData["TEXT"] = Text;
            data.OwnerData["START"] = start;
            data.TextInvisible = true;
            return data;
        }

        public double DYNAINFO(double N)
        {
            return this.DataProvider.GetConstData("DYNAINFO" + N);
        }

        public static FormulaData EMA(FormulaData f, double N)
        {
            if (Testing)
            {
                return TestData(((int) N) + DMATestCount, new FormulaData[] { f });
            }
            return DMA(f, 2.0 / (N + 1.0));
        }

        public static FormulaData EVERY(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                double num3 = 1.0;
                for (int j = i; j >= Math.Max(0, (i - n) + 1); j--)
                {
                    if (f.Data[j] == 0.0)
                    {
                        num3 = 0.0;
                        break;
                    }
                }
                f.Data[i] = num3;
            }
            return data;
        }

        public static FormulaData EXIST(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                double num3 = 0.0;
                for (int j = i; j >= Math.Max(0, (i - n) + 1); j--)
                {
                    if (f.Data[j] > 0.0)
                    {
                        num3 = 1.0;
                        break;
                    }
                }
                data.Data[i] = num3;
            }
            return data;
        }

        public static FormulaData EXP(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Exp(f.Data[i]);
            }
            return data;
        }

        public FormulaData EXTDATA(int N)
        {
            return this.DataProvider["EXTDATA" + N];
        }

        public FormulaData FILLAREA(FormulaData f1)
        {
            FormulaData data = new FormulaData(f1);
            data.RenderType = FormulaRenderType.FILLAREA;
            data.TextInvisible = true;
            return data;
        }

        private void FillLinerValue(FormulaData f)
        {
            int num = -1;
            for (int i = 0; i < f.Length; i++)
            {
                if (!double.IsNaN(f[i]))
                {
                    if (num >= 0)
                    {
                        for (int j = num + 1; j < i; j++)
                        {
                            f[j] = (((f[i] - f[num]) / ((double) (i - num))) * (j - num)) + f[num];
                        }
                    }
                    num = i;
                }
            }
        }

        public FormulaData FILLRGN(FormulaData Cond, FormulaData f1, FormulaData f2)
        {
            FormulaData.MakeSameLength(new FormulaData[] { Cond, f1, f2 });
            FormulaData data = new FormulaData(f1);
            data.RenderType = FormulaRenderType.FILLRGN;
            data["COND"] = Cond.Data;
            data["PRICE2"] = f2.Data;
            data.TextInvisible = true;
            return data;
        }

        public static FormulaData FILTER(FormulaData f, double N)
        {
            int n = (int) N;
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = f.Data[i];
                if (f.Data[i] > 0.0)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (i < (f.Length - 1))
                        {
                            data.Data[++i] = 0.0;
                        }
                    }
                }
            }
            return data;
        }

        public double FINANCE(double N)
        {
            return this.DataProvider.GetConstData("FINANCE" + N);
        }

        public FormulaData FINDPEAK(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 1; i < (data.Length - 1); i++)
            {
                if ((f[i] > f[i + 1]) && (f[i] > f[i - 1]))
                {
                    data[i] = 1.0;
                }
                else
                {
                    data[i] = 0.0;
                }
            }
            return data;
        }

        public FormulaData FINDPEAK(FormulaData f, double N)
        {
            FormulaData data = this.ZIG(f, N);
            return this.FINDPEAK(data);
        }

        public FormulaData FINDTROUGH(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 1; i < (data.Length - 1); i++)
            {
                if ((f[i] < f[i + 1]) && (f[i] < f[i - 1]))
                {
                    data[i] = 1.0;
                }
                else
                {
                    data[i] = 0.0;
                }
            }
            return data;
        }

        public FormulaData FINDTROUGH(FormulaData f, double N)
        {
            FormulaData data = this.ZIG(f, N);
            return this.FINDTROUGH(data);
        }

        public static FormulaData FLOOR(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Floor(f.Data[i]);
            }
            return data;
        }

        public FormulaData FML(string FormulaName)
        {
            return this.FML(this.DataProvider, FormulaName);
        }

        public FormulaData FML(IDataProvider dp, string FormulaName)
        {
            return this.GetFormulaData(dp, FormulaName);
        }

        public FormulaData FML(string Symbol, string FormulaName)
        {
            return this.FML(Symbol, FormulaName, "");
        }

        public FormulaData FML(IDataProvider dp, string FormulaName, string Cycle)
        {
            return this.GetFormulaData(dp, FormulaName + "#" + Cycle);
        }

        public FormulaData FML(string Symbol, string FormulaName, string Cycle)
        {
            if (this.DataProvider.DataManager == null)
            {
                throw new Exception(Symbol + " data not found!");
            }
            return this.FML(this.DataProvider.DataManager[Symbol], FormulaName, Cycle);
        }

        public FormulaData FML(string Symbol, string FormulaName, int i, int j)
        {
            return this.FML(Symbol, FormulaName);
        }

        public static FormulaData FORCAST(FormulaData f, double N)
        {
            return CalLine(f, N, true);
        }

        public static string[] GetAllFormulas()
        {
            Type type = typeof(FormulaBase);
            ArrayList list = new ArrayList();
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < methods.Length; i++)
            {
                ParameterInfo[] parameters = methods[i].GetParameters();
                bool flag = true;
                string str = "";
                for (int j = 0; j < parameters.Length; j++)
                {
                    if (str != "")
                    {
                        str = str + ",";
                    }
                    str = str + parameters[j].Name;
                    if ((parameters[j].ParameterType != typeof(double)) && (parameters[j].ParameterType != typeof(FormulaData)))
                    {
                        flag = false;
                    }
                }
                if (methods[i].ReturnType != typeof(FormulaData))
                {
                    flag = false;
                }
                if (flag)
                {
                    list.Add(methods[i].Name + "(" + str + ")");
                }
            }
            list.Sort(new CaseInsensitiveComparer());
            return (string[]) list.ToArray(typeof(string));
        }

        public static string[] GetAllIndicators(bool WithParam)
        {
            ArrayList list = new ArrayList();
            foreach (Assembly assembly in SupportedAssemblies.Values)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(FormulaBase)))
                    {
                        FormulaBase base2 = (FormulaBase) Activator.CreateInstance(type);
                        string str = type.FullName.Substring(4);
                        if (WithParam)
                        {
                            str = str + "(" + base2.Params.ToString() + ")";
                        }
                        list.Add(str);
                    }
                }
            }
            list.Sort(new CaseInsensitiveComparer());
            return (string[]) list.ToArray(typeof(string));
        }

        public static string GetAssemblyKey(Assembly a)
        {
            foreach (string str in SupportedAssemblies.Keys)
            {
                if (SupportedAssemblies[str].ToString() == a.ToString())
                {
                    return str;
                }
            }
            return null;
        }

        public static FormulaBase GetFormulaByName(string Name)
        {
            foreach (object obj2 in SupportedAssemblies.Values)
            {
                FormulaBase formulaByName = GetFormulaByName((Assembly) obj2, Name);
                if (formulaByName != null)
                {
                    return formulaByName;
                }
            }
            FormulaBase base3 = new ERROR();
            base3.SetParam("MSG", "Undefined formula \"" + Name + "\"");
            return base3;
        }

        public static FormulaBase GetFormulaByName(Assembly a, string Name)
        {
            int index = Name.IndexOf("(");
            int num2 = Name.LastIndexOf(")");
            string[] ss = null;
            if (num2 > index)
            {
                string str = Name.Substring(index + 1, (num2 - index) - 1);
                Name = Name.Substring(0, index) + Name.Substring(num2 + 1);
                ss = str.Split(new char[] { ',' });
            }
            index = Name.IndexOf('[');
            if (Name.IndexOf(']') > index)
            {
                Name = Name.Substring(0, index);
            }
            if (!Name.StartsWith("FML"))
            {
                Name = "FML." + Name.ToUpper();
            }
            Type type = a.GetType(Name, false, true);
            if (type != null)
            {
                FormulaBase base2 = (FormulaBase) Activator.CreateInstance(type);
                base2.SetParams(ss);
                return base2;
            }
            return null;
        }

        public FormulaData GetFormulaData(IDataProvider dp, string FormulaName)
        {
            double[] numArray = null;
            DataCycle dataCycle = null;
            FormulaData data2;
            if (this.DataProvider != null)
            {
                numArray = this.DataProvider["DATE"];
                dataCycle = this.DataProvider.DataCycle;
            }
            try
            {
                int index = FormulaName.IndexOf('(');
                int num2 = FormulaName.LastIndexOf(')');
                string[] ss = new string[0];
                if (num2 > index)
                {
                    ss = FormulaName.Substring(index + 1, (num2 - index) - 1).Split(new char[] { ',' });
                    FormulaName = FormulaName.Substring(0, index) + FormulaName.Substring(num2 + 1);
                }
                index = FormulaName.IndexOf('[');
                num2 = FormulaName.IndexOf(']');
                string formulaName = FormulaName;
                string str3 = "";
                if (!this.BasicFormula(FormulaName))
                {
                    FormulaData data;
                    if (num2 > index)
                    {
                        formulaName = FormulaName.Substring(0, index) + FormulaName.Substring(num2 + 1);
                        str3 = FormulaName.Substring(index + 1, (num2 - index) - 1);
                    }
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    this.BindFormulaCycle(dp, ref formulaName);
                    FormulaBase formulaByName = GetFormulaByName("FML." + formulaName);
                    if (formulaByName.GetType() == typeof(ERROR))
                    {
                        return (double) 1.0 / (double) 0.0;
                    }
                    for (int i = 0; i < ss.Length; i++)
                    {
                        FormulaParam param = this.Params[ss[i]];
                        if (param != null)
                        {
                            ss[i] = param.Value;
                        }
                    }
                    FormulaPackage package = formulaByName.Run(dp, ss);
                    if (str3 == "")
                    {
                        data = package[package.Count - 1];
                    }
                    else
                    {
                        data = package[str3];
                    }
                    return this.AdjustDateTime(numArray, dp["DATE"], data.Data);
                }
                Type type = typeof(FormulaBase);
                this.BindFormulaCycle(dp, ref FormulaName);
                FormulaBase target = this;
                if (this.DataProvider != dp)
                {
                    target = new FormulaBase(dp);
                }
                object obj2 = type.InvokeMember(FormulaName, BindingFlags.GetProperty | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, target, null);
                if (obj2 is FormulaData)
                {
                    return this.AdjustDateTime(numArray, dp["DATE"], (obj2 as FormulaData).Data);
                }
                data2 = (double) 1.0 / (double) 0.0;
            }
            finally
            {
                if (this.DataProvider != null)
                {
                    this.DataProvider.DataCycle = dataCycle;
                }
            }
            return data2;
        }

        public string GetParam(string ParamName)
        {
            FormulaParam param = this.Params[ParamName];
            if (param != null)
            {
                return param.Value;
            }
            return "";
        }

        public string GetParamString()
        {
            string str = this.Params.ToString();
            if (this.Params.Count > 0)
            {
                str = "(" + str + ")";
            }
            return str;
        }

        public FormulaData GETSTOCK(FormulaData fO, FormulaData fC, FormulaData fH, FormulaData fL)
        {
            FormulaData.MakeSameLength(new FormulaData[] { fO, fC, fH, fL });
            FormulaData data = new FormulaData(fC);
            data.RenderType = FormulaRenderType.STOCK;
            data["O"] = fO.Data;
            data["H"] = fH.Data;
            data["L"] = fL.Data;
            return data;
        }

        public static FormulaData HHV(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                if (n == 0)
                {
                    return TestData(MaxTestCount, new FormulaData[] { f });
                }
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            double minValue = double.MinValue;
            for (int i = 0; i < f.Length; i++)
            {
                if (n == 0)
                {
                    minValue = Math.Max(minValue, f.Data[i]);
                }
                else
                {
                    minValue = double.MinValue;
                    for (int j = Math.Max(0, (i - n) + 1); j <= i; j++)
                    {
                        minValue = Math.Max(minValue, f.Data[j]);
                    }
                }
                data.Data[i] = minValue;
            }
            return data;
        }

        public static FormulaData HHVBARS(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            int num2 = 0;
            double minValue = double.MinValue;
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                if (n == 0)
                {
                    if (f.Data[i] >= minValue)
                    {
                        num2 = i;
                        minValue = f.Data[i];
                    }
                }
                else
                {
                    minValue = double.MinValue;
                    for (int j = Math.Max(0, i - n); j <= i; j++)
                    {
                        if (f.Data[j] >= minValue)
                        {
                            num2 = j;
                            minValue = f.Data[j];
                        }
                    }
                }
                data.Data[i] = num2;
            }
            return data;
        }

        public static FormulaData IF(FormulaData f1, FormulaData f2, FormulaData f3)
        {
            FormulaData.MakeSameLength(new FormulaData[] { f1, f2, f3 });
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f1, f2, f3 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                if (f1.Data[i] != 0.0)
                {
                    data.Data[i] = f2.Data[i];
                }
                else
                {
                    data.Data[i] = f3.Data[i];
                }
            }
            return data;
        }

        public static FormulaData INTPART(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = (int) f.Data[i];
            }
            return data;
        }

        public static FormulaData LAST(FormulaData f, double A, double B)
        {
            int num = (int) A;
            int num2 = (int) B;
            FormulaData data = new FormulaData(f.Length);
            if (Testing)
            {
                return TestData(Math.Max(num, num2), new FormulaData[] { f });
            }
            for (int i = 0; i < f.Length; i++)
            {
                double num4 = 1.0;
                if (num == 0)
                {
                    if (f.Data[i] == 0.0)
                    {
                        num4 = 0.0;
                    }
                }
                else
                {
                    for (int j = Math.Max(0, (i - num) + 1); j < Math.Max(0, (i - num2) + 1); j++)
                    {
                        if (f.Data[j] == 0.0)
                        {
                            num4 = 0.0;
                            break;
                        }
                    }
                }
                f.Data[i] = num4;
            }
            return data;
        }

        public static FormulaData LLV(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            double maxValue = double.MaxValue;
            for (int i = 0; i < f.Length; i++)
            {
                if (N == 0.0)
                {
                    maxValue = Math.Min(maxValue, f.Data[i]);
                }
                else
                {
                    maxValue = double.MaxValue;
                    for (int j = Math.Max(0, (i - n) + 1); j <= i; j++)
                    {
                        maxValue = Math.Min(maxValue, f.Data[j]);
                    }
                }
                data.Data[i] = maxValue;
            }
            return data;
        }

        public static FormulaData LLVBARS(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            int num2 = 0;
            double maxValue = double.MaxValue;
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                if (n == 0)
                {
                    if (f.Data[i] <= maxValue)
                    {
                        num2 = i;
                        maxValue = f.Data[i];
                    }
                }
                else
                {
                    maxValue = double.MaxValue;
                    for (int j = Math.Max(0, i - n); j <= i; j++)
                    {
                        if (f.Data[j] <= maxValue)
                        {
                            num2 = j;
                            maxValue = f.Data[j];
                        }
                    }
                }
                data.Data[i] = num2;
            }
            return data;
        }

        public static FormulaData LN(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Log(f.Data[i]);
            }
            return data;
        }

        public static FormulaData LOG(FormulaData f)
        {
            return LOG10(f);
        }

        public static FormulaData LOG(FormulaData f, double N)
        {
            FormulaData data = new FormulaData(f.Length);
            double num = Math.Log(N);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Log(f.Data[i]) / num;
            }
            return data;
        }

        public static FormulaData LOG10(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Log10(f.Data[i]);
            }
            return data;
        }

        public static FormulaData LONGCROSS(FormulaData f1, FormulaData f2, double N)
        {
            FormulaData.MakeSameLength(f1, f2);
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            int num = 0;
            for (int i = 0; i < f1.Length; i++)
            {
                if (i == 0)
                {
                    data.Data[i] = 0.0;
                }
                else
                {
                    data.Data[i] = 0.0;
                    if (f1.Data[i] < f2.Data[i])
                    {
                        num++;
                    }
                    else
                    {
                        if (num >= N)
                        {
                            data.Data[i] = 1.0;
                        }
                        num = 0;
                    }
                }
            }
            return data;
        }

        public static FormulaData LR(FormulaData f, double N)
        {
            return LR(f, N, 0.0);
        }

        public static FormulaData LR(FormulaData f, double N, double Start)
        {
            int count = (int) N;
            int num2 = (int) Start;
            if (Testing)
            {
                return TestData(count + num2, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            data.Set(double.NaN);
            if (((f.Length - count) - num2) > 0)
            {
                double num3;
                double num4;
                CalcLinearRegression(f, (f.Length - 1) - num2, count, out num4, out num3);
                int num5 = (f.Length - count) - num2;
                for (int i = num5; i < (f.Length - num2); i++)
                {
                    data.Data[i] = num4 + (num3 * (i - num5));
                }
            }
            return data;
        }

        public FormulaData LSOLARTERM(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                DateTime time = DateTime.FromOADate(f.Data[i]);
                f.Data[i] = time.Year;
            }
            return f;
        }

        public FormulaData LSOLARTERMDATE(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                DateTime time = DateTime.FromOADate(f.Data[i]);
                f.Data[i] = time.Year;
            }
            return f;
        }

        public static FormulaData MA(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            double num2 = 0.0;
            for (int i = 0; i < f.Length; i++)
            {
                if (!double.IsNaN(f.Data[i]))
                {
                    num2 += f.Data[i];
                }
                else
                {
                    data.Data[i] = double.NaN;
                }
                if ((n != 0) && (i >= (n - 1)))
                {
                    if (!double.IsNaN(f.Data[(i - n) + 1]))
                    {
                        if (double.IsNaN(f.Data[i]))
                        {
                            data.Data[i] = double.NaN;
                        }
                        else
                        {
                            data.Data[i] = num2 / ((double) n);
                        }
                        num2 -= f.Data[(i - n) + 1];
                    }
                    else
                    {
                        data.Data[i] = double.NaN;
                    }
                }
                else if (n == 0)
                {
                    data.Data[i] = num2 / ((double) (i + 1));
                }
                else
                {
                    data.Data[i] = double.NaN;
                }
            }
            return data;
        }

        public static FormulaData MAX(params FormulaData[] fds)
        {
            FormulaData.MakeSameLength(fds);
            if (Testing)
            {
                return TestData(0, fds);
            }
            FormulaData data = new FormulaData(fds[0].Length);
            for (int i = 0; i < fds[0].Length; i++)
            {
                double num2 = fds[0][i];
                for (int j = 1; j < fds.Length; j++)
                {
                    num2 = Math.Max(num2, fds[j][i]);
                }
                data.Data[i] = num2;
            }
            return data;
        }

        public static double MAX(params double[] dd)
        {
            if (dd.Length > 0)
            {
                double num = dd[0];
                for (int i = 1; i < dd.Length; i++)
                {
                    num = Math.Max(num, dd[i]);
                }
                return num;
            }
            return double.NaN;
        }

        public static double MAXVALUE(FormulaData f)
        {
            double minValue = double.MinValue;
            for (int i = 0; i < f.Length; i++)
            {
                if (minValue < f.Data[i])
                {
                    minValue = f.Data[i];
                }
            }
            return minValue;
        }

        public static FormulaData MIN(params FormulaData[] fds)
        {
            FormulaData.MakeSameLength(fds);
            if (Testing)
            {
                return TestData(0, fds);
            }
            FormulaData data = new FormulaData(fds[0].Length);
            for (int i = 0; i < fds[0].Length; i++)
            {
                double num2 = fds[0][i];
                for (int j = 1; j < fds.Length; j++)
                {
                    num2 = Math.Min(num2, fds[j][i]);
                }
                data.Data[i] = num2;
            }
            return data;
        }

        public static double MIN(params double[] dd)
        {
            if (dd.Length > 0)
            {
                double num = dd[0];
                for (int i = 1; i < dd.Length; i++)
                {
                    num = Math.Min(num, dd[i]);
                }
                return num;
            }
            return double.NaN;
        }

        public static double MINVALUE(FormulaData f)
        {
            double maxValue = double.MaxValue;
            for (int i = 0; i < f.Length; i++)
            {
                if (maxValue > f.Data[i])
                {
                    maxValue = f.Data[i];
                }
            }
            return maxValue;
        }

        public static FormulaData MOD(FormulaData f1, FormulaData f2)
        {
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f1, f2 });
            }
            return (f1 % f2);
        }
        [Description("If f1==0 return 1 , otherwise return 0"),Category("6.Logic functions")]
        public static FormulaData NOT(FormulaData f1)
        {
            return !f1;
        }

        public FormulaData Num2FormulaData(double K)
        {
            if (K == 0.0)
            {
                return this.O;
            }
            if (K == 1.0)
            {
                return this.H;
            }
            if (K == 2.0)
            {
                return this.L;
            }
            return this.C;
        }

        public FormulaData ORGDATA(string DataName)
        {
            return this.DataProvider[DataName];
        }

        public FormulaData PARTLINE(FormulaData Cond, FormulaData f)
        {
            FormulaData.MakeSameLength(Cond, f);
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.PARTLINE;
            data["COND"] = Cond.Data;
            data.TextInvisible = true;
            return data;
        }

        public FormulaData PEAK(FormulaData f, double N)
        {
            return this.PEAK(f, N, 1.0);
        }

        public FormulaData PEAK(double K, double N)
        {
            return this.PEAK(K, N, 1.0);
        }

        public FormulaData PEAK(FormulaData f, double N, double M)
        {
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            FormulaData expr = this.FINDPEAK(f, N);
            return this.VALUEWHEN(expr, f, M);
        }

        public FormulaData PEAK(double K, double N, double M)
        {
            return this.PEAK(this.Num2FormulaData(K), N, M);
        }

        public FormulaData PEAKBARS(FormulaData f, double N)
        {
            return this.PEAKBARS(f, N, 1.0);
        }

        public FormulaData PEAKBARS(FormulaData f, double N, double M)
        {
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            FormulaData expr = this.FINDPEAK(f, N);
            return this.VALUEWHENBARS(expr, M);
        }

        public FormulaData PEAKBARS(double K, double N, double M)
        {
            return this.PEAKBARS(this.Num2FormulaData(K), N, M);
        }

        public FormulaData POLYLINE(FormulaData Cond, FormulaData f)
        {
            FormulaData.MakeSameLength(Cond, f);
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.POLY;
            data["COND"] = Cond.Data;
            return data;
        }

        public static FormulaData POW(FormulaData f, double N)
        {
            return POWER(f, N);
        }

        public static FormulaData POWER(FormulaData f, double N)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Pow(f.Data[i], N);
            }
            return data;
        }

        public static FormulaData RANGE(FormulaData f1, FormulaData f2, FormulaData f3)
        {
            FormulaData.MakeSameLength(new FormulaData[] { f1, f2, f3 });
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f1, f2, f3 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                if ((f1.Data[i] >= f2.Data[i]) && (f1.Data[i] <= f3.Data[i]))
                {
                    data.Data[i] = 1.0;
                }
            }
            return data;
        }

        public static FormulaData REF(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = f.Length - 1; i >= n; i--)
            {
                data.Data[i] = f.Data[i - n];
            }
            if (n <= data.Length)
            {
                for (int j = n - 1; j >= 0; j--)
                {
                    data.Data[j] = double.NaN;
                }
            }
            return data;
        }

        public static void RegAssembly(string Key, Assembly a)
        {
            if (!SupportedAssemblies.ContainsValue(a))
            {
                SupportedAssemblies[Key] = a;
            }
        }

        public static MemberInfo[] GetAllMembers()
        {
            Type t = typeof(FormulaBase);
            return t.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
        }

        public static FormulaData REVERSE(FormulaData f)
        {
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f });
            }
            return -f;
        }

        public virtual FormulaPackage Run(IDataProvider dp)
        {
            return null;
        }

        public FormulaPackage Run(IDataProvider dp, string[] ss)
        {
            this.SetParams(ss);
            return this.Run(dp);
        }

        public FormulaData SAR(double N, double STEP, double MAXP)
        {
            if (Testing)
            {
                return TestData(MaxTestCount, new FormulaData[] { this.C });
            }
            FormulaData tURNDATA = null;
            return this.SAR(N, STEP, MAXP, ref tURNDATA);
        }

        public FormulaData SAR(double N, double STEP, double MAXP, ref FormulaData TURNDATA)
        {
            int num;
            if (this.H.Length < N)
            {
                return (double) 1.0 / (double) 0.0;
            }
            FormulaData data = REF(HHV(this.H, N), 1.0);
            FormulaData data2 = REF(LLV(this.L, N), 1.0);
            FormulaData data3 = new FormulaData(data.Length);
            STEP /= 100.0;
            MAXP /= 100.0;
            if (((this.H[1] - this.H[0]) + (this.L[1] - this.L[0])) >= 0.0)
            {
                num = 0;
            }
            else
            {
                num = 1;
            }
            double num2 = STEP;
            int num3 = 0;
            SarHelper[] helperArray = new SarHelper[] { new SarHelper(false, new FormulaData[] { this.H, data, this.L, data2 }), new SarHelper(true, new FormulaData[] { this.L, data2, this.H, data }) };
            for (int i = 0; i < N; i++)
            {
                data3[i] = double.NaN;
            }
            SarHelper helper = helperArray[num];
            for (int j = (int) N; j < helper.H.Length; j++)
            {
                if (num3 == 0)
                {
                    data3[j] = helper.LL[j];
                }
                else
                {
                    if (helper.Dir ^ (helper.H[j] > helper.HH[j]))
                    {
                        num2 += STEP;
                        if (num2 > MAXP)
                        {
                            num2 = MAXP;
                        }
                    }
                    data3[j] = data3[j - 1] + (num2 * (helper.HH[j] - data3[j - 1]));
                }
                if (helper.Dir ^ (data3[j] < helper.L[j]))
                {
                    num3++;
                }
                else
                {
                    num = 1 - num;
                    helper = helperArray[num];
                    if (!object.Equals(TURNDATA, null))
                    {
                        TURNDATA[j] = 1 - (num * 2);
                    }
                    num3 = 0;
                    num2 = STEP;
                }
            }
            data3.Dot = FormulaDot.CIRCLEDOT;
            return data3;
        }

        public FormulaData SARTURN(double N, double STEP, double MAXP)
        {
            if (Testing)
            {
                return TestData(MaxTestCount, new FormulaData[] { this.C });
            }
            FormulaData tURNDATA = new FormulaData(this.DataProvider.Count);
            this.SAR(N, STEP, MAXP, ref tURNDATA);
            return tURNDATA;
        }

        public FormulaData SELLVOL(int N)
        {
            return this.DataProvider["SELLVOL" + N];
        }

        public void SETNAME(string Name)
        {
            this.SETNAME(Name, false);
        }

        public void SETNAME(FormulaData f, string Name)
        {
            f.Name = Name;
        }

        public void SETNAME(string Name, bool ShowParameter)
        {
            this.Name = Name;
            this.ShowParameter = ShowParameter;
        }

        private void SetParam(FormulaParam fp, string Value)
        {
            object obj2 = Value;
            if (fp.ParamType == FormulaParamType.Double)
            {
                double num = double.Parse(Value);
                if (num > double.Parse(fp.MaxValue))
                {
                    Value = fp.MaxValue;
                }
                if (num < double.Parse(fp.MinValue))
                {
                    Value = fp.MinValue;
                }
                obj2 = num;
            }
            fp.Value = Value;
            base.GetType().InvokeMember(fp.Name.ToUpper(), BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, this, new object[] { obj2 });
        }

        public void SetParam(string ParamName, string Value)
        {
            foreach (FormulaParam param in this.Params)
            {
                if (string.Compare(param.Name, ParamName, true) == 0)
                {
                    this.SetParam(param, Value);
                }
            }
        }

        public void SetParams(double[] P)
        {
            if (P != null)
            {
                for (int i = 0; (i < P.Length) && (i < this.Params.Count); i++)
                {
                    this.SetParam(this.Params[i], P[i].ToString());
                }
            }
        }

        public void SetParams(string[] ss)
        {
            if (ss != null)
            {
                for (int i = 0; (i < ss.Length) && (i < this.Params.Count); i++)
                {
                    this.SetParam(this.Params[i], ss[i]);
                }
            }
        }

        public void SETTEXTVISIBLE(bool Visible)
        {
            this.TextInvisible = !Visible;
        }

        public void SETTEXTVISIBLE(FormulaData f, bool Visible)
        {
            f.TextInvisible = !Visible;
        }

        public static FormulaData SGN(FormulaData f)
        {
            if (Testing)
            {
                return TestData(0, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Sign(f.Data[i]);
            }
            return data;
        }

        public static FormulaData SIN(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Sin(f.Data[i]);
            }
            return data;
        }

        public static FormulaData SLOPE(FormulaData f, double N)
        {
            return CalLine(f, N, false);
        }

        public static FormulaData SMA(FormulaData f, double N, double M)
        {
            if (Testing)
            {
                return TestData(((int) N) + DMATestCount, new FormulaData[] { f });
            }
            return DMA(f, M / N, (int) N);
        }

        public static FormulaData SQR(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = f.Data[i] * f.Data[i];
            }
            return data;
        }

        public static FormulaData SQRT(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Sqrt(f.Data[i]);
            }
            return data;
        }

        public static FormulaData STD(FormulaData f, double N)
        {
            return SQRT(VAR(f, N));
        }

        public static FormulaData STDP(FormulaData f, double N)
        {
            return SQRT(VARP(f, N));
        }

        public FormulaData STICKLINE(FormulaData f1, FormulaData f2, FormulaData f3, double Width, double Empty)
        {
            FormulaData.MakeSameLength(new FormulaData[] { f1, f2, f3 });
            FormulaData data = new FormulaData(f2);
            data.RenderType = FormulaRenderType.STICKLINE;
            data["COND"] = f1.Data;
            data["PRICE2"] = f3.Data;
            data.OwnerData["WIDTH"] = Width;
            data.OwnerData["EMPTY"] = Empty;
            return data;
        }

        public double STKINBLOCK(string s)
        {
            return 1.0;
        }

        public double STRCMP(string s1, string s2)
        {
            return (double) string.Compare(s1, s2);
        }

        public double STRNCMP(string s1, string s2, double N)
        {
            return (double) string.Compare(s1, 0, s2, 0, (int) N);
        }

        public static FormulaData SUM(FormulaData f, double N)
        {
            int n = (int) N;
            if (Testing)
            {
                return TestData(n, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            double num2 = 0.0;
            for (int i = 0; i < f.Length; i++)
            {
                if (!double.IsNaN(f.Data[i]))
                {
                    num2 += f.Data[i];
                    if (n == 0)
                    {
                        data.Data[i] = num2;
                    }
                    else if ((i > n) && !double.IsNaN(f.Data[(i - n) - 1]))
                    {
                        data.Data[i] = num2 - f.Data[(i - n) - 1];
                    }
                    else
                    {
                        data.Data[i] = double.NaN;
                    }
                }
                else
                {
                    data.Data[i] = double.NaN;
                }
            }
            return data;
        }

        public static FormulaData SUMBARS(FormulaData f, double N)
        {
            if (Testing)
            {
                return TestData(DefaultTestCount, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                double num2 = 0.0;
                int index = i;
                while (index >= 0)
                {
                    num2 += f.Data[index];
                    if (num2 >= N)
                    {
                        break;
                    }
                    index--;
                }
                data.Data[i] = (i - index) + 1;
            }
            return data;
        }

        public static FormulaData TAN(FormulaData f)
        {
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                data.Data[i] = Math.Tan(f.Data[i]);
            }
            return data;
        }

        public static FormulaData TestData(int N, params FormulaData[] fs)
        {
            FormulaData.MakeSameLength(fs);
            FormulaData data = new FormulaData(fs[0].Length);
            for (int i = 0; i < fs[0].Length; i++)
            {
                bool flag = false;
                for (int j = 0; j < fs.Length; j++)
                {
                    if (double.IsNaN(fs[j][i]))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    data[i] = double.NaN;
                }
                else
                {
                    for (int k = i; k < fs[0].Length; k++)
                    {
                        if (k < (i + N))
                        {
                            data[k] = double.NaN;
                        }
                        else
                        {
                            data[k] = 0.0;
                        }
                    }
                    return data;
                }
            }
            return data;
        }

        public override string ToString()
        {
            string name = this.Name;
            if ((this.Quote != null) && (this.Quote != ""))
            {
                name = name + "@" + this.Quote;
            }
            if (this.ShowParameter)
            {
                return (name + this.GetParamString());
            }
            return name;
        }

        public FormulaData TROUGH(FormulaData f, double N)
        {
            return this.TROUGH(f, N, 1.0);
        }

        public FormulaData TROUGH(double K, double N)
        {
            return this.TROUGH(K, N, 1.0);
        }

        public FormulaData TROUGH(FormulaData f, double N, double M)
        {
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            FormulaData expr = this.FINDTROUGH(f, N);
            return this.VALUEWHEN(expr, f, M);
        }

        public FormulaData TROUGH(double K, double N, double M)
        {
            return this.TROUGH(this.Num2FormulaData(K), N, M);
        }

        public FormulaData TROUGHBARS(FormulaData f, double N, double M)
        {
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            FormulaData expr = this.FINDTROUGH(f, N);
            return this.VALUEWHENBARS(expr, M);
        }

        public FormulaData TROUGHBARS(double K, double N, double M)
        {
            return this.TROUGHBARS(this.Num2FormulaData(K), N, M);
        }

        public static void UnregAssembly(string Key)
        {
            SupportedAssemblies.Remove(Key);
        }

        public FormulaData VALUEWHEN(FormulaData Expr, FormulaData f, double N)
        {
            FormulaData.MakeSameLength(Expr, f);
            FormulaData data = new FormulaData(f.Length);
            ArrayList list = new ArrayList();
            list.Add(f.Length);
            for (int i = f.Length - 1; i >= 0; i--)
            {
                if (Expr[i] > 0.0)
                {
                    list.Add(i);
                    if (list.Count > N)
                    {
                        int num2 = (int) list[list.Count - ((int) N)];
                        int num3 = (int) list[(list.Count - ((int) N)) - 1];
                        for (int j = num2; j < num3; j++)
                        {
                            data[j] = f[i];
                        }
                    }
                }
            }
            return data;
        }

        public FormulaData VALUEWHENBARS(FormulaData Expr, double N)
        {
            FormulaData data = new FormulaData(Expr.Length);
            ArrayList list = new ArrayList();
            list.Add(Expr.Length);
            for (int i = Expr.Length - 1; i >= 0; i--)
            {
                if (Expr[i] > 0.0)
                {
                    list.Add(i);
                    if (list.Count > N)
                    {
                        int num2 = (int) list[list.Count - ((int) N)];
                        int num3 = (int) list[(list.Count - ((int) N)) - 1];
                        for (int j = num2; j < num3; j++)
                        {
                            data[j] = j - i;
                        }
                    }
                }
            }
            return data;
        }

        public static FormulaData VAR(FormulaData f, double N)
        {
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            if (N == 0.0)
            {
                return f;
            }
            if (N == 1.0)
            {
                return VARP(f, N);
            }
            return ((VARP(f, N) * N) / (N - 1.0));
        }

        public static FormulaData VARP(FormulaData f, double N)
        {
            if (Testing)
            {
                return TestData((int) N, new FormulaData[] { f });
            }
            return (MA(SQR(f), N) - SQR(MA(f, N)));
        }

        public FormulaData VERTLINE(FormulaData f)
        {
            FormulaData data = new FormulaData(f);
            data.RenderType = FormulaRenderType.VERTLINE;
            return data;
        }

        public FormulaData WINNER(FormulaData f)
        {
            FormulaData h = this.H;
            FormulaData l = this.L;
            FormulaData v = this.V;
            FormulaData data4 = new FormulaData(this.H.Length);
            double num = 0.0;
            SortedList list = new SortedList();
            for (int i = 0; i < this.H.Length; i++)
            {
                double num3 = (v.Data[i] / (h.Data[i] - l.Data[i])) / 100.0;
                num += v.Data[i];
                for (double j = l.Data[i]; j <= h.Data[i]; j += 0.01)
                {
                    if (list[j] == null)
                    {
                        list[j] = num3;
                    }
                    else
                    {
                        list[j] = ((double) list[j]) + num3;
                    }
                }
                double num5 = 0.0;
                for (int k = 0; k < list.Count; k++)
                {
                    if (((double) list.GetByIndex(k)) > f.Data[i])
                    {
                        data4.Data[i] = num5 / num;
                        break;
                    }
                    num5 += (double) list.GetByIndex(k);
                }
            }
            return data4;
        }

        public FormulaData ZIG(FormulaData f, double N)
        {
            if (Testing)
            {
                return TestData(ZigTestCount, new FormulaData[] { f });
            }
            FormulaData data = new FormulaData(f.Length);
            for (int i = 0; i < f.Length; i++)
            {
                if ((i > 0) && (i < (f.Length - 1)))
                {
                    data.Data[i] = double.NaN;
                }
                else
                {
                    data.Data[i] = f.Data[i];
                }
            }
            double minValue = double.MinValue;
            double maxValue = double.MaxValue;
            int index = -1;
            int num5 = -1;
            int num6 = 3;
            for (int j = 0; j < f.Length; j++)
            {
                if (!double.IsNaN(f[j]))
                {
                    if (f.Data[j] > minValue)
                    {
                        minValue = f.Data[j];
                        index = j;
                    }
                    if (f.Data[j] < maxValue)
                    {
                        maxValue = f.Data[j];
                        num5 = j;
                    }
                    if (((j > 0) && ((num6 & 1) != 0)) && (((f.Data[j] / maxValue) > (1.0 + (N / 100.0))) || (j == (f.Length - 1))))
                    {
                        maxValue = double.MaxValue;
                        minValue = double.MinValue;
                        num6 = 2;
                        data.Data[num5] = f.Data[num5];
                        j = num5;
                    }
                    else if (((j > 0) && ((num6 & 2) != 0)) && (((f.Data[j] / minValue) < (1.0 - (N / 100.0))) || (j == (f.Length - 1))))
                    {
                        minValue = double.MinValue;
                        maxValue = double.MaxValue;
                        num6 = 1;
                        data.Data[index] = f.Data[index];
                        j = index;
                    }
                }
            }
            this.FillLinerValue(data);
            return data;
        }

        public FormulaData ZIG(double K, double N)
        {
            return this.ZIG(this.Num2FormulaData(K), N);
        }

        public FormulaData ADVANCE
        {
            get
            {
                return this.DataProvider["ADVANCE"];
            }
        }

        public FormulaData AMOUNT
        {
            get
            {
                return this.DataProvider["AMOUNT"];
            }
        }

        public FormulaData C
        {
            get
            {
                return this.CLOSE;
            }
        }

        public double CAPITAL
        {
            get
            {
                return this.DataProvider.GetConstData("CAPITAL");
            }
        }

        public FormulaData CLOSE
        {
            get
            {
                return this.DataProvider["CLOSE"];
            }
        }

        public string CODE
        {
            get
            {
                return this.DataProvider.GetStringData("Code");
            }
        }

        public double DATACOUNT
        {
            get
            {
                return (double) this.DataProvider.Count;
            }
        }

        public FormulaData DATAPERIOD
        {
            get
            {
                return new FormulaData(this.DataProvider["DATAPERIOD"]);
            }
        }

        public FormulaData DATE
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = (((time.Year - 0x7b2) * 0x2710) + (time.Month * 100)) + time.Day;
                }
                return data;
            }
        }

        public FormulaData DAY
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = time.Day;
                }
                return data;
            }
        }

        public FormulaData DECLINE
        {
            get
            {
                return this.DataProvider["DECLINE"];
            }
        }

        public string DisplayName
        {
            get
            {
                return this.ToString();
            }
        }

        public string EXCHANGE
        {
            get
            {
                return this.DataProvider.GetStringData("Exchange");
            }
        }

        public string FormulaName
        {
            get
            {
                string str = base.GetType().ToString().ToUpper();
                if (str.StartsWith("FML."))
                {
                    str = str.Substring(4);
                }
                return (str + this.GetParamString());
            }
        }

        public string FullName
        {
            get
            {
                return (base.GetType() + this.GetParamString());
            }
        }

        public FormulaData H
        {
            get
            {
                return this.HIGH;
            }
        }

        public FormulaData HIGH
        {
            get
            {
                return this.DataProvider["HIGH"];
            }
        }

        public FormulaData HOUR
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = time.Hour;
                }
                return data;
            }
        }

        public FormulaData ISBUYORDER
        {
            get
            {
                return this.DataProvider["ISBUYORDER"];
            }
        }

        public FormulaData ISDOWN
        {
            get
            {
                return (FormulaData) (this.C < this.O);
            }
        }

        public FormulaData ISEQUAL
        {
            get
            {
                return (FormulaData) (this.C == this.O);
            }
        }

        public FormulaData ISLASTBAR
        {
            get
            {
                FormulaData cLOSE = this.CLOSE;
                FormulaData data2 = new FormulaData(cLOSE.Length);
                for (int i = 0; i < cLOSE.Length; i++)
                {
                    data2.Data[i] = (i == (cLOSE.Length - 1)) ? ((double) 1) : ((double) 0);
                }
                return data2;
            }
        }

        public FormulaData ISUP
        {
            get
            {
                return (FormulaData) (this.C > this.O);
            }
        }

        public FormulaData L
        {
            get
            {
                return this.LOW;
            }
        }

        public FormulaData LDAY
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime d = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = Chinese.Lunar(d).Day;
                }
                return data;
            }
        }

        public FormulaData LMONTH
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime d = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = Chinese.Lunar(d).Month;
                }
                return data;
            }
        }

        public FormulaData LOW
        {
            get
            {
                return this.DataProvider["LOW"];
            }
        }

        public FormulaData LYEAR
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime d = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = Chinese.Lunar(d).Year;
                }
                return data;
            }
        }

        public FormulaData MINUTE
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = time.Minute;
                }
                return data;
            }
        }

        public FormulaData MONTH
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = time.Month;
                }
                return data;
            }
        }

        public string NAME
        {
            get
            {
                return this.DataProvider.GetStringData("Name");
            }
        }

        public FormulaData O
        {
            get
            {
                return this.OPEN;
            }
        }

        [Description("Get the stock open price"), Category("2.Basic Data")]
        public FormulaData OPEN
        {
            get
            {
                return this.DataProvider["OPEN"];
            }
        }

        public string STKLABEL
        {
            get
            {
                return this.CODE;
            }
        }

        public string STKMARKET
        {
            get
            {
                return this.EXCHANGE;
            }
        }

        public string STKNAME
        {
            get
            {
                return this.NAME;
            }
        }

        public FormulaData STOCK
        {
            get
            {
                return this.GETSTOCK(this.O, this.C, this.H, this.L);
            }
        }

        public FormulaData TIME
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = ((time.Hour * 0x2710) + (time.Minute * 100)) + time.Second;
                }
                return data;
            }
        }

        public FormulaData V
        {
            get
            {
                return this.VOLUME;
            }
        }

        public FormulaData VOL
        {
            get
            {
                return this.VOLUME;
            }
        }

        public FormulaData VOLUME
        {
            get
            {
                return new FormulaData(this.DataProvider["VOLUME"]);
            }
        }

        public double VOLUNIT
        {
            get
            {
                return this.DataProvider.GetConstData("VOLUNIT");
            }
        }

        public FormulaData WEEK
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = (double) time.DayOfWeek;
                }
                return data;
            }
        }

        public FormulaData WEEKDAY
        {
            get
            {
                return this.WEEK;
            }
        }

        public FormulaData YEAR
        {
            get
            {
                FormulaData data = new FormulaData(this.DataProvider["DATE"]);
                for (int i = 0; i < data.Length; i++)
                {
                    DateTime time = DateTime.FromOADate(data.Data[i]);
                    data.Data[i] = time.Year;
                }
                return data;
            }
        }

        public class SarHelper
        {
            public bool Dir;
            public double[] H;
            public double[] HH;
            public double[] L;
            public double[] LL;

            public SarHelper(bool Dir, params FormulaData[] Datas)
            {
                this.Dir = Dir;
                this.H = Datas[0].Data;
                this.HH = Datas[1].Data;
                this.L = Datas[2].Data;
                this.LL = Datas[3].Data;
            }
        }
    }
}

