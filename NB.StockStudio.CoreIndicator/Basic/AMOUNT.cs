// Decompiled with JetBrains decompiler
// Type: FML.AMOUNT
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class AMOUNT : FormulaBase
  {
    private double N1;
    private double N2;

    public AMOUNT()
    {
      base.\u002Ector();
      this.AddParam("N1", 5.0, 1.0, 100.0);
      this.AddParam("N2", 20.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData amount = this.get_AMOUNT();
      amount.SetAttrs("VOLSTICK");
      FormulaData formulaData1 = FormulaBase.MA(this.get_AMOUNT(), this.N1);
      formulaData1.Name = (__Null) "MA1";
      FormulaData formulaData2 = FormulaBase.MA(this.get_AMOUNT(), this.N2);
      formulaData2.Name = (__Null) "MA2";
      return new FormulaPackage(new FormulaData[3]
      {
        amount,
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
