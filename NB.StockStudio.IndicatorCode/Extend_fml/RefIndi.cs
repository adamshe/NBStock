// Decompiled with JetBrains decompiler
// Type: FML.Extend.RefIndi
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class RefIndi : FormulaBase
  {
    private string INDI;
    private double N;

    public RefIndi()
    {
      base.\u002Ector();
      this.AddParam("Indi", "MACD[DIFF]", "0", "0");
      this.AddParam("N", 10.0, 1.0, 10000.0);
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData = FormulaBase.REF(this.FML(this.INDI), this.N);
      this.SETNAME(this.INDI + (object) "-" + (string) (object) this.N);
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData
      }, "");
    }
  }
}
