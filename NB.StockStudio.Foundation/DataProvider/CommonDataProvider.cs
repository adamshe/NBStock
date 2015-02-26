namespace NB.StockStudio.Foundation.DataProvider
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public class CommonDataProvider : IDataProvider
    {
        private bool adjusted = true;
        private IDataProvider baseDataProvider;
        private DataCycle dataCycle = DataCycle.Day();
        private IDataManager dm;
        private Hashtable htAllCycle = new Hashtable();
        private Hashtable htConstData = new Hashtable();
        private Hashtable htData = new Hashtable();
        private Hashtable htRealtime = new Hashtable();
        private Hashtable htStringData = new Hashtable();
        private ExchangeIntraday intradayInfo;
        private int maxCount = -1;

        public CommonDataProvider(IDataManager dm)
        {
            this.dm = dm;
        }

        private double[] AdjustByBase(double[] Date, double[] dd)
        {
            double[] numArray = this.BaseDataProvider["DATE"];
            double[] numArray2 = new double[numArray.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray2[i] = double.NaN;
            }
            int index = dd.Length - 1;
            int num3 = numArray.Length - 1;
            while ((num3 >= 0) && (index >= 0))
            {
                if (numArray[num3] == Date[index])
                {
                    numArray2[num3--] = dd[index--];
                }
                else
                {
                    if (numArray[num3] > Date[index])
                    {
                        num3--;
                        continue;
                    }
                    index--;
                }
            }
            return numArray2;
        }

        public void DeleteData(DateTime FromTime, DateTime ToTime)
        {
            string[] strArray = new string[] { "DATE", "OPEN", "HIGH", "LOW", "CLOSE", "VOLUME", "ADJCLOSE" };
            ArrayList[] listArray = new ArrayList[strArray.Length];
            for (int i = 0; i < listArray.Length; i++)
            {
                listArray[i] = new ArrayList();
                listArray[i].AddRange(this[strArray[i]]);
            }
            double num2 = FromTime.ToOADate();
            double num3 = ToTime.ToOADate();
            int index = 0;
            while (index < listArray[0].Count)
            {
                double num5 = (double) listArray[0][index];
                if ((num5 > num2) && (num5 < num3))
                {
                    for (int k = 0; k < strArray.Length; k++)
                    {
                        listArray[k].RemoveAt(index);
                    }
                }
                else
                {
                    index++;
                }
            }
            this.htData.Clear();
            for (int j = 0; j < strArray.Length; j++)
            {
                this.htData.Add(strArray[j], (double[]) listArray[j].ToArray(typeof(double)));
            }
            this.htAllCycle.Clear();
        }

        private Hashtable DoExpandMinute(Hashtable ht)
        {
            double[] numArray = (double[]) ht["DATE"];
            if ((numArray == null) || (numArray.Length <= 0))
            {
                return ht;
            }
            double num = 0.00069444444444444436;
            double num2 = (int) numArray[0];
            double num3 = (int) (numArray[numArray.Length - 1] + 1.0);
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            double d = (int) num2;
            int index = 0;
            while (d <= num3)
            {
                if (this.intradayInfo.InTimePeriod(d))
                {
                    if (index >= numArray.Length)
                    {
                        list.Add(d);
                        list2.Add(-1);
                    }
                    else
                    {
                        if (numArray[index] < (d - (num * 0.0001)))
                        {
                            index++;
                            continue;
                        }
                        if (numArray[index] < (d + (num * 0.9999)))
                        {
                            list.Add(numArray[index]);
                            list2.Add(index);
                            index++;
                            continue;
                        }
                        if ((list.Count == 0) || (((double) list[list.Count - 1]) < (d + (num / 100.0))))
                        {
                            list.Add(d + (num / 100.0));
                            if (this.intradayInfo.InTimePeriod(numArray[index]))
                            {
                                if (list2.Count > 0)
                                {
                                    list2.Add(list2[list2.Count - 1]);
                                }
                                else
                                {
                                    list2.Add(0);
                                }
                            }
                            else
                            {
                                list2.Add(index);
                            }
                        }
                    }
                }
                d += num;
            }
            Hashtable hashtable = new Hashtable();
            foreach (string str in ht.Keys)
            {
                double[] numArray2 = (double[]) ht[str];
                double[] numArray3 = new double[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    if (str == "DATE")
                    {
                        numArray3[i] = (double) list[i];
                    }
                    else
                    {
                        int num7 = (int) list2[i];
                        if (num7 < 0)
                        {
                            numArray3[i] = double.NaN;
                        }
                        else if (str == "VOLUME")
                        {
                            if ((i > 0) && (((int) list2[i]) == ((int) list2[i - 1])))
                            {
                                numArray3[i] = 0.0;
                            }
                            else
                            {
                                numArray3[i] = numArray2[num7];
                            }
                        }
                        else
                        {
                            numArray3[i] = numArray2[num7];
                        }
                    }
                }
                hashtable[str] = numArray3;
            }
            return hashtable;
        }

        private double First(double d1, double d2)
        {
            if (double.IsNaN(d1))
            {
                return d2;
            }
            return d1;
        }

        public double GetConstData(string DataType)
        {
            return (double) this.htConstData[DataType];
        }

        public Hashtable GetCycleData(DataCycle dc)
        {
            if (((dc.CycleBase == DataCycleBase.DAY) && (dc.Repeat == 1)) && !this.Adjusted)
            {
                return this.htData;
            }
            Hashtable hashtable = (Hashtable) this.htAllCycle[dc.ToString()];
            if (hashtable == null)
            {
                if (this.htData == null)
                {
                    return this.htData;
                }
                Hashtable htData = this.htData;
                if (this.intradayInfo != null)
                {
                    htData = this.DoExpandMinute(htData);
                }
                if (htData["CLOSE"] != null)
                {
                    if (htData["OPEN"] == null)
                    {
                        htData["OPEN"] = htData["CLOSE"];
                    }
                    if (htData["HIGH"] == null)
                    {
                        htData["HIGH"] = htData["CLOSE"];
                    }
                    if (htData["LOW"] == null)
                    {
                        htData["LOW"] = htData["CLOSE"];
                    }
                }
                double[] oDATE = (double[]) htData["DATE"];
                if (oDATE == null)
                {
                    return null;
                }
                int[] nEWDATE = new int[oDATE.Length];
                int num = -2147483648;
                int num2 = -1;
                for (int i = 0; i < oDATE.Length; i++)
                {
                    int sequence = this.DataCycle.GetSequence(oDATE[i]);
                    if (sequence > num)
                    {
                        num2++;
                    }
                    nEWDATE[i] = num2;
                    num = sequence;
                }
                hashtable = new Hashtable();
                foreach (string str in htData.Keys)
                {
                    hashtable[str] = new double[num2 + 1];
                }
                bool flag = (this.Adjusted && (htData["ADJCLOSE"] != null)) && (htData["CLOSE"] != null);
                double[] cLOSE = (double[]) htData["CLOSE"];
                double[] aDJCLOSE = (double[]) htData["ADJCLOSE"];
                foreach (string str2 in htData.Keys)
                {
                    MergeCycleType oPEN;
                    bool doAdjust = flag;
                    doAdjust = false;
                    switch (str2)
                    {
                        case "DATE":
                            oPEN = MergeCycleType.OPEN;
                            break;

                        case "VOLUME":
                        case "AMOUNT":
                            oPEN = MergeCycleType.SUM;
                            break;

                        default:
                            try
                            {
                                oPEN = (MergeCycleType) Enum.Parse(typeof(MergeCycleType), str2);
                                doAdjust = true;
                            }
                            catch
                            {
                                oPEN = MergeCycleType.CLOSE;
                            }
                            break;
                    }
                    this.MergeCycle(oDATE, nEWDATE, cLOSE, aDJCLOSE, (double[]) htData[str2], (double[]) hashtable[str2], oPEN, doAdjust);
                }
                this.htAllCycle[dc.ToString()] = hashtable;
            }
            return hashtable;
        }

        private double[] GetData(string DataType)
        {
            Hashtable cycleData = this.GetCycleData(this.DataCycle);
            if (cycleData == null)
            {
                throw new Exception(string.Concat(new object[] { "Quote data ", DataType, " ", this.DataCycle, " not found" }));
            }
            double[] dd = (double[]) cycleData[DataType.ToUpper()];
            if (dd == null)
            {
                throw new Exception("The name " + DataType + " does not exist.");
            }
            if ((this.BaseDataProvider != null) && (this.BaseDataProvider != this))
            {
                dd = this.AdjustByBase((double[]) cycleData["DATE"], dd);
            }
            if ((this.MaxCount == -1) || (dd.Length <= this.MaxCount))
            {
                return dd;
            }
            double[] destinationArray = new double[this.MaxCount];
            Array.Copy(dd, dd.Length - this.MaxCount, destinationArray, 0, this.MaxCount);
            return destinationArray;
        }

        public DataPackage GetDataPackage(int Index)
        {
            return new DataPackage(this["DATE"][Index], (float) this["OPEN"][Index], (float) this["HIGH"][Index], (float) this["LOW"][Index], (float) this["CLOSE"][Index], this["VOLUME"][Index], (float) this["ADJCLOSE"][Index]);
        }

        public DataPackage[] GetLastDataPackages(int Count)
        {
            return this.GetLastDataPackages(this.Count - Count, Count);
        }

        public DataPackage[] GetLastDataPackages(int Start, int Count)
        {
            DataPackage[] packageArray = new DataPackage[Count];
            double[] numArray = this["DATE"];
            double[] numArray2 = this["OPEN"];
            double[] numArray3 = this["HIGH"];
            double[] numArray4 = this["LOW"];
            double[] numArray5 = this["CLOSE"];
            double[] numArray6 = this["VOLUME"];
            double[] numArray7 = this["ADJCLOSE"];
            for (int i = Start; i < (Start + Count); i++)
            {
                if ((i >= 0) && (i < this.Count))
                {
                    packageArray[i - Start] = new DataPackage(numArray[i], (float) numArray2[i], (float) numArray3[i], (float) numArray4[i], (float) numArray5[i], numArray6[i], (float) numArray7[i]);
                }
            }
            return packageArray;
        }

        public DataPackage GetLastPackage()
        {
            return this.GetDataPackage(this.Count - 1);
        }

        public string GetStringData(string DataType)
        {
            return (string) this.htConstData[DataType];
        }

        public string GetUnique()
        {
            return this.DataCycle.ToString();
        }

        public void LoadBinary(byte[] bs)
        {
            this.LoadBinary(bs, bs.Length / DataPackage.PackageByteSize);
        }

        public void LoadBinary(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);
            this.LoadBinary(buffer);
        }

        public void LoadBinary(string FileName)
        {
            FileStream stream = File.OpenRead(FileName);
            try
            {
                this.LoadBinary(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public void LoadBinary(double[][] ds)
        {
            if (ds.Length > 4)
            {
                this.htData.Clear();
                this.htData.Add("OPEN", ds[0]);
                this.htData.Add("HIGH", ds[1]);
                this.htData.Add("LOW", ds[2]);
                this.htData.Add("CLOSE", ds[3]);
                this.htData.Add("VOLUME", ds[4]);
                this.htData.Add("DATE", ds[5]);
                if (ds.Length > 6)
                {
                    this.htData.Add("ADJCLOSE", ds[6]);
                }
                else
                {
                    double[] dst = new double[ds[0].Length];
                    Buffer.BlockCopy(ds[3], 0, dst, 0, ds[0].Length * 8);
                    this.htData.Add("ADJCLOSE", dst);
                }
            }
            this.htAllCycle.Clear();
        }

        public void LoadBinary(byte[] bs, int N)
        {
            this.htData.Clear();
            double[] numArray = new double[N];
            double[] numArray2 = new double[N];
            double[] numArray3 = new double[N];
            double[] numArray4 = new double[N];
            double[] dst = new double[N];
            double[] numArray6 = new double[N];
            double[] numArray7 = new double[N];
            float[] numArray8 = new float[N * DataPackage.PackageSize];
            Buffer.BlockCopy(bs, 0, numArray8, 0, bs.Length);
            for (int i = 0; i < N; i++)
            {
                Buffer.BlockCopy(numArray8, i * DataPackage.PackageByteSize, numArray6, i * 8, 8);
                numArray2[i] = numArray8[(i * DataPackage.PackageSize) + 2];
                numArray3[i] = numArray8[(i * DataPackage.PackageSize) + 3];
                numArray4[i] = numArray8[(i * DataPackage.PackageSize) + 4];
                numArray[i] = numArray8[(i * DataPackage.PackageSize) + 5];
                Buffer.BlockCopy(numArray8, ((i * DataPackage.PackageSize) + 6) * 4, dst, i * 8, 8);
                numArray7[i] = numArray8[(i * DataPackage.PackageSize) + 8];
            }
            this.htData.Add("CLOSE", numArray);
            this.htData.Add("OPEN", numArray2);
            this.htData.Add("HIGH", numArray3);
            this.htData.Add("LOW", numArray4);
            this.htData.Add("VOLUME", dst);
            this.htData.Add("DATE", numArray6);
            this.htData.Add("ADJCLOSE", numArray7);
            this.htAllCycle.Clear();
        }

        public void LoadBinary(string DataType, double[] ds)
        {
            this.htData[DataType.ToUpper()] = ds;
            this.htAllCycle.Clear();
        }

        private double Max(double d1, double d2)
        {
            if (double.IsNaN(d1))
            {
                return d2;
            }
            return Math.Max(d1, d2);
        }

        public void Merge(DataPackage dp)
        {
            if (dp != null)
            {
                string[] strArray = new string[] { "DATE", "OPEN", "HIGH", "LOW", "CLOSE", "VOLUME", "ADJCLOSE" };
                ArrayList[] listArray = new ArrayList[strArray.Length];
                for (int i = 0; i < listArray.Length; i++)
                {
                    listArray[i] = new ArrayList();
                    listArray[i].AddRange(this[strArray[i]]);
                }
                for (int j = 0; j <= listArray[0].Count; j++)
                {
                    if (j < listArray[0].Count)
                    {
                        if (((int) ((double) listArray[0][j])) < ((int) dp.DoubleDate))
                        {
                            continue;
                        }
                        if (((int) ((double) listArray[0][j])) > ((int) dp.DoubleDate))
                        {
                            for (int m = 0; m < strArray.Length; m++)
                            {
                                listArray[m].Insert(j, dp[strArray[m]]);
                            }
                        }
                        else
                        {
                            for (int n = 1; n < strArray.Length; n++)
                            {
                                listArray[n][j] = dp[strArray[n]];
                            }
                        }
                    }
                    else
                    {
                        for (int num5 = 0; num5 < strArray.Length; num5++)
                        {
                            listArray[num5].Add(dp[strArray[num5]]);
                        }
                    }
                    break;                
                }
                this.htData.Clear();
                for (int k = 0; k < strArray.Length; k++)
                {
                    this.htData.Add(strArray[k], (double[]) listArray[k].ToArray(typeof(double)));
                }
                this.htAllCycle.Clear();
            }
        }

        public void Merge(IDataProvider idp)
        {
            string[] strArray = new string[] { "DATE", "OPEN", "HIGH", "LOW", "CLOSE", "VOLUME", "ADJCLOSE" };
            ArrayList[] listArray = new ArrayList[strArray.Length];
            ArrayList[] listArray2 = new ArrayList[strArray.Length];
            for (int i = 0; i < listArray.Length; i++)
            {
                listArray[i] = new ArrayList();
                listArray[i].AddRange(this[strArray[i]]);
                listArray2[i] = new ArrayList();
                listArray2[i].AddRange(idp[strArray[i]]);
            }
            int index = 0;
            int num3 = 0;
            while (num3 < listArray2[0].Count)
            {
                if (index < listArray[0].Count)
                {
                    if (((double) listArray[0][index]) < ((double) listArray2[0][num3]))
                    {
                        index++;
                    }
                    else if (((double) listArray[0][index]) >= ((double) listArray2[0][num3]))
                    {
                        if (((double) listArray[0][index]) > ((double) listArray2[0][num3]))
                        {
                            for (int k = 0; k < strArray.Length; k++)
                            {
                                listArray[k].Insert(index, listArray2[k][num3]);
                            }
                        }
                        else
                        {
                            for (int m = 1; m < strArray.Length; m++)
                            {
                                listArray[m][index] = listArray2[m][num3];
                            }
                        }
                        index++;
                        num3++;
                    }
                }
                else
                {
                    for (int n = num3; n < listArray2[0].Count; n++)
                    {
                        for (int num7 = 0; num7 < strArray.Length; num7++)
                        {
                            listArray[num7].Add(listArray2[num7][n]);
                        }
                    }
                    break;
                }
            }
            this.htData.Clear();
            for (int j = 0; j < strArray.Length; j++)
            {
                this.htData.Add(strArray[j], (double[]) listArray[j].ToArray(typeof(double)));
            }
            this.htAllCycle.Clear();
        }

        private void MergeCycle(double[] ODATE, int[] NEWDATE, double[] CLOSE, double[] ADJCLOSE, double[] ht, double[] htCycle, MergeCycleType mct, bool DoAdjust)
        {
            int num = -1;
            int index = -1;
            for (int i = 0; i < ODATE.Length; i++)
            {
                double num4 = 1.0;
                if (DoAdjust && (ADJCLOSE != null))
                {
                    num4 = ADJCLOSE[i] / CLOSE[i];
                }
                double d = ht[i] * num4;
                if (num != NEWDATE[i])
                {
                    index++;
                    htCycle[index] = d;
                }
                else if (!double.IsNaN(d))
                {
                    if (mct == MergeCycleType.HIGH)
                    {
                        htCycle[index] = this.Max(htCycle[index], d);
                    }
                    else if (mct == MergeCycleType.LOW)
                    {
                        htCycle[index] = this.Min(htCycle[index], d);
                    }
                    else if (mct == MergeCycleType.CLOSE)
                    {
                        htCycle[index] = d;
                    }
                    else if (mct == MergeCycleType.ADJCLOSE)
                    {
                        htCycle[index] = ht[i] / num4;
                    }
                    else if (mct == MergeCycleType.OPEN)
                    {
                        htCycle[index] = this.First(htCycle[index], d);
                    }
                    else
                    {
                        htCycle[index] = this.Sum(htCycle[index], d);
                    }
                }
                num = NEWDATE[i];
            }
        }

        public static byte[] MergeOneQuote(byte[] bs, DataPackage dp)
        {
            float[] dst = new float[(bs.Length / 4) + DataPackage.PackageSize];
            Buffer.BlockCopy(bs, 0, dst, 0, bs.Length);
            DateTime date = dp.Date.Date;
            int num = (dst.Length / DataPackage.PackageSize) - 1;
            for (int i = num - 1; i >= -1; i--)
            {
                DateTime minValue = DateTime.MinValue;
                if (i > -1)
                {
                    minValue = DataPackage.GetDateTime(dst, i).Date;
                }
                int num3 = 0;
                if (minValue <= date)
                {
                    if (minValue < date)
                    {
                        if ((i < (num - 1)) && (num > 0))
                        {
                            Buffer.BlockCopy(dst, (i + 1) * DataPackage.PackageByteSize, dst, (i + 2) * DataPackage.PackageByteSize, ((num - i) - 1) * DataPackage.PackageByteSize);
                        }
                        num3 = 1;
                    }
                    Buffer.BlockCopy(dp.GetFloat(), 0, dst, (i + num3) * DataPackage.PackageByteSize, DataPackage.PackageByteSize);
                    bs = new byte[(dst.Length * 4) - ((1 - num3) * DataPackage.PackageByteSize)];
                    Buffer.BlockCopy(dst, 0, bs, 0, bs.Length);
                    return bs;
                }
            }
            return bs;
        }

        private double Min(double d1, double d2)
        {
            if (double.IsNaN(d1))
            {
                return d2;
            }
            return Math.Min(d1, d2);
        }

        public byte[] SaveBinary()
        {
            MemoryStream stream = new MemoryStream();
            this.SaveBinary(stream);
            return stream.ToArray();
        }

        public void SaveBinary(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            try
            {
                double[] numArray = (double[]) this.htData["CLOSE"];
                double[] numArray2 = (double[]) this.htData["OPEN"];
                double[] numArray3 = (double[]) this.htData["HIGH"];
                double[] numArray4 = (double[]) this.htData["LOW"];
                double[] numArray5 = (double[]) this.htData["VOLUME"];
                double[] numArray6 = (double[]) this.htData["DATE"];
                double[] numArray7 = (double[]) this.htData["ADJCLOSE"];
                int length = numArray.Length;
                for (int i = 0; i < length; i++)
                {
                    writer.Write(numArray6[i]);
                    writer.Write((float) numArray2[i]);
                    writer.Write((float) numArray3[i]);
                    writer.Write((float) numArray4[i]);
                    writer.Write((float) numArray[i]);
                    writer.Write(numArray5[i]);
                    writer.Write((float) numArray7[i]);
                }
            }
            finally
            {
                writer.Close();
            }
        }

        public void SaveBinary(string FileName)
        {
            this.SaveBinary(File.OpenWrite(FileName));
        }

        public void SetStringData(string DataType, string Value)
        {
            if (!htConstData.Contains(DataType))
                htConstData.Add(DataType, Value);
            else
                this.htConstData[DataType] = Value;
        }

        private double Sum(double d1, double d2)
        {
            if (double.IsNaN(d1))
            {
                return d2;
            }
            return (d1 + d2);
        }

        public bool Adjusted
        {
            get
            {
                return this.adjusted;
            }
            set
            {
                this.adjusted = value;
            }
        }

        public IDataProvider BaseDataProvider
        {
            get
            {
                return this.baseDataProvider;
            }
            set
            {
                this.baseDataProvider = value;
            }
        }

        public int Count
        {
            get
            {
                return this["DATE"].Length;
            }
        }

        public DataCycle DataCycle
        {
            get
            {
                return this.dataCycle;
            }
            set
            {
                this.dataCycle = value;
            }
        }

        public IDataManager DataManager
        {
            get
            {
                if (this.dm == null)
                {
                    return this.dm;
                }
                return this.dm;
            }
        }

        public ExchangeIntraday IntradayInfo
        {
            get
            {
                return this.intradayInfo;
            }
            set
            {
                this.intradayInfo = value;
                this.htAllCycle.Clear();
            }
        }

        public double[] this[string Name]
        {
            get
            {
                return this.GetData(Name);
            }
        }

        public int MaxCount
        {
            get
            {
                return this.maxCount;
            }
            set
            {
                this.maxCount = value;
            }
        }
    }
}

