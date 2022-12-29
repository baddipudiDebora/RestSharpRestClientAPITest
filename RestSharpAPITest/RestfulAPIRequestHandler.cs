using NUnit.Framework;
using RestAPIBase.Reporting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;


namespace RestSharpAPITest
{
    internal class RestfulAPIRequestHandler
    {
        private readonly Dictionary<string, string> _urlParameters = new Dictionary<string, string>();
        private string _resourceParameters;
        RestClient restclient;
        RestRequest _request;
        RestResponse _response;
        internal void SetupRestFulRequetInformation(string verb, string actionMethod)
        {
            RESTfulAPICallDetails.Verb = verb.ToUpper();
            _resourceParameters = RESTfulAPICallDetails.URL = RestfulURLsHandler.GETURLForAPICalls(actionMethod, verb.ToUpper()).ToString();
            if (verb == "GET")
            {
                RESTfulAPICallDetails.RequiresJsonBody = false;
            }
            else
            {
                RESTfulAPICallDetails.RequiresJsonBody = true;

            }
            // Rest Client
            restclient = new RestClient("https://petstore.swagger.io");
            _request = new RestRequest(_resourceParameters.ToString(), Method.Get);
            Console.WriteLine("Resource parameters aadded is " + _resourceParameters.ToString());
        }
        internal void SetUpQueryParameters(Table table)
        {
            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];
                _request.AddQueryParameter(field, value);
                Console.WriteLine("URL Segment added is " + field + " , " + value);
                Console.WriteLine(_request);

            }
        }

        internal void SendRequestMessage()
        {
            try
            {
                 _response = restclient.Execute(_request);
                Console.WriteLine("The formed URL for the endpoint is " + _response.ResponseUri);
                var content = restclient.Execute(_request).Content;
                Console.WriteLine("Response from the endpoint is " + content);
              
            }
            catch (Exception ex)
            {
                ExtentReportsHandler.Fail("Failed to send the request succesfully");

            }

        }

        public void ValidateResponceStatusCode(string expectedStatuscode)
        {
            try
            {
                Assert.That(_response.StatusCode.ToString().Contains(expectedStatuscode));
                ExtentReportsHandler.Pass(_response.ToString());
                Console.WriteLine("Response Status code is: " + _response.StatusCode.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected Response Status code is: " + _response.StatusCode.ToString());
                ExtentReportsHandler.Fail(_response.ToString());
            }

        }
        //public void ValidateResponseMessage(Table table, bool expected = true)
        //{
        //    apiHandlerObject.ValidateNoErrorResponse(table, resultString, expected);
        //    Console.WriteLine("Response is: " + resultString);
        //}
    }
}
