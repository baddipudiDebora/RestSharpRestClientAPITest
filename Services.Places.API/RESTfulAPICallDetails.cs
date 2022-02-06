using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPITest
{
  public  static class RESTfulAPICallDetails
    {
        public static string Verb;
        public static bool RequiresJsonBody;
        public static Dictionary<string, string> URLParamters;
        public static string URL;
        public static string ActionMethod;

        public static bool ValidateURL()
        {
            bool result = true;

            if (String.IsNullOrWhiteSpace(URL))
            {
                result = false;
            }
            else
            {
                if (URL.Contains("{") || URL.Contains("}"))
                    result = false;
            }

            return result;
        }
    }
}
