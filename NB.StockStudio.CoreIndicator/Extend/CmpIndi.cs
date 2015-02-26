
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class CmpIndi : FormulaBase
    {
        private string STOCKCODE = "";
        private string INDI = "";
        public CmpIndi()
        {
            base.AddParam("StockCode", "^DJI", "0", "0");
            base.AddParam("Indi", "RSI(14)", "0", "0");
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = base.FML(this.INDI);
            formulaData.Name = "V1";
            formulaData.SetAttrs("HIGHQUALITY");
            FormulaData formulaData2 = base.FML(this.STOCKCODE, this.INDI);
            formulaData2.Name = "V2";
            formulaData2.SetAttrs("HIGHQUALITY");
            base.SETNAME(formulaData, base.STKLABEL);
            base.SETNAME(formulaData2, this.STOCKCODE);
            base.SETNAME(this.INDI);
            return new FormulaPackage(new FormulaData[]
			{
				formulaData, 
				formulaData2
			}, "");
        }
    }
}
