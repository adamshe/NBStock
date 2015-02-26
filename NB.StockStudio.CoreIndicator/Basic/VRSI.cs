// Decompiled with JetBrains decompiler
// Type: FML.VRSI
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class VRSI : FormulaBase
  {
    private double N;

    public VRSI()
    {
      base.\u002Ector();
      this.AddParam("N", 6.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaData.op_Multiply(FormulaData.op_Division(FormulaBase.SMA(FormulaBase.MAX(new FormulaData[2]
        {
          FormulaData.op_Subtraction(this.get_VOL(), FormulaBase.REF(this.get_VOL(), 1.0)),
          FormulaData.op_Implicit(0.0)
        }), this.N, 1.0), FormulaBase.SMA(FormulaBase.ABS(FormulaData.op_Subtraction(this.get_VOL(), FormulaBase.REF(this.get_VOL(), 1.0))), this.N, 1.0)), FormulaData.op_Implicit(100.0))
      }, "");
    }
  }
}
