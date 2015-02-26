
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class SlowSTO : FormulaBase
    {
        private double N = 0.0;
        private double M1 = 0.0;
        private double M2 = 0.0;
        public SlowSTO()
        {
            base.AddParam("N", 14.0, 1.0, 100.0);
            base.AddParam("M1", 3.0, 1.0, 50.0);
            base.AddParam("M2", 9.0, 1.0, 50.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = (base.C - FormulaBase.LLV(base.L, this.N)) / (FormulaBase.HHV(base.H, this.N) - FormulaBase.LLV(base.L, this.N)) * 100.0;
            formulaData.Name = "A";
            FormulaData formulaData2 = FormulaBase.MA(formulaData, this.M1);
            formulaData2.Name = "K";
            formulaData2.SetAttrs("COLORDARKGREEN,WIDTH2,HIGHQUALITY");
            FormulaData formulaData3 = FormulaBase.MA(formulaData2, this.M2);
            formulaData3.Name = "D";
            FormulaData formulaData4 = base.PARTLINE(formulaData2 >= formulaData3, formulaData2);
            formulaData4.SetAttrs("COLORRED,WIDTH2,HIGHQUALITY");
            return new FormulaPackage(new FormulaData[]
			{
				formulaData2, 
				formulaData3, 
				formulaData4
			}, "");
        }
    }
}
