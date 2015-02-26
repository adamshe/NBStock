namespace NB.StockStudio.Foundation.DataProvider
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Collections;
    using System.Reflection;

    public class RandomDataProvider : IDataProvider
    {
        private IDataProvider baseDataProvider;
        private DataCycle dataCycle = DataCycle.Day();
        private Hashtable htConstData = new Hashtable(10);
        private Hashtable htData = new Hashtable(500);
        private Hashtable htStringData = new Hashtable(10);
        private int maxCount = -1;
        private static Random Rnd = new Random();
        public RandomDataProvider(int N)
        {
            for (int i = 1; i < 0x26; i++)
            {
                this.htConstData.Add("FINANCE" + i, Rnd.NextDouble() * 1000.0);
            }
            for (int j = 1; j < 0x29; j++)
            {
                this.htConstData.Add("DYNAINFO" + j, Rnd.NextDouble() * 1000.0);
            }
            this.htConstData.Add("CAPITAL", Rnd.NextDouble() * 1000.0);
            this.htConstData.Add("VOLUNIT", 100);
            double[] numArray = new double[N];
            double[] numArray2 = new double[N];
            double[] numArray3 = new double[N];
            double[] numArray4 = new double[N];
            double[] numArray5 = new double[N];
            double[] numArray6 = new double[N];
            double[] numArray7 = new double[N];
            numArray[0] = 20.0;
            numArray2[0] = 19.0;
            numArray3[0] = 21.0;
            numArray4[0] = 18.0;
            numArray5[0] = 42123.0;
            numArray6[0] = 44212.0;
            for (int k = 1; k < N; k++)
            {
                numArray[k] = (numArray[k - 1] + Rnd.NextDouble()) - 0.48;
                numArray2[k] = (numArray2[k - 1] + Rnd.NextDouble()) - 0.48;
                numArray3[k] = Math.Max(numArray[k], numArray2[k]) + Rnd.NextDouble();
                numArray4[k] = Math.Min(numArray[k], numArray2[k]) - Rnd.NextDouble();
                numArray5[k] = Rnd.NextDouble() * 400000.0;
                numArray6[k] = numArray5[k] * (1.0 - (Rnd.NextDouble() * 0.1));
                numArray7[k] = DateTime.Today.AddHours(9.5 + (((double) k) / 60.0)).ToOADate();
            }
            numArray7[0] = DateTime.Today.AddHours(9.5).ToOADate();
            this.htData.Add("CLOSE", numArray);
            this.htData.Add("OPEN", numArray2);
            this.htData.Add("HIGH", numArray3);
            this.htData.Add("LOW", numArray4);
            this.htData.Add("VOLUME", numArray5);
            this.htData.Add("AMOUNT", numArray6);
            this.htData.Add("DATE", numArray7);
        }

        public double GetConstData(string DataType)
        {
            return (double) this.htConstData[DataType];
        }

        public double[] GetData(string DataType)
        {
            double[] sourceArray = (double[]) this.htData[DataType];
            if ((this.MaxCount == -1) || (sourceArray.Length <= this.MaxCount))
            {
                return sourceArray;
            }
            double[] destinationArray = new double[this.MaxCount];
            Array.Copy(sourceArray, sourceArray.Length - this.MaxCount, destinationArray, 0, this.MaxCount);
            return destinationArray;
        }

        public int GetDataCount()
        {
            return ((double[]) this.htData["OPEN"]).Length;
        }

        public string GetStringData(string DataType)
        {
            return (string) this.htConstData[DataType];
        }

        public void SetStringData(string dataType, string value)
        {
            if (!htConstData.ContainsKey(dataType))
                htConstData.Add(dataCycle, value);
            else
                htConstData[dataCycle] = value;
        }

        public string GetUnique()
        {
            return "0";
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
                return this["OPEN"].Length;
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
                return null;
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

