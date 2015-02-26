//http://html-color-codes.info/

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class SlowSTO : FormulaBase
  {
    private double N;
    private double M1;
    private double M2;

    public SlowSTO()
    {
        base.AddParam("N", 14.0, 1.0, 100.0);
        base.AddParam("M1", 3.0, 2.0, 50.0);
        base.AddParam("M2", 3.0, 2.0, 50.0);
    }

    public override FormulaPackage Run(IDataProvider dp)
    {
        this.DataProvider = dp;
        FormulaData formulaData = (base.CLOSE - FormulaBase.LLV(base.LOW, this.N)) / (FormulaBase.HHV(base.HIGH, this.N) - FormulaBase.LLV(base.LOW, this.N)) * 100.0;
        formulaData.Name = "A";
        FormulaData majorLine = FormulaBase.MA(formulaData, this.M1);
        majorLine.Name = "K";
        FormulaData majorLineMA = FormulaBase.MA(majorLine, this.M2);
        majorLineMA.Name = "D";

        FormulaData overboughtThreshold = 80.0;
        FormulaData oversoldThreshold = 20.0;

        FormulaData overboughtZone = base.FILLRGN(majorLine > overboughtThreshold, majorLine, overboughtThreshold);
        overboughtZone.SetAttrs("BRUSH#ABCE2A60"); //alpha = 00 means absolute transparent FF means solid
        FormulaData oversoldZone = base.FILLRGN(majorLine < oversoldThreshold, majorLine, oversoldThreshold);
        oversoldZone.SetAttrs("BRUSH#AB6BCE2A");

        return new FormulaPackage(new FormulaData[]
			{
				majorLine, 
				majorLineMA,
                overboughtThreshold,
                oversoldThreshold,
                overboughtZone,
                oversoldZone,
			}, "");
    }
  }
}
