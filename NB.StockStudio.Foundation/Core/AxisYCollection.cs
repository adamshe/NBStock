namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class AxisYCollection : CollectionBase
    {
        public virtual void Add(FormulaAxisY fay)
        {
            base.List.Add(fay);
        }

        public void Remove(FormulaAxisY value)
        {
            base.List.Remove(value);
        }

        public virtual FormulaAxisY this[int Index]
        {
            get
            {
                if (Index >= base.List.Count)
                {
                    throw new Exception("Formula Y-Axis bind out of range.(" + Index + ")");
                }
                return (FormulaAxisY) base.List[Index];
            }
        }
    }
}

