namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class FormulaPackageCollection : CollectionBase
    {
        public virtual void Add(FormulaPackage fp)
        {
            base.List.Add(fp);
        }

        public FormulaPackage this[string Name]
        {
            get
            {
                foreach (object obj2 in base.List)
                {
                    if (((FormulaPackage) obj2).Name == Name)
                    {
                        return (FormulaPackage) obj2;
                    }
                }
                return null;
            }
        }

        public virtual FormulaPackage this[int Index]
        {
            get
            {
                return (FormulaPackage) base.List[Index];
            }
        }
    }
}

