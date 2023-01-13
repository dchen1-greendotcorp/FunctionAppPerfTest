using MediatR;

namespace FunctionAppPerfTest.Models
{
    public class UpdateCardRegisterStatusRequest : BaseRequest, IRequest<UpdateCardRegisterStatusResponse>, IBaseRequest
    {
        public string PlasticId { get; set; }
        public dynamic OtherRequestData { get; set; }
    }

    
}
