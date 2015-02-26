namespace FML.NATIVE
{
    using NB.StockStudio.Foundation;
    using NB.StockStudio.Foundation.DataProvider;
    using System;

    public class ERROR : FormulaBase
    {
        public string MSG = "";

        public ERROR()
        {
            base.AddParam("MSG", "Errors", "0", "0");
        }

        public override FormulaPackage Run(IDataProvider dp)
        {
            base.DataProvider = dp;
            FormulaData f = (double) 1.0 / (double) 0.0;
            f.Name = "A";
            base.SETNAME(this.MSG);
            base.SETTEXTVISIBLE(f, false);
            return new FormulaPackage(new FormulaData[] { f }, "");
        }
    }
}

