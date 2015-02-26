// Decompiled with JetBrains decompiler
// Type: FML.Extend.SR
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class SR : FormulaBase
  {
    public SR()
    {
      base.\u002Ector();
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(this.get_H(), this.get_L()), this.get_C()), FormulaData.op_Implicit(3.0));
      formulaData1.Name = (__Null) "M ";
      FormulaData formulaData2 = FormulaData.op_Addition(formulaData1, FormulaData.op_Subtraction(FormulaData.op_Addition(FormulaData.op_UnaryNegation(this.get_L()), FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData1)), FormulaData.op_Addition(FormulaData.op_UnaryNegation(this.get_H()), FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData1))));
      formulaData2.Name = (__Null) "S";
      formulaData2.SetAttrs("COLOR#80C080");
      FormulaData formulaData3 = FormulaData.op_Subtraction(formulaData1, FormulaData.op_Subtraction(FormulaData.op_Addition(FormulaData.op_UnaryNegation(this.get_L()), FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData1)), FormulaData.op_Addition(FormulaData.op_UnaryNegation(this.get_H()), FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData1))));
      formulaData3.Name = (__Null) "R";
      formulaData3.SetAttrs("COLOR#80C080");
      FormulaData formulaData4 = this.FILLRGN(FormulaData.op_Implicit(1.0), formulaData2, formulaData3);
      formulaData4.SetAttrs("BRUSH#2000C000");
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData2,
        formulaData3,
        formulaData4
      }, "");
    }
  }
}
