namespace NB.StockStudio.Foundation
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text;

    public class ParamCollection : CollectionBase
    {
        public void Add(FormulaParam fp)
        {
            base.List.Add(fp);
        }

        public void Remove(FormulaParam fp)
        {
            base.List.Remove(fp);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < base.Count; i++)
            {
                if (i != 0)
                {
                    builder.Append(",");
                }
                builder.Append(this[i].Value);
            }
            return builder.ToString();
        }

        public FormulaParam this[string Name]
        {
            get
            {
                foreach (object obj2 in base.List)
                {
                    if (((FormulaParam) obj2).Name == Name)
                    {
                        return (FormulaParam) obj2;
                    }
                }
                return null;
            }
        }

        public FormulaParam this[int Index]
        {
            get
            {
                return (FormulaParam) base.List[Index];
            }
        }
    }
}

