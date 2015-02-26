// Decompiled with JetBrains decompiler
// Type: FML.BBWidth
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class BBWidth : FormulaBase
  {
    private double N;
    private double P;

    public BBWidth()
    {
      base.\u002Ector();
      this.AddParam("N", 20.0, 1.0, 100.0);
      this.AddParam("P", 2.0, 0.1, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData = FormulaData.op_Multiply(FormulaData.op_Multiply(FormulaData.op_Implicit(this.P), FormulaBase.STD(this.get_C(), this.N)), FormulaData.op_Implicit(2.0));
      formulaData.SetAttrs("WIDTH1.6,HIGHQUALITY");
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData
      }, "");
    }
  }
}
