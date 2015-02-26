namespace NB.StockStudio.Foundation.DataProvider
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;

    public class YahooDataManager : CacheDataManagerBase
    {
        const  string DateFormat = "yyyy-MM-dd";
        const  string Address = "http://finance.yahoo.com/d/quotes.csv?s={0}&f={1}";
        public static string DownloadRealtimeFromYahoo(string Code, string Param)
        {
            string str2 = Code.ToLower();
            Encoding aSCII = Encoding.ASCII;
            string address = String.Format(Address, Code, Param);// string.Format("http://" + str + "finance.yahoo.com/d/quotes.csv?s=" + Code + "&f=" + Param, Code);
            byte[] bytes = new WebClient().DownloadData(address);
            return aSCII.GetString(bytes);
        }

        public override IDataProvider GetData(string Code, int Count)
        {
            DateTime date;
            IDataProvider provider2;
            if (Count > 0x6815)
            {
                Count = 0x6815;
                date = new DateTime(0x78a, 1, 1);
            }
            else
            {
                date = DateTime.Now.AddDays((double) -Count).Date;
            }
            DateTime now = DateTime.Now;
            string format = "http://table.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=d&ignore=.csv";
            string address = string.Format(format, new object[] { Code, date.Month - 1, date.Day, date.Year, now.Month - 1, now.Day, now.Year });
            byte[] data = new WebClient().DownloadData(address);
            try
            {
                provider2 = LoadYahooCSV(this, data);
            }
            catch (Exception exception)
            {
                throw new Exception("Invalid data format!" + Code + ";" + exception.Message);
            }
            return provider2;
        }

        public static string[] GetStockName(string Code)
        {
            string[] strArray = new string[0];
            string str = DownloadRealtimeFromYahoo(Code, "snx");
            if (str != null)
            {
                strArray = str.Split(new char[] { ',' });
                for (int i = 0; i < 3; i++)
                {
                    strArray[i] = strArray[i].Trim();
                    if (strArray[i].Length > 2)
                    {
                        strArray[i] = strArray[i].Substring(1, strArray[i].Length - 2);
                        strArray[i] = strArray[i].Trim();
                    }
                }
            }
            return strArray;
        }

        public static CommonDataProvider LoadYahooCSV(IDataManager idm, byte[] data)
        {
            return LoadYahooCSV(idm, new MemoryStream(data));
        }

        public static CommonDataProvider LoadYahooCSV(IDataManager idm, Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string[] strArray = reader.ReadToEnd().Trim().Split(new char[] { '\n' });
            ArrayList list = new ArrayList();
            for (int i = 1; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Trim();
                if (!strArray[i].StartsWith("<!--"))
                {
                    list.Add(strArray[i]);
                }
            }
            int count = list.Count;
            double[] numArray = new double[count];
            double[] numArray2 = new double[count];
            double[] numArray3 = new double[count];
            double[] numArray4 = new double[count];
            double[] numArray5 = new double[count];
            double[] numArray6 = new double[count];
            double[] numArray7 = new double[count];
            DateTimeFormatInfo invariantInfo = DateTimeFormatInfo.InvariantInfo;
            NumberFormatInfo info2 = NumberFormatInfo.InvariantInfo;
            for (int j = 0; j < count; j++)
            {
                string[] strArray2 = ((string) list[j]).Split(new char[] { ',' });
                if (strArray2.Length < 7)
                {
                    string[] strArray3 = new string[7];
                    for (int k = 0; k < strArray2.Length; k++)
                    {
                        strArray3[k] = strArray2[k];
                    }
                    if (strArray2.Length == 6)
                    {
                        strArray3[6] = strArray2[4];
                    }
                    if (strArray2.Length == 2)
                    {
                        for (int m = 2; m < strArray3.Length; m++)
                        {
                            strArray3[m] = strArray2[1];
                        }
                    }
                    strArray2 = strArray3;
                }
                int index = (count - j) - 1;
                numArray6[index] = DateTime.ParseExact(strArray2[0], DateFormat, invariantInfo).ToOADate();
                numArray2[index] = double.Parse(strArray2[1], info2);
                numArray3[index] = double.Parse(strArray2[2], info2);
                numArray4[index] = double.Parse(strArray2[3], info2);
                numArray[index] = double.Parse(strArray2[4], info2);
                numArray5[index] = double.Parse(strArray2[5], info2);
                numArray7[index] = double.Parse(strArray2[6], info2);
            }
            CommonDataProvider provider = new CommonDataProvider(idm);
            provider.LoadBinary(new double[][] { numArray2, numArray3, numArray4, numArray, numArray5, numArray6, numArray7 });
            return provider;
        }

        public static CommonDataProvider LoadYahooCSV(IDataManager idm, string FileName)
        {
            return LoadYahooCSV(idm, System.IO.File.OpenRead(FileName));
        }
    }
}

