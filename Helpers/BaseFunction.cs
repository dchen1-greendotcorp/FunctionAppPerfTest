using FunctionAppPerfTest.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppPerfTest.Helpers
{
    public abstract class BaseFunction<ORequest, OResponse> : IFunctionInvocationFilter
    where ORequest : BaseRequest
    where OResponse : BaseResponse
    {

        protected readonly ILogger Logger;
        private readonly TimeSpan _orchestrationWaitTime;
        private ORequest _request;
        private HttpRequestMessage _rawRequestMessage;
        private IDurableOrchestrationClient _durableOrchestrationClient;

        protected ORequest Request
        {
            get
            {
                if (_request == null) throw new ArgumentNullException(nameof(Request));
                return _request;
            }
            private set => _request = value;
        }

        protected HttpRequestMessage RawRequestMessage
        {
            get
            {
                if (_rawRequestMessage == null) throw new ArgumentNullException(nameof(RawRequestMessage));
                return _rawRequestMessage;
            }
            private set => _rawRequestMessage = value;
        }

        protected IDurableOrchestrationClient DurableOrchestrationClient
        {
            get
            {
                if (_durableOrchestrationClient == null)
                    throw new ArgumentNullException(nameof(DurableOrchestrationClient));
                return _durableOrchestrationClient;
            }
            set => _durableOrchestrationClient = value;
        }


        protected BaseFunction(ILogger logger, TimeSpan? functionTimeout = null)
        {
            Logger = logger;
            _orchestrationWaitTime = functionTimeout ?? TimeSpan.FromSeconds(30);
        }

        protected virtual async Task<OResponse> ExecuteOrchestration(ORequest request, string orchestrationName)
        {
            var orchestrationId = $"{request.RequestId}";
            var status = await DurableOrchestrationClient.GetStatusAsync(orchestrationId);

            switch (status?.RuntimeStatus)
            {
                case OrchestrationRuntimeStatus.ContinuedAsNew: //The orchestration has restarted itself with a new history. This state is a transient state.
                case OrchestrationRuntimeStatus.Pending: //The orchestration was scheduled but has not yet started.
                case OrchestrationRuntimeStatus.Running: //The orchestration is running (it may be actively running or waiting for input).
                    return await WaitForCompletion(orchestrationId);
                case OrchestrationRuntimeStatus.Failed: //The orchestration failed with an error.
                case OrchestrationRuntimeStatus.Canceled: //The orchestration was canceled.
                case OrchestrationRuntimeStatus.Terminated: //The orchestration was terminated via an API call.
#pragma warning disable CS0618
                    await DurableOrchestrationClient.RewindAsync(orchestrationId, "Restart a failed orchestration.");
#pragma warning restore CS0618
                    return await WaitForCompletion(orchestrationId);
                case OrchestrationRuntimeStatus.Completed: //The orchestration ran to completion.
                    return ExtractOrchestrationResult(status);
                case OrchestrationRuntimeStatus.Suspended: //The orchestration was suspended
                    await DurableOrchestrationClient.ResumeAsync(orchestrationId,
                        $"Resume a {status.RuntimeStatus} orchestration.");
                    return await WaitForCompletion(orchestrationId);
                default:
                    await DurableOrchestrationClient.StartNewAsync(orchestrationName, orchestrationId, request);
                    return await WaitForCompletion(orchestrationId);
            }
        }

        private async Task<OResponse> WaitForCompletion(string orchestrationId)
        {
            // wait job to ran and check the job status
            await DurableOrchestrationClient.WaitForCompletionOrCreateCheckStatusResponseAsync(
                RawRequestMessage, orchestrationId, _orchestrationWaitTime, TimeSpan.FromSeconds(1));
            var status = await DurableOrchestrationClient.GetStatusAsync(orchestrationId);
            switch (status.RuntimeStatus)
            {
                case OrchestrationRuntimeStatus.Completed:
                    return ExtractOrchestrationResult(status)!;
                case OrchestrationRuntimeStatus.ContinuedAsNew:
                case OrchestrationRuntimeStatus.Failed:
                case OrchestrationRuntimeStatus.Canceled:
                case OrchestrationRuntimeStatus.Terminated:
                case OrchestrationRuntimeStatus.Suspended:
                case OrchestrationRuntimeStatus.Running:
                case OrchestrationRuntimeStatus.Pending:
                case OrchestrationRuntimeStatus.Unknown:
                default:
                    {
                        var detail = status.Output.ToString();
                        var statusCode = "500";
                        var subCode = HttpStatusCode.InternalServerError.ToString();
                        var message = detail;
                        try
                        {
                            var index = detail.IndexOf("{", StringComparison.Ordinal);
                            if (index != -1)
                            {
                                var detailObject = Newtonsoft.Json.Linq.JObject.Parse(detail.Substring(index));
                                statusCode = detailObject.GetValue("statusCode")!.ToString();
                                subCode = detailObject.GetValue("statusSubCode")!.ToString();
                                message = detailObject.GetValue("descriptionMessage")!.ToString();
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                        finally
                        {
                            throw new GdCustomException(statusCode, subCode,
                                message);
                        }
                    }

            }
        }


        /// <summary>
        /// Executed by runtime
        /// </summary>
        /// <param name="executingContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            RawRequestMessage = (HttpRequestMessage)executingContext.Arguments["request"];
            await ExtractRequestContentAsString();
        }

        /// <summary>
        /// executed by runtime
        /// </summary>
        /// <param name="executedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            //no-op
            return;
        }
        public virtual async Task<OResponse> ExecuteHttpGetAsync(string functionName, IDurableOrchestrationClient context, ORequest request)
        {
            _request = request;
            return await ExecuteAsync(functionName, context);
        }
        /// <summary>
        /// Executed by subclass
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public virtual async Task<OResponse> ExecuteAsync(string functionName, IDurableOrchestrationClient context)
        {
            OResponse response = Activator.CreateInstance<OResponse>();
            try
            {
                DurableOrchestrationClient = context;
                response = await ExecuteOrchestration(_request, functionName);
                if (response == null)
                    throw new GdCustomException(HttpStatusCode.InternalServerError.ToString(), "",
                        $"Function={functionName} orchestration did not return any result");
            }
            catch (GdCustomException gde)
            {
                int.TryParse(gde.StatusCode, out int statusCode);
                int.TryParse(gde.StatusSubCode, out int statusSubCode);
            }
            catch (Exception e)
            {
                
            }
            return response;
        }

        protected virtual async Task ExtractRequestContentAsString()
        {
            if (RawRequestMessage == null || RawRequestMessage.Content == null)
            {
                Request = default(ORequest)!;
                return;
            }
            var requestContent = await RawRequestMessage.Content.ReadAsStringAsync();
            Request = JsonConvert.DeserializeObject<ORequest>(requestContent)!;
        }

        protected virtual OResponse? ExtractOrchestrationResult(DurableOrchestrationStatus? statusResult)
        {
            if (statusResult == null || statusResult.Output == null)
                throw new GdCustomException(HttpStatusCode.InternalServerError.ToString(),
                    HttpStatusCode.BadRequest.ToString(), $"Status result is null for {statusResult?.Name}.");
            var result = statusResult.Output.ToObject<OResponse>();
            if (result == null)
                throw new GdCustomException(HttpStatusCode.InternalServerError.ToString(), "",
                    $"Failed to deserialize orchestration {statusResult.Name} output.");
            
            return result;
        }
    }
}
