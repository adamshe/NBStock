// Decompiled with JetBrains decompiler
// Type: FML.MASS
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class MASS : FormulaBase
  {
    private double N1;
    private double N2;

    public MASS()
    {
      base.\u002Ector();
      this.AddParam("N1", 9.0, 2.0, 100.0);
      this.AddParam("N2", 25.0, 5.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaBase.SUM(FormulaData.op_Division(FormulaBase.EMA(FormulaData.op_Subtraction(this.get_HIGH(), this.get_LOW()), this.N1), FormulaBase.EMA(FormulaBase.EMA(FormulaData.op_Subtraction(this.get_HIGH(), this.get_LOW()), this.N1), this.N1)), this.N2)
      }, "");
    }
  }
}
