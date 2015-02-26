// Decompiled with JetBrains decompiler
// Type: FML.CDP
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class CDP : FormulaBase
  {
    public CDP()
    {
      base.\u002Ector();
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Subtraction(FormulaBase.REF(this.get_HIGH(), 1.0), FormulaBase.REF(this.get_LOW(), 1.0));
      formulaData1.Name = (__Null) "PT  ";
      FormulaData formulaData2 = FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(this.get_HIGH(), this.get_LOW()), this.get_CLOSE()), FormulaData.op_Implicit(3.0));
      formulaData2.Name = (__Null) "CDP ";
      FormulaData formulaData3 = FormulaData.op_Addition(formulaData2, formulaData1);
      formulaData3.Name = (__Null) "AH  ";
      FormulaData formulaData4 = FormulaData.op_Subtraction(formulaData2, formulaData1);
      formulaData4.Name = (__Null) "AL  ";
      FormulaData formulaData5 = FormulaData.op_Subtraction(FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData2), this.get_LOW());
      formulaData5.Name = (__Null) "NH  ";
      FormulaData formulaData6 = FormulaData.op_Subtraction(FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData2), this.get_HIGH());
      formulaData6.Name = (__Null) "NL  ";
      return new FormulaPackage(new FormulaData[5]
      {
        formulaData2,
        formulaData3,
        formulaData4,
        formulaData5,
        formulaData6
      }, "");
    }
  }
}
