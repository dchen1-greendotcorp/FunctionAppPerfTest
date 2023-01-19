using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionAppPerfTest.Helpers;
using FunctionAppPerfTest.Models;
using FunctionAppPerfTest.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace FunctionAppPerfTest.Triggers
{
    public class OrderCardFn : BaseFunction<OrderCardRequest, OrderCardResponse>
    {
        public OrderCardFn(ILogger logger) : base(logger)
        {

        }


        [FunctionName(nameof(OrderCard))]
        [OpenApiOperation(operationId: nameof(OrderCard), tags: new[] { "ordercard" },
            Summary = "order card in ACI..", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(Application.Json, typeof(OrderCardRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Application.Json,
            bodyType: typeof(OrderCardResponse))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Application.Json,
            bodyType: typeof(BaseResponse<>))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Application.Json,
            bodyType: typeof(BaseResponse<>))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadGateway, contentType: Application.Json,
            bodyType: typeof(BaseResponse<>))]
        public async Task<IActionResult> OrderCard(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage request,
            [DurableClient] IDurableOrchestrationClient context)
        {
            var result = await ExecuteAsync(nameof(OrderCardOrchestrators.OrderCardOrchestrator), context);
            return new ObjectResult(result);
        }
    }
}