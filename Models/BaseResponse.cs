using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
 
    [ExcludeFromCodeCoverage]
    public class BaseResponse<T> : BaseResponse
    {
        [JsonProperty("data")] public T Data { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class BaseResponse
    {
        
    }
}
