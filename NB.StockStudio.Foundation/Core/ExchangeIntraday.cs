namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ExchangeIntraday : CollectionBase
    {
        public bool NativeCycle;
        public bool ShowFirstXLabel;
        public int TimeZone;
        public int YahooDelay = 30;

        public virtual void Add(TimePeriod tp)
        {
            base.List.Add(tp);
        }

        public static ExchangeIntraday GetExchangeIntraday(string Exchange)
        {
            if (Exchange == "")
            {
                return US;
            }
            try
            {
                Type type = typeof(ExchangeIntraday);
                return (ExchangeIntraday) type.InvokeMember(Exchange, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static, null, null, null);
            }
            catch
            {
                return US;
            }
        }

        public double GetOpenTimePerDay()
        {
            double num = 0.0;
            foreach (TimePeriod period in this)
            {
                num += period.Time2 - period.Time1;
            }
            return num;
        }

        public bool InTimePeriod(double D)
        {
            double num = D - ((int) D);
            foreach (TimePeriod period in this)
            {
                if ((num >= period.Time1) && (num <= (period.Time2 + 0.00069444444444444436)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEstimateOpen(DateTime D)
        {
            TimePeriod period = this[0];
            return ((D.Hour >= DateTime.FromOADate(period.Time1).Hour) && (D.Hour <= DateTime.FromOADate(this[base.List.Count - 1].Time2).AddMinutes((double) this.YahooDelay).Hour));
        }

        public double OneDayTime(double D)
        {
            double num = D - ((int) D);
            double num2 = 0.0;
            foreach (TimePeriod period in this)
            {
                if (num > period.Time2)
                {
                    num2 += period.Time2 - period.Time1;
                }
                else
                {
                    if (num > period.Time1)
                    {
                        num2 += num - period.Time1;
                    }
                    return num2;
                }
            }
            return num2;
        }

        public static string[] BuildInExchange
        {
            get
            {
                PropertyInfo[] properties = typeof(ExchangeIntraday).GetProperties(BindingFlags.Public | BindingFlags.Static);
                string[] strArray = new string[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    strArray[i] = properties[i].Name;
                }
                return strArray;
            }
        }

        public static ExchangeIntraday China
        {
            get
            {
                ExchangeIntraday intraday = new ExchangeIntraday();
                intraday.NativeCycle = true;
                intraday.Add(new TimePeriod(new DateTime(1, 1, 1, 9, 30, 0), new DateTime(1, 1, 1, 11, 30, 0)));
                intraday.Add(new TimePeriod(new DateTime(1, 1, 1, 13, 0, 0), new DateTime(1, 1, 1, 15, 0, 0)));
                intraday.TimeZone = 8;
                intraday.ShowFirstXLabel = true;
                return intraday;
            }
        }

        public static ExchangeIntraday France
        {
            get
            {
                ExchangeIntraday intraday = new ExchangeIntraday();
                DateTime time = new DateTime(1, 1, 1, 9, 0, 0);
                DateTime time2 = new DateTime(1, 1, 1, 0x11, 30, 0);
                intraday.Add(new TimePeriod(time.ToOADate(), time2.ToOADate()));
                intraday.TimeZone = 2;
                return intraday;
            }
        }

        public static ExchangeIntraday Germany
        {
            get
            {
                ExchangeIntraday intraday = new ExchangeIntraday();
                DateTime time = new DateTime(1, 1, 1, 9, 0, 0);
                DateTime time2 = new DateTime(1, 1, 1, 0x11, 0, 0);
                intraday.Add(new TimePeriod(time.ToOADate(), time2.ToOADate()));
                intraday.TimeZone = 2;
                return intraday;
            }
        }

        public virtual TimePeriod this[int Index]
        {
            get
            {
                return (TimePeriod) base.List[Index];
            }
        }

        public static ExchangeIntraday US
        {
            get
            {
                ExchangeIntraday intraday = new ExchangeIntraday();
                DateTime time = new DateTime(1, 1, 1, 9, 30, 0);
                DateTime time2 = new DateTime(1, 1, 1, 0x10, 0, 0);
                intraday.Add(new TimePeriod(time.ToOADate(), time2.ToOADate()));
                intraday.TimeZone = -4;
                return intraday;
            }
        }
    }
}

