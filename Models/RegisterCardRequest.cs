using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static FunctionAppPerfTest.Models.CreateAccountRequest;

namespace FunctionAppPerfTest.Models
{
    public class RegisterCardRequest : BaseRequest, IRequest<RegisterCardResponse>, IBaseRequest
    {
        public string CardId { get; set; }

        public dynamic OtherData { get; set; } 

    }
}
