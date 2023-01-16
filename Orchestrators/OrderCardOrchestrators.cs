using FunctionAppPerfTest.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using FunctionAppPerfTest.Activities;
using Microsoft.Extensions.Logging;

namespace FunctionAppPerfTest.Orchestrators
{
    public class OrderCardOrchestrators
    {
        [FunctionName(nameof(OrderCardOrchestrator))]
        public async Task<OrderCardResponse> OrderCardOrchestrator(
               [OrchestrationTrigger] IDurableOrchestrationContext context,
               ILogger log)
        {
            var request = context.GetInput<OrderCardRequest>();
            var response = new OrderCardResponse();

            var replaceCardResponse = await context.CallActivityAsync<ReplaceCardResponse>(
                    nameof(OrderCardActivities.ReplaceCard), new ReplaceCardRequest
                    { 
                        PlasticId = request.PlasticId,
                        OtherRequestData = request.OtherRequestData,
                    });

            return response;
        }
    }
}
