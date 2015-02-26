// Decompiled with JetBrains decompiler
// Type: FML.BBIBOLL
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class BBIBOLL : FormulaBase
  {
    private double N;
    private double P;

    public BBIBOLL()
    {
      base.\u002Ector();
      this.AddParam("N", 10.0, 1.0, 100.0);
      this.AddParam("P", 3.0, 0.1, 20.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(FormulaData.op_Addition(FormulaBase.MA(this.get_CLOSE(), 3.0), FormulaBase.MA(this.get_CLOSE(), 6.0)), FormulaBase.MA(this.get_CLOSE(), 12.0)), FormulaBase.MA(this.get_CLOSE(), 24.0)), FormulaData.op_Implicit(4.0));
      formulaData1.Name = (__Null) "BBI";
      FormulaData formulaData2 = FormulaData.op_Addition(formulaData1, FormulaData.op_Multiply(FormulaData.op_Implicit(this.P), FormulaBase.STD(formulaData1, this.N)));
      formulaData2.Name = (__Null) "UPR";
      FormulaData formulaData3 = FormulaData.op_Subtraction(formulaData1, FormulaData.op_Multiply(FormulaData.op_Implicit(this.P), FormulaBase.STD(formulaData1, this.N)));
      formulaData3.Name = (__Null) "DWN";
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData1,
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
