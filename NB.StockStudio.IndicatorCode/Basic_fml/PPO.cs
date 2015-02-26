// Decompiled with JetBrains decompiler
// Type: FML.PPO
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class PPO : FormulaBase
  {
    private double LONG;
    private double SHORT;
    private double N;

    public PPO()
    {
      base.\u002Ector();
      this.AddParam("LONG", 26.0, 5.0, 100.0);
      this.AddParam("SHORT", 12.0, 2.0, 40.0);
      this.AddParam("N", 9.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(FormulaBase.EMA(this.get_CLOSE(), this.SHORT), FormulaBase.EMA(this.get_CLOSE(), this.LONG)), FormulaBase.EMA(this.get_CLOSE(), this.LONG)), FormulaData.op_Implicit(100.0));
      formulaData1.Name = (__Null) "PPO";
      FormulaData formulaData2 = FormulaBase.EMA(formulaData1, this.N);
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
