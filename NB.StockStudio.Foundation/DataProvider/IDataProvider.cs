namespace NB.StockStudio.Foundation.DataProvider
{
    using NB.StockStudio.Foundation;
    using System;
    using System.Reflection;

    public interface IDataProvider
    {
        double GetConstData(string DataType);
        string GetStringData(string DataType);
        void SetStringData(string DataType, string value);
        string GetUnique();

        IDataProvider BaseDataProvider { get; set; }

        int Count { get; }

        DataCycle DataCycle { get; set; }

        IDataManager DataManager { get; }

        double[] this[string Name] { get; }

        int MaxCount { get; set; }
    }
}

