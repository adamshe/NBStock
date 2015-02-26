// Decompiled with JetBrains decompiler
// Type: FML.VOLMA
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
  public class VOLMA : FormulaBase
  {
    private double M1;

    public VOLMA()
    {
        base.AddParam("M1", 60.0, 1.0, 100.0);
    }
    public override FormulaPackage Run(IDataProvider dp)
    {
        this.DataProvider = dp;
        FormulaData v = base.V;
        v.Name = "VV";
        v.SetAttrs("VOLSTICK");
        base.SETNAME(v, "");
        FormulaData formulaData = FormulaBase.MA(v, this.M1);
        formulaData.Name = "MA1";
        base.SETNAME(formulaData, "MA");
        return new FormulaPackage(new FormulaData[]
			{
				v, 
				formulaData
			}, "");
    }
  }
}
