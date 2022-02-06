using ApiTestAutomation;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using RestAPIBase;
using RestSharpAPITest;
using RestAPIBase.Reporting;


namespace RestSharpAPITest
{
    class RESTfulAPIRequestHandler : JSONFormatting
    {
        public string jsonRequestString = String.Empty;
        public string jsonFolderName = "JsonSamples";
        public Dictionary<string, dynamic> jsonDictionaryObj = new Dictionary<string, dynamic>();
        public Dictionary<string, dynamic> jsonResultObj = new Dictionary<string, dynamic>();
        public ApiHandler apiHandlerObject = new ApiHandler();
        public string resultString = String.Empty;
        private readonly Dictionary<string, string> _urlParameters = new Dictionary<string, string>();
        private readonly List<string> _removeOptionalQueryParameters = new List<string>();
        const string C_GETAPPCONFIGVALUEFOR_ = "GetAppConfigValueFor_";
        const string C_GETSTOREDVALUEFOR_ = "GetStoredValueFor_";
        const string C_GETAPPCONFIGWITHENVIRONMENTVALUEFOR_ = "GetAppConfigWithEnvironmentValueFor_";
        private string _environment = ConfigurationManager.AppSettings["Environment"];
        private static Random _random = new Random();
        private string folderName = "deviceIDinstanceID";
    


[Obsolete]
        public void RemoveValuesFromJsonObject(Table table, string messageType)
        {
            if (!ScenarioContext.Current.ContainsKey("UpdateFlag"))
            {
                SetJsonStringInGlobalVariable(messageType);
                SetDeserialisedJsonDictionaryObject();
            }

            jsonDictionaryObj = RemoveFromRequestMessage(table, jsonDictionaryObj);
            if (ScenarioContext.Current.ContainsKey("UpdateFlag"))
            {
                ScenarioContext.Current.Remove("UpdateFlag");
            }

            jsonRequestString = GetSerialisedJsonString(jsonDictionaryObj);
        }

        private void SetJsonStringInGlobalVariable(string messageType)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var jsonSampleString = StoreJsonSampleString(rootPath, jsonFolderName, messageType.ToLower() + ".json");
            jsonSampleString.Should().NotBeNullOrEmpty();
            jsonRequestString = jsonSampleString;
        }

        private void SetDeserialisedJsonDictionaryObject()
        {
            jsonDictionaryObj = DeserialiseJsonStringIntoDictionary(jsonRequestString);
        }

        public void SendRequestMessage()
        {
            var httpRequest = GetHttpRequestMessage();
            if (RESTfulAPICallDetails.RequiresJsonBody)
            {
                Console.WriteLine("Actual SendRequestMessage json request string: " + jsonRequestString);
                ExtentReportsHandler.Info("Actual SendRequestMessage json request string: " + jsonRequestString);
            }
            resultString = apiHandlerObject.SendRequestMessage(httpRequest, jsonRequestString);
            jsonResultObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(resultString);
        }

        private HttpWebRequest GetHttpRequestMessage()
        {
            HttpWebRequest httpRequest;

            httpRequest = (HttpWebRequest)WebRequest.Create(GetURLForWebRESTfulRequest());
            httpRequest.Method = RESTfulAPICallDetails.Verb;
            httpRequest.ContentType = "application/json";
            //if (authorized)
            //{
            //    if (!FeatureContext.Current.ContainsKey("TokenProviderToken"))
            //    {
            //        this.GenerateTokenFromTokenProviderAPIAsync();
            //    }
            //    httpRequest.Headers.Add("Authorization", "Bearer " + GetStoredContextVariable("TokenProviderToken"));
            //}

            if (!RESTfulAPICallDetails.RequiresJsonBody)
                httpRequest.ContentLength = 0;

            return httpRequest;
        }

        private string GetURLForWebRESTfulRequest()
        {
            //   var url = ConfigurationManager.AppSettings[string.Format("URL_{0}", ConfigurationManager.AppSettings["Environment"].ToUpper())];
            var url = "http://rahulshettyacademy.com";
            if (_urlParameters.Count > 0)
            {
                url = url + UpdateUrlWithQueryParameters(RESTfulAPICallDetails.URL);
            }
            else
                url = url + RESTfulAPICallDetails.URL;

            if (_removeOptionalQueryParameters.Count > 0)
            {
                url = QueryParametersHandler.RemoveParameters(url, _removeOptionalQueryParameters);
            }

            Console.WriteLine("URL is: " + url);
            ExtentReportsHandler.Info("URL is: " + url);

            return url;
        }


