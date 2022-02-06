using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using RestSharpAPITest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPITest
{
    internal class POST_WithoutHeaders_WithAnonymousClass
    {
        [Test]
        public void Test1()
        {
            string _jsonData = "{\n"
+ "   \"location\":{\n"
+ "      \"lat\":-58.387494,\n"
+ "      \"lng\":53.827362\n"
+ "   },\n"
+ "   \"accuracy\":50,\n"
+ "   \"name\":\"Frontline house 123\",\n"
+ "   \"phone_number\":\"(+91) 983 893 3937\",\n"
+ "   \"address\":\"77, side layout, cohen 09\",\n"
+ "   \"types\":[\n"
+ "      \"shoe park\",\n"
+ "      \"shop\"\n"
+ "   ],\n"
+ "   \"website\":\"http://google.com\",\n"
+ "   \"language\":\"French-IN\"\n"
+ "}";
            var client = new RestClient("https://rahulshettyacademy.com/maps/api/place/add/json");
            var request = new RestRequest("?key= qaclick123", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(_jsonData);

            var response = client.Execute(request);
            var deserialize = new JsonDeserializer();
            var JSONObj = deserialize.Deserialize<Dictionary<string, string>>(response);
            Console.WriteLine(JSONObj.ToString());



        }
    }
}