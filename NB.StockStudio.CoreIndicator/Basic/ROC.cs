// Decompiled with JetBrains decompiler
// Type: FML.ROC
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ROC : FormulaBase
  {
    private double N;
    private double M;

    public ROC()
    {
      base.\u002Ector();
      this.AddParam("N", 12.0, 1.0, 100.0);
      this.AddParam("M", 6.0, 1.0, 50.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(this.get_CLOSE(), FormulaBase.REF(this.get_CLOSE(), this.N)), FormulaBase.REF(this.get_CLOSE(), this.N)), FormulaData.op_Implicit(100.0));
      formulaData1.Name = (__Null) "ROC";
      FormulaData formulaData2 = FormulaBase.MA(formulaData1, this.M);
      formulaData2.Name = (__Null) "ROCMA";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
