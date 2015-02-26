
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class Compare : FormulaBase
    {
        private string STOCKCODE = "";
        public Compare()
        {
            base.AddParam("StockCode", "^DJI", "0", "0");
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = base.FML(this.STOCKCODE, "C");
            formulaData.SetAttrs("FIRSTDATAOFVIEW,HIGHQUALITY");
            base.SETNAME(this.STOCKCODE);
            return new FormulaPackage(new FormulaData[]
			{
				formulaData
			}, "");
        }
    }
}
