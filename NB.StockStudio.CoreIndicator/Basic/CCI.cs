
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class CCI : FormulaBase
  {
    private double N;

    public CCI()
    {
      base.AddParam("N", 14.0, 2.0, 100.0);
    }

    public override FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = dp;
      FormulaData formulaData = (base.HIGH + base.LOW + base.CLOSE) / 3.0;
      formulaData.Name = "TYP ";
      FormulaData formulaData2 = (formulaData - FormulaBase.MA(formulaData, this.N)) / (0.015 * FormulaBase.AVEDEV(formulaData, this.N));
      return new FormulaPackage(new FormulaData[]
      {
        formulaData2
      }, "");
    }
  }
}
