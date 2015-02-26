namespace FML.NATIVE
{
    using NB.StockStudio.Foundation;
    using NB.StockStudio.Foundation.DataProvider;
    using System;
    using System.Collections;

    public class CustomFormula : FormulaBase
    {
        private ArrayList alFormulas;

        public CustomFormula()
        {
            this.alFormulas = new ArrayList();
        }

        public CustomFormula(string Name)
        {
            this.alFormulas = new ArrayList();
            base.Name = Name;
        }

        public void Add(double[] Data)
        {
            this.Add("", Data);
        }

        public void Add(string Name, double[] Data)
        {
            this.Add(Name, (double[]) null, Data);
        }

        public void Add(string Name, DateTime[] Date, double[] Data)
        {
            double[] date = new double[Date.Length];
            for (int i = 0; i < Date.Length; i++)
            {
                date[i] = Date[i].ToOADate();
            }
            this.Add(Name, date, Data);
        }

        public void Add(string Name, double[] Date, double[] Data)
        {
            this.alFormulas.Add(new CustomData(Name, Date, Data));
        }

        public override FormulaPackage Run(IDataProvider dp)
        {
            base.DataProvider = dp;
            FormulaData[] formulaDatas = new FormulaData[this.alFormulas.Count];
            for (int i = 0; i < formulaDatas.Length; i++)
            {
                CustomData data = (CustomData) this.alFormulas[i];
                if (data.Date != null)
                {
                    formulaDatas[i] = base.AdjustDateTime(data.Date, data.Data);
                }
                else
                {
                    formulaDatas[i] = data.Data;
                }
                formulaDatas[i].Name = data.Name;
            }
            return new FormulaPackage(formulaDatas, "");
        }
    }
}

