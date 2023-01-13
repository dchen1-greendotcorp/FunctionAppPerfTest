namespace FunctionAppPerfTest.Models
{
    public class RegisterCardResponse: BaseResponse<RegisterCardResponseData>
    {
    }
    public class RegisterCardResponseData
    {
        public string CardID { get; set; }
        public CreateAccountRequest CreateAccountRequest { get; set; }

    }
}