using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.Helpers.Extentions
{
    public static class HttpWebRequestExtention
    {
        public static string IfModifiedSince(this DateTime obj)
        {
            return obj.ToString("ddd, dd MMM yyyy HH:mm:ss \"GMT\"");
        }

        public static DateTime IfModifiedSince(this string obj)
        {
            DateTime dateTime;
            var enUs = new System.Globalization.CultureInfo("en-US");

            // "Fri, 29 Mar 2013 23:58:07 GMT"
            if (DateTime.TryParseExact(obj, "ddd MMM dd H:mm:ss \"GMT\" yyyy", enUs, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateTime))
            {
                return dateTime;
            }
            return dateTime;
        }
    }
}
