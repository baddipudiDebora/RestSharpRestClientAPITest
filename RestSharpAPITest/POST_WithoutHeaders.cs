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
    internal class POST_WithoutHeaders
    {
        Location l = new Location();
        public Rootobject ro()
        {
            l.lat = 89.78f;
            l.lng = -98.67f;
            new Rootobject() { location = { lat = l.lat, lng = l.lng }, accuracy = 50, name = "Frontline house 456", Address = "77, side layout, cohen 09", Types = new string[] { "shoe park", "shop" }, Website = "http://google.com", Language = "French-IN" };
            return new Rootobject();
        }
        [Test]
        public void Test1()
        {
            var client = new RestClient("https://rahulshettyacademy.com/maps/api/place/add/json");
            var request = new RestRequest("?key= qaclick123", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(ro());

            var response = client.Execute(request);
            var deserialize = new JsonDeserializer();
            var JSONObj = deserialize.Deserialize<Dictionary<string, string>>(response);
            Console.WriteLine(JSONObj["place_id"]);
            Assert.That(JSONObj["place_id"] == "ecfea18285fb5c0340cb73b3d07bdd21");
        }

    }
}