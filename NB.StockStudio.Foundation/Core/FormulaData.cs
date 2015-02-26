namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
    using System.Text;

    public class FormulaData
    {
        public FormulaAlign Align;
        public Brush AreaBrush;
        public int AxisMargin;
        public FormulaAxisX AxisX;
        public FormulaAxisY AxisY;
        public int AxisYIndex;
        public FormulaCanvas Canvas;
        public System.Drawing.Drawing2D.DashStyle DashStyle;
        public double[] Data;
        public FormulaDot Dot;
        public string Format;
        public Color FormulaColor;
        public FormulaType FormulaType;
        private Hashtable htData;
        private Hashtable htDataArray;
        public int LabelIndex;
        public bool LastValueInAxis;
        public Pen LinePen;
        public int LineWidth;
        public string Name;
        public Font NameFont;
        public FormulaBase ParentFormula;
        public double PercentView;
        public FormulaRenderType RenderType;
        public SmoothingMode Smoothing;
        public Font TextFont;
        public bool TextInvisible;
        public Transform Transform;
        public VerticalAlign VAlign;

        public FormulaData()
        {
            this.AxisYIndex = 0;
            this.FormulaType = FormulaType.Array;
            this.FormulaColor = Color.Empty;
            this.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.LineWidth = -1;
            this.Format = "f2";
            this.LabelIndex = 0;
            this.VAlign = VerticalAlign.Top;
        }

        public FormulaData(int N) : this()
        {
            this.Data = new double[N];
        }

        public FormulaData(double[] Data) : this()
        {
            this.Data = (double[]) Data.Clone();
        }

        public FormulaData(FormulaData f) : this(f.Length)
        {
            f.Data.CopyTo(this.Data, 0);
        }
        public override bool Equals(object obj)
        {
            if (obj is FormulaData)
            {
                FormulaData data = (FormulaData) obj;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data.Data[i] != this.Data[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return base.Equals(obj);
        }

        public void FillTo(int N)
        {
            this.FillTo(ref this.Data, N);
            if (this.htDataArray != null)
            {
                string[] strArray = new string[this.htDataArray.Count];
                int num = 0;
                foreach (string str in this.htDataArray.Keys)
                {
                    strArray[num++] = str;
                }
                foreach (string str2 in strArray)
                {
                    double[] dd = (double[]) this.htDataArray[str2];
                    this.FillTo(ref dd, N);
                    this.htDataArray[str2] = dd;
                }
            }
        }

        public void FillTo(ref double[] dd, int N)
        {
            if (dd.Length < N)
            {
                double[] numArray = dd;
                double naN = double.NaN;
                if ((this.FormulaType == FormulaType.Const) && (numArray.Length > 0))
                {
                    naN = numArray[0];
                }
                dd = new double[N];
                for (int i = 0; i < numArray.Length; i++)
                {
                    dd[(N - numArray.Length) + i] = numArray[i];
                }
                for (int j = 0; j < (N - numArray.Length); j++)
                {
                    dd[j] = naN;
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public double[] GetInViewData()
        {
            return this.GetInViewData(this.Data);
        }

        public double[] GetInViewData(string Name)
        {
            return this.GetInViewData((double[]) this.htDataArray[Name]);
        }

        public double[] GetInViewData(double[] dd)
        {
            int num = (dd.Length - this.Canvas.Start) - 1;
            ArrayList list = new ArrayList();
            for (int i = (dd.Length - this.Canvas.Start) - 1; i >= this.Canvas.Stop; i--)
            {
                if (!double.IsNaN(dd[i]))
                {
                    list.Add(dd[i]);
                }
            }
            return (double[]) list.ToArray(typeof(double));
        }

        public PointF[] GetPoints()
        {
            return this.GetPoints(this.Data, false);
        }

        public PointF[] GetPoints(bool AddEmptyPoint)
        {
            return this.GetPoints(this.Data, AddEmptyPoint);
        }

        public PointF[] GetPoints(string Name)
        {
            if (this.htDataArray == null)
            {
                return null;
            }
            return this.GetPoints((double[]) this.htDataArray[Name], false);
        }

        public PointF[] GetPoints(bool AddEmptyPoint, double[] Filter)
        {
            return this.GetPoints(this.Data, AddEmptyPoint, Filter);
        }

        public PointF[] GetPoints(double[] dd, bool AddEmptyPoint)
        {
            return this.GetPoints(dd, AddEmptyPoint, null);
        }

        public PointF[] GetPoints(string Name, bool AddEmptyPoint)
        {
            return this.GetPoints((double[]) this.htDataArray[Name], AddEmptyPoint);
        }

        public PointF[] GetPoints(double[] dd, bool AddEmptyPoint, double[] Filter)
        {
            return this.GetPoints(dd, AddEmptyPoint, Filter, false);
        }

        public PointF[] GetPoints(string Name, bool AddEmptyPoint, string Filter)
        {
            return this.GetPoints((double[]) this.htDataArray[Name], AddEmptyPoint, (double[]) this.htDataArray[Filter]);
        }

        public PointF[] GetPoints(double[] dd, bool AddEmptyPoint, double[] Filter, bool ValueOnly)
        {
            if (dd == null)
            {
                return null;
            }
            ArrayList list = new ArrayList();
            double minY = this.AxisY.MinY;
            double maxY = this.AxisY.MaxY;
            Rectangle rect = this.Canvas.Rect;
            if (this.AxisY.Transform != null)
            {
                minY = this.AxisY.Transform(minY);
                maxY = this.AxisY.Transform(maxY);
            }
            int num3 = 0;
            int num4 = 0;
            if (this.Canvas.DATE != null)
            {
                num3 = rect.Left + ((int) this.Canvas.ColumnWidth);
                num4 = rect.Left + ((int) (this.Canvas.ColumnWidth * (this.Canvas.Count - 1)));
            }
            int stop = this.Canvas.Stop;
            for (int i = (dd.Length - this.Canvas.Start) - 1; i >= stop; i--)
            {
                if (!double.IsNaN(dd[i]) && ((Filter == null) || ((Filter[i] != 0.0) && !double.IsNaN(Filter[i]))))
                {
                    float x;
                    if (this.Canvas.DATE != null)
                    {
                        x = this.Canvas.AxisX.GetX(this.Canvas.DATE[i], num3, num4);
                    }
                    else
                    {
                        x = this.Canvas.GetX(i);
                    }
                    double d = dd[i];
                    if (ValueOnly)
                    {
                        list.Add(new PointF(x, (float) i));
                    }
                    else if (minY != maxY)
                    {
                        if (this.AxisY.Transform != null)
                        {
                            d = this.AxisY.Transform(d);
                        }
                        list.Add(new PointF(x, this.AxisY.CalcY(d, maxY, minY)));
                    }
                    else
                    {
                        list.Add(new PointF(x, 0f));
                    }
                }
                else if (AddEmptyPoint)
                {
                    list.Add(PointF.Empty);
                }
            }
            return (PointF[]) list.ToArray(typeof(PointF));
        }

        public bool IsNaN()
        {
            for (int i = 0; i < this.Length; i++)
            {
                if (!double.IsNaN(this.Data[i]) && !double.IsInfinity(this.Data[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static void MakeSameLength(params FormulaData[] fs)
        {
            if ((fs != null) && (fs.Length != 0))
            {
                for (int i = 1; i < fs.Length; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        MakeSameLength(fs[i], fs[j]);
                    }
                }
            }
        }

        public static void MakeSameLength(FormulaData f1, FormulaData f2)
        {
            if (f1.Length != f2.Length)
            {
                if (f1.Length < f2.Length)
                {
                    f1.FillTo(f2.Length);
                }
                else
                {
                    f2.FillTo(f1.Length);
                }
            }
        }

        public double MaxValue()
        {
            return this.MaxValue(0, this.Length);
        }

        public double MaxValue(int Start, int Count)
        {
            double currentMax = this.MaxValue(this.Data, Start, Count, double.MinValue);
            if (this.htDataArray != null)
            {
                foreach (string str in this.htDataArray.Keys)
                {
                    if (!str.StartsWith("COND"))
                    {
                        currentMax = this.MaxValue((double[]) this.htDataArray[str], Start, Count, currentMax);
                    }
                }
            }
            return currentMax;
        }

        public double MaxValue(double[] dd, int Start, int Count, double CurrentMax)
        {
            double num = CurrentMax;
            for (int i = (dd.Length - Start) - 1; i >= Math.Max(0, (dd.Length - Start) - Count); i--)
            {
                if (!double.IsNaN(dd[i]) && !double.IsInfinity(dd[i]))
                {
                    num = Math.Max(num, dd[i]);
                }
            }
            return num;
        }

        public double MinValue()
        {
            return this.MinValue(0, this.Length);
        }

        public double MinValue(int Start, int Count)
        {
            if (this.RenderType == FormulaRenderType.VOLSTICK)
            {
                return 0.0;
            }
            double currentMin = this.MinValue(this.Data, Start, Count, double.MaxValue);
            if (this.htDataArray != null)
            {
                foreach (string str in this.htDataArray.Keys)
                {
                    if (!str.StartsWith("COND"))
                    {
                        currentMin = this.MinValue((double[]) this.htDataArray[str], Start, Count, currentMin);
                    }
                }
            }
            return currentMin;
        }

        public double MinValue(double[] dd, int Start, int Count, double CurrentMin)
        {
            double num = CurrentMin;
            for (int i = (this.Data.Length - Start) - 1; i >= Math.Max(0, (this.Data.Length - Start) - Count); i--)
            {
                if (!double.IsNaN(dd[i]))
                {
                    num = Math.Min(num, dd[i]);
                }
            }
            return num;
        }

        public static FormulaData operator +(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] + f2.Data[i];
            }
            return data;
        }

        public static FormulaData operator &(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = ((int) f1.Data[i]) & ((int) f2.Data[i]);
            }
            return data;
        }

        public static FormulaData operator |(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = ((int) f1.Data[i]) | ((int) f2.Data[i]);
            }
            return data;
        }

        public static FormulaData operator --(FormulaData f1)
        {
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] - 1.0;
            }
            return data;
        }

        public static FormulaData operator /(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] / f2.Data[i];
            }
            return data;
        }

        public static FormulaData operator ==(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = (f1.Data[i] == f2.Data[i]) ? ((double) 1) : ((double) 0);
            }
            return data;
        }

        public static FormulaData operator ^(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = ((int) f1.Data[i]) ^ ((int) f2.Data[i]);
            }
            return data;
        }

        public static FormulaData operator >(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = (f1.Data[i] > f2.Data[i]) ? ((double) 1) : ((double) 0);
            }
            return data;
        }

        public static FormulaData operator >=(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = (f1.Data[i] >= f2.Data[i]) ? ((double) 1) : ((double) 0);
            }
            return data;
        }

        public static implicit operator FormulaData(double N)
        {
            FormulaData data = new FormulaData(1);
            data.Data[0] = N;
            data.FormulaType = FormulaType.Const;
            return data;
        }

        public static implicit operator FormulaData(double[] dd)
        {
            return new FormulaData(dd);
        }

        public static implicit operator FormulaData(string s)
        {
            double num = 0.0;
            try
            {
                num = double.Parse(s);
            }
            catch
            {
            }
            return num;
        }

        public static FormulaData operator ++(FormulaData f1)
        {
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] + 1.0;
            }
            return data;
        }

        public static FormulaData operator !=(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = (f1.Data[i] != f2.Data[i]) ? ((double) 1) : ((double) 0);
            }
            return data;
        }

        public static FormulaData operator <(FormulaData f1, FormulaData f2)
        {
            return (FormulaData) (f2 > f1);
        }

        public static FormulaData operator <=(FormulaData f1, FormulaData f2)
        {
            return (FormulaData) (f2 >= f1);
        }

        public static FormulaData operator !(FormulaData f1)//op_LogicalNot(FormulaData f1)
        {
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                if (f1.Data[i] == 0.0)
                {
                    data.Data[i] = 1.0;
                }
                else
                {
                    data.Data[i] = 0.0;
                }
            }
            return data;
        }

        public static FormulaData operator %(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] % f2.Data[i];
            }
            return data;
        }

        public static FormulaData operator *(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1, f2 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] * f2.Data[i];
            }
            return data;
        }

        public static FormulaData operator ~(FormulaData f1)
        {
            if (FormulaBase.Testing)
            {
                return FormulaBase.TestData(0, new FormulaData[] { f1 });
            }
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = ~((int) f1.Data[i]);
            }
            return data;
        }

        public static FormulaData operator -(FormulaData f1, FormulaData f2)
        {
            MakeSameLength(f1, f2);
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = f1.Data[i] - f2.Data[i];
            }
            return data;
        }

        public static FormulaData operator -(FormulaData f1)
        {
            FormulaData data = new FormulaData(f1.Length);
            for (int i = 0; i < f1.Length; i++)
            {
                data.Data[i] = -f1.Data[i];
            }
            return data;
        }

        public static FormulaData operator +(FormulaData f1)
        {
            return new FormulaData(f1);
        }

        public void Set(double d)
        {
            for (int i = 0; i < this.Data.Length; i++)
            {
                this.Data[i] = d;
            }
        }

        public void SetAlign(string s)
        {
            try
            {
                this.Align = (FormulaAlign) int.Parse(s);
            }
            catch
            {
            }
        }

        public void SetAttr(string s)
        {
            try
            {
                this.RenderType = (FormulaRenderType) Enum.Parse(typeof(FormulaRenderType), s, true);
                return;
            }
            catch
            {

            }
          
            try
            {
                this.Dot = (FormulaDot) Enum.Parse(typeof(FormulaDot), s, true);
                return;
            }
            catch
            {
                
            }

            try
            {
                this.Align = (FormulaAlign) Enum.Parse(typeof(FormulaAlign), s, true);
                return;
            }
            catch
            {
                
            }

            try
            {
                this.Smoothing = (SmoothingMode) Enum.Parse(typeof(SmoothingMode), s, true);
                return;
            }
            catch
            {
                
            }

            try
            {
                this.VAlign = (VerticalAlign) Enum.Parse(typeof(VerticalAlign), s, true);
                return;
            }
            catch
            {
            }

            try
            {
                this.Transform = (Transform) Enum.Parse(typeof(Transform), s, true);
                return;
            }
            catch
            {
               
            }
            s = s.ToUpper();
            SetProperties(s);
        }

        private void SetProperties(string s)
        {
            if (s.StartsWith("COLOR"))
            {
                this.SetColor(s.Substring(5));
            }
            else if (s.StartsWith("WIDTH"))
            {
                this.SetWidth(s.Substring(5));
            }
            else if (s.StartsWith("LINETHICK"))
            {
                this.SetWidth(s.Substring(9));
            }
            else if (s.StartsWith("ALIGN"))
            {
                this.SetAlign(s.Substring(5));
            }
            else if (s.StartsWith("BRUSH"))
            {
                this.SetBrush(s.Substring(5));
            }
            else if (s.StartsWith("LABEL"))
            {
                this.SetLabel(s.Substring(5));
            }
            else if (s.StartsWith("VALIGN"))
            {
                this.SetVAlign(s.Substring(6));
            }
            else if (s.StartsWith("AXISMARGIN"))
            {
                this.SetAxisMargin(s.Substring(10));
            }
            else if (s.StartsWith("STYLE"))
            {
                this.SetStyle(s.Substring(5));
            }
        }

        public void SetAttrs(string s)
        {
            foreach (string str in s.Split(new char[] { ',' }))
            {
                this.SetAttr(str);
            }
        }

        private void SetAxisMargin(string s)
        {
            try
            {
                int num = int.Parse(s);
                this.AxisMargin = num;
            }
            catch
            {
            }
        }

        private void SetBrush(string s)
        {
            try
            {
                int alpha = 30;
                if (s.Length == 9)
                {
                    alpha = Convert.ToInt32(s.Substring(1, 2), 16);
                    string htmlcolorHex = "#" + s.Substring(2);
                    this.AreaBrush = new SolidBrush(Color.FromArgb(alpha, ColorTranslator.FromHtml(htmlcolorHex)));
                }
                else if (s.Length == 7)
                {
                    this.AreaBrush = new SolidBrush(Color.FromArgb(alpha, ColorTranslator.FromHtml(s)));
                }
            }
            catch
            {
            }
        }

        private void SetColor(string s)
        {
            try
            {
                this.FormulaColor = ColorTranslator.FromHtml(s);
            }
            catch
            {
            }
        }

        private void SetLabel(string s)
        {
            try
            {
                this.LabelIndex = int.Parse(s);
            }
            catch
            {
            }
        }

        private void SetStyle(string s)
        {
            try
            {
                this.DashStyle = (System.Drawing.Drawing2D.DashStyle) Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle), s, true);
            }
            catch
            {
            }
        }

        private void SetVAlign(string s)
        {
            try
            {
                this.VAlign = (VerticalAlign) int.Parse(s);
            }
            catch
            {
            }
        }

        private void SetWidth(string s)
        {
            try
            {
                int num = int.Parse(s);
                if ((num >= 0) && (num < 8))
                {
                    this.LineWidth = num;
                }
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            return this.ToString("f2");
        }

        public string ToString(string format)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.Length; i++)
            {
                if (i != 0)
                {
                    builder.Append(',');
                }
                builder.Append(this.Data[i].ToString(format));
            }
            return builder.ToString();
        }

        public double Avg
        {
            get
            {
                double num = 0.0;
                for (int i = 0; i < this.Length; i++)
                {
                    num += this.Data[i];
                }
                return (num / ((double) this.Length));
            }
        }

        public double ConstValue
        {
            get
            {
                if (this.Data.Length > 0)
                {
                    return this.Data[0];
                }
                return 0.0;
            }
        }

        public int ExtraDataCount
        {
            get
            {
                if (this.htDataArray == null)
                {
                    return 0;
                }
                return this.htDataArray.Count;
            }
        }

        public double this[int N]
        {
            get
            {
                return this.Data[N];
            }
            set
            {
                this.Data[N] = value;
            }
        }

        public double this[double N]
        {
            get
            {
                return this[(int) N];
            }
        }

        public double[] this[string Name]
        {
            get
            {
                if (this.htDataArray == null)
                {
                    this.htDataArray = new Hashtable();
                }
                return (double[]) this.htDataArray[Name];
            }
            set
            {
                if (this.htDataArray == null)
                {
                    this.htDataArray = new Hashtable();
                }
                this.htDataArray[Name] = value;
            }
        }

        public double LASTDATA
        {
            get
            {
                return this[this.Length - 1];
            }
        }

        public int Length
        {
            get
            {
                return this.Data.Length;
            }
        }

        public Hashtable OwnerData
        {
            get
            {
                if (this.htData == null)
                {
                    this.htData = new Hashtable();
                }
                return this.htData;
            }
        }
    }
}

