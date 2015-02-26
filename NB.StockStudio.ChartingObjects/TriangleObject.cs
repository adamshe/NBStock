namespace NB.StockStudio.ChartingObjects
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class TriangleObject : FillPolygonObject
    {
        private Shape shape;

        public override PointF[] CalcPoint()
        {
            if (this.shape == Shape.ParralleloGram)
            {
                PointF[] c = base.ToPoints(base.ControlPoints);
                ArrayList list = new ArrayList();
                list.AddRange(c);
                PointF tf = new PointF((c[0].X - c[1].X) + c[2].X, (c[0].Y - c[1].Y) + c[2].Y);
                list.Add(tf);
                return (PointF[]) list.ToArray(typeof(PointF));
            }
            return base.ToPoints(base.ControlPoints);
        }

        public void ParralleloGram()
        {
            this.shape = Shape.ParralleloGram;
        }

        public void ParralleloGramF()
        {
            this.ParralleloGram();
            base.Fill();
        }

        public override ObjectInit[] RegObject()
        {
            return new ObjectInit[] { new ObjectInit("Triangle", typeof(TriangleObject), "Triangle", "Shape", "Traingle"), new ObjectInit("ParralleloGram", typeof(TriangleObject), "ParralleloGram", "Shape", "ParralleloGram"), new ObjectInit("Triangle", typeof(TriangleObject), "TriangleF", "Shape", "TraingleF"), new ObjectInit("ParralleloGram", typeof(TriangleObject), "ParralleloGramF", "Shape", "ParralleloGramF") };
        }

        public void Triangle()
        {
            this.shape = Shape.Triangle;
        }

        public void TriangleF()
        {
            this.Triangle();
            base.Fill();
        }

        public override int ControlPointNum
        {
            get
            {
                return 3;
            }
        }

        public override int InitNum
        {
            get
            {
                return 3;
            }
        }

        public Shape Shape
        {
            get
            {
                return this.shape;
            }
            set
            {
                this.shape = value;
            }
        }
    }
}

