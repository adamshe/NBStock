// Decompiled with JetBrains decompiler
// Type: FML.ZigLabel
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ZigLabel : FormulaBase
  {
    private double N;

    public ZigLabel()
    {
      base.\u002Ector();
      this.AddParam("N", 6.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = this.DRAWNUMBER(this.FINDPEAK(this.get_H(), this.N), this.get_H(), this.get_H(), "F2");
      formulaData1.SetAttrs("LABEL3");
      FormulaData formulaData2 = this.DRAWNUMBER(this.FINDTROUGH(this.get_L(), this.N), this.get_L(), this.get_L(), "F2");
      formulaData2.SetAttrs("LABEL3,VALIGN2");
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
