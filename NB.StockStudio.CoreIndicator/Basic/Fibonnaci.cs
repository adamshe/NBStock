// Decompiled with JetBrains decompiler
// Type: FML.Fibonnaci
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class Fibonnaci : FormulaBase
  {
    private double N;

    public Fibonnaci()
    {
      base.\u002Ector();
      this.AddParam("N", 100.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.HHV(this.get_H(), this.N);
      formulaData1.Name = (__Null) "A ";
      FormulaData formulaData2 = FormulaBase.LLV(this.get_L(), this.N);
      formulaData2.Name = (__Null) "B ";
      FormulaData formulaData3 = FormulaData.op_Multiply(FormulaBase.BACKSET(this.get_ISLASTBAR(), this.N), FormulaData.op_Implicit(formulaData1.get_LASTDATA()));
      formulaData3.Name = (__Null) "HH";
      formulaData3.SetAttrs("WIDTH2");
      FormulaData formulaData4 = FormulaData.op_Multiply(FormulaBase.BACKSET(this.get_ISLASTBAR(), this.N), FormulaData.op_Implicit(formulaData2.get_LASTDATA()));
      formulaData4.Name = (__Null) "LL";
      formulaData4.SetAttrs("WIDTH2");
      FormulaData formulaData5 = FormulaData.op_Subtraction(formulaData3, formulaData4);
      formulaData5.Name = (__Null) "HEIGHT ";
      FormulaData formulaData6 = FormulaData.op_Addition(formulaData4, FormulaData.op_Multiply(formulaData5, FormulaData.op_Implicit(0.382)));
      formulaData6.Name = (__Null) "A1 ";
      FormulaData formulaData7 = FormulaData.op_Addition(formulaData4, FormulaData.op_Multiply(formulaData5, FormulaData.op_Implicit(0.5)));
      formulaData7.Name = (__Null) "A2 ";
      FormulaData formulaData8 = FormulaData.op_Addition(formulaData4, FormulaData.op_Multiply(formulaData5, FormulaData.op_Implicit(0.618)));
      formulaData8.Name = (__Null) "A3 ";
      this.SETTEXTVISIBLE(formulaData3, false);
      this.SETTEXTVISIBLE(formulaData4, false);
      this.SETTEXTVISIBLE(formulaData6, false);
      this.SETTEXTVISIBLE(formulaData7, false);
      this.SETTEXTVISIBLE(formulaData8, false);
      return new FormulaPackage(new FormulaData[5]
      {
        formulaData3,
        formulaData4,
        formulaData6,
        formulaData7,
        formulaData8
      }, "");
    }
  }
}
