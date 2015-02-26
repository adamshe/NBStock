// Decompiled with JetBrains decompiler
// Type: FML.VOSC
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class VOSC : FormulaBase
  {
    private double SHORT;
    private double LONG;

    public VOSC()
    {
      base.\u002Ector();
      this.AddParam("SHORT", 12.0, 2.0, 50.0);
      this.AddParam("LONG", 26.0, 15.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(FormulaBase.MA(this.get_VOL(), this.SHORT), FormulaBase.MA(this.get_VOL(), this.LONG)), FormulaBase.MA(this.get_VOL(), this.SHORT)), FormulaData.op_Implicit(100.0))
      }, "");
    }
  }
}
