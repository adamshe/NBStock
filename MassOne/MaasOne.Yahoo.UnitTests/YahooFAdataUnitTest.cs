using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MaasOne.Finance.YahooFinance;
using MaasOne.Base;
using System.Threading.Tasks;
using NB.StockStudio.Foundation.Valuation;
using MaasOne.Finance.Nasdaq;
using System.Linq;
namespace MaasOne.Yahoo.UnitTests
{
    [TestClass]
    public class YahooFAdataUnitTest
    {
        double inflation = 0.02;
        double bondRate = 0.1;
        IEnumerable<string> ids = new string[] {  
            "BIDU"
          /*  "GILD", "PCYC", "AMGN", "BIIB", "JAZZ", "REGN",
            "JPM", "GS", "MS", "BAC", "C", "MCO"
            "AAPL","MSFT","GOOG","AMZN","CSCO", "QCOM",
            "FB","LNKD","TWTR","NFLX", "PCLN",
            "QIHU","BIDU","JD","JMEI","VIPS","BITA","HTHT", "CTRP",
            "ICPT", "TKMR", "ALNY", "JAZZ",
            "TSLA", "SCTY", "LNG",
            "LOCO", "CMG", "MNST",
            "BA",
            "WYNN", "LVS"
           * */
        };

        #region Yahoo Download
        
        [TestMethod]
        public void CalculateFairValue ()
        {
            var dl = new CompanyStatisticsDownload();
            Parallel.ForEach(ids, id =>
            {
                var resp = dl.Download(id);
                if (resp.Result.Connection.State == ConnectionState.Success)
                {
                    var item = resp.Result.Result.Item;
                    var ti = item.TradingInfo;
                    var vm = item.ValuationMeasures;
                    var highlight = item.FinancialHighlights;
                    var eps = highlight.DilutedEPS;
                   // var growthRate1 = vm.TrailingPE / vm.ForwardPE;
                    var growthRate = highlight.QuarterlyRevenueGrowthPercent;//.QuaterlyEarningsGrowthPercent /100.0;
                    //var outStandingShare = vm.MarketCapitalisationInMillion / highlight.RevenuePerShare.

                    var fairValue = FairValue.DiscountedCurrentValue(eps, 3, growthRate/100.0, inflation, bondRate);
                    if (eps <= 0 && fairValue <= 0)
                        fairValue = FairValue.FutureValue(highlight.RevenuePerShare , growthRate/100.0, 1)  * 1.5;
                    Console.WriteLine("symbol:{0} forward P/E : {1} EV/Rev : {2} - Margin: {3} ShortPercentage : {4}  EPS: {5}  GrowthRate: {6} FairValue : {7}", id, vm.ForwardPE, vm.EnterpriseValueToRevenue, highlight.ProfitMarginPercent, ti.ShortPercentOfFloat, eps, growthRate, fairValue);
                }
            });
            
        }
        [TestMethod]
        public void DownloadCompanyTradingInformation()
        {
            var dl = new CompanyStatisticsDownload();
            var option = new ParallelOptions();
            option.MaxDegreeOfParallelism = Environment.ProcessorCount * 2;
            option.TaskScheduler = TaskScheduler.Default;
            Parallel.ForEach(ids, option,  id =>
            {
                var resp = dl.Download(id);
                if (resp.Result.Connection.State == ConnectionState.Success)
                {
                    var item = resp.Result.Result.Item;
                    var ti = item.TradingInfo;
                    var vm = item.ValuationMeasures;
                    var high = item.FinancialHighlights;
                    
                    Console.WriteLine("symbol:{0} forward P/E : {1} EV/Rev : {2} - Margin: {3} ShortPercentage : {4} ", id, vm.ForwardPE, vm.EnterpriseValueToRevenue, high.ProfitMarginPercent, ti.ShortPercentOfFloat);
                }
            });
        }

        [TestMethod]
        public void DownloadCompanyStatistics()
        {
            var dl = new CompanyStatisticsDownload();
            Parallel.ForEach(ids, id =>
            {
                var resp = dl.Download(id);
                if (resp.Result.Connection.State == ConnectionState.Success)
                {
                    var result = resp.Result.Result;
                    var item = result.Item;
                    var vm = item.ValuationMeasures;
                    var high = item.FinancialHighlights;
                    Console.WriteLine("symbol:{0} forward P/E : {1} EV/Rev : {2} - Margin: {3}", id, vm.ForwardPE, vm.EnterpriseValueToRevenue, high.ProfitMarginPercent);
                }
            });
        }

