// Decompiled with JetBrains decompiler
// Type: FML.ATR
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ATR : FormulaBase
  {
    private double N;

    public ATR()
    {
      base.\u002Ector();
      this.AddParam("N", 10.0, 1.0, 300.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.REF(this.get_CLOSE(), 1.0);
      formulaData1.Name = (__Null) "LC ";
      FormulaData formulaData2 = FormulaBase.MAX(new FormulaData[3]
      {
        FormulaData.op_Subtraction(this.get_HIGH(), this.get_LOW()),
        FormulaBase.ABS(FormulaData.op_Subtraction(formulaData1, this.get_HIGH())),
        FormulaBase.ABS(FormulaData.op_Subtraction(formulaData1, this.get_LOW()))
      });
      formulaData2.Name = (__Null) "TR ";
      FormulaData formulaData3 = FormulaBase.SMA(formulaData2, this.N, 1.0);
      formulaData3.Name = (__Null) "ATR ";
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData3
      }, "");
    }
  }
}
