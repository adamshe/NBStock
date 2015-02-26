// Decompiled with JetBrains decompiler
// Type: FML.LWR
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class LWR : FormulaBase
  {
    private double N;
    private double M1;
    private double M2;

    public LWR()
    {
      base.\u002Ector();
      this.AddParam("N", 9.0, 1.0, 100.0);
      this.AddParam("M1", 3.0, 2.0, 40.0);
      this.AddParam("M2", 3.0, 2.0, 40.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(FormulaBase.HHV(this.get_HIGH(), this.N), this.get_CLOSE()), FormulaData.op_Subtraction(FormulaBase.HHV(this.get_HIGH(), this.N), FormulaBase.LLV(this.get_LOW(), this.N))), FormulaData.op_Implicit(100.0));
      formulaData1.Name = (__Null) "RSV";
      FormulaData formulaData2 = FormulaBase.SMA(formulaData1, this.M1, 1.0);
      formulaData2.Name = (__Null) "LWR1";
      FormulaData formulaData3 = FormulaBase.SMA(formulaData2, this.M2, 1.0);
      formulaData3.Name = (__Null) "LWR2";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
