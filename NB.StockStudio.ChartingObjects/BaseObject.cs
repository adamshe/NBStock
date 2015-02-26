namespace NB.StockStudio.ChartingObjects
{
    using NB.StockStudio.Foundation;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Xml.Serialization;

    [Serializable]
    public class BaseObject
    {
        private FormulaArea area;
        private string areaName;
        private ObjectPoint[] controlPoints;
        [NonSerialized]
        protected Graphics CurrentGraphics;
        [XmlIgnore]
        public PointF[] InitPoints;
        [NonSerialized, XmlIgnore]
        public bool InMove;
        [NonSerialized, XmlIgnore]
        public bool InSetup;
        private ObjectPen linePen;
        [NonSerialized]
        protected ObjectManager Manager;
        private ObjectSmoothingMode smoothingMode;

        public BaseObject()
        {
            this.linePen = new ObjectPen();
            this.smoothingMode = ObjectSmoothingMode.AntiAlias;
            this.Init();
        }

        public BaseObject(ObjectManager Manager) : this()
        {
            this.SetObjectManager(Manager);
        }

        public float CalcDelta(FormulaData fd, double A, double B, int Bar1, int Bar2, string LineName, bool CalcMax)
        {
            double[] data = fd[LineName];
            if (data == null)
            {
                data = fd.Data;
            }
            double minValue = double.MinValue;
            if (!CalcMax)
            {
                minValue = double.MaxValue;
            }
            for (int i = Math.Max(0, Bar1); i < Math.Min(Bar2, data.Length); i++)
            {
                double num3 = A + (B * (i - Bar1));
                if (CalcMax)
                {
                    minValue = Math.Max(minValue, data[i] - num3);
                }
                else
                {
                    minValue = Math.Min(minValue, data[i] - num3);
                }
            }
            return (float) minValue;
        }

        public double Dist(PointF p1, PointF p2)
        {
            double num = p1.X - p2.X;
            double num2 = p1.Y - p2.Y;
            return Math.Sqrt((num * num) + (num2 * num2));
        }

        public float Dist(int X, int Y, PointF p1, PointF p2)
        {
            float num = p1.X - p2.X;
            float num2 = p1.Y - p2.Y;
            return ((((X - p1.X) * (p2.Y - p1.Y)) - ((p2.X - p1.X) * (Y - p1.Y))) / ((float) Math.Sqrt((double) ((num * num) + (num2 * num2)))));
        }

        public virtual void Draw(Graphics g)
        {
            if (this.SmoothingMode == ObjectSmoothingMode.AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }
            else if (this.SmoothingMode == ObjectSmoothingMode.Default)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
            this.CurrentGraphics = g;
        }

        public void DrawControlPoint(Graphics g)
        {
            int num = 6 + (this.LinePen.Width / 2);
            foreach (PointF tf in this.ToPoints(this.ControlPoints))
            {
                try
                {
                    g.FillRectangle(Brushes.LightGreen, tf.X - (num / 2), tf.Y - (num / 2), (float) num, (float) num);
                    g.DrawRectangle(Pens.Black, tf.X - (num / 2), tf.Y - (num / 2), (float) num, (float) num);
                }
                catch
                {
                }
            }
        }

        public void ExpandLine(ref PointF pf1, ref PointF pf2)
        {
            if ((this.Area != null) && (this.Area.Canvas != null))
            {
                float num = pf2.X - pf1.X;
                float num2 = pf2.Y - pf1.Y;
                float x = pf2.X;
                float y = pf2.Y;
                Rectangle rect = this.Area.Canvas.Rect;
                if (num < 0f)
                {
                    x = 0f;
                }
                else if (num > 0f)
                {
                    x = rect.Right;
                }
                if (num != 0f)
                {
                    y = (((x - pf2.X) / num) * num2) + pf2.Y;
                    if ((y < rect.Top) || (y > rect.Bottom))
                    {
                        if (y > rect.Bottom)
                        {
                            y = rect.Bottom;
                        }
                        else if (y < rect.Top)
                        {
                            y = rect.Top;
                        }
                        if (num2 != 0f)
                        {
                            x = (((y - pf2.Y) / num2) * num) + pf2.X;
                        }
                    }
                }
                else if (num2 > 0f)
                {
                    y = rect.Bottom;
                }
                else
                {
                    y = rect.Top;
                }
                pf2.X = x;
                pf2.Y = y;
            }
        }

        public void ExpandLine2(ref PointF pf1, ref PointF pf2)
        {
            this.ExpandLine(ref pf1, ref pf2);
            this.ExpandLine(ref pf2, ref pf1);
        }

        public int GetControlPoint(int X, int Y)
        {
            for (int i = 0; i < this.ControlPoints.Length; i++)
            {
                float num2 = this.ToPointF(this.ControlPoints[i]).X - X;
                float num3 = this.ToPointF(this.ControlPoints[i]).Y - Y;
                if (((num2 * num2) + (num3 * num3)) < 9f)
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual RectangleF GetMaxRect()
        {
            return this.GetMaxRect(this.ToPoints(this.ControlPoints));
        }

        public virtual RectangleF GetMaxRect(PointF[] pfs)
        {
            RectangleF maxRect = this.GetMaxRect(pfs, this.LinePen.Width + 6);
            if (maxRect.X < 0f)
            {
                maxRect.X = 0f;
            }
            if (maxRect.Y < 0f)
            {
                maxRect.Y = 0f;
            }
            return maxRect;
        }

        public RectangleF GetMaxRect(PointF[] pfs, int w)
        {
            float maxValue = float.MaxValue;
            float y = float.MaxValue;
            float minValue = float.MinValue;
            float num4 = float.MinValue;
            foreach (PointF tf in pfs)
            {
                if (tf.X < maxValue)
                {
                    maxValue = tf.X;
                }
                if (tf.X > minValue)
                {
                    minValue = tf.X;
                }
                if (tf.Y < y)
                {
                    y = tf.Y;
                }
                if (tf.Y > num4)
                {
                    num4 = tf.Y;
                }
            }
            return new RectangleF(maxValue - w, y - w, (minValue - maxValue) + (w * 2), (num4 - y) + (w * 2));
        }

        public virtual Region GetRegion()
        {
            return new Region(this.GetMaxRect());
        }

        public void Init()
        {
            this.ControlPoints = new ObjectPoint[this.ControlPointNum];
        }

        public bool InLineSegment(int X, int Y, ObjectSegment os, int PenWidth)
        {
            return this.InLineSegment(X, Y, os.op1, os.op2, PenWidth);
        }

        public bool InLineSegment(int X, int Y, PointF[] pfs, int PenWidth)
        {
            return this.InLineSegment(X, Y, pfs, PenWidth, false);
        }

        public bool InLineSegment(int X, int Y, PointF[] pfs, int PenWidth, bool Closed)
        {
            if (pfs != null)
            {
                for (int i = 0; i < (pfs.Length - (Closed ? 0 : 1)); i++)
                {
                    bool flag = this.InLineSegment(X, Y, pfs[i], pfs[(i + 1) % pfs.Length], PenWidth);
                    if (flag)
                    {
                        return flag;
                    }
                }
            }
            return false;
        }

        public bool InLineSegment(int X, int Y, ObjectPoint p1, ObjectPoint p2, int PenWidth)
        {
            return this.InLineSegment(X, Y, this.ToPointF(p1), this.ToPointF(p2), PenWidth);
        }

        public bool InLineSegment(int X, int Y, PointF p1, PointF p2, int PenWidth)
        {
            float num = Math.Max(p1.X, p2.X);
            float num2 = Math.Min(p1.X, p2.X);
            float num3 = Math.Max(p1.Y, p2.Y);
            float num4 = Math.Min(p1.Y, p2.Y);
            return ((((Math.Abs(this.Dist(X, Y, p1, p2)) <= PenWidth) && (X <= (num + PenWidth))) && ((X >= (num2 - PenWidth)) && (Y <= (num3 + PenWidth)))) && (Y >= (num4 - PenWidth)));
        }

        public virtual bool InObject(int X, int Y)
        {
            if (this.ControlPointNum == 2)
            {
                PointF tf = this.ToPointF(this.ControlPoints[0]);
                PointF tf2 = this.ToPointF(this.ControlPoints[1]);
                return RectangleF.FromLTRB(tf.X, tf.Y, tf2.X, tf2.Y).Contains((float) X, (float) Y);
            }
            return false;
        }

        public bool PointInEllipse(int X, int Y, float CenterX, float CenterY, float r1, float r2, int PenWidth)
        {
            PointF tf;
            PointF tf2;
            float num = Math.Abs(r1) - Math.Abs(r2);
            if (num > 0f)
            {
                float num2 = (float) Math.Sqrt((double) ((r1 * r1) - (r2 * r2)));
                tf = new PointF(CenterX + num2, CenterY);
                tf2 = new PointF(CenterX - num2, CenterY);
            }
            else
            {
                float num3 = (float) Math.Sqrt((double) ((r2 * r2) - (r1 * r1)));
                tf = new PointF(CenterX, CenterY + num3);
                tf2 = new PointF(CenterX, CenterY - num3);
            }
            PointF tf3 = new PointF((float) X, (float) Y);
            float num4 = Math.Max(Math.Abs(r1), Math.Abs(r2));
            return (Math.Abs((double) ((this.Dist(tf3, tf) + this.Dist(tf3, tf2)) - (num4 * 2f))) <= PenWidth);
        }

        public virtual ObjectInit[] RegObject()
        {
            return null;
        }

        public string ReplaceTag(ObjectPoint op, string s)
        {
            int num;
        Label_0005:
            num = s.IndexOf('{');
            int index = s.IndexOf('}');
            if (index <= num)
            {
                return s;
            }
            string str = s.Substring(num + 1, (index - num) - 1);
            int length = str.IndexOf(':');
            string format = "";
            string strA = str;
            if (length > 0)
            {
                strA = str.Substring(0, length);
                format = str.Substring(length + 1);
            }
            double[] dd = this.Manager.Canvas.BackChart.DataProvider["DATE"];
            int num4 = FormulaChart.FindIndex(dd, op.X);
            if (num4 >= dd.Length)
            {
                return s;
            }
            if (string.Compare(strA, "D") == 0)
            {
                if (format == "")
                {
                    format = "yyyy-MM-dd";
                }
                strA = DateTime.FromOADate(dd[num4]).ToString(format);
                goto Label_01BA;
            }
            FormulaData objA = null;
            try
            {
                length = int.Parse(strA);
                if (length < this.Area.FormulaDataArray.Count)
                {
                    dd = this.Area.FormulaDataArray[length].Data;
                }
            }
            catch
            {
                objA = this.Area.FormulaDataArray[strA];
                if (object.Equals(objA, null))
                {
                    foreach (FormulaData data2 in this.Area.FormulaDataArray)
                    {
                        dd = data2[strA];
                        if (dd != null)
                        {
                            goto Label_018F;
                        }
                    }
                }
                else
                {
                    dd = objA.Data;
                }
            }
        Label_018F:
            if (dd != null)
            {
                if (format == "")
                {
                    format = "f2";
                }
                strA = dd[num4].ToString(format);
            }
        Label_01BA:
            s = s.Substring(0, num) + strA + s.Substring(index + 1);
            goto Label_0005;
        }

        public void SetObjectManager(ObjectManager Manager)
        {
            this.Manager = Manager;
        }

        public void SetSnapPrice(SnapType snap)
        {
            if (snap == SnapType.Price)
            {
                double[] dd = this.Manager.Canvas.BackChart.DataProvider["DATE"];
                int index = 0;
                int num2 = 1;
                if (this.ControlPointNum == 1)
                {
                    num2 = 0;
                }
                int a = FormulaChart.FindIndex(dd, this.ControlPoints[index].X);
                int b = FormulaChart.FindIndex(dd, this.ControlPoints[num2].X);
                if (a > b)
                {
                    this.Swap(ref a, ref b);
                }
                FormulaData data = this.Area.FormulaDataArray[0];
                double[] numArray2 = data["L"];
                if (numArray2 == null)
                {
                    numArray2 = data.Data;
                }
                double[] numArray3 = data["H"];
                if (numArray3 == null)
                {
                    numArray3 = data.Data;
                }
                if ((a < numArray2.Length) && (b < numArray2.Length))
                {
                    float maxValue = float.MaxValue;
                    float minValue = float.MinValue;
                    for (int i = a; i <= b; i++)
                    {
                        maxValue = Math.Min(maxValue, (float) numArray2[i]);
                        minValue = Math.Max(minValue, (float) numArray3[i]);
                    }
                    if (this.ControlPointNum > 1)
                    {
                        if (this.ControlPoints[0].Y < this.ControlPoints[1].Y)
                        {
                            this.Swap(ref maxValue, ref minValue);
                        }
                        this.ControlPoints[0].Y = minValue;
                        this.ControlPoints[1].Y = maxValue;
                    }
                    else if (this.ControlPoints[0].Y < maxValue)
                    {
                        this.ControlPoints[0].Y = maxValue;
                    }
                    else if (this.ControlPoints[0].Y > minValue)
                    {
                        this.ControlPoints[0].Y = minValue;
                    }
                }
            }
        }

        public void Swap(ref double A, ref double B)
        {
            double num = A;
            A = B;
            B = num;
        }

        public void Swap(ref int A, ref int B)
        {
            int num = A;
            A = B;
            B = num;
        }

        public void Swap(ref float A, ref float B)
        {
            float num = A;
            A = B;
            B = num;
        }

        public PointF ToPointF(ObjectPoint op)
        {
            FormulaChart backChart = this.Manager.Canvas.BackChart;
            if (backChart != null)
            {
                return backChart.GetPointAt(this.AreaName, op.X, null, op.Y);
            }
            return PointF.Empty;
        }

        public PointF[] ToPoints(ObjectPoint[] ops)
        {
            PointF[] tfArray = new PointF[ops.Length];
            for (int i = 0; i < tfArray.Length; i++)
            {
                tfArray[i] = this.ToPointF(ops[i]);
            }
            return tfArray;
        }

        [Browsable(false), XmlIgnore]
        public FormulaArea Area
        {
            get
            {
                return this.area;
            }
            set
            {
                this.area = value;
            }
        }

        [Browsable(false)]
        public string AreaName
        {
            get
            {
                return this.areaName;
            }
            set
            {
                this.areaName = value;
            }
        }

        [Browsable(false)]
        public virtual int ControlPointNum
        {
            get
            {
                return 2;
            }
        }

        [TypeConverter(typeof(PointArrayConverter))]
        public ObjectPoint[] ControlPoints
        {
            get
            {
                return this.controlPoints;
            }
            set
            {
                this.controlPoints = value;
            }
        }

        [Browsable(false)]
        public virtual int InitNum
        {
            get
            {
                return 2;
            }
        }

        public ObjectPen LinePen
        {
            get
            {
                return this.linePen;
            }
            set
            {
                this.linePen = value;
            }
        }

        public ObjectSmoothingMode SmoothingMode
        {
            get
            {
                return this.smoothingMode;
            }
            set
            {
                this.smoothingMode = value;
            }
        }
    }
}

