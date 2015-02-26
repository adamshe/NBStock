// Decompiled with JetBrains decompiler
// Type: FML.SO
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class SO : FormulaBase
  {
    private double N1;
    private double N2;

    public SO()
    {
      base.\u002Ector();
      this.AddParam("N1", 3.0, 1.0, 10.0);
      this.AddParam("N2", 3.0, 1.0, 10.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Subtraction(this.get_C(), this.get_L()), FormulaData.op_Subtraction(this.get_H(), this.get_L()));
      formulaData1.Name = (__Null) "K ";
      FormulaData formulaData2 = FormulaBase.SMA(formulaData1, this.N1, 1.0);
      formulaData2.Name = (__Null) "SK ";
      FormulaData formulaData3 = FormulaBase.SMA(formulaData2, this.N2, 1.0);
      formulaData3.Name = (__Null) "SD ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
