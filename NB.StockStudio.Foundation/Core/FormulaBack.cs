namespace NB.StockStudio.Foundation
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FormulaBack : ICloneable
    {
        private Brush backGround;
        private Pen bottomPen;
        private Pen leftPen;
        private Pen rightPen;
        private Pen topPen;

        public FormulaBack()
        {
            this.BackGround = new SolidBrush(Color.White);
            this.LeftPen = new Pen(Color.Black, 1f);
            this.TopPen = new Pen(Color.Black, 1f);
            this.RightPen = new Pen(Color.Black, 1f);
            this.BottomPen = new Pen(Color.Black, 1f);
        }

        public object Clone()
        {
            FormulaBack back = new FormulaBack();
            back.BackGround = (Brush) this.BackGround.Clone();
            back.LeftPen = (Pen) this.LeftPen.Clone();
            back.RightPen = (Pen) this.RightPen.Clone();
            back.BottomPen = (Pen) this.BottomPen.Clone();
            back.TopPen = (Pen) this.TopPen.Clone();
            return back;
        }

        public void Render(Graphics g, Rectangle R)
        {
            g.FillRectangle(this.BackGround, R);
            R.Width--;
            R.Height--;
            int num = (int) (this.LeftPen.Width - 1f);
            if (num >= 0)
            {
                g.DrawLine(this.LeftPen, R.Left + num, R.Top, R.Left + num, R.Bottom);
            }
            num = (int) (this.TopPen.Width - 1f);
            if (num >= 0)
            {
                g.DrawLine(this.TopPen, R.Left, R.Top + num, R.Right, R.Top + num);
            }
            num = (int) (this.RightPen.Width - 1f);
            if (num >= 0)
            {
                g.DrawLine(this.RightPen, R.Right, R.Top, R.Right, R.Bottom);
            }
            num = (int) (this.BottomPen.Width - 1f);
            if (num >= 0)
            {
                g.DrawLine(this.BottomPen, R.Left, R.Bottom, R.Right, R.Bottom);
            }

            //WaterMark(g, R, "Adam");
        }
        
        public Brush BackGround
        {
            get
            {
                return this.backGround;
            }
            set
            {
                this.backGround = value;
            }
        }

        public Pen BottomPen
        {
            get
            {
                return this.bottomPen;
            }
            set
            {
                this.bottomPen = value;
            }
        }

        public Color FrameColor
        {
            set
            {
                this.LeftPen.Color = value;
                this.TopPen.Color = value;
                this.RightPen.Color = value;
                this.BottomPen.Color = value;
            }
        }

        public int FrameWidth
        {
            set
            {
                this.LeftPen.Width = value;
                this.TopPen.Width = value;
                this.RightPen.Width = value;
                this.BottomPen.Width = value;
            }
        }

        public Pen LeftPen
        {
            get
            {
                return this.leftPen;
            }
            set
            {
                this.leftPen = value;
            }
        }

        public Pen RightPen
        {
            get
            {
                return this.rightPen;
            }
            set
            {
                this.rightPen = value;
            }
        }

        public Pen TopPen
        {
            get
            {
                return this.topPen;
            }
            set
            {
                this.topPen = value;
            }
        }
    }
}

