// Decompiled with JetBrains decompiler
// Type: FML.RC
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class RC : FormulaBase
  {
    private double N;

    public RC()
    {
      base.\u002Ector();
      this.AddParam("N", 50.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(this.get_C(), FormulaBase.REF(this.get_C(), this.N));
      formulaData1.Name = (__Null) "RC";
      FormulaData formulaData2 = FormulaBase.SMA(FormulaBase.REF(formulaData1, 1.0), this.N, 1.0);
      formulaData2.Name = (__Null) "ARC";
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData2
      }, "");
    }
  }
}
