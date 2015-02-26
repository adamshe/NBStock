// Decompiled with JetBrains decompiler
// Type: FML.MIKE
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class MIKE : FormulaBase
  {
    private double N;

    public MIKE()
    {
      base.\u002Ector();
      this.AddParam("N", 12.0, 1.0, 200.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(this.get_HIGH(), this.get_LOW()), this.get_CLOSE()), FormulaData.op_Implicit(3.0));
      formulaData1.Name = (__Null) "TYP";
      FormulaData formulaData2 = FormulaBase.LLV(this.get_LOW(), this.N);
      formulaData2.Name = (__Null) "LL";
      FormulaData formulaData3 = FormulaBase.HHV(this.get_HIGH(), this.N);
      formulaData3.Name = (__Null) "HH";
      FormulaData formulaData4 = FormulaData.op_Addition(formulaData1, FormulaData.op_Subtraction(formulaData1, formulaData2));
      formulaData4.Name = (__Null) "WR";
      FormulaData formulaData5 = FormulaData.op_Addition(formulaData1, FormulaData.op_Subtraction(formulaData3, formulaData2));
      formulaData5.Name = (__Null) "MR";
      FormulaData formulaData6 = FormulaData.op_Subtraction(FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData3), formulaData2);
      formulaData6.Name = (__Null) "SR";
      FormulaData formulaData7 = FormulaData.op_Subtraction(formulaData1, FormulaData.op_Subtraction(formulaData3, formulaData1));
      formulaData7.Name = (__Null) "WS";
      FormulaData formulaData8 = FormulaData.op_Subtraction(formulaData1, FormulaData.op_Subtraction(formulaData3, formulaData2));
      formulaData8.Name = (__Null) "MS";
      FormulaData formulaData9 = FormulaData.op_Subtraction(FormulaData.op_Multiply(FormulaData.op_Implicit(2.0), formulaData2), formulaData3);
      formulaData9.Name = (__Null) "SS";
      return new FormulaPackage(new FormulaData[6]
      {
        formulaData4,
        formulaData5,
        formulaData6,
        formulaData7,
        formulaData8,
        formulaData9
      }, "");
    }
  }
}
