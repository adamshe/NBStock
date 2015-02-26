using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE
{
    public class MAIN : FormulaBase
    {
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData stock = base.STOCK;
            stock.Name = "M";
            base.SETNAME(base.STKLABEL);
            base.SETTEXTVISIBLE(false);
            base.SETTEXTVISIBLE(stock, false);
            return new FormulaPackage(new FormulaData[]
			{
				stock
			}, "");
        }
    }
}
