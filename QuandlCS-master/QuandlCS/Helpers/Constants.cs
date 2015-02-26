using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuandlCS.Helpers
{
  internal static class Constants
  {
    #region Addresses 

      internal static class ApiAddress
      {
          internal const string APIMultisetsAddress = "http://www.quandl.com/api/v1/multisets";
          internal const string APIDatasetsAddress = "http://www.quandl.com/api/v1/datasets";
          internal const string APIDatasetsImportAddress = "http://www.quandl.com/api/v1/datasets/import";
          internal const string APIFavouritesAddress = "http://www.quandl.com/api/v1/current_user/collections/datasets/favourites";
      }

    #endregion

    #region Variables

      internal static class ApiProperty
      {
          internal const string APIColumns = "columns=";
          internal const string APIAuthorization = "auth_token=";
          internal const string APISortOrder = "sort_order=";
          internal const string APIStartDate = "trim_start=";
          internal const string APIEndDate = "trim_end=";
          internal const string APITransformation = "transformation=";
          internal const string APIFrequency = "collapse=";
          internal const string APITruncation = "rows=";
          internal const string APIExcludeHeader = "exclude_headers=";
          internal const string APIExcludeData = "exclude_data=";
          internal const string APIQuery = "query=";
      }

      #endregion

      #region Transformation

      internal static class Transformation
      {
          const string DIFF = "diff";   // y'[t] = y[t] - y[t-1];
          const string RDIFF = "rdiff"; // y'[t] = (y[t]-y[t-1])/y[t-1]
          const string CUMUL = "cumul"; // y'[t] = y[t] + y[t-1] ... y[0]
          const string NORMALIZE = "normalize"; // y'[t] = (y[t]/y[0]) * 100
      }


      #endregion

      #region Frequency

      internal static class Frequency
      {
          const string NONE = "none";
          const string DAILY ="daily";
          const string WEEKLY = "weekly";
          const string MONTHLY = "monthly";
          const string QUARTERLY = "quarterly";
          const string ANNUAL = "annual";
      }

      #endregion

      #region DataFormat

      internal static class DataFormat
      {
          internal const string CSV = ".csv";
          internal const string JSON = ".json";
          internal const string XML = ".xml";
      }

      #endregion

      internal const string APIKEY = "Q4Kpynx_S2pWYVB4N49J";
      internal const string DateFormat = "yyyy-mm-dd";
  }
}
