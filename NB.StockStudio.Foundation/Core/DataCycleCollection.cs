namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class DataCycleCollection : CollectionBase
    {
        public int Add(DataCycle value)
        {
            return base.List.Add(value);
        }

        public DataCycle this[int index]
        {
            get
            {
                return (DataCycle) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

