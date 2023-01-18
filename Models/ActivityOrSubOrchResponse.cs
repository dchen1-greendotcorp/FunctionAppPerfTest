using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
   
    public class ActivityOrSubOrchResponse
    {
        [JsonProperty("activityName")]
        public string ActivityName { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }

    }
}
