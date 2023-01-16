using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
    public class ReplaceCardResponse : BaseResponse<ReplaceCardExternalResponse>
    {
    }

    public class ReplaceCardExternalResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("subErrorMessage")]
        public string SubErrorMessage { get; set; }
    }
}
