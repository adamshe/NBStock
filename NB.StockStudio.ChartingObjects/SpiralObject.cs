namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class SpiralObject : PolygonObject
    {
        private ArrayList alPoint = new ArrayList();
        private SpiralType spiralType;
        private int sweepAngle = 0x708;

        public void Archimedes()
        {
            this.spiralType = SpiralType.Archimedes;
        }

        public override PointF[] CalcPoint()
        {
            PointF[] tfArray = base.ToPoints(base.ControlPoints);
            this.alPoint.Clear();
            this.alPoint.Add(tfArray[0]);
            float num = tfArray[1].X - tfArray[0].X;
            float num2 = tfArray[1].Y - tfArray[0].Y;
            double num3 = base.Dist(tfArray[0], tfArray[1]);
            double d = Math.Atan2((double) num2, (double) num) + 6.2831853071795862;
            Rectangle rect = base.Area.Canvas.Rect;
            double num5 = Math.Sqrt((double) ((rect.Width * rect.Width) + (rect.Height * rect.Height)));
            double num6 = 1.0;
            switch (this.spiralType)
            {
                case SpiralType.Archimedes:
                    num6 = num3 / d;
                    break;

                case SpiralType.Logarithmic:
                    num6 = num3 / Math.Exp(d);
                    break;

                case SpiralType.Parabolic:
                    num6 = Math.Sqrt((num3 * num3) / d);
                    break;

                case SpiralType.Hyperbolic:
                    num6 = num3 * d;
                    break;

                case SpiralType.Lituus:
                    num6 = Math.Sqrt((num3 * num3) * d);
                    break;
            }
            for (int i = 0; i < this.sweepAngle; i++)
            {
                double num8 = ((((double) i) / 180.0) * 3.1415926535897931) + d;
                double num9 = 0.0;
                switch (this.spiralType)
                {
                    case SpiralType.Archimedes:
                        num9 = num6 * num8;
                        break;

                    case SpiralType.Logarithmic:
                        num9 = num6 * Math.Exp(num8);
                        break;

                    case SpiralType.Parabolic:
                        num9 = Math.Sqrt((num6 * num6) * num8);
                        break;

                    case SpiralType.Hyperbolic:
                        num9 = num6 / num8;
                        break;

                    case SpiralType.Lituus:
                        num9 = Math.Sqrt((num6 * num6) / num8);
                        break;
                }
                if (num9 > num5)
                {
                    break;
                }
                float f = tfArray[0].X + ((float) (num9 * Math.Cos(num8)));
                float num11 = tfArray[0].Y + ((float) (num9 * Math.Sin(num8)));
                if (!float.IsInfinity(f) && !float.IsInfinity(num11))
                {
                    this.alPoint.Add(new PointF(f, num11));
                }
            }
            return (PointF[]) this.alPoint.ToArray(typeof(PointF));
        }

        public void Hyperbolic()
        {
            this.spiralType = SpiralType.Hyperbolic;
        }

        public void Lituus()
        {
            this.spiralType = SpiralType.Lituus;
        }

        public void Logarithmic()
        {
            this.spiralType = SpiralType.Logarithmic;
        }

        public void Parabolic()
        {
            this.spiralType = SpiralType.Parabolic;
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Logarithmic Spiral", typeof(SpiralObject), "Logarithmic", "Circle", "SpiralL"), new ObjectInit("Archimedes Spiral", typeof(SpiralObject), "Archimedes", "Circle", "SpiralA"), new ObjectInit("Parabolic Spiral", typeof(SpiralObject), "Parabolic", "Circle", "SpiralP"), new ObjectInit("Hyperbolic Spiral", typeof(SpiralObject), "Hyperbolic", "Circle", "SpiralH"), new ObjectInit("Lituus Spiral", typeof(SpiralObject), "Lituus", "Circle", "SpiralLi") };
        }

        public SpiralType SpiralType
        {
            get
            {
                return this.spiralType;
            }
            set
            {
                this.spiralType = value;
            }
        }

        public int SweepAngle
        {
            get
            {
                return this.sweepAngle;
            }
            set
            {
                this.sweepAngle = value;
            }
        }
    }
}

