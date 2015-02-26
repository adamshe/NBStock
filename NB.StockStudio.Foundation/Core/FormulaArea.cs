namespace NB.StockStudio.Foundation
{
    using NB.StockStudio.Foundation.DataProvider;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class FormulaArea
    {
        public FormulaAxisX AxisX;
        public AxisXCollection AxisXs;
        public FormulaAxisY AxisY;
        public AxisYCollection AxisYs;
        public FormulaBack Back;
        public Brush[] BarBrushes;
        public Pen[] BarPens;
        private string BindingErrors;
        public double BottomMargin;
        public FormulaCanvas Canvas;
        public Color[] Colors;
        public bool DrawVolumeAsLine;
        public FormulaPackage FormulaDataArray;
        public FormulaCollection Formulas;
        public double HeightPercent;
        public FormulaLabel[] Labels;
        public Pen LinePen;
        private int MaxLen;
        public Brush NameBrush;
        public Font NameFont;
        public FormulaPackageCollection Packages;
        public FormulaChart Parent;
        public Rectangle Rect;
        private Random Rnd;
        public ScaleType ScaleType;
        public bool Selected;
        public Pen SelectedPen;
        public StockRenderType StockRenderType;
        public Font TextFont;
        public double TopMargin;
        public bool Visible;

        public FormulaArea(FormulaChart fc)
        {
            this.Visible = true;
            this.AxisYs = new AxisYCollection();
            this.AxisXs = new AxisXCollection();
            this.SelectedPen = new Pen(Color.Red, 2f);
            this.DrawVolumeAsLine = true;
            this.StockRenderType = StockRenderType.Candle;
            this.Colors = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Black, Color.Orange, Color.DarkGray, Color.DarkTurquoise };
            this.Labels = new FormulaLabel[] { FormulaLabel.EmptyLabel, FormulaLabel.RedLabel, FormulaLabel.GreenLabel, FormulaLabel.WhiteLabel };
            this.LinePen = new Pen(Color.Black);
            this.BarPens = new Pen[] { new Pen(Color.Black, 1f), new Pen(Color.Black, 1f), new Pen(Color.Blue, 1f) };
            Brush[] brushArray = new Brush[3];
            brushArray[2] = Brushes.Blue;
            this.BarBrushes = brushArray;
            this.NameBrush = new SolidBrush(Color.Black);
            this.NameFont = new Font("Verdana", 8f);
            this.TextFont = new Font("Verdana", 8f);
            this.Rnd = new Random();
            this.Parent = fc;
            this.Packages = new FormulaPackageCollection();
            this.FormulaDataArray = new FormulaPackage();
            this.Formulas = new FormulaCollection();
            this.Back = new FormulaBack();
            this.AxisX = new FormulaAxisX();
            this.AxisX.Format = "MMMyy";
            this.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
            this.AxisXs.Add(this.AxisX);
            this.AxisY = new FormulaAxisY();
            this.AxisYs.Add(this.AxisY);
        }

        public FormulaArea(FormulaChart fc, string Name, string Quote) : this(fc)
        {
            this.AddFormula(Name, Quote);
        }

        public FormulaArea(FormulaChart fc, string Name, string Quote, double HeightPercent) : this(fc, Name, Quote)
        {
            this.HeightPercent = HeightPercent;
        }

        public void AddFormula(FormulaBase fb)
        {
            this.Formulas.Add(fb);
        }

        public void AddFormula(string Name)
        {
            this.Formulas.Add(Name);
        }

        public void AddFormula(string Name, string Quote)
        {
            this.Formulas.Add(Name, Quote);
        }

        public FormulaAxisY AddNewAxisY(AxisPos ap)
        {
            FormulaAxisY fay = new FormulaAxisY();
            fay.CopyFrom(this.AxisY);
            this.AxisYs.Add(fay);
            fay.AutoScale = true;
            fay.MajorTick.ShowLine = false;
            fay.AxisPos = AxisPos.Left;
            fay.Back.RightPen.Width = 1f;
            return fay;
        }

        private void AdjustMoving()
        {
            bool flag = false;
            for (int i = 1; i < this.FormulaDataArray.Count; i++)
            {
                FormulaData data = this.FormulaDataArray[i];
                if (data.Transform == Transform.FirstDataOfView)
                {
                    int num2;
                    double num3 = this.GetInViewData(i, 0, out num2);
                    double num4 = this.GetInViewData(0, num2) / num3;
                    for (int j = 0; j < data.Length; j++)
                    {
                        FormulaData data3;
                        int num10;
                        (data3 = data)[num10 = j] = data3[num10] * num4;
                    }
                    flag = true;
                }
            }
            if (flag)
            {
                this.CalcMinMax();
            }
            foreach (FormulaData data2 in this.FormulaDataArray)
            {
                if (data2.Transform == Transform.PercentView)
                {
                    double num6 = data2.MaxValue(this.Canvas.Start, this.Canvas.Count);
                    double num7 = ((data2.AxisY.MaxY - data2.AxisY.MinY) / num6) * data2.PercentView;
                    double minY = data2.AxisY.MinY;
                    for (int k = 0; k < data2.Length; k++)
                    {
                        data2[k] = (data2[k] * num7) + minY;
                    }
                }
            }
        }

        private Pen[] AdjustPenWidth(int w)
        {
            if (this.BarPens != null)
            {
                if (this.BarPens[0].Width > 1f)
                {
                    this.StockRenderType = StockRenderType.Bar;
                }
                Pen[] penArray = new Pen[3];
                for (int i = 0; i < this.BarPens.Length; i++)
                {
                    penArray[i] = (Pen) this.BarPens[i].Clone();
                    if ((this.BarPens[i].Width > 1f) && (w < (penArray[i].Width * 1.5)))
                    {
                        penArray[i].Width = ((float) w) / 1.5f;
                    }
                    if ((w < 2) && (this.BarBrushes[i] is SolidBrush))
                    {
                        penArray[i].Color = (this.BarBrushes[i] as SolidBrush).Color;
                    }
                }
                return penArray;
            }
            return this.BarPens;
        }

        public void Bind()
        {
            this.BindingErrors = null;
            try
            {
                this.FormulaDataArray.Clear();
                if (this.DataProvider != null)
                {
                    this.Packages.Clear();
                    foreach (FormulaBase base2 in this.Formulas)
                    {
                        IDataProvider dataProvider = this.DataProvider;
                        if (((base2.Quote != null) && (base2.Quote != "")) && ((dataProvider.DataManager != null) && (string.Compare(dataProvider.GetStringData("Code"), base2.Quote, true) != 0)))
                        {
                            if (dataProvider.DataManager == null)
                            {
                                throw new Exception(base2.Quote + " not found!");
                            }
                            dataProvider = dataProvider.DataManager[base2.Quote];
                            dataProvider.BaseDataProvider = this.DataProvider;
                        }
                        try
                        {
                            FormulaPackage fp = base2.Run(dataProvider);
                            foreach (FormulaData data in fp)
                            {
                                data.ParentFormula = base2;
                            }
                            this.Packages.Add(fp);
                            continue;
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Errors in formula \"" + base2.DisplayName + "\":" + exception.Message);
                        }
                    }
                }
                foreach (FormulaPackage package2 in this.Packages)
                {
                    this.FormulaDataArray.AddRange(package2);
                }
            }
            catch (Exception exception2)
            {
                this.BindingErrors = exception2.Message;
            }
        }

        public PointF CalcCenter(PointF p0, PointF p1, PointF p3, PointF p2)
        {
            float y = ((p3.Y * (p2.Y - p1.Y)) - (p2.Y * (p3.Y - p0.Y))) / (((p0.Y + p2.Y) - p1.Y) - p3.Y);
            float num2 = p3.Y - p0.Y;
            float naN = float.NaN;
            if (num2 != 0f)
            {
                naN = (((p2.X - p0.X) * (y - p3.Y)) / num2) + p2.X;
            }
            else
            {
                num2 = p2.Y - p1.Y;
                naN = (((p2.X - p0.X) * (y - p2.Y)) / num2) + p2.X;
            }
            if ((double.IsNaN((double) naN) || double.IsInfinity((double) naN)) || (double.IsNaN((double) y) || double.IsInfinity((double) y)))
            {
                return PointF.Empty;
            }
            return new PointF(naN, y);
        }

        private void CalcInViewPoint()
        {
            Rectangle frameRect = this.Canvas.FrameRect;
            foreach (FormulaAxisY sy in this.AxisYs)
            {
                frameRect.Width -= sy.Width;
                sy.FrameRect = new Rectangle(frameRect.Right, frameRect.Top, sy.Width, frameRect.Height);
                if (sy.AxisPos == AxisPos.Left)
                {
                    sy.FrameRect.X = frameRect.X;
                    frameRect.X += sy.Width;
                }
            }
            int num = (int) Math.Ceiling((double) this.Canvas.LabelHeight);
            if (!this.Parent.ShowValueLabel)
            {
                num /= 2;
            }
            frameRect.Y += num;
            frameRect.Height -= num;
            int height = frameRect.Height;
            for (int i = this.AxisXs.Count - 1; i >= 0; i--)
            {
                FormulaAxisX sx = this.AxisXs[i];
                if (sx.Visible)
                {
                    frameRect.Height -= sx.Height;
                    sx.Rect = frameRect;
                    sx.Rect.Y = frameRect.Bottom;
                    sx.Rect.Height = sx.Height;
                    sx.Rect.Width++;
                    if (i < (this.AxisXs.Count - 1))
                    {
                        sx.Rect.Height++;
                    }
                }
            }
            if (frameRect.Height == height)
            {
                frameRect.Height -= (int) Math.Ceiling((double) (this.Canvas.LabelHeight / 2f));
            }
            this.Canvas.Rect = frameRect;
            if (!this.Parent.FixedTime)
            {
                this.Canvas.Count = (int) (((double) (frameRect.Width - this.Parent.MarginWidth)) / this.Canvas.ColumnWidth);
                if ((this.Canvas.Start + this.Canvas.Count) > this.MaxLen)
                {
                    if (this.Canvas.Count > this.MaxLen)
                    {
                        this.Canvas.Start = 0;
                        this.Canvas.Count = this.MaxLen;
                    }
                    else
                    {
                        this.Canvas.Start = this.MaxLen - this.Canvas.Count;
                    }
                }
                this.Canvas.Stop = Math.Max(0, (this.MaxLen - this.Canvas.Start) - this.Canvas.Count);
            }
            else
            {
                this.Canvas.Count = this.MaxLen;
                this.Canvas.ColumnWidth = (frameRect.Width - this.Parent.MarginWidth) / ((double) this.Canvas.Count);
                this.Canvas.Start = 0;
                this.Canvas.Stop = 0;
            }
        }

        public int CalcLabelWidth(Graphics g)
        {
            this.InitCanvas(g, this.Rect, this.Parent.Start, this.Parent.ColumnWidth, (double) this.Parent.ColumnPercent);
            int num = 0;
            foreach (FormulaData data in this.FormulaDataArray)
            {
                num = Math.Max(data.AxisMargin, num);
            }
            return (this.AxisY.CalcLabelWidth(this.Canvas) + num);
        }

        private void CalcMinMax()
        {
            foreach (FormulaAxisY sy in this.AxisYs)
            {
                if (!sy.AutoScale)
                {
                    continue;
                }
                sy.MinY = double.MaxValue;
                sy.MaxY = double.MinValue;
                if ((this.AxisY.AutoScale || (this.AxisY.MinY == double.MinValue)) || (this.AxisY.MaxY == double.MaxValue))
                {
                    foreach (FormulaData data in this.FormulaDataArray)
                    {
                        if (((data.AxisY == sy) && (data.RenderType != FormulaRenderType.VERTLINE)) && ((data.LineWidth != 0) && (data.Transform != Transform.PercentView)))
                        {
                            sy.MaxY = Math.Max(sy.MaxY, data.MaxValue(this.Canvas.Start, this.Canvas.Count));
                            sy.MinY = Math.Min(sy.MinY, data.MinValue(this.Canvas.Start, this.Canvas.Count));
                        }
                    }
                    if ((this.TopMargin != 0.0) || (this.BottomMargin != 0.0))
                    {
                        double num = Math.Abs(Math.Min(sy.MaxY - sy.MinY, sy.MinY));
                        sy.MinY -= (num * this.BottomMargin) / 100.0;
                        sy.MaxY += (num * this.TopMargin) / 100.0;
                    }
                    if (sy.MinY != sy.MaxY)
                    {
                        double num2 = ((sy.MaxY - sy.MinY) / ((double) this.Canvas.Rect.Height)) * 24.0;
                        foreach (FormulaData data2 in this.FormulaDataArray)
                        {
                            if (data2.RenderType == FormulaRenderType.TEXT)
                            {
                                if (data2.VAlign == VerticalAlign.Top)
                                {
                                    sy.MaxY = Math.Max(sy.MaxY, data2.MaxValue(this.Canvas.Start, this.Canvas.Count) + num2);
                                }
                                else if (data2.VAlign == VerticalAlign.Bottom)
                                {
                                    sy.MinY = Math.Min(sy.MinY, data2.MinValue(this.Canvas.Start, this.Canvas.Count) - num2);
                                }
                            }
                        }
                        if (this.IsMain() & (sy.MinY < 0.0))
                        {
                            double num3 = 0.0;
                            foreach (FormulaData data3 in this.FormulaDataArray)
                            {
                                if (data3.RenderType == FormulaRenderType.STOCK)
                                {
                                    num3 = data3.MinValue(this.Canvas.Start, this.Canvas.Count);
                                }
                            }
                            sy.MinY = num3;
                        }
                    }
                }
                if ((double.IsInfinity(sy.MaxY) || double.IsNaN(sy.MaxY)) || ((double.IsInfinity(sy.MinY) || double.IsNaN(sy.MinY)) || (sy.MaxY < sy.MinY)))
                {
                    sy.MaxY = 1.0;
                    sy.MinY = 0.0;
                }
                if (sy.MaxY == sy.MinY)
                {
                    sy.MinY -= 0.5;
                    sy.MaxY += 0.5;
                }
            }
        }

        private void DrawBackground(Rectangle R)
        {
            this.Back.Render(this.Canvas.CurrentGraph, R);
        }

        private void DrawPoints(Graphics g, FormulaData f, Pen CurrentPen, PointF[] pfs, double ColumnWidth)
        {
            float num;
            switch (f.Dot)
            {
                case FormulaDot.NORMAL:
                    if ((pfs.Length <= 1) || (CurrentPen.Width <= 0f))
                    {
                        return;
                    }
                    if (f.Smoothing != SmoothingMode.Invalid)
                    {
                        g.SmoothingMode = f.Smoothing;
                        break;
                    }
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    break;

                case FormulaDot.CROSSDOT:
                    num = (float) (ColumnWidth * this.Parent.ColumnPercent);
                    for (int i = 0; i < pfs.Length; i++)
                    {
                        g.DrawLine(CurrentPen, pfs[i].X - (num / 2f), pfs[i].Y, pfs[i].X + (num / 2f), pfs[i].Y);
                        g.DrawLine(CurrentPen, pfs[i].X, pfs[i].Y - (num / 2f), pfs[i].X, pfs[i].Y + (num / 2f));
                    }
                    return;

                case FormulaDot.POINTDOT:
                {
                    num = (float) (ColumnWidth * this.Parent.ColumnPercent);
                    Brush brush = new SolidBrush(CurrentPen.Color);
                    for (int j = 0; j < pfs.Length; j++)
                    {
                        g.FillEllipse(brush, pfs[j].X - (num / 2f), pfs[j].Y - (num / 2f), num, num);
                    }
                    return;
                }
                case FormulaDot.CIRCLEDOT:
                    num = (float) (ColumnWidth * this.Parent.ColumnPercent);
                    CurrentPen.Width = 1f;
                    for (int k = 0; k < pfs.Length; k++)
                    {
                        g.DrawEllipse(CurrentPen, pfs[k].X - (num / 2f), pfs[k].Y - (num / 2f), num, num);
                    }
                    return;

                default:
                    return;
            }
            g.DrawLines(CurrentPen, pfs);
            g.SmoothingMode = SmoothingMode.Default;
        }

        public void DrawValueText()
        {
            this.DrawValueText(this.Canvas.CurrentGraph);
        }

        public void DrawValueText(Graphics g)
        {
            if ((this.Canvas != null) && this.Parent.ShowValueLabel)
            {
                int cursorPos = this.Parent.CursorPos;
                RectangleF rect = new RectangleF((this.Canvas.Rect.Left + this.Back.LeftPen.Width) + 2f, this.Canvas.FrameRect.Top + this.Back.TopPen.Width, ((this.Canvas.Rect.Width - this.Back.LeftPen.Width) - this.Back.RightPen.Width) - 2f, ((int) this.Canvas.LabelHeight) - this.Back.TopPen.Width);
                if (g != this.Canvas.CurrentGraph)
                {
                    rect.Offset((PointF) this.Parent.Rect.Location);
                    g.FillRectangle(this.Back.BackGround, rect);
                }
                int left = (int) rect.Left;
                int top = (int) rect.Top;
                int num4 = 0;
                int num5 = 0;
                g.SetClip(rect);
                try
                {
                    for (int i = 0; i < this.FormulaDataArray.Count; i++)
                    {
                        FormulaData data = this.FormulaDataArray[i];
                        Brush brush = new SolidBrush(this.GetCurrentPen(i).Color);
                        if (num5 <= i)
                        {
                            FormulaBase base2 = this.Formulas[num4];
                            FormulaPackage package = this.Packages[num4];
                            string displayName = base2.DisplayName;
                            Brush nameBrush = brush;
                            if (num4 == 0)
                            {
                                nameBrush = this.NameBrush;
                            }
                            if (!base2.TextInvisible)
                            {
                                g.DrawString(displayName, this.NameFont, nameBrush, (float) left, (float) top);
                                left += ((int) g.MeasureString(displayName, this.NameFont).Width) + 4;
                            }
                            num4++;
                            num5 += package.Count;
                        }
                        if (!data.TextInvisible && ((cursorPos >= 0) && (cursorPos < data.Length)))
                        {
                            string name = data.Name;
                            if ((name != null) && (name != ""))
                            {
                                name = name + ":";
                            }
                            name = name + ((data[cursorPos] / this.AxisYs[data.AxisYIndex].MultiplyFactor)).ToString(data.Format, NumberFormatInfo.InvariantInfo);
                            g.DrawString(name, this.NameFont, brush, (float) left, (float) top);
                            left += ((int) g.MeasureString(name, this.NameFont).Width) + 4;
                        }
                    }
                }
                finally
                {
                    g.ResetClip();
                }
            }
        }

        public Bitmap GetBitmap(int Width, int Height, int Start, float ColumnWidth)
        {
            Bitmap image = new Bitmap(Width, Height);
            this.Render(Graphics.FromImage(image), new Rectangle(0, 0, image.Width, image.Height), Start, (double) ColumnWidth);
            return image;
        }

        private Font GetCurrentFont(int Index)
        {
            FormulaData data = this.FormulaDataArray[Index];
            if (data.TextFont == null)
            {
                return this.TextFont;
            }
            return data.TextFont;
        }

        public Pen GetCurrentPen(int Index)
        {
            FormulaData data = this.FormulaDataArray[Index];
            if (data.LinePen == null)
            {
                Pen pen = (Pen) this.LinePen.Clone();
                pen.DashStyle = data.DashStyle;
                if (!data.FormulaColor.IsEmpty)
                {
                    pen.Color = data.FormulaColor;
                }
                else
                {
                    pen.Color = this.Colors[Index % this.Colors.Length];
                }
                if (data.LineWidth >= 0)
                {
                    pen.Width = data.LineWidth;
                }
                return pen;
            }
            return data.LinePen;
        }

        public FormulaData GetFormulaData(int Index)
        {
            return this.FormulaDataArray[Index];
        }

        private double GetInViewData(int LineIndex, int DataIndex)
        {
            int num;
            return this.GetInViewData(LineIndex, DataIndex, out num);
        }

        private double GetInViewData(string LineName, int DataIndex)
        {
            for (int i = 0; i < this.FormulaDataArray.Count; i++)
            {
                FormulaData data = this.FormulaDataArray[i];
                if (string.Compare(data.Name, LineName, true) == 0)
                {
                    return this.GetInViewData(i, DataIndex);
                }
            }
            return double.NaN;
        }

        private double GetInViewData(int LineIndex, int DataIndex, out int Index)
        {
            FormulaData data = this.FormulaDataArray[LineIndex];
            Index = Math.Max(0, ((data.Length - this.Canvas.Start) - this.Canvas.Count) + DataIndex);
            if (Index < data.Length)
            {
                double d = data[Index];
                int num2 = 0;
                while (((d == 0.0) || double.IsNaN(d)) && (Index < data.Length))
                {
                    d = data[Index];
                    Index++;
                    num2++;
                }
                Index = DataIndex + num2;
                return d;
            }
            return double.NaN;
        }

        public string GetUnique()
        {
            return (this.Name + "." + this.DataProvider.GetUnique());
        }

        private void InitCanvas(Graphics g, Rectangle R, int Start, double ColumnWidth, double ColumnPercent)
        {
            if (this.Canvas == null)
            {
                this.Canvas = new FormulaCanvas();
            }
            this.Canvas.CurrentGraph = g;
            this.Canvas.AxisX = this.AxisX;
            if (this.Parent.FixedTime)
            {
                this.Canvas.DATE = this.DataProvider["DATE"];
                this.AxisX.StartTime = this.Parent.StartTime;
                this.AxisX.EndTime = this.Parent.EndTime;
                if (this.DataProvider is CommonDataProvider)
                {
                    this.AxisX.IntradayInfo = (this.DataProvider as CommonDataProvider).IntradayInfo;
                }
            }
            this.MakeSameLength();
            this.Canvas.FrameRect = R;
            this.Canvas.Start = Start;
            this.Canvas.ColumnWidth = ColumnWidth;
            this.Canvas.ColumnPercent = ColumnPercent;
            this.Canvas.LabelHeight = this.Canvas.CurrentGraph.MeasureString("0", this.AxisY.LabelFont).Height;
            this.CalcInViewPoint();
            foreach (FormulaData data in this.FormulaDataArray)
            {
                data.Canvas = this.Canvas;
                if (data.AxisYIndex < this.AxisYs.Count)
                {
                    data.AxisY = this.AxisYs[data.AxisYIndex];
                }
                else
                {
                    data.AxisY = this.AxisY;
                }
            }
            this.CalcMinMax();
            this.AdjustMoving();
            if (this.AxisY.ShowAsPercent && (this.AxisY.RefValue == 0.0))
            {
                this.AxisY.RefValue = this.GetInViewData(0, 0);
            }
        }

        public void InsertFormula(int Index, FormulaBase fb)
        {
            this.Formulas.Insert(Index, fb);
        }

        public void InsertFormula(int Index, string Name)
        {
            this.Formulas.Insert(Index, Name);
        }

        private void InsertPoint(ArrayList al, PointF[] pfs, PointF[] pfs2, int j)
        {
            if ((j > 0) && (j < pfs.Length))
            {
                PointF tf = this.CalcCenter(pfs[j - 1], pfs2[j - 1], pfs[j], pfs2[j]);
                if (tf != PointF.Empty)
                {
                    al.Insert(al.Count / 2, tf);
                }
            }
        }

        public bool IsMain()
        {
            for (int i = 0; i < this.FormulaDataArray.Count; i++)
            {
                if (this[i].RenderType == FormulaRenderType.STOCK)
                {
                    return true;
                }
            }
            return false;
        }

        private void MakeSameLength()
        {
            this.MaxLen = this.DataProvider.Count;
            foreach (FormulaData data in this.FormulaDataArray)
            {
                if (data.Length < this.MaxLen)
                {
                    data.FillTo(this.MaxLen);
                }
            }
            if (this.Parent.CursorPos == -1)
            {
                this.Parent.CursorPos = this.MaxLen - 1;
            }
        }

        private void MovePoint(PointF[] pfs, int Delta)
        {
            for (int i = 0; i < pfs.Length; i++)
            {
                pfs[i].X += Delta;
                pfs[i].Y += Delta;
            }
        }

        public void RemoveFormula(FormulaBase fb)
        {
            this.Formulas.Remove(fb);
        }

        public void RemoveFormula(string Name)
        {
            this.Formulas.Remove(Name);
        }

        public void Render(Graphics g)
        {
            this.Render(g, this.Rect, this.Parent.Start, this.Parent.ColumnWidth);
        }

        public void Render(Graphics g, Rectangle R, int Start, double ColumnWidth)
        {
            this.DrawBackground(R);
           
            this.InitCanvas(g, R, Start, ColumnWidth, (double) this.Parent.ColumnPercent);
            foreach (FormulaAxisX sx in this.AxisXs)
            {
                sx.Render(this.Canvas, this);
            }
            foreach (FormulaAxisY sy in this.AxisYs)
            {
                sy.Rect = sy.FrameRect;
                sy.Rect.Y = this.Canvas.Rect.Y;
                sy.Rect.Height = this.Canvas.Rect.Height;
                sy.Render(this.Canvas, this);
            }
            R = this.Canvas.Rect;
            if (this.BindingErrors != null)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(this.BindingErrors, new Font("Verdana", 10f), this.NameBrush, R, format);
                return;
            }
            
            
                g.SetClip(new Rectangle(R.Left, R.Top, this.Canvas.FrameRect.Width, R.Height));
                try
                {
                    PointF[] tfArray;
                    Pen[] penArray;
                    RectangleF[] efArray;
                    FormulaData closeData;
                    FormulaData resultFormulaDataDiff;
                    float num6;
                    PointF tf,  tf5;
                    double[] numArray2;
                    int num12;
                    int num13;
                    PointF[] tfArray4;
                    PointF[] tfArray7;
                    PointF[] tfArray8;
                    ArrayList list;
                    int num18;
                    ArrayList list3;
                    string str3;
                    float num25;
                    float num26;
                    float num27;
                    FormulaAlign right;
                    FormulaData formulaData;
                    Pen currentPen;
                    Font currentFont;
                    PointF[] points;
                    for (int i = 0; i < FormulaDataArray.Count; i++)
                    {
                        
                        formulaData = FormulaDataArray[i];
                        if (formulaData.IsNaN())
                        {
                            continue;
                        }
                        double[] filter = null;
                        if (
                             formulaData.RenderType == FormulaRenderType.POLY || formulaData.RenderType == FormulaRenderType.PARTLINE || 
                             formulaData.RenderType == FormulaRenderType.FILLRGN || formulaData.RenderType == FormulaRenderType.LINE ||
                             formulaData.RenderType == FormulaRenderType.TEXT || formulaData.RenderType == FormulaRenderType.STICKLINE ||
                             formulaData.RenderType == FormulaRenderType.ICON
                            )
                        {
                            filter = formulaData["COND"];
                        }

                        points = formulaData.GetPoints(formulaData.RenderType == FormulaRenderType.PARTLINE || formulaData.RenderType == FormulaRenderType.FILLRGN, filter);
                        currentPen = this.GetCurrentPen(i);
                        currentFont = this.GetCurrentFont(i);

                        int w = (int) ((ColumnWidth * this.Parent.ColumnPercent) + 0.4);
                        switch (formulaData.RenderType)
                        {
                            case FormulaRenderType.AXISY:
                            
                            if (points.Length > 0)
                            {
                                double num20 = (double) formulaData.OwnerData["START"];
                                double num21 = (double) formulaData.OwnerData["END"];
                                float y = points[0].Y;
                                R = formulaData.AxisY.FrameRect;
                                float num23 = (float) (R.X + num20);
                                float num24 = (float) (R.Right - num21);
                                if (formulaData.AxisY.AxisPos == AxisPos.Left)
                                {
                                    num23 = (float) (R.Right - num20);
                                    num24 = (float) (R.X + num21);
                                }
                                g.DrawLine(currentPen, num23, y, num24, y);
                            }
                            continue;
                            
                            case FormulaRenderType.COLORSTICK:
                                num6 = formulaData.AxisY.CalcY(0.0);
                                penArray = this.AdjustPenWidth(w);
                                //num7 = 0;
                                if (0 < points.Length)
                                {
                                    tf = new PointF(points[0].X, num6);
                                    g.DrawLine(penArray[(points[0].Y < num6) ? 0 : 2], points[0], tf);
                                  //  num7++;
                                }
                                break;

                            case FormulaRenderType.VOLSTICK:
                                efArray = new RectangleF[1];
                                closeData = this.Parent.DataProvider["CLOSE"]; 
                                if (this.StockRenderType != StockRenderType.Bar)
                                {
                                    resultFormulaDataDiff = (FormulaData)(closeData > this.DataProvider["OPEN"]);
                                }                               
                                else
                                    resultFormulaDataDiff = (FormulaData) (closeData > FormulaBase.REF(closeData, 1.0));
                                    resultFormulaDataDiff *= formulaData.MaxValue(this.Canvas.Start, this.Canvas.Count);
                                    resultFormulaDataDiff.AxisY = this.AxisY;
                                    resultFormulaDataDiff.Canvas = this.Canvas;
                                    tfArray = resultFormulaDataDiff.GetPoints();
                                    float num3 = this.AxisY.CalcY(0.0);
                                    penArray = this.AdjustPenWidth(w);
                                    if ((ColumnWidth > 2.0) && !this.DrawVolumeAsLine)
                                    {
                                        efArray[0] = new RectangleF();
                                        for (int m = 0; m < points.Length; m++)
                                        {
                                            efArray[0].X = (float) ((points[m].X - (ColumnWidth / 2.0)) + 1.0);
                                            efArray[0].Width = (float) (ColumnWidth - 2.0);
                                            efArray[0].Y = points[m].Y;
                                            efArray[0].Height = R.Bottom - points[m].Y;
                                            if (tfArray == null)
                                            {
                                                g.DrawRectangles(currentPen, efArray);
                                            }
                                            else
                                            {
                                                g.DrawRectangles(penArray[(tfArray[m].Y == num3) ? 2 : 0], efArray);
                                                g.FillRectangles(penArray[(tfArray[m].Y == num3) ? 2 : 0].Brush, efArray);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int n = 0; n < points.Length; n++)
                                        {
                                            if (tfArray != null)
                                            {
                                                currentPen = penArray[(tfArray[n].Y == num3) ? 2 : 0];
                                            }
                                            g.DrawLine(currentPen, points[n].X, points[n].Y, points[n].X, (float) R.Bottom);
                                        }
                                    }
                                break;

                            case FormulaRenderType.STICKLINE:
                              //  num8 = 0;
                                if (0 < points.Length)
                                {
                                    numArray2 = formulaData["PRICE2"];
                                    PointF tf2 = new PointF(points[0].X, formulaData.AxisY.CalcY(numArray2[0]));
                                    g.DrawLine(currentPen, points[0], tf2);
             //                       num8++;
                                }
                                continue;

                            case FormulaRenderType.ICON:
                             //   num10 = 0;
                                if (0 < points.Length)
                                {
                                    if (formulaData["COND"][0] == 1.0)
                                    {
                                        g.DrawString(formulaData.OwnerData["ICON"].ToString(), new Font("Verdana", 9f), Brushes.Black, points[0]);
                                    }
                              //      num10++;
                                }
                                continue;

                            case FormulaRenderType.TEXT:
                            
                                double[] dd = formulaData["NUMBER"];
                                string text = (string) formulaData.OwnerData["TEXT"];
                                object obj2 = formulaData.OwnerData["FORMAT"];
                                string str2 = "";
                                if (obj2 != null)
                                {
                                    str2 = obj2.ToString();
                                }
                                PointF[] tfArray3 = formulaData.GetPoints(dd, false, formulaData["COND"], true);
                                for (int k = 0; k < points.Length; k++)
                                {
                                    if (dd != null)
                                    {
                                        text = dd[(int) tfArray3[k].Y].ToString(str2);
                                    }
                                    PointF pos = new PointF(points[k].X, points[k].Y);
                                    FormulaLabel label = this.Labels[formulaData.LabelIndex % this.Labels.Length];
                                    Brush brush = label.TextBrush;
                                    if (brush == null)
                                    {
                                        brush = new SolidBrush(currentPen.Color);
                                    }
                                    label.DrawString(g, text, this.TextFont, brush, formulaData.VAlign, formulaData.Align, pos, formulaData.VAlign != VerticalAlign.VCenter);
                                }
                                continue;
                            
                            case FormulaRenderType.AXISYTEXT:
                                if (points.Length <= 0)
                                {
                                    continue;
                                }
                                str3 = (string) formulaData.OwnerData["TEXT"];
                                num25 = (float) ((double) formulaData.OwnerData["START"]);
                                num26 = points[0].Y;
                                num27 = 0f;
                                R = formulaData.AxisY.FrameRect;
                                right = formulaData.Align;
                                if (formulaData.AxisY.AxisPos != AxisPos.Right)
                                {
                                    right = FormulaAlign.Left;
                                    num27 = R.X + num25;
                                }
                                else
                                {
                                    right = FormulaAlign.Right;
                                    num27 = R.Right - num25;                                    
                                }
                                tf5 = new PointF(num27, num26);
                                FormulaLabel label2 = this.Labels[formulaData.LabelIndex % this.Labels.Length];
                                Brush textBrush = label2.TextBrush;
                                if (textBrush == null)
                                {
                                    textBrush = new SolidBrush(currentPen.Color);
                                }
                                label2.DrawString(g, str3, this.TextFont, textBrush, formulaData.VAlign, right, tf5, formulaData.VAlign != VerticalAlign.VCenter);                                
                                break;

                            case FormulaRenderType.FILLRGN:
                                tfArray = formulaData.GetPoints("PRICE2", true, "COND");
                                tfArray7 = formulaData.GetPoints(true);
                                tfArray8 = formulaData.GetPoints("PRICE2", true);
                                list = new ArrayList();
                                num18 = 0;
                                while (num18 <= points.Length)
                                {
                                    if ((num18 < points.Length) && (points[num18] != PointF.Empty))
                                    {
                                        if (list.Count == 0)
                                        {
                                            this.InsertPoint(list, tfArray7, tfArray8, num18);
                                        }
                                        list.Insert(list.Count / 2, tfArray[num18]);
                                        list.Insert(list.Count / 2, points[num18]);
                                    }
                                    else if (list.Count > 0)
                                    {
                                        this.InsertPoint(list, tfArray7, tfArray8, num18);
                                        PointF[] tfArray9 = (PointF[])list.ToArray(typeof(PointF));
                                        Brush brush3 = formulaData.AreaBrush;
                                        if (brush3 == null)
                                        {
                                            brush3 = new SolidBrush(Color.FromArgb(30, Color.LightBlue));//
                                        }
                                        if (points.Length > 1)
                                        {
                                            g.FillPolygon(brush3, tfArray9);
                                        }
                                        list.Clear();
                                    }
                                    num18++;                                   
                                }
                                break;

                            case FormulaRenderType.FILLAREA:
                            {
                                PointF[] c = formulaData.GetPoints(false);
                                if (c.Length > 0)
                                {
                                    ArrayList list2 = new ArrayList();
                                    list2.AddRange(c);
                                    list2.Add(new PointF(c[c.Length - 1].X, (float) R.Bottom));
                                    list2.Add(new PointF(c[0].X, (float) R.Bottom));
                                    c = (PointF[]) list2.ToArray(typeof(PointF));
                                    Brush areaBrush = formulaData.AreaBrush;
                                    if (areaBrush == null)
                                    {
                                        areaBrush = new SolidBrush(Color.FromArgb(0x80, Color.Blue));
                                    }
                                    if (points.Length > 1)
                                    {
                                        g.FillPolygon(areaBrush, c);
                                    }
                                }
                                continue;
                            }
                            case FormulaRenderType.PARTLINE:
                                list3 = new ArrayList();
                             //   num19 = 0;
                                if (0 < points.Length)
                                {
                                    if ((0 == (points.Length - 1)) || points[0].IsEmpty)
                                    {
                                        if (list3.Count > 0)
                                        {
                                            tfArray = (PointF[])list3.ToArray(typeof(PointF));
                                            this.DrawPoints(g, formulaData, currentPen, tfArray, ColumnWidth);
                                        }
                                        list3.Clear();
                                    }
                                    else
                                    {
                                        list3.Add(points[0]);
                                    }
                          //          num19++;
                                }
                                continue;

                            case FormulaRenderType.LINE:
                                tfArray = formulaData.GetPoints("PRICE2", false, "COND2");
                               num12 = 0;
                                num13 = 0;
                                while (num13 < tfArray.Length)
                                {
                                    if (points[num12].X < tfArray[num13].X)
                                    {
                                        g.DrawLine(currentPen, points[num12].X, points[num12].Y, tfArray[num13].X, tfArray[num13].Y);
                                        while ((num12 < points.Length) && (points[num12].X < tfArray[num13].X))
                                        {
                                            num12++;
                                        }
                                        break;
                                    }
                                    num13++;
                                }
                                num12++;
                                continue;

                            case FormulaRenderType.VERTLINE:
                            //    num11 = 0;
                                if (0 < points.Length)
                                {
                                    if (formulaData.Data[0] == 1.0)
                                    {
                                        g.DrawLine(currentPen, points[0].X, (float)R.Top, points[0].X, (float)R.Bottom);
                                    }
                                  //  0++;
                                }
                                continue;

                            case FormulaRenderType.STOCK:
                            
                                if (this.StockRenderType !=StockRenderType.Line)
                                {
                                    tfArray4 = formulaData.GetPoints("H");
                                    PointF[] tfArray5 = formulaData.GetPoints("L");
                                    PointF[] tfArray6 = formulaData.GetPoints("O");
                                    penArray = this.AdjustPenWidth(w);
                                    for (int j = 0; j < points.Length; j++)
                                    {
                                            bool flag = tfArray6[j].Y > points[j].Y;
                                            if (this.StockRenderType == StockRenderType.Bar)
                                            {
                                                if (j < (points.Length - 1))
                                                {
                                                    flag = points[j + 1].Y > points[j].Y;
                                                }
                                                else
                                                {
                                                    flag = true;
                                                }
                                            }
                                            Pen pen = penArray[flag ? 0 : 2];
                                            Brush brush2 = this.BarBrushes[flag ? 0 : 2];
                                            if ((w <= 1) || (this.StockRenderType == StockRenderType.Bar))
                                            {
                                                g.DrawLine(pen, tfArray4[j], tfArray5[j]);
                                            }
                                            if (this.StockRenderType == StockRenderType.Bar)
                                            {
                                                PointF tf4 = points[j];
                                                tf4.X += w + 1;
                                                g.DrawLine(pen, points[j], tf4);
                                            }
                                            else
                                            {
                                                float num15 = points[j].X - (w / 2);
                                                float num16 = Math.Min(points[j].Y, tfArray6[j].Y);
                                                float height = Math.Abs((float)(tfArray6[j].Y - points[j].Y));
                                                if (height == 0f)
                                                {
                                                    pen = this.BarPens[1];
                                                    if (w > 0)
                                                    {
                                                        g.DrawLine(pen, num15, num16, num15 + w, num16);
                                                    }
                                                    else
                                                    {
                                                        g.DrawLine(pen, num15, num16, num15, num16 + 1f);
                                                    }
                                                    g.DrawLine(pen, tfArray4[j], tfArray5[j]);
                                                }
                                                else if (w > 1)
                                                {
                                                    RectangleF rect = new RectangleF(num15, num16, (float)w, height);
                                                    g.DrawLine(pen, tfArray4[j], new PointF(tfArray4[j].X, rect.Top));
                                                    g.DrawLine(pen, tfArray5[j], new PointF(tfArray5[j].X, rect.Bottom));
                                                    if (brush2 != null)
                                                    {
                                                        g.FillRectangle(brush2, rect);
                                                    }
                                                    RectangleF[] rects = new RectangleF[] { rect };
                                                    g.DrawRectangles(pen, rects);
                                                }
                                            }
                                        }
                                    }
                                else
                                    this.DrawPoints(g, formulaData, currentPen, points, ColumnWidth);
                                continue;

                            default:
                                this.DrawPoints(g, formulaData, currentPen, points, ColumnWidth);
                                break;
                        }                        
                    }
                }
                finally
                {
                    g.ResetClip();
                }
            
        
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.FormulaDataArray.Count; i++)
            {
                if (i != 0)
                {
                    builder.Append(';');
                }
                builder.Append(this.FormulaDataArray[i]);
            }
            return builder.ToString();
        }

        public IDataProvider DataProvider
        {
            get
            {
                return this.Parent.DataProvider;
            }
        }

        public FormulaData this[int Index]
        {
            get
            {
                return this.FormulaDataArray[Index];
            }
        }

        public FormulaData this[string Name]
        {
            get
            {
                return this.FormulaDataArray[Name];
            }
        }

        public string Name
        {
            get
            {
                if (this.Formulas.Count > 0)
                {
                    return this.Formulas[0].GetType().ToString();
                }
                return "NoName";
            }
        }
    }
}

