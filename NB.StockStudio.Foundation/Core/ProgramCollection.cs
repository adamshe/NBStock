namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ProgramCollection : CollectionBase
    {
        public int Add(FormulaProgram value)
        {
            return base.List.Add(value);
        }

        public void Remove(FormulaProgram value)
        {
            base.List.Remove(value);
        }

        public FormulaProgram this[int index]
        {
            get
            {
                return (FormulaProgram) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

