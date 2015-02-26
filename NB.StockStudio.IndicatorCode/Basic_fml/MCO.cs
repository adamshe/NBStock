// Decompiled with JetBrains decompiler
// Type: FML.MCO
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class MCO : FormulaBase
  {
    private double N1;
    private double N2;

    public MCO()
    {
      base.\u002Ector();
      this.AddParam("N1", 19.0, 10.0, 80.0);
      this.AddParam("N2", 39.0, 30.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaData.op_Subtraction(FormulaData.op_Division(FormulaBase.EMA(FormulaData.op_Subtraction(this.get_ADVANCE(), this.get_DECLINE()), this.N1), FormulaData.op_Implicit(10.0)), FormulaData.op_Division(FormulaBase.EMA(FormulaData.op_Subtraction(this.get_ADVANCE(), this.get_DECLINE()), this.N2), FormulaData.op_Implicit(20.0)))
      }, "");
    }
  }
}
