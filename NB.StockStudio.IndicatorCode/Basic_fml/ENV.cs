// Decompiled with JetBrains decompiler
// Type: FML.ENV
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ENV : FormulaBase
  {
    private double N;

    public ENV()
    {
      base.\u002Ector();
      this.AddParam("N", 14.0, 2.0, 300.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaBase.MA(this.get_CLOSE(), this.N), FormulaData.op_Implicit(1.06));
      formulaData1.Name = (__Null) "UPPER ";
      FormulaData formulaData2 = FormulaData.op_Multiply(FormulaBase.MA(this.get_CLOSE(), this.N), FormulaData.op_Implicit(0.94));
      formulaData2.Name = (__Null) "LOWER ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