        private string UpdateUrlWithQueryParameters(string url)
        {
            string result = url;

            foreach (KeyValuePair<string, string> parameter in _urlParameters)
            {
                result = result.Replace("{" + parameter.Key + "}", parameter.Value);
            }

            return result;
        }

        internal void ValidateResponseMessage(Table table, bool expected = true)
        {
            apiHandlerObject.ValidateNoErrorResponse(table, resultString, expected);
            //  apiHandlerObject.ValidateNoErrorResponse(table, jsonRequestString, expected);
            Console.WriteLine("Response is: " + resultString);
            ExtentReportsHandler.Info("Expected Response is: " + table);
            ExtentReportsHandler.Info("Response is: " + resultString);
        }
        internal void ValidateResponseStatusCode(int statusCode)
        {
            HttpStatusCode httpStatusCode = apiHandlerObject.GetStatusCode();

            int actualStatusCode = (int)httpStatusCode;

            try
            {
                actualStatusCode.Should().Be(statusCode);
                ExtentReportsHandler.Info(string.Format("Expected Status Code : {0} and actual Status Code : {1}", statusCode, actualStatusCode));
            }
            catch (Exception)
            {
                ExtentReportsHandler.Fail(string.Format("Expected Status Code : {0} but actual Status Code : {1}", statusCode, actualStatusCode));
                throw;
            }
        }
        internal void SetupRESTfulRequestInformation(string verb, string actionMethod)
        {
            _urlParameters.Clear();
            _removeOptionalQueryParameters.Clear();

            RESTfulAPICallDetails.Verb = verb.ToUpper();
            RESTfulAPICallDetails.URL = RESTfulURLsHandler.GetURLForAPICall(actionMethod, verb.ToUpper());
            RESTfulAPICallDetails.RequiresJsonBody = false;
            RESTfulAPICallDetails.ActionMethod = actionMethod.ToUpper();
            ExtentReportsHandler.Info("Http method is " + verb + " and the endpoint is " + actionMethod);
        }
        [Obsolete]
        internal void SetupJsonFile(string jsonFileName, Table table)
        {
            RESTfulAPICallDetails.RequiresJsonBody = true;
            UpdateJsonObjectWithNewValues(table, jsonFileName);
        }
        [Obsolete]
        internal void UpdateJsonObjectWithNewValues(Table table, string requestType)
        {
            StoreSampleStringFromJSON(requestType);
            SetDeserialisedJsonDictionaryObject();
            table = UpdateTableForStoredValues(table);
            jsonDictionaryObj = JSONFormatting.UpdateJsonObject(table, jsonDictionaryObj);
            jsonDictionaryObj = SetAppConfigPlaceHolderValues(table, jsonDictionaryObj);
            jsonRequestString = GetSerialisedJsonString(jsonDictionaryObj);
            if (ScenarioContext.Current.ContainsKey("UpdateFlag"))
            {
                ScenarioContext.Current.Remove("UpdateFlag");
            }

            ScenarioContext.Current.Add("UpdateFlag", jsonRequestString);
            Console.WriteLine("Generated Request Message: " + jsonRequestString);
        }

