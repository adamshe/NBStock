// Decompiled with JetBrains decompiler
// Type: FML.ZIG
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ZIG : FormulaBase
  {
    private double PER;

    public ZIG()
    {
      base.\u002Ector();
      this.AddParam("PER", 10.0, 1.0, 60.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData = this.ZIG(3.0, this.PER);
      formulaData.SetAttrs("WIDTH2");
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData
      }, "");
    }
  }
}
