// Decompiled with JetBrains decompiler
// Type: FML.SRMI
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class SRMI : FormulaBase
  {
    private double N;

    public SRMI()
    {
      base.\u002Ector();
      this.AddParam("N", 9.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaBase.IF(FormulaData.op_LessThan(this.get_C(), FormulaBase.REF(this.get_C(), this.N)), FormulaData.op_Division(FormulaData.op_Subtraction(this.get_C(), FormulaBase.REF(this.get_C(), this.N)), FormulaBase.REF(this.get_C(), this.N)), FormulaBase.IF(FormulaData.op_Equality(this.get_C(), FormulaBase.REF(this.get_C(), this.N)), FormulaData.op_Implicit(0.0), FormulaData.op_Division(FormulaData.op_Subtraction(this.get_C(), FormulaBase.REF(this.get_C(), this.N)), this.get_C())))
      }, "");
    }
  }
}
