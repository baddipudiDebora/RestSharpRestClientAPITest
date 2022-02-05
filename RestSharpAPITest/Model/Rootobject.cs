using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPITest.Model
{
    public class Rootobject  
    {

        public Location location { get; set; }
        public int accuracy { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string Address { get; set; }
        public string[] Types { get; set; }
        public string Website { get; set; }
        public string Language { get; set; }
    }

    public class Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }


}

