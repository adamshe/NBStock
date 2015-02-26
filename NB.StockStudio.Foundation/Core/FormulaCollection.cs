namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class FormulaCollection : CollectionBase
    {
        public FormulaCollection(int capacity = 10) 
        {
            Capacity = capacity;
        }

        public void Add(FormulaBase fb)
        {
            base.List.Add(fb);
        }

        public void Add(string Name)
        {
            this.Add(Name, "");
        }

        public void Add(string Name, string Quote)
        {
            FormulaBase formulaByName = FormulaBase.GetFormulaByName(Name);
            formulaByName.Quote = Quote;
            this.Add(formulaByName);
        }

        public void Insert(int Index, FormulaBase fb)
        {
            base.List.Insert(Index, fb);
        }

        public void Insert(int Index, string Name)
        {
            FormulaBase formulaByName = FormulaBase.GetFormulaByName(Name);
            this.Insert(Index, formulaByName);
        }

        public void Remove(FormulaBase fb)
        {
            base.List.Remove(fb);
        }

        public void Remove(string Name)
        {
            foreach (object obj2 in base.List)
            {
                if (((FormulaBase) obj2).GetType().ToString() == Name)
                {
                    this.Remove((FormulaBase) obj2);
                }
            }
        }

        public FormulaBase this[string Name]
        {
            get
            {
                foreach (object obj2 in base.List)
                {
                    if (((FormulaBase) obj2).GetType().ToString() == Name)
                    {
                        return (FormulaBase) obj2;
                    }
                }
                return null;
            }
        }

        public FormulaBase this[int Index]
        {
            get
            {
                return (FormulaBase) base.List[Index];
            }
        }
    }
}

