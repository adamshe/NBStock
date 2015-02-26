namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class RectangleObject : FillPolygonObject
    {
        private int roundWidth = 0;
        private SnapType snap;

        public override PointF[] CalcPoint()
        {
            base.SetSnapPrice(this.snap);
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            if (tfArray[0].X > tfArray[1].X)
            {
                float x = tfArray[0].X;
                tfArray[0].X = tfArray[1].X;
                tfArray[1].X = x;
            }
            if (tfArray[0].Y > tfArray[1].Y)
            {
                float y = tfArray[0].Y;
                tfArray[0].Y = tfArray[1].Y;
                tfArray[1].Y = y;
            }
            if (this.snap == SnapType.Band)
            {
                Rectangle rect = base.Area.Canvas.Rect;
                tfArray[0].Y = rect.Top - 30;
                tfArray[1].Y = rect.Bottom + 30;
            }
            float num3 = Math.Min((float) this.roundWidth, Math.Min((float) (tfArray[1].X - tfArray[0].X), (float) (tfArray[1].Y - tfArray[0].Y)) / 2f);
            ArrayList list = new ArrayList();
            list.Add(tfArray[0]);
            list.Add(new PointF(tfArray[1].X, tfArray[0].Y));
            list.Add(tfArray[1]);
            list.Add(new PointF(tfArray[0].X, tfArray[1].Y));
            tfArray = (PointF[]) list.ToArray(typeof(PointF));
            GraphicsPath path = new GraphicsPath();
            if (num3 > 0f)
            {
                path.AddArc(tfArray[0].X, tfArray[0].Y, num3 * 2f, num3 * 2f, 180f, 90f);
            }
            path.AddLine(tfArray[0].X + num3, tfArray[0].Y, tfArray[1].X - num3, tfArray[1].Y);
            if (num3 > 0f)
            {
                path.AddArc(tfArray[1].X - (num3 * 2f), tfArray[0].Y, num3 * 2f, num3 * 2f, 270f, 90f);
            }
            path.AddLine(tfArray[1].X, tfArray[1].Y + num3, tfArray[2].X, tfArray[2].Y - num3);
            if (num3 > 0f)
            {
                path.AddArc((float) (tfArray[2].X - (num3 * 2f)), (float) (tfArray[2].Y - (num3 * 2f)), (float) (num3 * 2f), (float) (num3 * 2f), 0f, 90f);
            }
            path.AddLine(tfArray[2].X - num3, tfArray[2].Y, tfArray[3].X + num3, tfArray[3].Y);
            if (num3 > 0f)
            {
                path.AddArc(tfArray[3].X, tfArray[3].Y - (num3 * 2f), num3 * 2f, num3 * 2f, 90f, 90f);
            }
            path.AddLine(tfArray[3].X, tfArray[3].Y - num3, tfArray[0].X, tfArray[0].Y + num3);
            path.Flatten();
            return path.PathPoints;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Rectangle", typeof(RectangleObject), null, "Shape", "Rect", 700), new ObjectInit("Price Rectangle", typeof(RectangleObject), "SnapPrice", "Shape", "RectPrice"), new ObjectInit("Band Rectangle", typeof(RectangleObject), "SnapBand", "Shape", "RectBand"), new ObjectInit("Round Rectangle", typeof(RectangleObject), "Round20", "Shape", "RectRound") };
        }

        public void Round20()
        {
            this.roundWidth = 20;
        }

        public void SnapBand()
        {
            this.snap = SnapType.Band;
            base.Brush.BrushStyle = BrushStyle.Solid;
            base.Brush.Color = Color.FromArgb(160, 160, 0x40);
            base.Brush.Alpha = 40;
        }

        public void SnapPrice()
        {
            this.snap = SnapType.Price;
            base.Brush.BrushStyle = BrushStyle.Solid;
            base.Brush.Color = Color.FromArgb(160, 160, 0x40);
            base.Brush.Alpha = 40;
        }

        public int RoundWidth
        {
            get
            {
                return this.roundWidth;
            }
            set
            {
                this.roundWidth = value;
            }
        }

        public SnapType Snap
        {
            get
            {
                return this.snap;
            }
            set
            {
                this.snap = value;
            }
        }
    }
}

