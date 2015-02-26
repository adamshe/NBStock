using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.StockStudio.Foundation.Valuation
{
	public static class FairValue
	{
		public static double GeomSeries (double startGrowthRate, int factor, int years)
		{
			double amount = 0;
			if (startGrowthRate == 1.0)
				amount = years + 1;
			else
				amount = (Math.Pow(startGrowthRate, years + 1) - 1) / (startGrowthRate - 1);

			if (factor >= 1)
				amount -= GeomSeries(startGrowthRate, 0, factor - 1);

			return amount;
		}

		public static double ReturnCompoundedAnnualRate(double presentValue, double futureValue, int year)
		{
			//compounded annual return
			return Math.Pow(futureValue / presentValue, 1.0 / year) - 1.0;
		}

		public static double FutureValue(double presentValue, double growthRate, int year)
		{
			return presentValue * Math.Pow(1 + growthRate, year);
		}

		public static double PresentValue(double futureValue, double interestRate, int year)
		{
			return futureValue * Math.Pow(1 + interestRate, -year);
		}

		public static double DiscountedCurrentValue (double eps, int years, double growthRate, double inflationRate, double bondRate )
		{
			var earningCashOfYears = eps * GeomSeries((1 + growthRate) / (1 + bondRate), 1, years);
			var v2 = FutureValue(eps, growthRate, years) * (1 + inflationRate) / (bondRate - inflationRate);
			var v = earningCashOfYears + PresentValue(v2, bondRate, years);
			return Math.Round(v, 2);
		}

		public static double CalculateDiscountRate (double percentReturnOfRiskFreeInvestment, double percentReturnOfBenchMarkInvestment, double beta)
		{
			return percentReturnOfRiskFreeInvestment + beta * (percentReturnOfBenchMarkInvestment - percentReturnOfRiskFreeInvestment);
		}
	}


}

/*
 * function doCalcDiscountRate()
{
	zeroBlanks(document.mainform);

	var Rf = numval(document.mainform.Rf.value);
	var Km = numval(document.mainform.Km.value);
	var beta = numval(document.mainform.beta.value);

	if (Km <= Rf)
	{
		formErrMsg("Error! This calculation only makes sense if the market benchmark gives a greater return than the risk-free benchmark.", document.mainform.Km);
		return;
	}

	var Kc = Rf + (Km - Rf)*beta;
	
	document.mainform.Kc.value = formatNumber(Kc, 2);
}


 * 
 function doCalcValuation()
{
	if (document.mainform.eps.value == "")
	{
		document.mainform.eps.focus();
		return;
	}	

	if (document.mainform.g1.value == "") document.mainform.g1.value = "0";

	var eps = numval(document.mainform.eps.value);
	var y1 = numval(document.mainform.y1.value); // how many years of growth
	var g1 = numval(document.mainform.g1.value)/100.0; // annual growth rate
	var g2 = numval(document.mainform.g2.value)/100.0; //  annual growth rate after y1 
	var Kc = numval(document.mainform.Kc.value)/100.0; // bond rate 
	var c = numval(document.mainform.c.value)/100.0; //confidence

	if (g2 >= Kc)
	{
		formErrMsg("Error! The long-term growth rate needs to be less than the discount rate.", document.mainform.g2);
		return;
	}

	var v1 = c*eps*geomSeries((1 + g1)/(1 + Kc), 1, y1);
	var v2 = futureValue(c*eps, g1, y1)*(1 + g2)/(Kc - g2);
	var v = v1 + presentValue(v2, Kc, y1);

	document.mainform.v.value = formatNumber(v, 2);
}

 */