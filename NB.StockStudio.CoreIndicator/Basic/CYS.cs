// Decompiled with JetBrains decompiler
// Type: FML.CYS
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class CYS : FormulaBase
  {
    private double P1;
    private double P2;

    public CYS()
    {
      base.\u002Ector();
      this.AddParam("P1", 4.0, 1.0, 15.0);
      this.AddParam("P2", 5.0, 1.0, 15.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.IF(FormulaData.op_BitwiseAnd(FormulaData.op_GreaterThanOrEqual(this.get_YEAR(), FormulaData.op_Implicit(2010.0)), FormulaData.op_GreaterThanOrEqual(this.get_MONTH(), FormulaData.op_Implicit(2.0))), FormulaData.op_Implicit(0.0), FormulaData.op_Implicit(1.0));
      formulaData1.Name = (__Null) "VAR1";
      FormulaData formulaData2 = FormulaData.op_Multiply(this.get_VOL(), this.get_C());
      formulaData2.Name = (__Null) "VAR2";
      FormulaData formulaData3 = FormulaData.op_Division(FormulaBase.EMA(formulaData2, 13.0), FormulaBase.EMA(this.get_VOL(), 13.0));
      formulaData3.Name = (__Null) "VAR3";
      FormulaData formulaData4 = FormulaData.op_Multiply(FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(FormulaBase.EMA(this.get_CLOSE(), this.P1), formulaData3), formulaData3), FormulaData.op_Implicit(100.0)), formulaData1);
      formulaData4.Name = (__Null) "CYS";
      FormulaData formulaData5 = FormulaData.op_Multiply(FormulaBase.EMA(formulaData4, this.P2), formulaData1);
      formulaData5.Name = (__Null) "ML";
      FormulaData formulaData6 = FormulaData.op_Implicit(0.0);
      formulaData6.Name = (__Null) "LO";
      formulaData6.SetAttrs(" POINTDOT");
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData4,
        formulaData5,
        formulaData6
      }, "");
    }
  }
}
