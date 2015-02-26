// Decompiled with JetBrains decompiler
// Type: FML.OBV
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class OBV : FormulaBase
  {
    private double N;

    public OBV()
    {
      base.\u002Ector();
      this.AddParam("N", 20.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.SUM(FormulaBase.IF(FormulaData.op_GreaterThan(this.get_CLOSE(), FormulaBase.REF(this.get_CLOSE(), 1.0)), this.get_VOL(), FormulaBase.IF(FormulaData.op_LessThan(this.get_CLOSE(), FormulaBase.REF(this.get_CLOSE(), 1.0)), FormulaData.op_UnaryNegation(this.get_VOL()), FormulaData.op_Implicit(0.0))), 0.0);
      formulaData1.Name = (__Null) "OBV";
      formulaData1.SetAttrs("WIDTH2");
      FormulaData formulaData2 = FormulaBase.MA(formulaData1, this.N);
      formulaData2.Name = (__Null) "M";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
