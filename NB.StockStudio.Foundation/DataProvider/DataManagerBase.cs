namespace NB.StockStudio.Foundation.DataProvider
{
    using System;
    using System.Reflection;

    public class DataManagerBase : IDataManager
    {
        public virtual IDataProvider GetData(string Code, int Count)
        {
            return null;
        }

        protected double ToDouble(object o)
        {
            if (o == DBNull.Value)
            {
                return double.NaN;
            }
            return (double) o;
        }

        protected int ToInt(object o)
        {
            if (o == DBNull.Value)
            {
                return 0;
            }
            return (int) o;
        }

        public IDataProvider this[string Code, int Count]
        {
            get
            {
                return this.GetData(Code, Count);
            }
        }

        public IDataProvider this[string Code]
        {
            get
            {
                return this[Code, DataPackage.MaxValue];
            }
        }
    }
}

