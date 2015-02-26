namespace NB.StockStudio.Foundation.DataProvider
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public class DataPackage
    {
        static string quoteParam = "snd1t1oml1vp";
        public double AdjClose;
        public double Close;
        public DateTime Date;
        public double DoubleDate;
        public double High;
        public double Last;
        public double Low;
        public static int MaxValue;
        public double Open;
        public static int PackageSize;
        public static int PackageByteSize;
        public string Symbol;
        public double Volume;
        public string CompanyName;
        static DataPackage()
        {
            PackageSize = 9;
            PackageByteSize = PackageSize * 4;
            MaxValue = 0x591c8;
        }

        public DataPackage(DateTime Date, double Open, double High, double Low, double Close, double Volume, double AdjClose, string ticker = "", string companyName = "")
            : this(Date, Open, High, Low, Close, Volume, AdjClose, 0f, ticker, companyName)
        {
        }

        public DataPackage(double DoubleDate, double Open, double High, double Low, double Close, double Volume, double AdjClose, string ticker = "", string companyName = "")
            : this(DoubleDate, Open, High, Low, Close, Volume, AdjClose, 0f, ticker, companyName)
        {
        }

        public DataPackage(DateTime Date, double Open, double High, double Low, double Close, double Volume, double AdjClose, double Last, string ticker = "", string companyName = "")
        {
            this.Date = Date;
            this.DoubleDate = Date.ToOADate();
            this.Close = Close;
            this.AdjClose = AdjClose;
            this.Open = Open;
            this.High = High;
            this.Low = Low;
            this.Volume = Volume;
            this.Last = Last;
            Symbol = ticker;
            CompanyName = companyName;
        }

        public DataPackage(double DoubleDate, double Open, double High, double Low, double Close, double Volume, double AdjClose, double Last, string ticker = "", string companyName = "")
            : this(DateTime.FromOADate(DoubleDate), Open, High, Low, Close, Volume, AdjClose, Last, ticker, companyName)
        {
        }

        public static DataPackage DownloadFromYahoo(string Code)
        {
            return ParseYahoo(YahooDataManager.DownloadRealtimeFromYahoo(Code, quoteParam));
        }

        public byte[] GetByte()
        {
            byte[] dst = new byte[PackageByteSize];
            Buffer.BlockCopy(this.GetFloat(), 0, dst, 0, dst.Length);
            return dst;
        }

        public static DateTime GetDateTime(float[] fs)
        {
            return GetDateTime(fs, 0);
        }

        public static DateTime GetDateTime(float[] fs, int i)
        {
            DateTime time;
            double[] dst = new double[1];
            try
            {
                Buffer.BlockCopy(fs, i * PackageByteSize, dst, 0, 8);
                time = DateTime.FromOADate(dst[0]);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Concat(new object[] { exception.Message, ";", dst[0], ";i=", i, ";Length=", fs.Length / PackageByteSize }));
            }
            return time;
        }

        public float[] GetFloat()
        {
            float[] dst = new float[PackageSize];
            double[] src = new double[] { this.DoubleDate };
            Buffer.BlockCopy(src, 0, dst, 0, 8);
            dst[2] = (float)this.Open;
            dst[3] = (float)this.High;
            dst[4] = (float)this.Low;
            dst[5] = (float)this.Close;
            src[0] = (float)this.Volume;
            Buffer.BlockCopy(src, 0, dst, 0x18, 8);
            dst[8] = (float)this.AdjClose;
            return dst;
        }

        public static double GetVolume(float[] fs)
        {
            double[] dst = new double[1];
            Buffer.BlockCopy(fs, 0x18, dst, 0, 8);
            return dst[0];
        }

        public static DataPackage ParseEODData(string s)
        {
            DataPackage package2;
            string[] strArray = s.Split(new char[] { ',' });
            IFormatProvider provider = new CultureInfo("en-US", true);
            try
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Trim();
                }
                DataPackage package = new DataPackage(DateTime.Parse(strArray[1], provider), float.Parse(strArray[2]), float.Parse(strArray[3]), float.Parse(strArray[4]), float.Parse(strArray[5]), (double) float.Parse(strArray[6]), float.Parse(strArray[5]));
                package.Symbol = strArray[0];
                package2 = package;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message + ";" + s);
            }
            return package2;
        }

        public static DataPackage ParseYahoo(string s)
        {
            DataPackage package = null;
            try
            {
                string[] strArray = s.Split(new char[] { ',' });
                string[] strArray2 = RemoveQuotation(strArray[5]).Split(new char[] { '-' });
                float def = 0f;
                if (strArray.Length > 8)
                {
                    def = ToFloat(strArray[8], def);
                }
                IFormatProvider invariantInfo = DateTimeFormatInfo.InvariantInfo;
                package = new DataPackage(DateTime.ParseExact(RemoveQuotation(strArray[2]) + " " + RemoveQuotation(strArray[3]), "M/%d/yyyy h:mmtt", invariantInfo, DateTimeStyles.AllowWhiteSpaces), ToFloat(strArray[4], 0f), ToFloat(strArray2[1], 0f), ToFloat(strArray2[0], 0f), ToFloat(strArray[6], 0f), ToDouble(strArray[7], 0.0), ToFloat(strArray[6], 0f), def);
                package.Symbol = RemoveQuotation(strArray[0]);
                package.CompanyName = RemoveQuotation(strArray[1]);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message + ";" + s);
            }
            return package;
        }

        public static string RemoveQuotation(string s)
        {
            s = s.Trim();
            if (s.StartsWith("\"") && s.EndsWith("\""))
            {
                s = s.Substring(1, s.Length - 2);
            }
            return s;
        }

        public static double ToDouble(string s, double Def)
        {
            try
            {
                return double.Parse(s);
            }
            catch
            {
                return Def;
            }
        }

        public static float ToFloat(string s, float Def)
        {
            try
            {
                return float.Parse(s);
            }
            catch
            {
                return Def;
            }
        }

        public bool IsZeroValue
        {
            get
            {
                return ((((this.Open == 0f) || (this.High == 0f)) || (this.Low == 0f)) || (this.Close == 0f));
            }
        }

        public double this[string Type]
        {
            get
            {
                switch (Type.ToUpper())
                {
                    case "OPEN":
                        return (double) this.Open;

                    case "DATE":
                        return this.DoubleDate;

                    case "HIGH":
                        return (double) this.High;

                    case "LOW":
                        return (double) this.Low;

                    case "CLOSE":
                        return (double) this.Close;

                    case "VOLUME":
                        return this.Volume;

                    case "ADJCLOSE":
                        return (double) this.AdjClose;
                }
                return double.NaN;
            }
        }
    }
}

