namespace NB.StockStudio.Foundation
{
    using System;
    using System.Xml.Serialization;

    public class DataCycle
    {
        [XmlAttribute]
        public DataCycleBase CycleBase;
        [XmlAttribute]
        public int Repeat;

        public DataCycle()
        {
        }

        public DataCycle(DataCycleBase CycleBase, int Repeat)
        {
            this.CycleBase = CycleBase;
            this.Repeat = Repeat;
        }

        public static DataCycle Day()
        {
            return new DataCycle(DataCycleBase.DAY, 1);
        }

        public int GetSequence(double D)
        {
            int year = 0;
            if (this.CycleBase == DataCycleBase.DAY)
            {
                year = (int) D;
            }
            else if (this.CycleBase == DataCycleBase.WEEK)
            {
                year = ((int) D) / 7;
            }
            else if (this.CycleBase == DataCycleBase.YEAR)
            {
                year = DateTime.FromOADate(D).Year;
            }
            else if (this.CycleBase > DataCycleBase.WEEK)
            {
                DateTime time = DateTime.FromOADate(D);
                year = ((time.Year * 12) + time.Month) - 1;
                switch (this.CycleBase)
                {
                    case DataCycleBase.MONTH:
                        break;

                    case DataCycleBase.QUARTER:
                        year /= 3;
                        break;

                    case DataCycleBase.HALFYEAR:
                        year /= 6;
                        break;
                }
            }
            else if (this.CycleBase == DataCycleBase.HOUR)
            {
                year = (int) (D * 24.0);
            }
            else if (this.CycleBase == DataCycleBase.MINUTE)
            {
                year = (int) ((D * 24.0) * 60.0);
            }
            else
            {
                year = (int) (((((((int) D) % 100) + D) - ((int) D)) * 24.0) * 3600.0);
            }
        
            return (year / this.Repeat);
        }

        public static DataCycle Minute()
        {
            return new DataCycle(DataCycleBase.MINUTE, 1);
        }

        public static DataCycle Month()
        {
            return new DataCycle(DataCycleBase.MONTH, 1);
        }

        public static DataCycle Parse(string s)
        {
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (char.IsDigit(s, i))
                    {
                        return new DataCycle((DataCycleBase) Enum.Parse(typeof(DataCycleBase), s.Substring(0, i), true), int.Parse(s.Substring(i)));
                    }
                }
                return new DataCycle((DataCycleBase) Enum.Parse(typeof(DataCycleBase), s, true), 1);
            }
            catch
            {
                return Day();
            }
        }

        public static DataCycle Quarter()
        {
            return new DataCycle(DataCycleBase.QUARTER, 1);
        }

        public override string ToString()
        {
            return (this.CycleBase.ToString() + this.Repeat);
        }

        public static DataCycle Week()
        {
            return new DataCycle(DataCycleBase.WEEK, 1);
        }

        public static DataCycle Year()
        {
            return new DataCycle(DataCycleBase.YEAR, 1);
        }
    }
}

