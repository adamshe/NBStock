namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class AxisXCollection : CollectionBase
    {
        public virtual void Add(FormulaAxisX fax)
        {
            base.List.Add(fax);
        }

        public void Remove(FormulaAxisX value)
        {
            base.List.Remove(value);
        }

        public virtual FormulaAxisX this[int Index]
        {
            get
            {
                if (Index < base.List.Count)
                {
                    return (FormulaAxisX) base.List[Index];
                }
                return null;
            }
        }
    }
}

