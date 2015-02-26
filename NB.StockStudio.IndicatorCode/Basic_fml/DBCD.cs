// Decompiled with JetBrains decompiler
// Type: FML.DBCD
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class DBCD : FormulaBase
  {
    private double N;
    private double M;
    private double T;

    public DBCD()
    {
      base.\u002Ector();
      this.AddParam("N", 5.0, 1.0, 100.0);
      this.AddParam("M", 16.0, 1.0, 100.0);
      this.AddParam("T", 76.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Subtraction(this.get_C(), FormulaBase.MA(this.get_C(), this.N)), FormulaBase.MA(this.get_C(), this.N));
      formulaData1.Name = (__Null) "BIAS";
      FormulaData formulaData2 = FormulaData.op_Subtraction(formulaData1, FormulaBase.REF(formulaData1, this.M));
      formulaData2.Name = (__Null) "DIF";
      FormulaData formulaData3 = FormulaBase.SMA(formulaData2, this.T, 1.0);
      formulaData3.Name = (__Null) "DBCD";
      FormulaData formulaData4 = FormulaBase.MA(formulaData3, 5.0);
      formulaData4.Name = (__Null) "MM";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData3,
        formulaData4
      }, "");
    }
  }
}
