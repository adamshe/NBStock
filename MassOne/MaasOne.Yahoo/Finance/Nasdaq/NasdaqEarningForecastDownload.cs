using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaasOne.Base;
using MaasOne.Finance.YahooFinance;
using MaasOne.Xml;
using System.Text.RegularExpressions;
using System.Web;
using SysXml = System.Xml.Linq;
using System.Xml.XPath;
namespace MaasOne.Finance.Nasdaq
{
    #region Input
    
    public class NasdaqEarningForecastDownloadSettings : Base.SettingsBase
    {
        public string ID { get; set; }

        public NasdaqEarningForecastDownloadSettings()
        {
            this.ID = string.Empty;
        }
        public NasdaqEarningForecastDownloadSettings(string id)
        {
            this.ID = id;
        }

        protected override string GetUrl()
        {
            if (this.ID == string.Empty) { throw new ArgumentException("ID is empty.", "ID"); }
            return string.Format("http://www.nasdaq.com/symbol/{0}/earnings-forecast", this.ID);
        }

        public override object Clone()
        {
            return new NasdaqEarningForecastDownloadSettings(this.ID);
        }
    }

    #endregion

    #region Processor

    public class NasdaqEarningForecastDownload : DownloadClient<NasdaqEarningForecastResult>
    {
        #region Constructor
        
        public NasdaqEarningForecastDownload()//(string symbol)
        {
            this.Settings = new NasdaqEarningForecastDownloadSettings();
        }

        #endregion

        #region Download in Async Mode

        public void DownloadAsync(IID managedID, object userArgs)
        {
            if (managedID == null)
                throw new ArgumentException("The IID is null", "managedID");
            this.DownloadAsync(managedID.ID, userArgs);
        }
        public void DownloadAsync(string unmanagedID, object userArgs)
        {
            if (unmanagedID.Trim() == string.Empty)
                throw new ArgumentException("The ID is empty", "unmanagedID");
            this.DownloadAsync(new NasdaqEarningForecastDownloadSettings(unmanagedID), userArgs);
        }

        public void DownloadAsync(NasdaqEarningForecastDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        #endregion

        #region Download Sync Mode

        public async Task<Base.Response<NasdaqEarningForecastResult>> Download(string unmanagedID)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            return this.Download(new NasdaqEarningForecastDownloadSettings(unmanagedID));
        }

        public Response<NasdaqEarningForecastResult> Download(NasdaqEarningForecastDownloadSettings settings)
        {
            return base.Download(settings);
        }

        #endregion


        #region Property for Setting input

        public NasdaqEarningForecastDownloadSettings Settings 
        { 
            get 
            { 
                return (NasdaqEarningForecastDownloadSettings)base.Settings; 
            } 
            set 
            { 
                base.SetSettings(value); 
            }
        }

        #endregion

        #region Overrid ConvertResult

        protected override NasdaqEarningForecastResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<NasdaqEarningForecastData> yearly = new List<NasdaqEarningForecastData>(10);
            List<NasdaqEarningForecastData> quarterly = new List<NasdaqEarningForecastData>(10);
            System.Globalization.CultureInfo culture = Factory.DownloadCultureInfo;
            string pattern = @"<title>.*\((\w*)\).*</title>";
                      
            if (stream != null)            
            {
                var content = MyHelper.StreamToString(stream, System.Text.Encoding.UTF8);

                var matchPattern = "(<div class=\"genTable\">.*?</div>)";
                var match = Regex.Matches(content, matchPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

                XParseDocument year = MyHelper.ParseXmlDocument(match[0].Groups[0].Value);
                XParseDocument quarter = MyHelper.ParseXmlDocument(match[1].Groups[0].Value);


                var symbol = Regex.Match(content, pattern).Groups[1].Value;
                var resultNode = XPath.GetElement("//table", year);
                ParseTable(yearly, resultNode, symbol, "");

                resultNode = XPath.GetElement("//table", quarter);
                ParseTable(quarterly, resultNode, symbol, "");

                return new NasdaqEarningForecastResult(yearly.ToArray(), quarterly.ToArray());
            }
            return null;
        }

