namespace NB.StockStudio.Foundation
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;

    public class FormulaSkin
    {
        public XFormatCollection AllXFormats;
        public FormulaAxisX AxisX;
        public AxisXCollection AxisXs;
        public FormulaAxisY AxisY;
        private FormulaBack back;
        public Brush[] BarBrushes;
        public Pen[] BarPens = new Pen[] { new Pen(Color.Black, 1f), new Pen(Color.White, 1f), new Pen(Color.Blue, 1f) };
        public Color[] Colors = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Black, Color.Orange, Color.DarkGray, Color.DarkTurquoise };
        public Pen CursorPen;
        public string DisplayName;
        public bool DrawVolumeAsLine;
        public Pen LinePen = new Pen(Color.Black);
        public Brush NameBrush;
        public Font NameFont;
        public ScaleType ScaleType;
        public bool ShowDateInLastArea;
        public bool ShowValueLabel;
        public StockRenderType StockRenderType = StockRenderType.Candle;
        public Font TextFont;

        public FormulaSkin()
        {
            Brush[] brushArray = new Brush[3];
            brushArray[2] = Brushes.Blue;
            this.BarBrushes = brushArray;
            this.NameBrush = new SolidBrush(Color.Black);
            this.NameFont = new Font("Verdana", 7f);
            this.TextFont = new Font("Verdana", 7f);
            this.CursorPen = new Pen(Color.Black);
            this.ShowValueLabel = true;
            this.AxisX = new FormulaAxisX();
            this.AxisXs = new AxisXCollection();
            this.AxisXs.Add(this.AxisX);
            this.AxisY = new FormulaAxisY();
            this.Back = new FormulaBack();
        }

        public void Bind(FormulaChart fc)
        {
            foreach (FormulaArea area in fc.Areas)
            {
                if (area.AxisXs.Count != this.AxisXs.Count)
                {
                    area.AxisXs.Clear();
                    for (int j = 0; j < this.AxisXs.Count; j++)
                    {
                        area.AxisXs.Add(new FormulaAxisX());
                    }
                    if (this.AxisXs.Count > 0)
                    {
                        area.AxisX = area.AxisXs[0];
                    }
                }
                for (int i = 0; i < this.AxisXs.Count; i++)
                {
                    area.AxisXs[i].CopyFrom(this.AxisXs[i]);
                }
                foreach (FormulaAxisY sy in area.AxisYs)
                {
                    sy.CopyFrom(this.AxisY);
                }
                area.Back = (FormulaBack) this.Back.Clone();
                area.Colors = (Color[]) this.Colors.Clone();
                area.LinePen = (Pen) this.LinePen.Clone();
                area.BarPens = (Pen[]) this.BarPens.Clone();
                area.BarBrushes = (Brush[]) this.BarBrushes.Clone();
                area.NameBrush = (Brush) this.NameBrush.Clone();
                area.NameFont = (Font) this.NameFont.Clone();
                area.TextFont = (Font) this.TextFont.Clone();
                area.DrawVolumeAsLine = this.DrawVolumeAsLine;
                area.StockRenderType = this.StockRenderType;
            }
            fc.CursorPen = this.CursorPen;
            fc.ShowDateInLastArea = this.ShowDateInLastArea;
            fc.ShowValueLabel = this.ShowValueLabel;
            fc.AllXFormats = this.AllXFormats;
        }

        public static string[] GetBuildInSkins()
        {
            PropertyInfo[] properties = typeof(FormulaSkin).GetProperties(BindingFlags.Public | BindingFlags.Static);
            string[] strArray = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                strArray[i] = properties[i].Name;
            }
            return strArray;
        }

        public static FormulaSkin GetSkinByName(string SkinName)
        {
            try
            {
                Type type = typeof(FormulaSkin);
                return (FormulaSkin) type.InvokeMember(SkinName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static, null, null, null);
            }
            catch
            {
                return null;
            }
        }

        public FormulaBack Back
        {
            get
            {
                return this.back;
            }
            set
            {
                this.back = value;
            }
        }

        public static FormulaSkin BlackBlue
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.AxisX.Back.BackGround = new SolidBrush(Color.FromArgb(0xe0, 0xe0, 0xe0));
                skin.AxisX.AxisLabelAlign = AxisLabelAlign.TickCenter;
                skin.AxisX.DataCycle = DataCycle.Day();
                skin.AxisX.DataCycle.Repeat = 20;
                skin.AllXFormats = XFormatCollection.Default;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.BarPens = new Pen[] { new Pen(Color.Black, 1f), new Pen(Color.Black, 1f), new Pen(Color.Blue, 1f) };
                Brush[] brushArray = new Brush[3];
                brushArray[2] = Brushes.Blue;
                skin.BarBrushes = brushArray;
                skin.AxisY.AxisPos = AxisPos.Left;
                skin.CursorPen = new Pen(Color.Black);
                return skin;
            }
        }

        public static FormulaSkin BlackWhite
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.Back.BackGround = Brushes.Black;
                skin.Back.FrameColor = Color.White;
                skin.Colors = new Color[] { Color.White, Color.Yellow, Color.LightBlue, Color.LightCoral, Color.LightGray, Color.LightPink, Color.Olive, Color.LightPink, Color.Aqua };
                skin.BarPens = new Pen[] { Pens.DeepPink, Pens.White, Pens.Aqua };
                Brush[] brushArray = new Brush[3];
                brushArray[2] = Brushes.Aqua;
                skin.BarBrushes = brushArray;
                skin.NameBrush = Brushes.Yellow;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AxisX.Back.BackGround = Brushes.Black;
                skin.AxisX.Back.FrameColor = Color.White;
                skin.AxisX.LabelBrush = new SolidBrush(Color.White);
                skin.AllXFormats = XFormatCollection.Default;
                skin.AxisY.LabelBrush = Brushes.White;
                skin.AxisY.Back = (FormulaBack) skin.AxisX.Back.Clone();
                skin.AxisY.MultiplyBack.BackGround = Brushes.Black;
                skin.AxisY.MultiplyBack.FrameColor = Color.Yellow;
                skin.CursorPen = new Pen(Color.White);
                return skin;
            }
        }

        public static FormulaSkin CyanGreen
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.Colors = new Color[] { Color.Blue, Color.Fuchsia, Color.DarkGray, Color.Maroon, Color.DarkGreen, Color.Cyan, Color.Olive };
                skin.BarPens = new Pen[] { Pens.Fuchsia, Pens.White, Pens.Green };
                Brush[] brushArray = new Brush[3];
                brushArray[2] = Brushes.Green;
                skin.BarBrushes = brushArray;
                skin.NameBrush = Brushes.Black;
                skin.Back.BackGround = new SolidBrush(Color.FromArgb(0xc0, 0xe0, 0xc0));
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AxisX.Back.BackGround = new SolidBrush(Color.FromArgb(0xd0, 0xe0, 0xd0));
                skin.AllXFormats = XFormatCollection.Default;
                skin.AxisY.Back.BackGround = new SolidBrush(Color.FromArgb(0xd0, 0xe0, 0xd0));
                return skin;
            }
        }

        public static FormulaSkin GreenRed
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.AxisX.Back.BackGround = new SolidBrush(Color.FromArgb(0xe0, 0xe0, 0xe0));
                skin.AxisX.AxisLabelAlign = AxisLabelAlign.TickCenter;
                skin.AxisX.DataCycle = DataCycle.Day();
                skin.AxisX.DataCycle.Repeat = 20;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AllXFormats = XFormatCollection.Default;
                skin.BarPens = new Pen[] { Pens.Green, Pens.Black, Pens.Red };
                skin.BarBrushes = new Brush[] { new SolidBrush(Color.FromArgb(0x88, 0xff, 0x88)), Brushes.White, Brushes.Red };
                skin.CursorPen = new Pen(Color.Black);
                return skin;
            }
        }

        public static FormulaSkin GreenWhite
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.Colors = new Color[] { Color.Blue, Color.Fuchsia, Color.DarkGray, Color.Maroon, Color.DarkGreen, Color.DarkBlue, Color.Olive };
                skin.BarPens = new Pen[] { Pens.Red, Pens.White, Pens.Green };
                Brush[] brushArray = new Brush[3];
                brushArray[0] = Brushes.White;
                brushArray[2] = Brushes.Green;
                skin.BarBrushes = brushArray;
                skin.NameBrush = Brushes.Black;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AllXFormats = XFormatCollection.Default;
                return skin;
            }
        }

        public static FormulaSkin OceanBlue
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.Back.BackGround = Brushes.Navy;
                skin.Back.FrameColor = Color.Yellow;
                skin.Colors = new Color[] { Color.White, Color.Yellow, Color.Fuchsia, Color.Green, Color.LightGray, Color.Blue, Color.Olive, Color.Purple, Color.Aqua };
                skin.BarPens = new Pen[] { Pens.Red, Pens.White, Pens.Aqua };
                Brush[] brushArray = new Brush[3];
                brushArray[2] = Brushes.Aqua;
                skin.BarBrushes = brushArray;
                skin.NameBrush = Brushes.Yellow;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AxisX.Back.BackGround = Brushes.Blue;
                skin.AxisX.Back.FrameColor = Color.Yellow;
                skin.AxisX.LabelBrush = new SolidBrush(Color.White);
                skin.AllXFormats = XFormatCollection.Default;
                skin.AxisY.LabelBrush = Brushes.White;
                skin.AxisY.Back = (FormulaBack) skin.AxisX.Back.Clone();
                skin.AxisY.MultiplyBack.BackGround = Brushes.Black;
                skin.AxisY.MultiplyBack.FrameColor = Color.Yellow;
                skin.CursorPen = new Pen(Color.Yellow);
                return skin;
            }
        }

        public static FormulaSkin PinkBlue
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.AxisX.Back.BackGround = Brushes.White;
                skin.AxisX.AxisLabelAlign = AxisLabelAlign.TickCenter;
                skin.AxisX.DataCycle = DataCycle.Week();
                skin.AxisX.DataCycle.Repeat = 2;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AxisX.Format = "%d";
                skin.AxisX.AxisLabelAlign = AxisLabelAlign.TickRight;
                skin.AxisX.MajorTick.FullTick = true;
                skin.AxisX.MajorTick.Inside = false;
                skin.AxisX.MajorTick.LinePen.DashPattern = new float[] { 1f, 3f };
                skin.AxisX.MajorTick.LinePen.Color = Color.Black;
                skin.AxisX.MajorTick.TickPen.Color = Color.Black;
                skin.AxisX.MinorTick.Visible = false;
                skin.AxisX.MinorTick.DataCycle = DataCycle.Week();
                skin.AxisX.MinorTick.LinePen = (Pen) skin.AxisX.MajorTick.LinePen.Clone();
                skin.AxisX.MinorTick.ShowText = false;
                skin.AxisX.MinorTick.ShowLine = true;
                FormulaAxisX fax = new FormulaAxisX();
                fax.CopyFrom(skin.AxisX);
                fax.DataCycle = DataCycle.Quarter();
                fax.Format = "MMMM";
                fax.MajorTick.LinePen = Pens.Black;
                fax.MajorTick.ShowText = false;
                fax.MajorTick.TickPen.Color = Color.Black;
                fax.MinorTick.FullTick = true;
                fax.MinorTick.Inside = false;
                fax.MinorTick.ShowText = true;
                fax.MinorTick.ShowTick = true;
                fax.MinorTick.ShowLine = false;
                fax.MinorTick.DataCycle = DataCycle.Month();
                fax.MinorTick.Format = "{yy}MMMM";
                fax.MinorTick.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                fax.MinorTick.TickPen.Color = Color.Black;
                skin.AxisXs.Add(fax);
                skin.AxisY.MajorTick.LinePen.DashPattern = new float[] { 1f, 3f };
                skin.AxisY.MajorTick.LinePen.Color = Color.Black;
                skin.AllXFormats = XFormatCollection.TwoAxisX;
                skin.BarPens = new Pen[] { new Pen(Color.Blue, 2f), new Pen(Color.Black, 2f), new Pen(Color.DeepPink, 2f) };
                skin.BarBrushes = new Brush[3];
                skin.CursorPen = new Pen(Color.Black);
                skin.ShowDateInLastArea = true;
                skin.StockRenderType = StockRenderType.Bar;
                return skin;
            }
        }

        public static FormulaSkin RedBlack
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.Back.BackGround = Brushes.Ivory;
                skin.Back.FrameWidth = 1;
                skin.Colors = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Fuchsia, Color.DarkGray, Color.Maroon, Color.DarkGreen, Color.Plum, Color.Olive };
                skin.BarPens = new Pen[] { Pens.Black, Pens.Black, Pens.Firebrick };
                Brush[] brushArray = new Brush[3];
                brushArray[2] = Brushes.Firebrick;
                skin.BarBrushes = brushArray;
                skin.NameBrush = Brushes.Black;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AxisX.Back.BackGround = Brushes.WhiteSmoke;
                skin.AxisX.Back.LeftPen.Width = 1f;
                skin.AxisX.MajorTick.LinePen = Pens.DarkKhaki;
                skin.AllXFormats = XFormatCollection.Default;
                skin.AxisY.Back.BackGround = Brushes.WhiteSmoke;
                skin.AxisY.MultiplyBack.BackGround = Brushes.BlanchedAlmond;
                skin.AxisY.MajorTick.LinePen = Pens.DarkKhaki;
                skin.AxisY.Back.RightPen.Width = 1f;
                skin.AxisY.Back.TopPen.Width = 1f;
                skin.CursorPen = new Pen(Color.Black);
                return skin;
            }
        }

        public static FormulaSkin RedWhite
        {
            get
            {
                FormulaSkin skin = new FormulaSkin();
                skin.Back.BackGround = Brushes.WhiteSmoke;
                skin.Back.FrameWidth = 2;
                skin.Colors = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Fuchsia, Color.DarkGray, Color.Maroon, Color.DarkGreen, Color.Plum, Color.Olive };
                skin.BarPens = new Pen[] { Pens.Black, Pens.Black, Pens.Maroon };
                Brush[] brushArray = new Brush[3];
                brushArray[2] = Brushes.Maroon;
                skin.BarBrushes = brushArray;
                skin.NameBrush = Brushes.Black;
                skin.AxisX.DateFormatProvider = DateTimeFormatInfo.InvariantInfo;
                skin.AxisX.Back.BackGround = Brushes.Azure;
                skin.AxisX.Back.LeftPen.Width = 2f;
                skin.AllXFormats = XFormatCollection.Default;
                skin.AxisY.Back.BackGround = Brushes.AliceBlue;
                skin.AxisY.MultiplyBack.BackGround = Brushes.BlanchedAlmond;
                skin.AxisY.Back.RightPen.Width = 2f;
                skin.AxisY.Back.TopPen.Width = 2f;
                skin.CursorPen = new Pen(Color.Black);
                return skin;
            }
        }
    }
}

