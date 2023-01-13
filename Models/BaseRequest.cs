using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
   
    public class BaseRequest
    {
        [JsonProperty("requestId")] public string RequestId { get; set; } = string.Empty;
    }
}
