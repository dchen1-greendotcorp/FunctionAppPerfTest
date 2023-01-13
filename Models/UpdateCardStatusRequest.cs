using MediatR;

namespace FunctionAppPerfTest.Models
{
    public class UpdateCardStatusRequest: BaseRequest, IRequest<UpdateCardStatusResponse>, IBaseRequest
    {
        public string PlasticId { get; set; }
        public dynamic OtherRequestData { get; set; }
    }
}
