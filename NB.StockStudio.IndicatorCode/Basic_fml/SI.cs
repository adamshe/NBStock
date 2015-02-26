// Decompiled with JetBrains decompiler
// Type: FML.SI
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class SI : FormulaBase
  {
    public SI()
    {
      base.\u002Ector();
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.REF(this.get_C(), 1.0);
      formulaData1.Name = (__Null) "LC";
      FormulaData formulaData2 = FormulaBase.ABS(FormulaData.op_Subtraction(this.get_H(), formulaData1));
      formulaData2.Name = (__Null) "AA";
      FormulaData formulaData3 = FormulaBase.ABS(FormulaData.op_Subtraction(this.get_L(), formulaData1));
      formulaData3.Name = (__Null) "BB";
      FormulaData formulaData4 = FormulaBase.ABS(FormulaData.op_Subtraction(this.get_H(), FormulaBase.REF(this.get_L(), 1.0)));
      formulaData4.Name = (__Null) "CC";
      FormulaData formulaData5 = FormulaBase.ABS(FormulaData.op_Subtraction(formulaData1, FormulaBase.REF(this.get_O(), 1.0)));
      formulaData5.Name = (__Null) "DD";
      FormulaData formulaData6 = FormulaBase.IF(FormulaData.op_BitwiseAnd(FormulaData.op_GreaterThan(formulaData2, formulaData3), FormulaData.op_GreaterThan(formulaData2, formulaData4)), FormulaData.op_Addition(FormulaData.op_Addition(formulaData2, FormulaData.op_Division(formulaData3, FormulaData.op_Implicit(2.0))), FormulaData.op_Division(formulaData5, FormulaData.op_Implicit(4.0))), FormulaBase.IF(FormulaData.op_BitwiseAnd(FormulaData.op_GreaterThan(formulaData3, formulaData4), FormulaData.op_GreaterThan(formulaData3, formulaData2)), FormulaData.op_Addition(FormulaData.op_Addition(formulaData3, FormulaData.op_Division(formulaData2, FormulaData.op_Implicit(2.0))), FormulaData.op_Division(formulaData5, FormulaData.op_Implicit(4.0))), FormulaData.op_Addition(formulaData4, FormulaData.op_Division(formulaData5, FormulaData.op_Implicit(4.0)))));
      formulaData6.Name = (__Null) "R";
      FormulaData formulaData7 = FormulaData.op_Subtraction(FormulaData.op_Addition(FormulaData.op_Addition(FormulaData.op_Subtraction(this.get_C(), formulaData1), FormulaData.op_Division(FormulaData.op_Subtraction(this.get_C(), this.get_O()), FormulaData.op_Implicit(2.0))), formulaData1), FormulaBase.REF(this.get_O(), 1.0));
      formulaData7.Name = (__Null) "X";
      FormulaData formulaData8 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Multiply(FormulaData.op_Implicit(16.0), formulaData7), formulaData6), FormulaBase.MAX(new FormulaData[2]
      {
        formulaData2,
        formulaData3
      }));
      formulaData8.Name = (__Null) "SI";
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData8
      }, "");
    }
  }
}
