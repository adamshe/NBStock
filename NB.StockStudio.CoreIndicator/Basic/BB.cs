// Decompiled with JetBrains decompiler
// Type: FML.BB
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class BB : FormulaBase
  {
    private double N;
    private double P;

    public BB()
    {
      this.AddParam("N", 20.0, 5.0, 300.0);
      this.AddParam("P", 2.0, 0.1, 10.0);
    }

    public override FormulaPackage Run(IDataProvider dp)
    {
        this.DataProvider = dp;
        FormulaData formulaData = FormulaBase.MA(base.CLOSE, this.N);
        formulaData.Name = "MID ";
        FormulaData formulaData2 = formulaData + this.P * FormulaBase.STD(base.CLOSE, this.N);
        formulaData2.Name = "UPPER";
        FormulaData formulaData3 = formulaData - this.P * FormulaBase.STD(base.CLOSE, this.N);
        formulaData3.Name = "LOWER";
        return new FormulaPackage(new FormulaData[]
			{
				formulaData, 
				formulaData2, 
				formulaData3
			}, "");
    }
  }
}
