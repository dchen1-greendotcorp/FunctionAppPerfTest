using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using FunctionAppPerfTest.Helpers;
using FunctionAppPerfTest.Models;
using FunctionAppPerfTest.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace FunctionAppPerfTest.Triggers
{
    public class CreateAccountFn : BaseFunction<CreateAccountRequest, CreateAccountResponse>
    {
        public CreateAccountFn(ILogger logger) : base(logger)
        {

        }
        

        [FunctionName(nameof(CreateAccount))]
        [OpenApiOperation(operationId: nameof(CreateAccount), tags: new[] { "accounts" },
            Summary = "Create an account (DDA or GPR) types.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(Application.Json, typeof(CreateAccountRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Application.Json,
            bodyType: typeof(CreateAccountResponse))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Application.Json,
            bodyType: typeof(BaseResponse<>))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Application.Json,
            bodyType: typeof(BaseResponse<>))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadGateway, contentType: Application.Json,
            bodyType: typeof(BaseResponse<>))]
        public async Task<IActionResult> CreateAccount(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient context)
        {
            var result = await ExecuteAsync(nameof(GPRAccountOrchestrators.CreateGPRAccount), context);
            return new ObjectResult(result);
        }
    }
}