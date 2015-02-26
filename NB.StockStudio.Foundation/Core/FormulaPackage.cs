namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class FormulaPackage : CollectionBase
    {
        public string Description;
        public string Name;

        public FormulaPackage()
        {
        }

        public FormulaPackage(FormulaData[] FormulaDatas) : this(FormulaDatas, "")
        {
        }

        public FormulaPackage(FormulaData[] FormulaDatas, string Name) : this(FormulaDatas, Name, "")
        {
        }

        public FormulaPackage(FormulaData[] FormulaDatas, string Name, string Description) : this()
        {
            this.AddRange(FormulaDatas);
            this.Description = Description;
            this.Name = Name;
        }

        public virtual void Add(FormulaData fd)
        {
            base.List.Add(fd);
        }

        public void AddRange(ICollection ic)
        {
            foreach (object obj2 in ic)
            {
                this.Add((FormulaData) obj2);
            }
        }

        public FormulaData GetFormulaData(string Name)
        {
            return this[Name];
        }

        public int IndexOf(FormulaData fa)
        {
            return base.List.IndexOf(fa);
        }

        public virtual void Insert(int Index, FormulaData fd)
        {
            base.List.Insert(Index, fd);
        }

        public void Remove(FormulaData value)
        {
            base.List.Remove(value);
        }

        public void Remove(string Name)
        {
            base.List.Remove(this[Name]);
        }

        public FormulaData this[int i]
        {
            get
            {
                return (FormulaData) base.List[i];
            }
        }

        public FormulaData this[string Name]
        {
            get
            {
                foreach (FormulaData data in base.List)
                {
                    if ((data.Name != null) && (string.Compare(data.Name.Trim(), Name, true) == 0))
                    {
                        return data;
                    }
                }
                return null;
            }
        }
    }
}

