
using NB.StockStudio.Foundation;
using NB.StockStudio.Foundation.DataProvider;

namespace FML.EXTEND
{

        public class SRAxisY : FormulaBase
        {
            public override FormulaPackage Run(IDataProvider dp)
            {
                this.DataProvider = dp;
                FormulaData formulaData = (base.H + base.L + base.C) / 3.0;
                formulaData.Name = "M ";
                FormulaData formulaData2 = base.H - base.L;
                formulaData2.Name = "A ";
                FormulaData formulaData3 = formulaData + formulaData2;
                formulaData3.Name = "RR";
                FormulaData formulaData4 = formulaData - formulaData2;
                formulaData4.Name = "SS";
                FormulaData formulaData5 = base.DRAWAXISY(formulaData3, -10.0, 12.0);
                formulaData5.Name = "R ";
                formulaData5.SetAttrs("WIDTH2,COLOR#A0FF0000,AXISMARGIN12");
                FormulaData formulaData6 = base.DRAWAXISY(formulaData4, -10.0, 12.0);
                formulaData6.Name = "S ";
                formulaData6.SetAttrs("WIDTH2,COLOR#A0004000");
                FormulaData formulaData7 = base.DRAWTEXTAXISY(formulaData3, "R", 1.0);
                formulaData7.SetAttrs("COLOR#FF0000,VCENTER");
                FormulaData formulaData8 = base.DRAWTEXTAXISY(formulaData4, "S", 1.0);
                formulaData8.SetAttrs("COLOR#004000,VCENTER");
                base.SETNAME("SR");
                return new FormulaPackage(new FormulaData[]
			{
				formulaData5, 
				formulaData6, 
				formulaData7, 
				formulaData8
			}, "");
            }
        }

}
