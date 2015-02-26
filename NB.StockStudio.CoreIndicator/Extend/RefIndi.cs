
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class RefIndi : FormulaBase
    {
        private string INDI = "";
        private double N = 0.0;
        public RefIndi()
        {
            base.AddParam("Indi", "MACD[DIFF]", "0", "0");
            base.AddParam("N", 10.0, 1.0, 10000.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.REF(base.FML(this.INDI), this.N);
            base.SETNAME(this.INDI + "-" + this.N);
            return new FormulaPackage(new FormulaData[]
			{
				formulaData
			}, "");
        }
    }
}
