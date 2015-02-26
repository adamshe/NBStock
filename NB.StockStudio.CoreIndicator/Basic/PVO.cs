// Decompiled with JetBrains decompiler
// Type: FML.PVO
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class PVO : FormulaBase
  {
    private double N1;
    private double N2;
    private double N3;

    public PVO()
    {
      base.\u002Ector();
      this.AddParam("N1", 12.0, 1.0, 100.0);
      this.AddParam("N2", 26.0, 1.0, 100.0);
      this.AddParam("N3", 9.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.EMA(this.get_V(), this.N1);
      formulaData1.Name = (__Null) "E1 ";
      FormulaData formulaData2 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaData.op_Subtraction(formulaData1, FormulaBase.EMA(this.get_V(), this.N2)), formulaData1), FormulaData.op_Implicit(100.0));
      formulaData2.Name = (__Null) "PVO ";
      formulaData2.SetAttrs("WIDTH1.6,HIGHQUALITY");
      FormulaData formulaData3 = FormulaBase.EMA(formulaData2, this.N3);
      formulaData3.Name = (__Null) "M ";
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData2,
        formulaData3
      }, "");
    }
  }
}
