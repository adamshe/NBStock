
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class RSI : FormulaBase
    {
        private double N1 = 0.0;
        public RSI()
        {
            base.AddParam("N1", 14.0, 2.0, 100.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.REF(base.CLOSE, 1.0);
            formulaData.Name = "LC ";
            FormulaData formulaData2 = FormulaBase.SMA(FormulaBase.MAX(new FormulaData[]
			{
				base.CLOSE - formulaData, 
				0.0
			}), this.N1, 1.0) / FormulaBase.SMA(FormulaBase.ABS(base.CLOSE - formulaData), this.N1, 1.0) * 100.0;
            formulaData2.Name = "RSI";
            FormulaData formulaData3 = 70.0;
            formulaData3.SetAttrs("HIGHSPEED");
            FormulaData formulaData4 = 30.0;
            formulaData4.SetAttrs("HIGHSPEED");
            FormulaData formulaData5 = base.FILLRGN(formulaData2 > 70.0, formulaData2, 70.0);
            formulaData5.SetAttrs("BRUSH#ABD930E2");////20808000");
            FormulaData formulaData6 = base.FILLRGN(formulaData2 < 30.0, formulaData2, 30.0);
            formulaData6.SetAttrs("BRUSH#AB2ACE97");//20800000");
            return new FormulaPackage(new FormulaData[]
			{
				formulaData2, 
				formulaData3, 
				formulaData4, 
				formulaData5, 
				formulaData6
			}, "");
        }
    }
}
