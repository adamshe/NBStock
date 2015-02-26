// Decompiled with JetBrains decompiler
// Type: FML.CHAIKIN
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class CHAIKIN : FormulaBase
  {
    private double LONG;
    private double SHORT;

    public CHAIKIN()
    {
      base.\u002Ector();
      this.AddParam("LONG", 10.0, 5.0, 300.0);
      this.AddParam("SHORT", 3.0, 1.0, 300.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.SUM(FormulaData.op_Subtraction(this.get_ADVANCE(), this.get_DECLINE()), 0.0);
      formulaData1.Name = (__Null) "ADL ";
      FormulaData formulaData2 = FormulaData.op_Subtraction(FormulaBase.MA(formulaData1, this.SHORT), FormulaBase.MA(formulaData1, this.LONG));
      formulaData2.Name = (__Null) "CHA ";
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData2
      }, "");
    }
  }
}
