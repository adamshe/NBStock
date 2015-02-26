
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
    public class MA : FormulaBase
    {
        private double P1 = 0.0;
        private double P2 = 0.0;
        private double P3 = 0.0;
        private double P4 = 0.0;
        public MA()
        {
            base.AddParam("P1", 5.0, 0.0, 300.0);
            base.AddParam("P2", 10.0, 0.0, 300.0);
            base.AddParam("P3", 20.0, 0.0, 300.0);
            base.AddParam("P4", 30.0, 0.0, 300.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.MA(base.CLOSE, this.P1);
            formulaData.Name = "MA1";
            FormulaData formulaData2 = FormulaBase.MA(base.CLOSE, this.P2);
            formulaData2.Name = "MA2";
            FormulaData formulaData3 = FormulaBase.MA(base.CLOSE, this.P3);
            formulaData3.Name = "MA3";
            FormulaData formulaData4 = FormulaBase.MA(base.CLOSE, this.P4);
            formulaData4.Name = "MA4";
            return new FormulaPackage(new FormulaData[]
			{
				formulaData, 
				formulaData2, 
				formulaData3, 
				formulaData4
			}, "");
        }
    }
  
}
