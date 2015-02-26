
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class BIAS : FormulaBase
  {
    private double L1;
    private double L2;
    private double L3;

    public BIAS()
    {
      this.AddParam("L1", 6.0, 1.0, 300.0);
      this.AddParam("L2", 12.0, 1.0, 300.0);
      this.AddParam("L3", 24.0, 1.0, 300.0);
    }

    public override FormulaPackage Run(IDataProvider dp)
    {
        this.DataProvider = dp;
		FormulaData formulaData = (base.CLOSE - FormulaBase.MA(base.CLOSE, this.L1)) / FormulaBase.MA(base.CLOSE, this.L1) * 100.0;
		formulaData.Name = "BIAS1 ";
		FormulaData formulaData2 = (base.CLOSE - FormulaBase.MA(base.CLOSE, this.L2)) / FormulaBase.MA(base.CLOSE, this.L2) * 100.0;
		formulaData2.Name = "BIAS2 ";
		FormulaData formulaData3 = (base.CLOSE - FormulaBase.MA(base.CLOSE, this.L3)) / FormulaBase.MA(base.CLOSE, this.L3) * 100.0;
		formulaData3.Name = "BIAS3 ";
		return new FormulaPackage(new FormulaData[]
		{
			formulaData, 
			formulaData2, 
			formulaData3
		}, "");
    }
  }
}
