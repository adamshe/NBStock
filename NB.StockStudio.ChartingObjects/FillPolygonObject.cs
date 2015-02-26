namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Drawing;

    public class FillPolygonObject : PolygonObject
    {
        private ObjectBrush brush = new ObjectBrush(Color.Empty);

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            if ((this.Brush.BrushStyle != BrushStyle.Empty) && ((this.Brush.BrushStyle != BrushStyle.Solid) || (this.Brush.Color != Color.Empty)))
            {
                try
                {
                    g.FillPolygon(this.Brush.GetBrush(this.GetMaxRect()), base.AllPoints);
                }
                catch
                {
                }
            }
        }

        public void Fill()
        {
            this.Brush.BrushStyle = BrushStyle.Solid;
            this.Brush.Color = Color.FromArgb(160, 160, 0x40);
            this.Brush.Alpha = 40;
        }

        public ObjectBrush Brush
        {
            get
            {
                return this.brush;
            }
            set
            {
                this.brush = value;
            }
        }

        public override bool Closed
        {
            get
            {
                return true;
            }
        }
    }
}

