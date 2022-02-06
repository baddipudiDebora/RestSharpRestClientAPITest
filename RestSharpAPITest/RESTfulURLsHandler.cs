using System;
using System.Collections.Generic;
using System.Linq;
using RestAPIBase.Reporting;

namespace RestSharpAPITest
{
    public static class RESTfulURLsHandler
    {
        private const string GET_VERB = "GET";
        private const string DELETE_VERB = "DELETE";
        private const string POST_VERB = "POST";
        private const string PUT_VERB = "PUT";
        private const string PATCH_VERB = "PATCH";

        private static List<Tuple<string, string, string>> URLs = new List<Tuple<string, string, string>>()
        {
            // Verb, Action, URL
            Tuple.Create(GET_VERB, "GET_PLACEINFO", "/maps/api/place/get/json?key={key}&place_id={placeID}")


        };

        public static string GetURLForAPICall(string actionMethod, string verb)
        {
            string result = string.Empty;

            try
            {
                result = URLs.FirstOrDefault(s => s.Item1.ToUpper() == verb.ToUpper() && s.Item2.ToUpper() == actionMethod.ToUpper()).Item3;

                return result;
            }
            catch (Exception)
            {
                Console.WriteLine(String.Format("Error when trying to get url for verb : {0}, action method : {1} ", verb, actionMethod));
                ExtentReportsHandler.Error(String.Format("Error when trying to get url for verb : {0}, action method : {1} ", verb, actionMethod));
                throw;
            }
        }
    }
}
