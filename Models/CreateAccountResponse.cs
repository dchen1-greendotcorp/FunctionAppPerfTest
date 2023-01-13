using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
    public class CreateAccountResponse : BaseResponse<CreateAccountResponseData>
    {

    }

    public class CreateAccountResponseData
    {
        public string CardID { get; set; }

        public dynamic OtherData { get; set; }
    }
}