        private static void ParseTable(List<NasdaqEarningForecastData> yearly, XParseElement sourceNode, string symbol, string xPath)
        {
            var resultNode = sourceNode;
            if (!(string.IsNullOrWhiteSpace(xPath) || string.IsNullOrEmpty(xPath)))
                resultNode = XPath.GetElement(xPath, sourceNode);
            int cnt = 0;
            float tempVal;
            if (resultNode != null)
            {
                foreach (XParseElement node in resultNode.Elements())
                {
                    if (node.Name.LocalName == "tr")
                    {
                        cnt++;
                        if (cnt > 1) // skip row header
                        {
                            XParseElement tempNode = null;

                            var data = new NasdaqEarningForecastData();
                            data.SetID(symbol);
                            tempNode = XPath.GetElement("/td[1]", node);
                            if (tempNode != null)
                                data.FiscalEnd = HttpUtility.HtmlDecode(tempNode.Value);

                            tempNode = XPath.GetElement("/td[2]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.ConsensusEpsForecast = tempVal;

                            tempNode = XPath.GetElement("/td[3]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.HighEpsForecast = tempVal;

                            tempNode = XPath.GetElement("/td[4]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.LowEpsForecast = tempVal;

                            tempNode = XPath.GetElement("/td[5]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.NumberOfEstimate = (int)tempVal;

                            tempNode = XPath.GetElement("/td[6]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.NumOfRevisionUp = (int)tempVal;

                            tempNode = XPath.GetElement("/td[7]", node);
                            float.TryParse(tempNode.Value, out tempVal);
                            data.NumOfrevisionDown = (int)tempVal;

                            yearly.Add(data);
                        }
                    }
                }
            }
        }

        #endregion
    }

    #endregion

    #region Output

    public class NasdaqEarningForecastResult
    {
        NasdaqEarningForecastData[] m_yearlyEarningForecasts;

        public NasdaqEarningForecastData[] YearlyEarningForecasts
        {
            get { return m_yearlyEarningForecasts; }
            set { m_yearlyEarningForecasts = value; }
        }
        NasdaqEarningForecastData[] m_quarterlyEarningForecasts;

        public NasdaqEarningForecastData[] QuarterlyEarningForecasts
        {
            get { return m_quarterlyEarningForecasts; }
            set { m_quarterlyEarningForecasts = value; }
        }

        public NasdaqEarningForecastResult (NasdaqEarningForecastData[] yearly, NasdaqEarningForecastData[] quarterly)
        {
            m_yearlyEarningForecasts = yearly;
            m_quarterlyEarningForecasts = quarterly;
        }



    }

    public class NasdaqEarningForecastData : ISettableID
     {
         string m_fiscalEnd;
         float m_consensusEpsForecast;
         float m_highEpsForecast;
         float m_lowEpsForecast;
         int numberOfEstimate;

         public string FiscalEnd
         {
             get { return m_fiscalEnd; }
             set { m_fiscalEnd = value; }
         }

         public float ConsensusEpsForecast
         {
             get { return m_consensusEpsForecast; }
             set { m_consensusEpsForecast = value; }
         }

         public float HighEpsForecast
         {
             get { return m_highEpsForecast; }
             set { m_highEpsForecast = value; }
         }

         public float LowEpsForecast
         {
             get { return m_lowEpsForecast; }
             set { m_lowEpsForecast = value; }
         }

         public int NumberOfEstimate
         {
             get { return numberOfEstimate; }
             set { numberOfEstimate = value; }
         }
         /// <summary>
         /// Over the last 4 weeks Number of Revision Up
         /// </summary>
         int numOfRevisionUp;

         public int NumOfRevisionUp
         {
             get { return numOfRevisionUp; }
             set { numOfRevisionUp = value; }
         }

         /// <summary>
         /// /// Over the last 4 weeks Number of Revision Down
         /// </summary>
         int numOfrevisionDown;

         public int NumOfrevisionDown
         {
             get { return numOfrevisionDown; }
             set { numOfrevisionDown = value; }
         }

         string m_id; //Symbol
         public void SetID(string id)
         {
             m_id = id;
         }

         public string ID
         {
             get { return m_id; }
         }
     }

     #endregion
}
