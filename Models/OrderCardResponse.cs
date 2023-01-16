using Newtonsoft.Json;

namespace FunctionAppPerfTest.Models
{
    public class OrderCardResponse : BaseResponse<OrderCardResponseData>
    {
    }
    public class OrderCardResponseData
    {
        public string CardID { get; set; }
        public CreateAccountRequest CreateAccountRequest { get; set; }

    }
    
}