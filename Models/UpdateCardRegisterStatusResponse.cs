namespace FunctionAppPerfTest.Models
{
    public class UpdateCardRegisterStatusResponse: BaseResponse<UpdateCardRegisterStatusResponseData>
    {

    }
    public class UpdateCardRegisterStatusResponseData
    {
        public string PlasticId { get; set; }

        public dynamic OtherData { get; set; }
    }
}
