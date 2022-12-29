using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPITest
{
    public static class RESTfulAPICallDetails
    {
        public static string URL;
        public static string Verb;
        public static bool RequiresJsonBody;
        public static Dictionary<string, string> URLParameters;
        public static string ActionMethod;
        public static bool ValidateURL()
        {
            bool result = false;
            if (String.IsNullOrWhiteSpace(URL))
            {
            }
            else
            {
                if (URL.Contains("{")  || URL.Contains("}"))
                        result = false;
            }
            return result;
        }
    }
}
