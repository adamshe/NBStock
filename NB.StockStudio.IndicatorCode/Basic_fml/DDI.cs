// Decompiled with JetBrains decompiler
// Type: FML.DDI
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class DDI : FormulaBase
  {
    private double N;
    private double N1;
    private double M;
    private double M1;

    public DDI()
    {
      base.\u002Ector();
      this.AddParam("N", 13.0, 1.0, 100.0);
      this.AddParam("N1", 30.0, 1.0, 100.0);
      this.AddParam("M", 10.0, 1.0, 100.0);
      this.AddParam("M1", 5.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaBase.MAX(new FormulaData[2]
      {
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_H(), FormulaBase.REF(this.get_H(), 1.0))),
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_L(), FormulaBase.REF(this.get_L(), 1.0)))
      }).Name = (__Null) "TR";
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
      FormulaData formulaData3 = FormulaData.op_Division(FormulaBase.SUM(formulaData1, this.N), FormulaData.op_Addition(FormulaBase.SUM(formulaData1, this.N), FormulaBase.SUM(formulaData2, this.N)));
      formulaData3.Name = (__Null) "DIZ";
      FormulaData formulaData4 = FormulaData.op_Division(FormulaBase.SUM(formulaData2, this.N), FormulaData.op_Addition(FormulaBase.SUM(formulaData2, this.N), FormulaBase.SUM(formulaData1, this.N)));
      formulaData4.Name = (__Null) "DIF";
      FormulaData formulaData5 = FormulaData.op_Subtraction(formulaData3, formulaData4);
      formulaData5.Name = (__Null) "DDI";
      formulaData5.SetAttrs("COLORSTICK");
      FormulaData formulaData6 = FormulaBase.SMA(formulaData5, this.N1, this.M);
      formulaData6.Name = (__Null) "ADDI";
      FormulaData formulaData7 = FormulaBase.MA(formulaData6, this.M1);
      formulaData7.Name = (__Null) "AD";
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData5,
        formulaData6,
        formulaData7
      }, "");
    }
  }
}
