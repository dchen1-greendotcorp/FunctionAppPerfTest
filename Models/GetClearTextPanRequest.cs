using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
    public class GetClearTextPanRequest : BaseRequest, IRequest<GetClearTextPanResponse>, IBaseRequest
    {
        public dynamic OtherData { get; set; }
    }
}
