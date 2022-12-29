using RestAPIBase.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPITest
{

    public static class RestfulURLsHandler
    { // 
        private const string GET_VERB = "GET";
        private const string PUT_VERB = "PUT";
        private const string POST_VERB = "POST";
        private const string DELETE_VERB = "DELETE";

        private static List<Tuple<string, string, string>> URLs = new List<Tuple<string, string, string>>()
        {
            Tuple.Create(GET_VERB,"getPetsByStatus","v2/pet/findByStatus"),
             Tuple.Create(GET_VERB,"getPetsByID","v2/pet/")         
        };
        public static string GETURLForAPICalls(string actionMethod , string verb)
        {
            string result = string.Empty;
            try
            {
                result = URLs.FirstOrDefault(s => s.Item1.ToUpper() == verb.ToUpper() && s.Item2.ToUpper() == actionMethod.ToUpper()).Item3;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Error when trying to get URl for verb :{0} , actionMethod : {1} ",verb,actionMethod));
                ExtentReportsHandler.Error(String.Format("Error when trying to get URl for verb :{0} , actionMethod : {1} ", verb, actionMethod));
                throw;
            }
        }
    }
}
