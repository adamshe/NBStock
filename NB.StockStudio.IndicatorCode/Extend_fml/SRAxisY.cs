// Decompiled with JetBrains decompiler
// Type: FML.Extend.SRAxisY
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class SRAxisY : FormulaBase
  {
    public SRAxisY()
    {
      base.\u002Ector();
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = FormulaData.op_Division(FormulaData.op_Addition(FormulaData.op_Addition(this.get_H(), this.get_L()), this.get_C()), FormulaData.op_Implicit(3.0));
      formulaData1.Name = (__Null) "M ";
      FormulaData formulaData2 = FormulaData.op_Subtraction(this.get_H(), this.get_L());
      formulaData2.Name = (__Null) "A ";
      FormulaData formulaData3 = FormulaData.op_Addition(formulaData1, formulaData2);
      formulaData3.Name = (__Null) "RR";
      FormulaData formulaData4 = FormulaData.op_Subtraction(formulaData1, formulaData2);
      formulaData4.Name = (__Null) "SS";
      FormulaData formulaData5 = this.DRAWAXISY(formulaData3, -10.0, 12.0);
      formulaData5.Name = (__Null) "R ";
      formulaData5.SetAttrs("WIDTH2,COLOR#A0FF0000,AXISMARGIN12");
      FormulaData formulaData6 = this.DRAWAXISY(formulaData4, -10.0, 12.0);
      formulaData6.Name = (__Null) "S ";
      formulaData6.SetAttrs("WIDTH2,COLOR#A0004000");
      FormulaData formulaData7 = this.DRAWTEXTAXISY(formulaData3, "R", 1.0);
      formulaData7.SetAttrs("COLOR#FF0000,VCENTER");
      FormulaData formulaData8 = this.DRAWTEXTAXISY(formulaData4, "S", 1.0);
      formulaData8.SetAttrs("COLOR#004000,VCENTER");
      this.SETNAME("SR");
      return new FormulaPackage(new FormulaData[4]
      {
        formulaData5,
        formulaData6,
        formulaData7,
        formulaData8
      }, "");
    }
  }
}
