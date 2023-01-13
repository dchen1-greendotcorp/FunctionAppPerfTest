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
    public class PerfFunction : BaseFunction<CreateAccountRequest, CreateAccountResponse>
    {
        public PerfFunction(ILogger logger) : base(logger)
        {

        }
        [FunctionName("Function")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
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
            //var request = await req.Content.ReadAsAsync<CreateAccountRequest>();

            //var orchestrationId = $"{request.Header.ProgramCode}.{request.GetType().Name}.{request.Header.RequestId}";


            //// Function input comes from the request content.
            //string instanceId = await starter.StartNewAsync("Function", null);

            //log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            //return starter.CreateCheckStatusResponse(req, instanceId);


            var result = await ExecuteAsync(nameof(GPRAccountOrchestrators.CreateGPRAccount), context);
            return new ObjectResult(result);
        }
    }
}