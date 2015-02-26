using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE
{
    public class MA : FormulaBase
    {
        private double N = 0.0;
        public MA()
        {
            base.AddParam("N", 12.0, 1.0, 1000.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.MA(base.C, this.N);
            return new FormulaPackage(new FormulaData[]
			{
			   formulaData
			}, string.Empty);
        }
    }
}
