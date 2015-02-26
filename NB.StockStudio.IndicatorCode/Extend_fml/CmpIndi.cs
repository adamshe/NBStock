// Decompiled with JetBrains decompiler
// Type: FML.Extend.CmpIndi
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class CmpIndi : FormulaBase
  {
    private string STOCKCODE;
    private string INDI;

    public CmpIndi(): base()
    {
      this.AddParam("StockCode", "^DJI", "0", "0");
      this.AddParam("Indi", "RSI(14)", "0", "0");
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData1 = this.FML(this.INDI);
      formulaData1.Name = (__Null) "V1";
      formulaData1.SetAttrs("HIGHQUALITY");
      FormulaData formulaData2 = this.FML(this.STOCKCODE, this.INDI);
      formulaData2.Name = (__Null) "V2";
      formulaData2.SetAttrs("HIGHQUALITY");
      this.SETNAME(formulaData1, this.get_STKLABEL());
      this.SETNAME(formulaData2, this.STOCKCODE);
      this.SETNAME(this.INDI);
      return new FormulaPackage(new FormulaData[2]
      {
        formulaData1,
        formulaData2
      }, "");
    }
  }
}
