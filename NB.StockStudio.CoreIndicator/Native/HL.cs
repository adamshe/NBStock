using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE
{
    public class HL : FormulaBase
    {
        private double N = 0.0;
        public HL()
        {
            base.AddParam("N", 0.0, 1.0, 20000.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = this.N;
            return new FormulaPackage(new FormulaData[]
			{
				formulaData
			}, "");
        }
    }
}
