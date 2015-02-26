// Decompiled with JetBrains decompiler
// Type: FML.DMA
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class DMA : FormulaBase
  {
    private double SHORT;
    private double LONG;
    private double M;

    public DMA()
    {
      base.\u002Ector();
      this.AddParam("SHORT", 10.0, 2.0, 300.0);
      this.AddParam("LONG", 50.0, 2.0, 300.0);
      this.AddParam("M", 10.0, 1.0, 300.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Subtraction(FormulaBase.MA(this.get_CLOSE(), this.SHORT), FormulaBase.MA(this.get_CLOSE(), this.LONG));
      formulaData1.Name = (__Null) "DDD ";
      FormulaData formulaData2 = FormulaBase.MA(formulaData1, this.M);
      formulaData2.Name = (__Null) "AMA ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