        [TestMethod]
        public void DownloadCompanyInfo()
        {           
           
            //Download
           CompanyInfoDownload dl = new CompanyInfoDownload();
           var resp = dl.Download(ids);

            //Response/Result
            if (resp.Connection.State == ConnectionState.Success)
            {
                foreach (CompanyInfoData info in resp.Result.Items)
                {
                    string id = info.ID;
                    string name = info.Name;
                    int employees = info.FullTimeEmployees;
                    System.DateTime start = info.StartDate;
                    string industry = info.IndustryName;
                    

                }
            }
        }
        [TestMethod]
        public void DownloadCompanyFundamentals()
        {
            //Parameters
           
            IEnumerable<QuoteProperty> properties = new QuoteProperty[] {
                QuoteProperty.Symbol,
                QuoteProperty.Name,
                QuoteProperty.BidRealtime,
                QuoteProperty.EPSEstimateCurrentYear,
                QuoteProperty.EPSEstimateNextYear,
                QuoteProperty.PERatioRealtime,
                QuoteProperty.ShortRatio,
                QuoteProperty.PEGRatio,
                QuoteProperty.OneyrTargetPrice,
                QuoteProperty.MarketCapRealtime,
                QuoteProperty.LastTradePriceOnly,
                QuoteProperty.YearHigh,
                QuoteProperty.PreviousClose,
        };

            //Download
            QuotesDownload dl = new QuotesDownload();
            var resp = dl.Download(ids, properties);

            //Response/Result
            if (resp.Connection.State == MaasOne.Base.ConnectionState.Success)
            {
                string id ;
                string Symbol;
                string name ;
                double bidRealtime ;
                double ePSEstimateCurrentYear;
                double epsNextYear ;
                double peg;
                double oneYearTargetprice;
                double lastPrice ;
                double yearHigh ;
                double previousClose;
                foreach (var qd in resp.Result.Items)
                {
                     id = qd.ID;
                     Symbol = qd[QuoteProperty.Symbol].ToString();
                     name = qd.Name;
                     bidRealtime = Convert.ToDouble(qd[QuoteProperty.BidRealtime]);
                     double.TryParse(qd[QuoteProperty.EPSEstimateCurrentYear].ToString(), out ePSEstimateCurrentYear);
                     double.TryParse(qd[QuoteProperty.EPSEstimateNextYear].ToString(), out epsNextYear);
                     double.TryParse(qd[QuoteProperty.PEGRatio].ToString(), out peg);
                     double.TryParse(qd[QuoteProperty.OneyrTargetPrice].ToString(), out oneYearTargetprice);
                     lastPrice = qd.LastTradePriceOnly;
                     yearHigh = Convert.ToDouble(qd[QuoteProperty.YearHigh]);
                     previousClose = Convert.ToDouble(qd[QuoteProperty.PreviousClose]);
                    Console.WriteLine("Symbol {0}, lastPrice {1:C}, OneYearTarget {2:C} , EPSNextYear {3:C}, Growth {4:p2}"
                        , 
                        Symbol, lastPrice, oneYearTargetprice, epsNextYear, (epsNextYear / ePSEstimateCurrentYear));

                }
            }
        }
        #endregion

        #region Nasdaq Parse

        [TestMethod]//79708
        public void TestNasdaqEarningForecast ()
        {
            var dl = new NasdaqEarningForecastDownload();
            //var setting = new NasdaqEarningForecastDownloadSettings("NFLX");
               var resp = dl.Download("NFLX");
                if (resp.Result.Connection.State == ConnectionState.Success)
                {
                    var result = resp.Result.Result;
                    Console.WriteLine("Symbol {0}", dl.Settings.ID);
                    foreach (var yearly in result.YearlyEarningForecasts)
                    {
                        Console.WriteLine("year {0} - consensus eps {1} ", yearly.FiscalEnd, yearly.ConsensusEpsForecast);
                    }
                    foreach (var quarterly in result.QuarterlyEarningForecasts)
                    {
                        Console.WriteLine("year {0} - consensus eps {1} ", quarterly.FiscalEnd, quarterly.ConsensusEpsForecast);
                    }

                }
        }

        #endregion
    }

}
