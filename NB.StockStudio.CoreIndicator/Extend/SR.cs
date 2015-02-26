
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class SR : FormulaBase
    {
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = (base.H + base.L + base.C) / 3.0;
            formulaData.Name = "M ";
            FormulaData formulaData2 = formulaData + (-base.L + 2.0 * formulaData - (-base.H + 2.0 * formulaData));
            formulaData2.Name = "S";
            formulaData2.SetAttrs("COLOR#80C080");
            FormulaData formulaData3 = formulaData - (-base.L + 2.0 * formulaData - (-base.H + 2.0 * formulaData));
            formulaData3.Name = "R";
            formulaData3.SetAttrs("COLOR#80C080");
            FormulaData formulaData4 = base.FILLRGN(1.0, formulaData2, formulaData3);
            formulaData4.SetAttrs("BRUSH#2000C000");
            return new FormulaPackage(new FormulaData[]
			{
				formulaData2, 
				formulaData3, 
				formulaData4
			}, "");
        }
    }
}
