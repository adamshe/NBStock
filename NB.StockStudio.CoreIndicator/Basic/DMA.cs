// Decompiled with JetBrains decompiler
// Type: FML.DMA
// Assembly: Basic_fml, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9284717-8834-4B37-BC08-9823918209B3
// Assembly location: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio\bin\Debug\dllReference\formula\Basic_fml.dll

using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML
{
    public class DMA : FormulaBase
    {
        private double SHORT = 0.0;
        private double LONG = 0.0;
        private double M = 0.0;
        public DMA()
        {
            base.AddParam("SHORT", 10.0, 2.0, 300.0);
            base.AddParam("LONG", 50.0, 2.0, 300.0);
            base.AddParam("M", 10.0, 1.0, 300.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData formulaData = FormulaBase.MA(base.CLOSE, this.SHORT) - FormulaBase.MA(base.CLOSE, this.LONG);
            formulaData.Name = "DDD ";
            FormulaData formulaData2 = FormulaBase.MA(formulaData, this.M);
            formulaData2.Name = "AMA ";
            return new FormulaPackage(new FormulaData[]
			{
				formulaData, 
				formulaData2
			}, "");
        }
    }
}
