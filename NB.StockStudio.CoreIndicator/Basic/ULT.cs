// Decompiled with JetBrains decompiler
// Type: FML.ULT
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class ULT : FormulaBase
  {
    private double N1;
    private double N2;
    private double N3;

    public ULT()
    {
      base.\u002Ector();
      this.AddParam("N1", 7.0, 1.0, 100.0);
      this.AddParam("N2", 14.0, 1.0, 100.0);
      this.AddParam("N3", 28.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.REF(this.get_C(), 1.0);
      formulaData1.Name = (__Null) "LC ";
      FormulaData formulaData2 = FormulaBase.MIN(new FormulaData[2]
      {
        this.get_L(),
        formulaData1
      });
      formulaData2.Name = (__Null) "TL ";
      FormulaData formulaData3 = FormulaData.op_Subtraction(this.get_C(), formulaData2);
      formulaData3.Name = (__Null) "BP ";
      FormulaData formulaData4 = FormulaBase.MAX(new FormulaData[3]
      {
        FormulaData.op_Subtraction(this.get_H(), this.get_L()),
        FormulaBase.ABS(FormulaData.op_Subtraction(formulaData1, this.get_H())),
        FormulaBase.ABS(FormulaData.op_Subtraction(formulaData1, this.get_L()))
      });
      formulaData4.Name = (__Null) "TR ";
      FormulaData formulaData5 = FormulaBase.MA(formulaData3, this.N1);
      formulaData5.Name = (__Null) "BPSUM1 ";
      FormulaData formulaData6 = FormulaBase.MA(formulaData3, this.N2);
      formulaData6.Name = (__Null) "BPSUM2 ";
      FormulaData formulaData7 = FormulaBase.MA(formulaData3, this.N3);
      formulaData7.Name = (__Null) "BPSUM3 ";
      FormulaData formulaData8 = FormulaBase.MA(formulaData4, this.N1);
      formulaData8.Name = (__Null) "TRSUM1 ";
      FormulaData formulaData9 = FormulaBase.MA(formulaData4, this.N2);
      formulaData9.Name = (__Null) "TRSUM2 ";
      FormulaData formulaData10 = FormulaBase.MA(formulaData4, this.N3);
      formulaData10.Name = (__Null) "TRSUM3 ";
      FormulaData formulaData11 = FormulaData.op_Addition(FormulaData.op_Addition(FormulaData.op_Multiply(FormulaData.op_Implicit(4.0), FormulaData.op_Division(formulaData5, formulaData8)), FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), FormulaData.op_Division(formulaData6, formulaData9))), FormulaData.op_Division(formulaData7, formulaData10));
      formulaData11.Name = (__Null) "RAWUO ";
      FormulaData formulaData12 = FormulaData.op_Multiply(FormulaData.op_Division(formulaData11, FormulaData.op_Implicit(7.0)), FormulaData.op_Implicit(100.0));
      formulaData12.SetAttrs("WIDTH1.6,HIGHQUALITY");
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData12
      }, "");
    }
  }
}
