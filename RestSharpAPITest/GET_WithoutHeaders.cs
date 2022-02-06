using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;

namespace RestSharpAPITest
{
    public class GET_WithoutHeaders
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var client = new RestClient("http://www.omdbapi.com");
            var request = new RestRequest("/?apikey=9c7073c4&t=star+is+born&y=2018&type=movie", Method.GET);
            var response = client.Execute(request);
            var deserialize = new JsonDeserializer();
            var JSONObj = deserialize.Deserialize<Dictionary<string, string>>(response);
            Console.WriteLine(JSONObj["Writer"]);
            Assert.That(JSONObj["Writer"] == "Eric Roth, Bradley Cooper, Will Fetters");
        }
    }
}