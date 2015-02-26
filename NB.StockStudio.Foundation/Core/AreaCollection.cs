namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class AreaCollection : CollectionBase
    {
        public virtual void Add(FormulaArea fa)
        {
            base.List.Add(fa);
        }

        public int IndexOf(FormulaArea fa)
        {
            return base.List.IndexOf(fa);
        }

        public virtual void Insert(int Index, FormulaArea fa)
        {
            base.List.Insert(Index, fa);
        }

        public void Remove(FormulaArea value)
        {
            base.List.Remove(value);
        }

        public void Remove(string Name)
        {
            base.List.Remove(this[Name]);
        }

        public FormulaArea this[string Name]
        {
            get
            {
                foreach (object obj2 in base.List)
                {
                    if (((FormulaArea) obj2).Name == Name)
                    {
                        return (FormulaArea) obj2;
                    }
                }
                return null;
            }
        }

        public virtual FormulaArea this[int Index]
        {
            get
            {
                return (FormulaArea) base.List[Index];
            }
        }
    }
}

