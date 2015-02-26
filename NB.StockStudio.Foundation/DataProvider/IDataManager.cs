namespace NB.StockStudio.Foundation.DataProvider
{
    using System;
    using System.Reflection;

    public interface IDataManager
    {
        IDataProvider this[string Code, int Count] { get; }

        IDataProvider this[string Code] { get; }
    }
}

