using MediatR;

namespace FunctionAppPerfTest.Models
{
    public class OrderCardRequest : BaseRequest, IRequest<OrderCardResponse>, IBaseRequest
    {
        public string PlasticId { get; set; }
        public dynamic OtherRequestData { get; set; }
    }
}
