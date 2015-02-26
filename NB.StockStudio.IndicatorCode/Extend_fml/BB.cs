// Decompiled with JetBrains decompiler
// Type: FML.Extend.BB
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class BB : FormulaBase
  {
    private double N;
    private double P;

    public BB()
    {
      base.\u002Ector();
      this.AddParam("N", 26.0, 5.0, 300.0);
      this.AddParam("P", 2.0, 0.1, 10.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.MA(this.get_CLOSE(), this.N);
      formulaData1.Name = (__Null) "MID ";
      FormulaData formulaData2 = FormulaData.op_Addition(formulaData1, FormulaData.op_Multiply(FormulaData.op_Implicit(this.P), FormulaBase.STD(this.get_CLOSE(), this.N)));
      formulaData2.Name = (__Null) "UPPER";
      formulaData2.SetAttrs("COLOR#8080C0");
      FormulaData formulaData3 = FormulaData.op_Subtraction(formulaData1, FormulaData.op_Multiply(FormulaData.op_Implicit(this.P), FormulaBase.STD(this.get_CLOSE(), this.N)));
      formulaData3.Name = (__Null) "LOWER";
      formulaData3.SetAttrs("COLOR#8080C0");
      FormulaData formulaData4 = this.FILLRGN(FormulaData.op_Implicit(1.0), formulaData3, formulaData2);
      formulaData4.SetAttrs("BRUSH#200000C0");
      return new FormulaPackage(new FormulaData[3]
      {
        formulaData2,
        formulaData3,
        formulaData4
      }, "");
    }
  }
}
