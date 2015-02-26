// Decompiled with JetBrains decompiler
// Type: FML.WVAD
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class WVAD : FormulaBase
  {
    private double N1;
    private double N2;

    public WVAD()
    {
      base.\u002Ector();
      this.AddParam("N1", 10.0, 1.0, 100.0);
      this.AddParam("N2", 20.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(this.get_CLOSE(), this.get_OPEN()), FormulaData.op_Subtraction(this.get_HIGH(), this.get_LOW())), this.get_VOL());
      formulaData1.Name = (__Null) "WVAD ";
      FormulaData formulaData2 = FormulaBase.MA(formulaData1, this.N1);
      formulaData2.Name = (__Null) "MA1";
      FormulaData formulaData3 = FormulaBase.MA(formulaData1, this.N2);
      formulaData3.Name = (__Null) "MA2";
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData1,
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
