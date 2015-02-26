using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE
{
    public class EMA : FormulaBase
    {
        private double N = 0.0;
        public EMA()
        {
            base.AddParam("N", 12.0, 1.0, 1000.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.EMA(base.C, this.N);
            return new FormulaPackage(new FormulaData[]
			{
				formulaData
			}, "");
        }
    }
}
