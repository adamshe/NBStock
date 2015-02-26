using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuandlCS.Helpers
{
    public class Utility
    {
        public static string GetQuandlDateFormat (DateTime d)
        {
            return d.ToString(Constants.DateFormat);
        }
    }
}
