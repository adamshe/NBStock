namespace NB.StockStudio.Foundation
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;

    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectPoint
    {
        private double x;
        private double y;
        [XmlAttribute]
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        [XmlAttribute]
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
        public override string ToString()
        {
            return ("{" + DateTime.FromOADate(this.x).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + "," + this.y.ToString("f3") + "}");
        }

        public ObjectPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static ObjectPoint Empty
        {
            get
            {
                ObjectPoint point = new ObjectPoint();
                point.X = double.NaN;
                point.Y = double.NaN;
                return point;
            }
        }
    }
}

