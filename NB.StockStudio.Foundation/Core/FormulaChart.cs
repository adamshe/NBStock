namespace NB.StockStudio.Foundation
{
    using NB.StockStudio.Foundation.DataProvider;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;
    using System.Web.Caching;

    public class FormulaChart
    {
        public XFormatCollection AllXFormats;
        public AreaCollection Areas;
        public bool BitmapCache;
        public float ColumnPercent;
        private double columnWidth;
        private Graphics CurrentGraph;
        public Pen CursorPen;
        public int CursorPos;
        private IDataProvider dataProvider;
        public DateTime EndTime;
        private Bitmap ExtraMemBmp;
        public bool FixedTime;
        private int LabelWidth;
        private RectangleF LastHLine;
        private RectangleF LastVLine;
        public LatestValueType LatestValueType;
        public int MarginWidth;
        private Bitmap MemBmp;
        private Graphics MemG;
        public bool NeedRedraw;
        public Rectangle Rect;
        public int RenderCount;
        private static Random Rnd = new Random();
        public bool ShowCursorLabel;
        public bool ShowDateInLastArea;
        public bool ShowHLine;
        public bool ShowValueLabel;
        public bool ShowVLine;
        public int Start;
        public DateTime StartTime;
        public string Code;

        public event NativePaintHandler ExtraPaint;

        public event NativePaintHandler NativePaint;

        public event ViewChangedHandler ViewChanged;

        public FormulaChart()
        {
            this.columnWidth = 7.0;
            this.LastHLine = Rectangle.Empty;
            this.LastVLine = Rectangle.Empty;
            this.LatestValueType = LatestValueType.None;
            this.NeedRedraw = true;
            this.ShowHLine = true;
            this.ShowVLine = true;
            this.CursorPen = Pens.Black;
            this.MarginWidth = 6;
            this.ColumnPercent = 0.6f;
            this.StartTime = DateTime.MinValue;
            this.EndTime = DateTime.MaxValue;
            this.ShowValueLabel = true;
            this.Areas = new AreaCollection();
            this.EndTime = DateTime.Now.AddDays(1.0);
            this.StartTime = this.EndTime.AddMonths(-6);
        }

        public FormulaChart(Rectangle Rect) : this()
        {
            this.ColumnWidth = 7.0;
            this.Start = 0;
            this.Rect = Rect;
        }

        public void AddArea(FormulaBase fb)
        {
            FormulaArea fa = new FormulaArea(this);
            fa.HeightPercent = 1.0;
            fa.AddFormula(fb);
            this.Areas.Add(fa);
        }

        public void AddArea(string Name)
        {
            this.AddArea(Name, 1.0);
        }

        public void AddArea(string Name, double Percent)
        {
            this.AddArea(Name, "", Percent);
        }

        public void AddArea(string Name, string Quote, double Percent)
        {
            this.Areas.Add(new FormulaArea(this, Name, Quote, Percent));
        }

        public void AddAreas(string Indicators)
        {
            if ((Indicators != null) && (Indicators != ""))
            {
                foreach (string str in Indicators.Split(new char[] { ';' }))
                {
                    int index = str.IndexOf('(');
                    int num2 = str.IndexOf(')');
                    string name = str;
                    int num3 = 1;
                    if (num2 > index)
                    {
                        name = str.Substring(0, index);
                        try
                        {
                            num3 = int.Parse(str.Substring(index + 1, (num2 - index) - 1));
                        }
                        catch
                        {
                        }
                    }
                    this.AddArea(name, (double) num3);
                }
            }
        }

        public void AddOverlays(string Overlays)
        {
            if (((Overlays != null) && (Overlays != "")) && (this.Areas.Count > 0))
            {
                foreach (string str in Overlays.Split(new char[] { ';' }))
                {
                    this[0].AddFormula(str);
                }
            }
        }

        public void AdjustCursorByPos(Graphics g, int NewPos, int NewStart)
        {
            this.AdjustCursorByPos(g, NewPos, NewStart, false);
        }

        public void AdjustCursorByPos(Graphics g, int NewPos, int NewStart, bool NeedDrawCursor)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if (!area.IsMain())
                {
                    continue;
                }
                double[] numArray = this.DataProvider["DATE"];
                FormulaData fd = this.DataProvider["CLOSE"];
                if (fd.Length <= 0)
                {
                    break;
                }
                int start = this.Start;
                this.Start = NewStart;
                if (this.Start < 0)
                {
                    this.Start = 0;
                }
                if (this.Start > (fd.Length - 1))
                {
                    this.Start = fd.Length - 1;
                }
                if (NewPos < 0)
                {
                    NewPos = 0;
                }
                if (NewPos > (fd.Length - 1))
                {
                    NewPos = fd.Length - 1;
                }
                if (NewPos != this.CursorPos)
                {
                    if (NewPos < area.Canvas.Stop)
                    {
                        this.Start += area.Canvas.Stop - NewPos;
                    }
                    if (NewPos > ((fd.Length - 1) - this.Start))
                    {
                        this.Start = (fd.Length - 1) - NewPos;
                    }
                    this.CursorPos = NewPos;
                }
                if (NeedDrawCursor)
                {
                    this.DrawCursor(g, area, fd);
                }
            }
        }

        private void AdjustLabelWidth(Graphics g)
        {
            this.LabelWidth = -2147483648;
            foreach (FormulaArea area in this.Areas)
            {
                if (area.Visible)
                {
                    this.LabelWidth = Math.Max(this.LabelWidth, area.CalcLabelWidth(g));
                }
            }
            this.LabelWidth += 6;
            foreach (FormulaArea area2 in this.Areas)
            {
                if (area2.Visible)
                {
                    area2.AxisY.Width = this.LabelWidth;
                }
            }
            foreach (FormulaArea area3 in this.Areas)
            {
                if (area3.Visible)
                {
                    for (int i = 1; i < area3.AxisYs.Count; i++)
                    {
                        area3.AxisYs[i].Width = area3.AxisYs[i].CalcLabelWidth(area3.Canvas) + 6;
                    }
                }
            }
        }

        public void AdjustStartEndTime()
        {
            this.AdjustStartEndTime((int) (((double) (this.Rect.Width - this.LabelWidth)) / this.columnWidth));
        }

        public void AdjustStartEndTime(int InViewBarsCount)
        {
            if (this.DataProvider != null)
            {
                double[] numArray = this.DataProvider["DATE"];
                if (numArray != null)
                {
                    this.EndTime = DateTime.FromOADate(numArray[(numArray.Length - 1) - this.Start]);
                    if (this.columnWidth < 1E-05)
                    {
                        this.columnWidth = 1E-05;
                    }
                    int num = this.Start + InViewBarsCount;
                    if (num >= numArray.Length)
                    {
                        num = numArray.Length - 1;
                        this.Start = 0;
                        this.EndTime = DateTime.FromOADate(numArray[0] + InViewBarsCount);
                    }
                    this.StartTime = DateTime.FromOADate(numArray[(numArray.Length - 1) - num]);
                }
            }
        }

        public void ApplyXFormat(double Days100)
        {
            if (this.AllXFormats != null)
            {
                foreach (FormulaXFormat format in this.AllXFormats)
                {
                    if (Days100 < format.Days100Pixel)
                    {
                        int repeat = format.Interval.Repeat;
                        if (format.CycleDivide > 0.0)
                        {
                            repeat = (int) (Days100 / format.CycleDivide);
                        }
                        if (repeat == 0)
                        {
                            repeat = 1;
                        }
                        this.DataCycle = new DataCycle(format.Interval.CycleBase, repeat);
                        this.XCursorFormat = format.XCursorFormat;
                        this.AxisLabelAlign = format.AxisLabelAlign;
                        this.AxisXFormat = format.XFormat;
                        if (format.Visible != null)
                        {
                            format.SetVisible(this);
                        }
                        if (format.ShowMajorLine != null)
                        {
                            format.SetMajorLine(this);
                        }
                        if (format.ShowMinorLine != null)
                        {
                            format.SetMinorLine(this);
                        }
                        break;
                    }
                }
            }
        }

        public void Bind()
        {
            this.CursorPos = -1;
            foreach (FormulaArea area in this.Areas)
            {
                area.Bind();
            }
        }

        public static FormulaChart CreateChart(IDataProvider idp)
        {
            return CreateChart(idp, "RedWhite");
        }

        public static FormulaChart CreateChart(IDataProvider idp, string Skin)
        {
            return CreateChart("Native.Main(3);VOLMA;SlowSTO;MACD", "Native.MA(14);Native.MA(28)", idp, Skin);
        }

        public static FormulaChart CreateChart(string Indicators, string Overlays, IDataProvider DataProvider, string Skin)
        {
            FormulaChart chart = new FormulaChart();
            chart.AddAreas(Indicators);
            chart.AddOverlays(Overlays);
            if (DataProvider != null)
            {
                chart.DataProvider = DataProvider;
            }
            if ((Skin != null) && (Skin != ""))
            {
                chart.SetSkin(Skin);
            }
            return chart;
        }


        public static void WaterMark(Graphics g, RectangleF r, string text, float angle = 0)
        {
            var color = Color.FromArgb(120, Color.Cyan);
            using (var pen = new Pen(color, 5))
            using (var gp = new GraphicsPath())
            using (var font = new Font("Arial", 160))
            using (var brush = new SolidBrush(color))
            {
                var sf = new StringFormat();
                sf.LineAlignment = sf.Alignment = StringAlignment.Center;
                gp.AddString(text, font.FontFamily, (int)font.Style, font.SizeInPoints,
                             new RectangleF(-r.Width / 2, -r.Height / 2, r.Width, r.Height),
                             sf);
                g.TranslateTransform(r.Width / 2, r.Height / 2);
                //   g.RotateTransform(-angle);
                g.DrawPath(pen, gp);
            }
        }

        public void DrawCursor(Graphics g)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if (area.IsMain())
                {
                    FormulaData data = this.DataProvider["CLOSE"];
                    if ((this.CursorPos >= 0) && (this.CursorPos < data.Length))
                    {
                        float x = this.Rect.X + area.Canvas.GetX(this.CursorPos);
                        float y = this.Rect.Y + area.AxisY.CalcY(data[this.CursorPos]);
                        this.DrawCursor(g, x, y, false);                       
                    }
                }
            }
        }

        public void DrawCursor(Graphics g, FormulaArea fa, FormulaData fd)
        {
            float x = this.Rect.X + fa.Canvas.GetX(this.CursorPos);
            float y = this.Rect.Y + fa.AxisY.CalcY(fd[this.CursorPos]);
            this.DrawCursor(g, x, y);
        }

        public void DrawCursor(Graphics g, float X, float Y)
        {
            this.DrawCursor(g, X, Y, true);
        }

        public void DrawCursor(Graphics g, float X, float Y, bool ChangeCursorPosByXY)
        {
            if ((this.DataProvider != null) && this.ShowCursorLabel)
            {
                this.CurrentGraph = g;
                FormulaHitInfo hitInfo = this.GetHitInfo(X, Y);
                if (hitInfo.HitType != FormulaHitType.htArea)
                {
                    this.HideCursor(g);
                }
                else if (this.Rect.Contains((int) X, (int) Y))
                {
                    int num = 0;
                    if (this.ShowHLine)
                    {
                        if ((this.BitmapCache && !this.LastHLine.IsEmpty) && (this.MemBmp != null))
                        {
                            this.RestoreMemBmp(g, this.LastHLine);
                        }
                        g.DrawLine(this.CursorPen, (float) this.Rect.X, Y, (float) this.Rect.Right, Y);
                        if (Y != Math.Floor((double) Y))
                        {
                            num = 1;
                        }
                        this.LastHLine.X = 0f;
                        this.LastHLine.Y = (Y - this.Rect.Y) - num;
                        this.LastHLine.Width = this.Rect.Width;
                        this.LastHLine.Height = this.CursorPen.Width + num;

                    }
                    if (this.ShowVLine)
                    {
                        if ((this.BitmapCache && !this.LastVLine.IsEmpty) && (this.MemBmp != null))
                        {
                            this.RestoreMemBmp(g, this.LastVLine);
                        }
                        g.DrawLine(this.CursorPen, X, (float) this.Rect.Y, X, (float) this.Rect.Bottom);
                        if (X != Math.Floor((double) X))
                        {
                            num = 1;
                        }
                        this.LastVLine.X = (X - this.Rect.X) - num;
                        this.LastVLine.Y = 0f;
                        this.LastVLine.Width = this.CursorPen.Width + num;
                        this.LastVLine.Height = this.Rect.Height;
                        
                    }
                    if (ChangeCursorPosByXY)
                    {
                        this.SetCursorPos(X, Y);
                    }
                    this.DrawValueText(g);
                    foreach (FormulaArea area in this.Areas)
                    {
                        if (area.Visible && (area != hitInfo.Area))
                        {
                            foreach (FormulaAxisY sy in area.AxisYs)
                            {
                                this.RestoreMemBmp(g, sy.LastCursorRect);
                                sy.LastCursorRect = RectangleF.Empty;
                            }
                            continue;
                        }
                    }
                    foreach (FormulaAxisY sy2 in hitInfo.Area.AxisYs)
                    {
                        sy2.DrawCursor(g, this, hitInfo.Area, Y);
                    }
                    for (int i = this.Areas.Count - 1; i >= 0; i--)
                    {
                        if (this.Areas[i].AxisX.Visible)
                        {
                            this.Areas[i].AxisX.DrawCursor(g, this, this.Areas[i], X);
                            break;
                        }
                    }

                  //  WaterMark(g, this.Areas[0].Rect, this.Code);
                }
            }
        }

        public static void DrawLastestData(Graphics g, IDataProvider idp, Font F, float X, float Y, float Width, bool AlignRight, Color[] Colors, string Format)
        {
            int count = idp.Count;
            double num2 = 0.0;
            if (count > 1)
            {
                num2 = idp["CLOSE"][count - 2];
            }
            double num3 = idp["OPEN"][count - 1];
            double num4 = idp["HIGH"][count - 1];
            double num5 = idp["LOW"][count - 1];
            double num6 = idp["CLOSE"][count - 1];
            double num7 = idp["VOLUME"][count - 1];
            for (int i = AlignRight ? 0 : 1; i < 2; i++)
            {
                float currentX = X;
                Brush bText = new SolidBrush(Colors[3]);
                Font f = new Font(F, FontStyle.Bold);
                if (num2 != 0.0)
                {
                    DrawString(g, "Prev Close:", f, bText, ref currentX, Y, i);
                    DrawString(g, num2.ToString(Format, NumberFormatInfo.InvariantInfo), F, GetBrush(num2, num2, Colors), ref currentX, Y, i);
                }
                DrawString(g, "O:", f, bText, ref currentX, Y, i);
                DrawString(g, num3.ToString(Format, NumberFormatInfo.InvariantInfo), F, GetBrush(num2, num3, Colors), ref currentX, Y, i);
                DrawString(g, "H:", f, bText, ref currentX, Y, i);
                DrawString(g, num4.ToString(Format, NumberFormatInfo.InvariantInfo), F, GetBrush(num2, num4, Colors), ref currentX, Y, i);
                DrawString(g, "L:", f, bText, ref currentX, Y, i);
                DrawString(g, num5.ToString(Format, NumberFormatInfo.InvariantInfo), F, GetBrush(num2, num5, Colors), ref currentX, Y, i);
                DrawString(g, "C:", f, bText, ref currentX, Y, i);
                DrawString(g, num6.ToString(Format, NumberFormatInfo.InvariantInfo), F, GetBrush(num2, num6, Colors), ref currentX, Y, i);
                DrawString(g, "V:", f, bText, ref currentX, Y, i);
                DrawString(g, num7.ToString(NumberFormatInfo.InvariantInfo), F, GetBrush(num2, num6, Colors), ref currentX, Y, i);
                if (num2 != 0.0)
                {
                    DrawString(g, "Chg:", f, bText, ref currentX, Y, i);
                    double num10 = num6 - num2;
                    DrawString(g, num10.ToString("+0.##;-0.##;0", NumberFormatInfo.InvariantInfo) + "(" + ((num10 / num2)).ToString("p2", NumberFormatInfo.InvariantInfo) + ")", F, GetBrush(num2, num6, Colors), ref currentX, Y, i);
                }
                X = Width - currentX;
            }
        }

        private static void DrawString(Graphics g, string Text, Font F, Brush bText, ref float CurrentX, float Y, int Mode)
        {
            if (Mode == 1)
            {
                g.DrawString(Text, F, bText, CurrentX, Y);
            }
            CurrentX += g.MeasureString(Text, F).Width;
        }

        public void DrawValueText(Graphics g)
        {
            if (this.DataProvider != null)
            {
                foreach (FormulaArea area in this.Areas)
                {
                    if (area.Visible)
                    {
                        area.DrawValueText(g);
                    }
                }
            }
        }

        public void DrawValueText(float X, float Y)
        {
            this.SetCursorPos(X, Y);
            this.DrawValueText(this.CurrentGraph);
        }

        public static int FindIndex(double[] dd, double d)
        {
            return FindIndex(dd, d, 0);
        }

        public static int FindIndex(double[] dd, double d, int Dir)
        {
            int num = 0;
            int num2 = dd.Length - 1;
            while (num < num2)
            {
                int index = ((num + num2) - Dir) / 2;
                if (dd[index] < d)
                {
                    num = (index + Dir) + 1;
                }
                else
                {
                    if (dd[index] > d)
                    {
                        num2 = index + Dir;
                        continue;
                    }
                    return index;
                }
            }
            return num;
        }

        public string GetAreaTextData(string Separator, bool ShowHeader)
        {
            if (this.Areas.Count > 0)
            {
                FormulaArea selectedArea = this.SelectedArea;
                if (selectedArea == null)
                {
                    selectedArea = this.Areas[this.Areas.Count - 1];
                }
                return this.GetAreaTextData(selectedArea, Separator, ShowHeader);
            }
            return "";
        }

        public string GetAreaTextData(FormulaArea fa, string Separator, bool ShowHeader)
        {
            return this.GetTextData(fa.FormulaDataArray, Separator, ShowHeader);
        }

        public Bitmap GetBitmap(int Width, int Height)
        {
            return this.GetBitmap(Width, Height, new Rectangle(0, 0, Width, Height - 1));
        }

        public Bitmap GetBitmap(int Width, int Height, Rectangle R)
        {
            Bitmap image = new Bitmap(Width, Height);
            this.Rect = R;
            Graphics g = Graphics.FromImage(image);
            this.Render(g);
            return image;
        }

        private static Brush GetBrush(double D1, double D2, Color[] C)
        {
            if (D1 == 0.0)
            {
                return new SolidBrush(C[1]);
            }
            return new SolidBrush(C[D1.CompareTo(D2) + 1]);
        }

        public string GetChartTextData(string Separator, bool ShowHeader)
        {
            FormulaPackage formulaDataArray = new FormulaPackage();
            foreach (FormulaArea area in this.Areas)
            {
                formulaDataArray.AddRange(area.FormulaDataArray);
            }
            return this.GetTextData(formulaDataArray, Separator, ShowHeader);
        }

        public FormulaHitInfo GetHitInfo(float X, float Y)
        {
            FormulaHitInfo info = new FormulaHitInfo();
            if (this.DataProvider != null)
            {
                X -= this.Rect.X;
                Y -= this.Rect.Y;
                info.X = X;
                info.Y = Y;
                info.HitType = FormulaHitType.htNoWhere;
                int x = (int) X;
                int y = (int) Y;
                int num3 = 0;
                foreach (FormulaArea area in this.Areas)
                {
                    if (!area.Visible)
                    {
                        continue;
                    }
                    if (num3 == 0)
                    {
                        Rectangle rect = area.Canvas.Rect;
                        int num4 = ((int) X) - rect.X;
                        info.CursorPos = area.Canvas.Stop + ((int) (((double) num4) / this.ColumnWidth));
                        num3++;
                    }
                    if (Math.Abs((int) (area.Rect.Bottom - y)) < 3)
                    {
                        info.Area = area;
                        info.HitType = FormulaHitType.htSize;
                        return info;
                    }
                    if (area.Rect.Contains(x, y))
                    {
                        info.Area = area;
                        info.HitType = FormulaHitType.htArea;
                        foreach (FormulaAxisY sy in area.AxisYs)
                        {
                            if (sy.Visible && sy.FrameRect.Contains(x, y))
                            {
                                info.HitType = FormulaHitType.htAxisY;
                                info.AxisY = sy;
                                break;
                            }
                        }
                        foreach (FormulaAxisX sx in area.AxisXs)
                        {
                            if (sx.Visible && sx.Rect.Contains(x, y))
                            {
                                info.HitType = FormulaHitType.htAxisX;
                                info.AxisX = sx;
                                break;
                            }
                        }
                        continue;
                    }
                }
            }
            return info;
        }

        public Bitmap GetMemBitmap()
        {
            return this.GetMemBitmap(this.Rect);
        }

        public Bitmap GetMemBitmap(Rectangle R)
        {
            R.Offset(-R.X, -R.Y);
            R.Inflate(1, 1);
            if (this.NeedRedraw)
            {
                this.SetMemGraphics(R);
                this.InternalRender(this.MemG);
                this.DrawValueText(this.MemG);
            }
            if (this.ExtraPaint != null)
            {
                NativePaintArgs e = new NativePaintArgs(this.CurrentGraph, R, this.MemBmp);
                this.ExtraPaint(this, e);
                if (e.NewBitmap != null)
                {
                    this.ExtraMemBmp = e.NewBitmap;
                }
            }
            if (this.ExtraMemBmp != null)
            {
                return this.ExtraMemBmp;
            }
            return this.MemBmp;
        }

        public PointF GetPointAt(DateTime DateIndex, double Price)
        {
            return this.GetPointAt(this.MainArea, DateIndex, null, Price);
        }

        public PointF GetPointAt(DateTime DateIndex, string DataType)
        {
            return this.GetPointAt(DateIndex, DataType, 0.0);
        }

        public PointF GetPointAt(int DateIndex, double Price)
        {
            return this.GetPointAt(this.MainArea, DateIndex, Price);
        }

        public PointF GetPointAt(FormulaArea fa, DateTime DateIndex, double Price)
        {
            return this.GetPointAt(fa, DateIndex, null, Price);
        }

        public PointF GetPointAt(FormulaArea fa, int DateIndex, double Price)
        {
            if (((fa != null) && (fa.Canvas != null)) && (fa.AxisY != null))
            {
                float x = (float) (((DateIndex - fa.Canvas.Stop) + 0.5) * this.ColumnWidth);
                float y = fa.AxisY.CalcY(Price);
                foreach (FormulaAxisY sy in fa.AxisYs)
                {
                    if (sy.AxisPos == AxisPos.Left)
                    {
                        x += sy.Width;
                    }
                }
                return new PointF(x, y);
            }
            return PointF.Empty;
        }

        public PointF GetPointAt(DateTime DateIndex, string DataType, double Price)
        {
            return this.GetPointAt(this.MainArea, DateIndex, DataType, Price);
        }

        public PointF GetPointAt(double d, string DataType, double Price)
        {
            return this.GetPointAt(this.MainArea, d, DataType, Price);
        }

        public PointF GetPointAt(string AreaName, int DateIndex, double Price)
        {
            return this.GetPointAt(this[AreaName], DateIndex, Price);
        }

        public PointF GetPointAt(FormulaArea fa, DateTime DateIndex, string DataType, double Price)
        {
            double d = DateIndex.ToOADate();
            return this.GetPointAt(fa, d, DataType, Price);
        }

        public PointF GetPointAt(FormulaArea fa, double d, string DataType, double Price)
        {
            if (this.DataProvider != null)
            {
                double[] dd = this.DataProvider["DATE"];
                double[] numArray2 = null;
                if (DataType != null)
                {
                    numArray2 = this.DataProvider[DataType];
                }
                if ((dd != null) && (dd.Length > 0))
                {
                    int index = FindIndex(dd, d);
                    if (numArray2 != null)
                    {
                        Price = numArray2[index];
                    }
                    return this.GetPointAt(fa, index, Price);
                }
            }
            return PointF.Empty;
        }

        public PointF GetPointAt(string AreaName, double d, string DataType, double Price)
        {
            return this.GetPointAt(this[AreaName], d, DataType, Price);
        }

        public double GetPriceAt(DateTime DateIndex, string DataType)
        {
            double d = DateIndex.ToOADate();
            if (this.DataProvider != null)
            {
                double[] dd = this.DataProvider["DATE"];
                double[] numArray2 = this.DataProvider[DataType];
                if ((dd != null) && (dd.Length > 0))
                {
                    int index = FindIndex(dd, d);
                    if (numArray2 != null)
                    {
                        return numArray2[index];
                    }
                }
            }
            return double.NaN;
        }

        private string GetTextData(FormulaPackage FormulaDataArray, string Separator, bool ShowHeader)
        {
            FormulaData data = this.DataProvider["DATE"];
            data.Name = "Date";
            ArrayList list = new ArrayList();
            list.Add(data);
            list.AddRange(FormulaDataArray);
            StringBuilder builder = new StringBuilder();
            if (list.Count > 0)
            {
                for (int i = -(ShowHeader ? 1 : 0); i < data.Length; i++)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        FormulaData data2 = (FormulaData) list[j];
                        if (ShowHeader && (i < 0))
                        {
                            builder.Append(data2.Name);
                        }
                        if (i >= 0)
                        {
                            if (j == 0)
                            {
                                builder.Append(DateTime.FromOADate(data2[i]).ToString("dd-MMM-yy", DateTimeFormatInfo.InvariantInfo));
                            }
                            else
                            {
                                builder.Append(data2[i].ToString("f2", NumberFormatInfo.InvariantInfo));
                            }
                        }
                        if (j < (FormulaDataArray.Count - 1))
                        {
                            builder.Append(Separator);
                        }
                    }
                    builder.Append("\r\n");
                }
            }
            return builder.ToString();
        }

        public ObjectPoint GetValueFromPos(float X, float Y)
        {
            FormulaArea fa = null;
            return this.GetValueFromPos(X, Y, ref fa);
        }

        public ObjectPoint GetValueFromPos(float X, float Y, ref FormulaArea fa)
        {
            ObjectPoint point = new ObjectPoint();
            FormulaHitInfo hitInfo = this.GetHitInfo(X, Y);
            if (fa == null)
            {
                fa = hitInfo.Area;
            }
            point.Y = fa.AxisY.GetValueFromY(Y - this.Rect.Y) * fa.AxisY.MultiplyFactor;
            int cursorPos = hitInfo.CursorPos;
            double[] numArray = this.DataProvider["DATE"];
            if ((numArray != null) && (numArray.Length > 0))
            {
                if (cursorPos < 0)
                {
                    point.X = numArray[0] + cursorPos;
                    return point;
                }
                if (cursorPos >= numArray.Length)
                {
                    point.X = numArray[numArray.Length - 1] + ((cursorPos - numArray.Length) + 1);
                    return point;
                }
                point.X = numArray[cursorPos];
            }
            return point;
        }

        public void HideCursor(Graphics g)
        {
            if (this.BitmapCache && (this.MemBmp != null))
            {
                this.RestoreMemBmp(g, this.LastHLine);
            }
            if (this.BitmapCache && (this.MemBmp != null))
            {
                this.RestoreMemBmp(g, this.LastVLine);
            }
            foreach (FormulaArea area in this.Areas)
            {
                foreach (FormulaAxisY sy in area.AxisYs)
                {
                    this.RestoreMemBmp(g, sy.LastCursorRect);
                    sy.LastCursorRect = RectangleF.Empty;
                }
                this.RestoreMemBmp(g, area.AxisX.LastCursorRect);
                area.AxisX.LastCursorRect = RectangleF.Empty;
            }
        }

        public void InsertArea(int Index, FormulaBase fb)
        {
            FormulaArea fa = new FormulaArea(this);
            fa.HeightPercent = 1.0;
            fa.AddFormula(fb);
            this.Areas.Insert(Index, fa);
        }

        public void InsertArea(int Index, string Name)
        {
            this.InsertArea(Index, Name, 1.0);
        }

        public void InsertArea(int Index, string Name, double Percent)
        {
            this.InsertArea(Index, Name, "", Percent);
        }

        public void InsertArea(int Index, string Name, string Quote, double Percent)
        {
            this.Areas.Insert(Index, new FormulaArea(this, Name, Quote, Percent));
        }

        private void InternalRender(Graphics g)
        {
            try
            {
                if (this.DataProvider != null)
                {
                    double num = 0.0;
                    foreach (FormulaArea area in this.Areas)
                    {
                        if (area.Visible)
                        {
                            num += area.HeightPercent;
                        }
                    }
                    double num2 = 0.0;
                    for (int i = 0; i < this.Areas.Count; i++)
                    {
                        FormulaArea area2 = this.Areas[i];
                        if (area2.Visible)
                        {
                            area2.Rect = new Rectangle(this.Rect.X, this.Rect.Y + ((int) ((this.Rect.Height * num2) / num)), this.Rect.Width, ((int) ((this.Rect.Height * area2.HeightPercent) / num)) + 1);
                            if (i < (this.Areas.Count - 1))
                            {
                                area2.Rect.Height++;
                            }
                            else
                            {
                                area2.Rect.Height = this.Rect.Bottom - area2.Rect.Top;
                            }
                            num2 += area2.HeightPercent;
                        }
                    }
                    this.AdjustLabelWidth(g);
                    this.SetView();
                    for (int j = 0; j < this.Areas.Count; j++)
                    {
                        FormulaArea formulaPanel = this.Areas[j];
                        if (formulaPanel.Visible)
                        {
                            try
                            {
                                if (this.ShowDateInLastArea && (j < (this.Areas.Count - 1)))
                                {
                                    foreach (FormulaAxisX sx in formulaPanel.AxisXs)
                                    {
                                        sx.Visible = false;
                                    }
                                }
                                formulaPanel.Render(g);
               //                 if (j == 0)
               //                     formulaPanel.Back.WaterMark(g, formulaPanel.Rect, this.Code);
                            }
                            catch (Exception exception)
                            {
                                StringFormat format = new StringFormat();
                                format.Alignment = StringAlignment.Center;
                                format.LineAlignment = StringAlignment.Center;
                                g.DrawString(exception.ToString(), new Font("verdana", 10f), Brushes.Red, formulaPanel.Rect, format);
                            }
                        }
                    }
                    if (this.NativePaint != null)
                    {
                        this.NativePaint(this, new NativePaintArgs(g, this.Areas[0].Rect, this.MemBmp));
                    }
                }
            }
            catch (Exception exception2)
            {
                g.DrawString(exception2.ToString(), new Font("verdana", 10f), Brushes.Red, (float) 1f, (float) 30f);
            }
        }

        public void RemoveArea(string Name)
        {
            this.Areas.Remove(Name);
        }

        public void Render(Graphics g)
        {
            this.CurrentGraph = g;
            if (this.BitmapCache)
            {
                g.DrawImage(this.GetMemBitmap(), this.Rect.X, this.Rect.Y);
                this.NeedRedraw = false;
            }
            else
            {
                this.InternalRender(g);
                this.DrawValueText(g);
            }
            this.RenderCount++;
        }

        public void RestoreMemBmp(Graphics g, RectangleF R)
        {
            if (!R.IsEmpty && this.BitmapCache)
            {
                RectangleF destRect = R;
                destRect.Offset((float) this.Rect.X, (float) this.Rect.Y);
                if (this.ExtraMemBmp != null)
                {
                    g.DrawImage(this.ExtraMemBmp, destRect, R, GraphicsUnit.Pixel);                   
                }
                else
                {
                    g.DrawImage(this.MemBmp, destRect, R, GraphicsUnit.Pixel);                   
                }
            }
        }

        public int SaveToImageStream(Stream s)
        {
            if (HttpRuntime.Cache != null)
            {
                int num = Rnd.Next(0x7fffffff);
                HttpRuntime.Cache.Add(num.ToString(), s, null, DateTime.Now.AddSeconds(40.0), TimeSpan.Zero, CacheItemPriority.High, null);
                return num;
            }
            return -1;
        }

        public int SaveToImageStream(int Width, int Height, ImageFormat ifm, float X, float Y)
        {
            MemoryStream stream = new MemoryStream();
            this.SaveToStream(stream, Width, Height, new Rectangle(0, 0, Width, Height), ifm, X, Y);
            return this.SaveToImageStream(stream);
        }

        public void SaveToStream(Stream stream, int Width, int Height)
        {
            this.SaveToStream(stream, Width, Height, new Rectangle(0, 0, Width, Height), ImageFormat.Png);
        }

        public void SaveToStream(Stream stream, int Width, int Height, Rectangle R, ImageFormat ifm)
        {
            this.SaveToStream(stream, Width, Height, R, ifm, 0f, 0f);
        }

        public void SaveToStream(Stream stream, int Width, int Height, float X, float Y)
        {
            this.SaveToStream(stream, Width, Height, new Rectangle(0, 0, Width, Height), ImageFormat.Png, X, Y);
        }

        public void SaveToStream(Stream stream, int Width, int Height, Rectangle R, ImageFormat ifm, float X, float Y)
        {
            Bitmap image = this.GetBitmap(Width, Height, R);
            this.Rect = new Rectangle(0, 0, Width, Height);
            if ((X > 0f) && (Y > 0f))
            {
                this.DrawCursor(Graphics.FromImage(image), X, Y);
            }
            MemoryStream stream2 = new MemoryStream();
            image.Save(stream2, ifm);
            stream2.WriteTo(stream);
        }

        public string SaveToWeb(int Width, int Height)
        {
            return this.SaveToWeb(Width, Height, ImageFormat.Png);
        }

        public string SaveToWeb(int Width, int Height, ImageFormat ifm)
        {
            return this.SaveToWeb(Width, Height, new Rectangle(0, 0, Width, Height), ifm);
        }

        public string SaveToWeb(int Width, int Height, Rectangle Rect, ImageFormat ifm)
        {
            Bitmap bitmap = this.GetBitmap(Width, Height, Rect);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ifm);
            return ("~/ImageFromCache.aspx?CacheId=" + this.SaveToImageStream(stream));
        }

        public void SelectAreaByPos(float X, float Y)
        {
            FormulaHitInfo hitInfo = this.GetHitInfo(X, Y);
            if (hitInfo.Area != null)
            {
                this.SelectedArea = hitInfo.Area;
            }
        }

        public void SetAxisXDataCycle(int Index, DataCycle dc)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if ((Index >= 0) && (Index < area.AxisXs.Count))
                {
                    area.AxisXs[Index].DataCycle = dc;
                }
            }
        }

        public void SetAxisXFormat(int Index, string value)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if ((Index >= 0) && (Index < area.AxisXs.Count))
                {
                    area.AxisXs[Index].Format = value;
                }
            }
        }

        public void SetAxisXShowMajorLine(int Index, bool value)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if ((Index >= 0) && (Index < area.AxisXs.Count))
                {
                    area.AxisXs[Index].MajorTick.ShowLine = value;
                }
            }
        }

        public void SetAxisXShowMinorLine(int Index, bool value)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if ((Index >= 0) && (Index < area.AxisXs.Count))
                {
                    area.AxisXs[Index].MinorTick.ShowLine = value;
                }
            }
        }

        public void SetAxisXVisible(int Index, bool value)
        {
            foreach (FormulaArea area in this.Areas)
            {
                if ((Index >= 0) && (Index < area.AxisXs.Count))
                {
                    area.AxisXs[Index].Visible = value;
                }
            }
        }

        public void SetCursorPos(float X, float Y)
        {
            FormulaHitInfo hitInfo = this.GetHitInfo(X, Y);
            this.CursorPos = hitInfo.CursorPos;
        }

        private void SetMemGraphics(Rectangle R)
        {
            if (this.MemBmp == null)
            {
                this.MemBmp = new Bitmap(R.Width, R.Height, PixelFormat.Format32bppPArgb);
                this.MemG = Graphics.FromImage(this.MemBmp);
            }
            else if ((this.MemBmp.Width != R.Width) || (this.MemBmp.Height != R.Height))
            {
                this.MemBmp = new Bitmap(R.Width, R.Height, PixelFormat.Format32bppPArgb);
                this.MemG = Graphics.FromImage(this.MemBmp);
            }
        }

        public void SetSkin(FormulaSkin fs)
        {
            if (fs != null)
            {
                fs.Bind(this);
            }
        }

        public void SetSkin(string Skin)
        {
            FormulaSkin skinByName = FormulaSkin.GetSkinByName(Skin);
            if (skinByName != null)
            {
                this.SetSkin(skinByName);
            }
        }

        private void SetView()
        {
            double[] dd = this.DataProvider["DATE"];
            double num = (this.Rect.Width - this.LabelWidth) - this.MarginWidth;
            if ((this.StartTime == DateTime.MinValue) || (this.EndTime == DateTime.MaxValue))
            {
                int num2 = (dd.Length - 1) - this.Start;
                if (num2 < 0)
                {
                    num2 = 0;
                }
                if (num2 >= dd.Length)
                {
                    num2 = dd.Length - 1;
                }
                if (num2 >= 0)
                {
                    this.EndTime = DateTime.FromOADate(dd[num2]);
                    num2 -= (int) (num / this.ColumnWidth);
                    if (num2 < 0)
                    {
                        num2 = 0;
                    }
                    if (num2 >= dd.Length)
                    {
                        num2 = dd.Length - 1;
                    }
                    this.StartTime = DateTime.FromOADate(dd[num2]);
                }
            }
            int index = FindIndex(dd, this.EndTime.ToOADate(), -1);
            int num4 = FindIndex(dd, this.StartTime.ToOADate());
            if (num4 < 0)
            {
                num4 = 0;
            }
            if (index < num4)
            {
                index = num4;
            }
            if (index >= dd.Length)
            {
                index = dd.Length - 1;
            }
            if (index >= 0)
            {
                this.StartTime = DateTime.FromOADate(dd[num4]);
                this.EndTime = DateTime.FromOADate(dd[index]);
                this.columnWidth = num / ((double) ((index - num4) + 1));
                this.Start = (dd.Length - 1) - index;
                this.ApplyXFormat(((dd[index] - dd[num4]) / num) * 100.0);
                if (this.Start < 0)
                {
                    this.Start = 0;
                }
                if (this.ViewChanged != null)
                {
                    this.ViewChanged(this, new ViewChangedArgs(num4, index, 0, dd.Length - 1));
                }
            }
        }

        public AxisLabelAlign AxisLabelAlign
        {
            set
            {
                foreach (FormulaArea area in this.Areas)
                {
                    area.AxisX.AxisLabelAlign = value;
                }
            }
        }

        public string AxisXFormat
        {
            set
            {
                foreach (FormulaArea area in this.Areas)
                {
                    area.AxisX.Format = value;
                }
            }
        }

        public string AxisYFormat
        {
            set
            {
                foreach (FormulaArea area in this.Areas)
                {
                    area.AxisY.Format = value;
                }
            }
        }

        public double ColumnWidth
        {
            get
            {
                return this.columnWidth;
            }
            set
            {
                this.columnWidth = value;
            }
        }

        public DataCycle DataCycle
        {
            set
            {
                foreach (FormulaArea area in this.Areas)
                {
                    area.AxisX.DataCycle = value;
                }
            }
        }

        public IDataProvider DataProvider
        {
            get
            {
                return this.dataProvider;
            }
            set
            {
                this.dataProvider = value;
                this.Bind();
            }
        }

        public FormulaArea this[int index]
        {
            get
            {
                return this.Areas[index];
            }
        }

        public FormulaArea this[string Name]
        {
            get
            {
                return this.Areas[Name];
            }
        }

        public FormulaArea MainArea
        {
            get
            {
                foreach (FormulaArea area in this.Areas)
                {
                    if (area.IsMain())
                    {
                        return area;
                    }
                }
                return null;
            }
        }

        public FormulaArea SelectedArea
        {
            get
            {
                foreach (FormulaArea area in this.Areas)
                {
                    if (area.Selected)
                    {
                        return area;
                    }
                }
                return null;
            }
            set
            {
                foreach (FormulaArea area in this.Areas)
                {
                    area.Selected = area == value;
                }
            }
        }

        public string XCursorFormat
        {
            get
            {
                if (this.Areas.Count > 0)
                {
                    return this.Areas[0].AxisX.CursorFormat;
                }
                return "dd-MMM-yyyy dddd";
            }
            set
            {
                foreach (FormulaArea area in this.Areas)
                {
                    area.AxisX.CursorFormat = value;
                }
            }
        }
    }
}

