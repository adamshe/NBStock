// Decompiled with JetBrains decompiler
// Type: FML.SR
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
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
      FormulaData formulaData2 = FormulaData.op_Subtraction(this.get_H(), this.get_L());
      formulaData2.Name = (__Null) "A ";
      FormulaData formulaData3 = FormulaData.op_Addition(formulaData1, formulaData2);
      formulaData3.Name = (__Null) "RR";
      FormulaData formulaData4 = FormulaData.op_Subtraction(formulaData1, formulaData2);
      formulaData4.Name = (__Null) "SS";
      FormulaData formulaData5 = FormulaData.op_Multiply(FormulaBase.BACKSET(this.get_ISLASTBAR(), 5.0), FormulaData.op_Implicit(formulaData3.get_LASTDATA()));
      formulaData5.Name = (__Null) "R";
      formulaData5.SetAttrs("WIDTH2,HIGHSPEED,COLORRED");
      FormulaData formulaData6 = FormulaData.op_Multiply(FormulaBase.BACKSET(this.get_ISLASTBAR(), 5.0), FormulaData.op_Implicit(formulaData4.get_LASTDATA()));
      formulaData6.Name = (__Null) "S";
      formulaData6.SetAttrs("WIDTH2,HIGHSPEED,COLORDARKGREEN");
      FormulaData formulaData7 = this.DRAWNUMBER(FormulaData.op_Equality(FormulaBase.BARSSINCE(formulaData5), FormulaData.op_Implicit(1.0)), formulaData5, formulaData5, "F2");
      formulaData7.SetAttrs("LABEL0,VCENTER,RIGHT,COLORRED");
      FormulaData formulaData8 = this.DRAWNUMBER(FormulaData.op_Equality(FormulaBase.BARSSINCE(formulaData6), FormulaData.op_Implicit(1.0)), formulaData6, formulaData6, "F2");
      formulaData8.SetAttrs("LABEL0,VCENTER,RIGHT,COLORDARKGREEN");
      return new FormulaPackage(new FormulaData[4]
      {
        formulaData5,
        formulaData6,
        formulaData7,
        formulaData8
      }, "");
    }
  }
}
