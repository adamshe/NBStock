namespace NB.StockStudio.Foundation
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential), TypeConverter(typeof(ExpandableObjectConverter))]
    public struct TimePeriod
    {
        public double Time1;
        public double Time2;
        public TimePeriod(double t1, double t2)
        {
            this.Time1 = t1;
            this.Time2 = t2;
        }

        public TimePeriod(DateTime t1, DateTime t2) : this(t1.ToOADate(), t2.ToOADate())
        {
        }
    }
}

