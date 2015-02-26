
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class BB : FormulaBase
    {
        private double N = 0.0;
        private double P = 0.0;
        public BB()
        {
            base.AddParam("N", 20.0, 5.0, 300.0);
            base.AddParam("P", 2.0, 0.1, 10.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.MA(base.CLOSE, this.N);
            formulaData.Name = "MID ";
            FormulaData formulaData2 = formulaData + this.P * FormulaBase.STD(base.CLOSE, this.N);
            formulaData2.Name = "UPPER";
            formulaData2.SetAttrs("COLOR#8080C0");
            FormulaData formulaData3 = formulaData - this.P * FormulaBase.STD(base.CLOSE, this.N);
            formulaData3.Name = "LOWER";
            formulaData3.SetAttrs("COLOR#8080C0");
            FormulaData formulaData4 = base.FILLRGN(1.0, formulaData3, formulaData2);
            formulaData4.SetAttrs("BRUSH#99FFFF");//("BRUSH#200000C0");
            return new FormulaPackage(new FormulaData[]
			{
				formulaData2, 
				formulaData3, 
				formulaData4
			}, "");
        }
    }
}
