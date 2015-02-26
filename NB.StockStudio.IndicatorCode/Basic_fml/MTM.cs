// Decompiled with JetBrains decompiler
// Type: FML.MTM
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class MTM : FormulaBase
  {
    private double N;
    private double N1;

    public MTM()
    {
      base.\u002Ector();
      this.AddParam("N", 6.0, 1.0, 100.0);
      this.AddParam("N1", 6.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Subtraction(this.get_CLOSE(), FormulaBase.REF(this.get_CLOSE(), this.N));
      formulaData1.Name = (__Null) "MTM ";
      FormulaData formulaData2 = FormulaBase.MA(formulaData1, this.N1);
      formulaData2.Name = (__Null) "MTMMA ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
