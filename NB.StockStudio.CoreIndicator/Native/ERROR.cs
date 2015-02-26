using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE
{
    public class ERROR : FormulaBase
    {
        private string MSG = "";
        public ERROR()
        {
            base.AddParam("MSG", "Errors", "0", "0");
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = double.NaN;
            formulaData.Name = "A";
            base.SETNAME(this.MSG);
            base.SETTEXTVISIBLE(formulaData, false);
            return new FormulaPackage(new FormulaData[]
			{
				formulaData
			}, "");
        }
    }
}
