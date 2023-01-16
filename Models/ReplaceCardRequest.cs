using MediatR;

namespace FunctionAppPerfTest.Models
{
    public class ReplaceCardRequest : BaseRequest, IRequest<ReplaceCardResponse>, IBaseRequest
    {
        public string PlasticId { get; set; }
        public dynamic OtherRequestData { get; set; }
    }
}
