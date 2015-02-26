// Decompiled with JetBrains decompiler
// Type: FML.CMF
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class CMF : FormulaBase
  {
    private double N;

    public CMF()
    {
      base.\u002Ector();
      this.AddParam("N", 20.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(FormulaData.op_Subtraction(this.get_CLOSE(), this.get_LOW()), FormulaData.op_Subtraction(this.get_HIGH(), this.get_CLOSE())), FormulaData.op_Subtraction(this.get_HIGH(), this.get_LOW())), this.get_VOL());
      formulaData1.Name = (__Null) "AD";
      FormulaData formulaData2 = FormulaData.op_Division(FormulaBase.MA(formulaData1, this.N), FormulaBase.MA(this.get_VOL(), this.N));
      formulaData2.Name = (__Null) "CMF";
      formulaData2.SetAttrs("COLORSTICK");
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData2
      }, "");
    }
  }
}
