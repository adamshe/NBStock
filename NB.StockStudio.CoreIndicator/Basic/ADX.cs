// Decompiled with JetBrains decompiler
// Type: FML.ADX
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ADX : FormulaBase
  {
    private double N;

    public ADX()
    {
      base.\u002Ector();
      this.AddParam("N", 14.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.SUM(FormulaBase.MAX(new FormulaData[2]
      {
        FormulaBase.MAX(new FormulaData[2]
        {
          FormulaData.op_Subtraction(this.get_HIGH(), this.get_LOW()),
          FormulaBase.ABS(FormulaData.op_Subtraction(this.get_HIGH(), FormulaBase.REF(this.get_CLOSE(), 1.0)))
        }),
        FormulaBase.ABS(FormulaData.op_Subtraction(this.get_LOW(), FormulaBase.REF(this.get_CLOSE(), 1.0)))
      }), this.N);
      formulaData1.Name = (__Null) "TR ";
      FormulaData formulaData2 = FormulaData.op_Subtraction(this.get_HIGH(), FormulaBase.REF(this.get_HIGH(), 1.0));
      formulaData2.Name = (__Null) "HD ";
      FormulaData formulaData3 = FormulaData.op_Subtraction(FormulaBase.REF(this.get_LOW(), 1.0), this.get_LOW());
      formulaData3.Name = (__Null) "LD ";
      FormulaData formulaData4 = FormulaBase.SUM(FormulaBase.IF(FormulaData.op_BitwiseAnd(FormulaData.op_GreaterThan(formulaData2, FormulaData.op_Implicit(0.0)), FormulaData.op_GreaterThan(formulaData2, formulaData3)), formulaData2, FormulaData.op_Implicit(0.0)), this.N);
      formulaData4.Name = (__Null) "DMP";
      FormulaData formulaData5 = FormulaBase.SUM(FormulaBase.IF(FormulaData.op_BitwiseAnd(FormulaData.op_GreaterThan(formulaData3, FormulaData.op_Implicit(0.0)), FormulaData.op_GreaterThan(formulaData3, formulaData2)), formulaData3, FormulaData.op_Implicit(0.0)), this.N);
      formulaData5.Name = (__Null) "DMM";
      FormulaData formulaData6 = FormulaData.op_Division(FormulaData.op_Multiply(formulaData4, FormulaData.op_Implicit(100.0)), formulaData1);
      formulaData6.Name = (__Null) "PDI";
      this.SETNAME(formulaData6, "+DI");
      FormulaData formulaData7 = FormulaData.op_Division(FormulaData.op_Multiply(formulaData5, FormulaData.op_Implicit(100.0)), formulaData1);
      formulaData7.Name = (__Null) "MDI";
      this.SETNAME(formulaData7, "-DI");
      FormulaData formulaData8 = FormulaBase.MA(FormulaData.op_Multiply(FormulaData.op_Division(FormulaBase.ABS(FormulaData.op_Subtraction(formulaData7, formulaData6)), FormulaData.op_Addition(formulaData7, formulaData6)), FormulaData.op_Implicit(100.0)), this.N);
      formulaData8.Name = (__Null) "ADX";
      formulaData8.SetAttrs("WIDTH2");
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData6,
        formulaData7,
        formulaData8
      }, "");
    }
  }
}
