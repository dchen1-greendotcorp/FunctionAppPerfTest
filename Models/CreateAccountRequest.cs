using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
    public class CreateAccountRequest : BaseRequest, IRequest<CreateAccountResponse>, IBaseRequest
    {
        public string AccountId { get; set; }
        public dynamic OtherRequestData { get; set; }
    }
}
