using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE
{
    public class VOL : FormulaBase
    {
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData vol = base.VOL;
            vol.SetAttrs("VOLSTICK");
            return new FormulaPackage(new FormulaData[]
			{
				vol
			}, "Volume");
        }
    }
}