        internal void StoreSampleStringFromJSON(string requestType)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            jsonRequestString = StoreJsonSampleString(rootPath, jsonFolderName, requestType + ".json");
            jsonRequestString.Should().NotBeNullOrEmpty();
        }

        internal void SetupQueryParameters(Table table)
        {
            bool includeEnvironmentValue;
            string appConfigSettingToFind;
            _urlParameters.Clear();

            try
            {
                foreach (var row in table.Rows)
                {
                    var field = row["Field"];
                    var value = row["Value"];

                    if (value.StartsWith("GetStoredValueFor_"))
                    {
                        string storedValueName = value.Replace("GetStoredValueFor_", String.Empty);

                        value = GetStoredContextVariable(storedValueName);
                    }
                   
                    else if (value.StartsWith(C_GETAPPCONFIGVALUEFOR_) || value.StartsWith(C_GETAPPCONFIGWITHENVIRONMENTVALUEFOR_))
                    {
                        if (value.Contains("Environment"))
                        {
                            includeEnvironmentValue = true;
                            appConfigSettingToFind = value.Replace(C_GETAPPCONFIGWITHENVIRONMENTVALUEFOR_, string.Empty);
                        }
                        else
                        {
                            includeEnvironmentValue = false;
                            appConfigSettingToFind = value.Replace(C_GETAPPCONFIGVALUEFOR_, string.Empty);
                        }

                        value = GetValueFromAppConfig(appConfigSettingToFind, includeEnvironmentValue);
                    }

                    _urlParameters.Add(field, value);
                    StoreFeatureContextVariable(field, value);
                    Console.WriteLine(_urlParameters);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error when setting up query parameters");
                ExtentReportsHandler.Fail("Error when setting up query parameters");
                throw;
            }
        }

        private string GetStoredContextVariable(string name)
        {
            return FeatureContext.Current[name].ToString();
        }
        [Obsolete]
        internal void StoreFeatureContextVariable(string name)
        {
            var nameValue = jsonResultObj[name].ToString();
            if (FeatureContext.Current.ContainsKey(name))
            {
                FeatureContext.Current.Remove(name);
            }
            FeatureContext.Current.Add(name, nameValue);
        }
      
        private string GenerateUniqueKeycode(int length)
        {
            const string pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string result = string.Empty;
            var temp = string.Empty;

            while (result.Length < length)
            {
                temp = result.Length == 4 ? "TEST" : pool[_random.Next(pool.Length)].ToString();

                result = result + temp;
            }
            StoreFeatureContextVariable("licenseKey", result);
            return result;
        }
        internal void SetupOptionalQueryParametersToBeRemoved(Table table)
        {
            _removeOptionalQueryParameters.Clear();

            try
            {
                foreach (var row in table.Rows)
                {
                    var field = row["Field"];
                    _removeOptionalQueryParameters.Add(field);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error when setting up optional query parameters removal list");
                ExtentReportsHandler.Fail("Error when setting up optional query parameters removal list");
                throw;
            }
        }

        internal void SetupOptionalQueryParametersToBeRemovedBySeparator(char separator, Table table)
        {
            _removeOptionalQueryParameters.Clear();

            try
            {
                List<string> parameters = table.Rows[0]["Field"].Split(separator).ToList<string>();

                foreach (var parameter in parameters)
                {
                    _removeOptionalQueryParameters.Add(parameter.Trim());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error when setting up optional query parameters removal list");
                ExtentReportsHandler.Fail("Error when setting up optional query parameters removal list");
                throw;
            }
        }

        private Dictionary<string, dynamic> SetAppConfigPlaceHolderValues(Table table, Dictionary<string, dynamic> jsonDictionaryObj)
        {
            bool includeEnvironmentValue;
            string appConfigSettingToFind;

            try
            {
                foreach (var row in table.Rows)
                {
                    var field = row["Field"];
                    var value = row["Value"];

                    if (value.StartsWith(C_GETAPPCONFIGVALUEFOR_) || value.StartsWith(C_GETAPPCONFIGWITHENVIRONMENTVALUEFOR_))
                    {
                        if (value.Contains("Environment"))
                        {
                            includeEnvironmentValue = true;
                            appConfigSettingToFind = value.Replace(C_GETAPPCONFIGWITHENVIRONMENTVALUEFOR_, string.Empty);
                        }
                        else
                        {
                            includeEnvironmentValue = false;
                            appConfigSettingToFind = value.Replace(C_GETAPPCONFIGVALUEFOR_, string.Empty);
                        }

                        UpdatePlaceHolderObjectsInDeserialisedDictionary(field, GetValueFromAppConfig(appConfigSettingToFind, includeEnvironmentValue), jsonDictionaryObj);
                    }
                }

                return jsonDictionaryObj;
            }
            catch (Exception)
            {
                Console.WriteLine("Error when working out if a json body is required");
                ExtentReportsHandler.Fail("Error when working out if a json body is required");
                throw;
            }
        }

        private string GetValueFromAppConfig(string appConfigSettingToFind, bool includeEnvironmentValue = false)
        {
            string settingNameToGet = includeEnvironmentValue ? appConfigSettingToFind + "_" + GetEnvironment() : appConfigSettingToFind;
            string valueFromAppConfig = ConfigurationManager.AppSettings[settingNameToGet];

            if (valueFromAppConfig == null)
            {
                Console.WriteLine("Error : Null returned when attempting to get app.config setting for " + settingNameToGet);
                ExtentReportsHandler.Error("Error : Null returned when attempting to get app.config setting for " + settingNameToGet);
            }

            return valueFromAppConfig;
        }

        private string GetEnvironment()
        {
            return ConfigurationManager.AppSettings["Environment"].ToUpper();
        }

        internal Table UpdateTableForStoredValues(Table table)
        {
            string valueToFind;
            string storedValue;
            var newTable = table;

            try
            {
                foreach (var row in table.Rows)
                {
                    var value = row["Value"];
                    if (value.StartsWith(C_GETSTOREDVALUEFOR_))
                    {
                        valueToFind = value.Replace(C_GETSTOREDVALUEFOR_, string.Empty);
                        storedValue = GetStoredContextVariable(valueToFind);
                        row["Value"] = storedValue;
                    }
                }

                return newTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when updating table with stored values. " + ex.Message);
                ExtentReportsHandler.Fail("Error when updating table with stored values. " + ex.Message);
                throw;
            }
        }
     internal void PassExpiredTokenProviderToken()
        {
            string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IlNreUlkZW50aXR5LVFBL0lkZW50aXR5U2VydmVyU2lnbmluZ0tleSIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MzU3ODgyODQsImV4cCI6MTYzNTc5MTg4NCwiaXNzIjoiaHR0cHM6Ly9za3lpZGVudGl0eS1xYS53ZWJyb290Y2xvdWRhdi5jb20iLCJhdWQiOlsiaHR0cHM6Ly9za3lpZGVudGl0eS1xYS53ZWJyb290Y2xvdWRhdi5jb20vcmVzb3VyY2VzIiwiU2VydmljZXMuU2t5LlNreURldmljZUluZm8iXSwiY2xpZW50X2lkIjoiU2t5RGV2aWNlSW5mb1FBIiwic2NvcGUiOlsiU2VydmljZXMuU2t5LlNreURldmljZUluZm8uRGVsZXRlIiwiU2VydmljZXMuU2t5LlNreURldmljZUluZm8uRnVsbC5SZWFkIiwiU2VydmljZXMuU2t5LlNreURldmljZUluZm8uRnVsbC5VcGRhdGUiLCJTZXJ2aWNlcy5Ta3kuU2t5RGV2aWNlSW5mby5SZWFkIiwiU2VydmljZXMuU2t5LlNreURldmljZUluZm8uVXBkYXRlIl19.Jr-DMLSnu_OrkvamL4iRuQZLOx-yUi_s-W-g33h1UUjyTM9zlU-c7dKna7CcYogJ_TDL_ST0JOl52sKPPFt5Z8wcKebrUKqxl7iclY0B6FdYstOtFuDzwdaagCbRnr_77GvZ8Yox2ihZvitF5qdwP2qJSi9WxEDJGvSwpoSRoYdFKdysQdmf23Q8UnQAfSYXi8WcbqJWDiyg7-BbAsVvzTLtM1MXwWM86ktuid7TF9jADnDbjOJSQ6-qfoDzEXTy_8qKKhltRZOrrVkEdbwlRdWXP3l85lseQ2lrArX2E8F5hJnA-PE1r7kQVUcU9WyK5D8DHiRSKcmM3OSnsDnWVQ";
            StoreFeatureContextVariable("TokenProviderToken", token);
        }
        internal void StoreFeatureContextVariable(string name, string nameValue)
        {
            if (FeatureContext.Current.ContainsKey(name))
            {
                FeatureContext.Current.Remove(name);
            }
            FeatureContext.Current.Add(name, nameValue);
        }
    }
}
