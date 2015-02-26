namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class FormulaSpaceCollection : CollectionBase
    {
        public int Add(FormulaSpace value)
        {
            return base.List.Add(value);
        }

        public void Remove(FormulaSpace value)
        {
            base.List.Remove(value);
        }

        public FormulaSpace this[int index]
        {
            get
            {
                return (FormulaSpace) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

