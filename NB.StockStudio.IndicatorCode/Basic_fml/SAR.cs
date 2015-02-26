// Decompiled with JetBrains decompiler
// Type: FML.SAR
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class SAR : FormulaBase
  {
    private double N;
    private double STEP;
    private double MAXP;

    public SAR()
    {
      base.\u002Ector();
      this.AddParam("N", 10.0, 1.0, 100.0);
      this.AddParam("STEP", 2.0, 1.0, 100.0);
      this.AddParam("MAXP", 20.0, 5.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData = this.SAR(this.N, this.STEP, this.MAXP);
      formulaData.SetAttrs("CIRCLEDOT");
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData
      }, "");
    }
  }
}
