// Decompiled with JetBrains decompiler
// Type: FML.MACD
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class MACD : FormulaBase
  {
    private double LONG;
    private double SHORT;
    private double M;

    public MACD()
    {
      this.AddParam("LONG", 26.0, 20.0, 100.0);
      this.AddParam("SHORT", 12.0, 5.0, 40.0);
      this.AddParam("M", 9.0, 2.0, 60.0);
    }

    public override FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = dp;
      FormulaData formulaData = FormulaBase.EMA(base.CLOSE, this.SHORT) - FormulaBase.EMA(base.CLOSE, this.LONG);
      formulaData.Name = "DIFF ";
      FormulaData formulaData2 = FormulaBase.EMA(formulaData, this.M);
      formulaData2.Name = "DEA  ";
      FormulaData formulaData3 = 2.0 * (formulaData - formulaData2);
      formulaData3.Name =  "MACD ";
      formulaData3.SetAttrs(" COLORSTICK");
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData,
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
