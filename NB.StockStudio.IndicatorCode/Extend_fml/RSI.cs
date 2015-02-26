// Decompiled with JetBrains decompiler
// Type: FML.Extend.RSI
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class RSI : FormulaBase
  {
    private double N1;

    public RSI()
    {
      base.\u002Ector();
      this.AddParam("N1", 14.0, 2.0, 100.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaBase.REF(this.get_CLOSE(), 1.0);
      formulaData1.Name = (__Null) "LC ";
      FormulaData formulaData2 = FormulaData.op_Multiply(FormulaData.op_Division(FormulaBase.SMA(FormulaBase.MAX(new FormulaData[2]
      {
        FormulaData.op_Subtraction(this.get_CLOSE(), formulaData1),
        FormulaData.op_Implicit(0.0)
      }), this.N1, 1.0), FormulaBase.SMA(FormulaBase.ABS(FormulaData.op_Subtraction(this.get_CLOSE(), formulaData1)), this.N1, 1.0)), FormulaData.op_Implicit(100.0));
      formulaData2.Name = (__Null) "RSI";
      FormulaData formulaData3 = FormulaData.op_Implicit(70.0);
      formulaData3.SetAttrs("HIGHSPEED");
      FormulaData formulaData4 = FormulaData.op_Implicit(30.0);
      formulaData4.SetAttrs("HIGHSPEED");
      FormulaData formulaData5 = this.FILLRGN(FormulaData.op_GreaterThan(formulaData2, FormulaData.op_Implicit(70.0)), formulaData2, FormulaData.op_Implicit(70.0));
      formulaData5.SetAttrs("BRUSH#20808000");
      FormulaData formulaData6 = this.FILLRGN(FormulaData.op_LessThan(formulaData2, FormulaData.op_Implicit(30.0)), formulaData2, FormulaData.op_Implicit(30.0));
      formulaData6.SetAttrs("BRUSH#20800000");
      return new FormulaPackage(new FormulaData[5]
      {
        formulaData2,
        formulaData3,
        formulaData4,
        formulaData5,
        formulaData6
      }, "");
    }
  }
}
