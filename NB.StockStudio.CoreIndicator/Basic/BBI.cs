// Decompiled with JetBrains decompiler
// Type: FML.BBI
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class BBI : FormulaBase
  {
    private double N1;
    private double N2;
    private double N3;
    private double N4;

    public BBI()
    {
      base.\u002Ector();
      this.AddParam("N1", 3.0, 1.0, 100.0);
      this.AddParam("N2", 6.0, 1.0, 100.0);
      this.AddParam("N3", 12.0, 1.0, 100.0);
      this.AddParam("N4", 24.0, 1.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      return new FormulaPackage(new FormulaData[1]
      {
        FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(FormulaData.op_Addition(FormulaBase.MA(this.get_C(), this.N1), FormulaBase.MA(this.get_C(), this.N2)), FormulaBase.MA(this.get_C(), this.N3)), FormulaBase.MA(this.get_C(), this.N4)), FormulaData.op_Implicit(4.0))
      }, "");
    }
  }
}
