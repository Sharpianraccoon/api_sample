using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using SpecFlowProject.StepDefinitions;

namespace Specflow.Sample.Support
{
    public class RestHelpers
    {
        public T? DeserializeJsonResponse<T>(RestResponse restResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(restResponse.Content ?? "", options);
        }
    }
}
