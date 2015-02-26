
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.NATIVE// NB.StockStudio.CoreIndicator
{
	public class COMPARESTOCK : FormulaBase
	{
		private string STOCKNAME = "";
		public COMPARESTOCK()
		{
			base.AddParam("StockName", "0", "0", "0");
		}
		public override FormulaPackage Run(IDataProvider dp)
		{
			this.DataProvider = dp;
			FormulaData formulaData = base.FML(this.STOCKNAME, "C");
			formulaData.SetAttrs(" FIRSTDATAOFVIEW");
			base.SETNAME(this.STOCKNAME);
			return new FormulaPackage(new FormulaData[]
			{
				formulaData
			}, "");
		}
	}
}
