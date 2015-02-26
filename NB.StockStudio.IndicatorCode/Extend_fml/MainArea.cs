// Decompiled with JetBrains decompiler
// Type: FML.Extend.MainArea
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class MainArea : FormulaBase
  {
    private double N;

    public MainArea()
    {
      base.\u002Ector();
      this.AddParam("N", 100.0, 1.0, 100000.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData c = this.get_C();
      c.Name = (__Null) "MAIN ";
      c.SetAttrs("HIGHQUALITY");
      FormulaData formulaData = this.FILLAREA(c);
      formulaData.SetAttrs("BRUSH#20808000");
      this.SETTEXTVISIBLE(c, false);
      this.SETTEXTVISIBLE(false);
      return new FormulaPackage(new FormulaData[2]
      {
        c,
        formulaData
      }, "");
    }
  }
}
