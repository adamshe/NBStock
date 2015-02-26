
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{
    public class MainArea : FormulaBase
    {
        private double N = 0.0;
        public MainArea()
        {
            base.AddParam("N", 100.0, 1.0, 100000.0);
        }
        public override FormulaPackage Run(IDataProvider dp)
        {
            this.DataProvider = dp;
            FormulaData c = base.C;
            c.Name = "MAIN ";
            c.SetAttrs("HIGHQUALITY");
            FormulaData formulaData = base.FILLAREA(c);
            formulaData.SetAttrs("BRUSH#20808000");
            base.SETTEXTVISIBLE(c, false);
            base.SETTEXTVISIBLE(false);
            return new FormulaPackage(new FormulaData[]
			{
				c, 
				formulaData
			}, "");
        }
    }
}
