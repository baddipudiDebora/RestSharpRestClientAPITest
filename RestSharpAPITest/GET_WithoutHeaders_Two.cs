using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;

namespace RestSharpAPITest
{
    public class GET_WithoutHeaders_Two
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var client = new RestClient("http://rahulshettyacademy.com");
            var request = new RestRequest("/maps/api/place/get/json?key=qaclick123&place_id=eefb40a1eaca7e4fdb9c990ac7a130a5", Method.GET);
            var response = client.Execute(request);
            var deserialize = new JsonDeserializer();
            var JSONObj = deserialize.Deserialize<Dictionary<string, string>>(response);
            Console.WriteLine(JSONObj["name"]);
            Assert.That(JSONObj["name"] == "Frontline house");
        }
    }
}