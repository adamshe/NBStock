// Decompiled with JetBrains decompiler
// Type: FML.BIAS
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class BIAS : FormulaBase
  {
    private double L1;
    private double L2;
    private double L3;

    public BIAS()
    {
      base.\u002Ector();
      this.AddParam("L1", 6.0, 1.0, 300.0);
      this.AddParam("L2", 12.0, 1.0, 300.0);
      this.AddParam("L3", 24.0, 1.0, 300.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(this.get_CLOSE(), FormulaBase.MA(this.get_CLOSE(), this.L1)), FormulaBase.MA(this.get_CLOSE(), this.L1)), FormulaData.op_Implicit(100.0));
      formulaData1.Name = (__Null) "BIAS1 ";
      FormulaData formulaData2 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(this.get_CLOSE(), FormulaBase.MA(this.get_CLOSE(), this.L2)), FormulaBase.MA(this.get_CLOSE(), this.L2)), FormulaData.op_Implicit(100.0));
      formulaData2.Name = (__Null) "BIAS2 ";
      FormulaData formulaData3 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(this.get_CLOSE(), FormulaBase.MA(this.get_CLOSE(), this.L3)), FormulaBase.MA(this.get_CLOSE(), this.L3)), FormulaData.op_Implicit(100.0));
      formulaData3.Name = (__Null) "BIAS3 ";
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData1,
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
