using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Specflow.Sample.Contexts
{
    public class DomainContext
    {
        public RestClient RestClient;
        public RestRequest RestRequest;
        public RestResponse RestResponse;
        public int Count;
        public string? Name;
        public int Age;
    }
}
