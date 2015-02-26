// Decompiled with JetBrains decompiler
// Type: FML.LinRegr
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class LinRegr : FormulaBase
  {
    private double N;
    private double P;

    public LinRegr()
    {
      base.\u002Ector();
      this.AddParam("N", 14.0, 1.0, 1000.0);
      this.AddParam("P", 100.0, 0.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.LR(this.get_C(), this.N);
      formulaData1.Name = (__Null) "A ";
      FormulaData formulaData2 = FormulaData.op_Subtraction(this.get_C(), formulaData1);
      formulaData2.Name = (__Null) "DIST ";
      FormulaData formulaData3 = FormulaData.op_Implicit(FormulaBase.MAX(new double[2]
      {
        FormulaBase.MAXVALUE(formulaData2),
        FormulaBase.MINVALUE(formulaData2)
      }) * this.P / 100.0);
      formulaData3.Name = (__Null) "M  ";
      FormulaData formulaData4 = FormulaData.op_Addition(formulaData1, formulaData3);
      formulaData4.Name = (__Null) "UPPER ";
      FormulaData formulaData5 = FormulaData.op_Subtraction(formulaData1, formulaData3);
      formulaData5.Name = (__Null) "LOWER ";
      FormulaData formulaData6 = formulaData1;
      this.SETNAME(formulaData1, "");
      this.SETTEXTVISIBLE(formulaData4, false);
      this.SETTEXTVISIBLE(formulaData5, false);
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData4,
        formulaData5,
        formulaData6
      }, "");
    }
  }
}
