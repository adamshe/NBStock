// Decompiled with JetBrains decompiler
// Type: FML.TRIX
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class TRIX : FormulaBase
  {
    private double N;
    private double M;

    public TRIX()
    {
      base.\u002Ector();
      this.AddParam("N", 12.0, 3.0, 100.0);
      this.AddParam("M", 20.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.EMA(FormulaBase.EMA(FormulaBase.EMA(this.get_CLOSE(), this.N), this.N), this.N);
      formulaData1.Name = (__Null) "TR";
      FormulaData formulaData2 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(formulaData1, FormulaBase.REF(formulaData1, 1.0)), FormulaBase.REF(formulaData1, 1.0)), FormulaData.op_Implicit(100.0));
      formulaData2.Name = (__Null) "TRIX ";
      FormulaData formulaData3 = FormulaBase.MA(formulaData2, this.M);
      formulaData3.Name = (__Null) "TRMA ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
