using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.Helpers.Extentions
{
    public static class StringExtention
    {
        public static string StringSplit(this string obj, string splitValue, params object[] parameters)
        {
            var str = "";
            if (parameters.Length == 1)
            {
                if (parameters[0] != null)
                {
                    return parameters[0].ToString();
                }
            }
            else if (parameters.Length > 0)
            {
                int i = 0;
                for (; i < parameters.Length - 1; i++)
                {
                    if (parameters[i] != null)
                    {
                        str += parameters[i] + splitValue;
                    }
                }
                if (parameters[i] != null)
                {
                    str += parameters[i];
                }
            }

            return str;
        }

        public static string EncodeTo64(this string toEncode)
        {

            byte[] toEncodeAsBytes

                  = System.Text.Encoding.Unicode.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }

        public static string DecodeFrom64(this string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.Encoding.Unicode.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Count());

            return returnValue;

        }

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }
    }
}
