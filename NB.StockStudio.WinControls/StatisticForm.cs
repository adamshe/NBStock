namespace NB.StockStudio.WinControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class StatisticForm : Form
    {
        public bool AutoHeight = true;
        public bool AutoWidth = true;
        public int ColumnSpace = 2;
        public int[] ColumnWidth = new int[] { 80, 80 };
        private Container components = null;
        private string Data;
        public static bool Enable;
        public Color FrameColor = Color.LightGray;
        private static Random Rnd = new Random();
        public Brush[] RowBrushs = new Brush[] { Brushes.Khaki, Brushes.Beige, Brushes.WhiteSmoke };
        public int RowHeight = 20;
        public int RowSpace = 2;
        private PointF StartDrag = PointF.Empty;
        private static StatisticForm Statistic;

        public StatisticForm()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static void FreeStatistic()
        {
            Statistic = null;
        }

        public static void HideStatistic()
        {
            if (Statistic != null)
            {
                Statistic.Hide();
            }
        }

        private void InitializeComponent()
        {
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0xe2, 0x160);
            this.Font = new Font("Verdana", 8.25f);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "StatisticForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Statistic";
            base.TopMost = true;
            base.MouseDown += new MouseEventHandler(this.StatisticForm_MouseDown);
            base.Closing += new CancelEventHandler(this.StatisticForm_Closing);
            base.Load += new EventHandler(this.StatisticForm_Load);
            base.MouseUp += new MouseEventHandler(this.StatisticForm_MouseUp);
            base.Paint += new PaintEventHandler(this.StatisticForm_Paint);
            base.MouseMove += new MouseEventHandler(this.StatisticForm_MouseMove);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        public void PaintForm(Graphics g, Rectangle Rect)
        {
            string data = this.Data;
            if (data.EndsWith(";"))
            {
                data = data.Substring(0, data.Length - 1);
            }
            string[] strArray = data.Split(new char[] { ';' });
            string[][] strArray2 = new string[strArray.Length][];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray2[i] = strArray[i].Split(new char[] { '=' });
            }
            if (this.AutoWidth)
            {
                this.ColumnWidth[0] = -2147483648;
                this.ColumnWidth[1] = -2147483648;
                for (int k = 0; k < 2; k++)
                {
                    for (int m = 0; m < strArray.Length; m++)
                    {
                        SizeF ef = g.MeasureString(strArray2[m][k], this.Font);
                        this.ColumnWidth[k] = Math.Max(this.ColumnWidth[k], (int) ef.Width);
                    }
                }
                int num4 = (this.ColumnWidth[0] + this.ColumnWidth[1]) + (this.ColumnSpace * 4);
                if (num4 > base.Width)
                {
                    base.Width = num4;
                }
            }
            if (this.AutoHeight)
            {
                this.RowHeight = (int) g.MeasureString(strArray2[0][0], this.Font, 0x3e8).Height;
                base.Height = (((strArray.Length * (this.RowHeight + this.RowSpace)) + base.Height) - base.ClientRectangle.Height) + 4;
            }
            Pen pen = new Pen(this.FrameColor);
            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle.Inflate(-1, -1);
            clientRectangle.Offset(Rect.X, Rect.Y);
            g.DrawRectangle(new Pen(Color.Black, 2f), clientRectangle);
            for (int j = 0; j < strArray.Length; j++)
            {
                for (int n = 0; n < 2; n++)
                {
                    Rectangle rect = new Rectangle(((this.ColumnSpace + Rect.X) + ((n == 1) ? (this.ColumnWidth[0] + this.ColumnSpace) : 0)) + n, (2 + Rect.Y) + ((this.RowHeight + this.RowSpace) * j), (n == 0) ? (this.ColumnWidth[n] + this.ColumnSpace) : ((clientRectangle.Width - this.ColumnWidth[0]) - (this.ColumnSpace * 3)), (this.RowHeight + this.RowSpace) - 1);
                    g.DrawRectangle(pen, rect);
                    g.FillRectangle(this.RowBrushs[j % this.RowBrushs.Length], rect);
                    StringFormat format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    if (n == 0)
                    {
                        format.Alignment = StringAlignment.Far;
                    }
                    g.DrawString(strArray2[j][n], this.Font, new SolidBrush(this.ForeColor), rect, format);
                }
            }
        }

        public static void PaintTo(Graphics g)
        {
            if ((Statistic != null) && Statistic.Visible)
            {
                Rectangle rect = new Rectangle(Statistic.Location, Statistic.Size);
                rect.Offset(0, -22);
                Statistic.PaintForm(g, rect);
            }
        }

        public void RefreshData(string s)
        {
            this.Data = s;
            base.Invalidate();
        }

        public static void ShowStatistic(Form Owner, string s)
        {
            if (Statistic == null)
            {
                Statistic = new StatisticForm();
                Statistic.Left = 30;
                Statistic.Top = (Owner.Bottom - Statistic.Height) - 40;
            }
            Statistic.Owner = Owner;
            if (!Statistic.Visible)
            {
                Statistic.Show();
            }
            if (Owner != null)
            {
                if (Owner.ParentForm != null)
                {
                    Owner.ParentForm.Focus();
                }
                Owner.Focus();
            }
            Statistic.RefreshData(s);
        }

        private void StatisticForm_Closing(object sender, CancelEventArgs e)
        {
            Statistic.Visible = false;
            Enable = false;
            e.Cancel = true;
        }

        private void StatisticForm_Load(object sender, EventArgs e)
        {
        }

        private void StatisticForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.StartDrag = new PointF((float) e.X, (float) e.Y);
        }

        private void StatisticForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.StartDrag != PointF.Empty)
            {
                base.Location = Point.Round(new PointF((base.Location.X + e.X) - this.StartDrag.X, (base.Location.Y + e.Y) - this.StartDrag.Y));
            }
        }

        private void StatisticForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.StartDrag = PointF.Empty;
        }

        private void StatisticForm_Paint(object sender, PaintEventArgs e)
        {
            this.PaintForm(e.Graphics, base.ClientRectangle);
        }
    }
}

