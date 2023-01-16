using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
    public class GetClearTextPanResponse : BaseResponse<ClearTextPan>
    {

    }

    public class ClearTextPan
    {
        public string Pan { get; set; }
    }
}
