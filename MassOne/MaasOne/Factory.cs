using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaasOne
{
    public static class Factory
    {
        static CultureInfo cultureInfo = new CultureInfo("en-US");
         public static CultureInfo DownloadCultureInfo
         {
            get 
            {
                return cultureInfo;
            }
        }
    }
}
