// Decompiled with JetBrains decompiler
// Type: FML.Extend.Compare
// Assembly: Extend_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 80B8D39B-C1FC-4273-9C0E-5AD2F813E720
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Extend_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.Extend
{
  public class Compare : FormulaBase
  {
    private string STOCKCODE;

    public Compare(): base()
    {
      this.AddParam("StockCode", "^DJI", "0", "0");
    }

    public virtual FormulaPackage Run(IDataProvider dp)
    {
      this.DataProvider = (__Null) dp;
      FormulaData formulaData = this.FML(this.STOCKCODE, "C");
      formulaData.SetAttrs("FIRSTDATAOFVIEW,HIGHQUALITY");
      this.SETNAME(this.STOCKCODE);
      return new FormulaPackage(new FormulaData[1]
      {
        formulaData
      }, "");
    }
  }
}
