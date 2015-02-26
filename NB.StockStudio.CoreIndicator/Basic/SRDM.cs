// Decompiled with JetBrains decompiler
// Type: FML.SRDM
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class SRDM : FormulaBase
  {
    private double N;

    public SRDM()
    {
      base.\u002Ector();
      this.AddParam("N", 30.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.IF(FormulaData.op_LessThanOrEqual(FormulaData.op_Addition(this.get_H(), this.get_L()), FormulaData.op_Addition(FormulaBase.REF(this.get_H(), 1.0), FormulaBase.REF(this.get_L(), 1.0))), FormulaData.op_Implicit(0.0), FormulaBase.MAX(new FormulaData[2]
      {
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_H(), FormulaBase.REF(this.get_H(), 1.0))),
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_L(), FormulaBase.REF(this.get_L(), 1.0)))
      }));
      formulaData1.Name = (__Null) "DMZ";
      FormulaData formulaData2 = FormulaBase.IF(FormulaData.op_GreaterThanOrEqual(FormulaData.op_Addition(this.get_H(), this.get_L()), FormulaData.op_Addition(FormulaBase.REF(this.get_H(), 1.0), FormulaBase.REF(this.get_L(), 1.0))), FormulaData.op_Implicit(0.0), FormulaBase.MAX(new FormulaData[2]
      {
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_H(), FormulaBase.REF(this.get_H(), 1.0))),
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_L(), FormulaBase.REF(this.get_L(), 1.0)))
      }));
      formulaData2.Name = (__Null) "DMF";
      FormulaData formulaData3 = FormulaBase.MA(formulaData1, 10.0);
      formulaData3.Name = (__Null) "ADMZ";
      FormulaData formulaData4 = FormulaBase.MA(formulaData2, 10.0);
      formulaData4.Name = (__Null) "ADMF";
      FormulaData formulaData5;
      FormulaData formulaData6 = FormulaBase.IF(FormulaData.op_GreaterThan(formulaData3, formulaData4), FormulaData.op_Division(FormulaData.op_Subtraction(formulaData3, formulaData4), formulaData3), FormulaBase.IF(formulaData5 = formulaData4, FormulaData.op_Implicit(0.0), FormulaData.op_Division(FormulaData.op_Subtraction(formulaData5, formulaData4), formulaData4)));
      formulaData6.Name = (__Null) "SRDM";
      FormulaData formulaData7 = FormulaBase.SMA(formulaData6, this.N, 1.0);
      formulaData7.Name = (__Null) "ASRDM";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData6,
        formulaData7
      }, "");
    }
  }
}
