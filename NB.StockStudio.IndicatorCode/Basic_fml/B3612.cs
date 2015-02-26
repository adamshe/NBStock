// Decompiled with JetBrains decompiler
// Type: FML.B3612
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class B3612 : FormulaBase
  {
    public B3612()
    {
      base.\u002Ector();
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Subtraction(FormulaBase.MA(this.get_CLOSE(), 3.0), FormulaBase.MA(this.get_CLOSE(), 6.0));
      formulaData1.Name = (__Null) "B36 ";
      FormulaData formulaData2 = FormulaData.op_Subtraction(FormulaBase.MA(this.get_CLOSE(), 6.0), FormulaBase.MA(this.get_CLOSE(), 12.0));
      formulaData2.Name = (__Null) "B612 ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
