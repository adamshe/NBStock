// Decompiled with JetBrains decompiler
// Type: FML.VHF
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class VHF : FormulaBase
  {
    private double N;

    public VHF()
    {
      base.\u002Ector();
      this.AddParam("N", 28.0, 3.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaData.op_Division(FormulaData.op_Subtraction(FormulaBase.HHV(this.get_CLOSE(), this.N), FormulaBase.LLV(this.get_CLOSE(), this.N)), FormulaBase.SUM(FormulaBase.ABS(FormulaData.op_Subtraction(this.get_CLOSE(), FormulaBase.REF(this.get_CLOSE(), 1.0))), this.N))
      }, "");
    }
  }
}
