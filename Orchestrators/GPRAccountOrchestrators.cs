using FunctionAppPerfTest.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionAppPerfTest.Activities;
using FunctionAppPerfTest.Factories;

namespace FunctionAppPerfTest.Orchestrators
{
    public class GPRAccountOrchestrators
    {
        [FunctionName(nameof(CreateGPRAccount))]
        public async Task<CreateAccountResponse> CreateGPRAccount(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var requestContext = context.GetInput<CreateAccountRequest>();

            var responseContext = new CreateAccountResponse
            {
                Data = new CreateAccountResponseData()
            };

            var parallelTasks = new List<Task<ActivityOrSubOrchResponse>>();
            var addCardAccountTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(
                nameof(AccountOrchestrationActivities.AddCardAccount), requestContext);
            parallelTasks.Add(addCardAccountTask);

            var resultList = await Task.WhenAll(parallelTasks);

            var createCardAccountResult = resultList.First(c => c.ActivityName == nameof(AccountOrchestrationActivities.AddCardAccount)).Data as AddCardAccountActivityData;


            var getCardPanActivityResult = await context.CallActivityAsync<ClearTextPan>(nameof(AccountOrchestrationActivities.GetCardPan),
                    new GetClearTextPanRequest
                    {
                        OtherData = DataFactory.CreateFakeData(),
                        RequestId = requestContext.RequestId
                    });

            //parallelTaskList include RegisterCard,UpdateCardRegistrationStatus,UpdateCardStatus
            var parallelTaskList = new List<Task<ActivityOrSubOrchResponse>>();

            var registerCardTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(nameof(AccountOrchestrationActivities.RegisterCard),
                new RegisterCardActivityData { OtherData = DataFactory.CreateFakeData(), CreateAccountRequest = requestContext });

            parallelTaskList.Add(registerCardTask);


            var UpdateCardRegistrationTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(nameof(AccountOrchestrationActivities.UpdateCardRegistrationStatus),
                new UpdateCardRegisterStatusRequest
                {
                    PlasticId = createCardAccountResult.CardAccountId,
                    OtherRequestData = DataFactory.CreateFakeData(),
                });

            parallelTaskList.Add(UpdateCardRegistrationTask);


            var UpdateCardStatusTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(nameof(AccountOrchestrationActivities.UpdateCardStatus),
                new UpdateCardStatusRequest
                {
                    PlasticId = createCardAccountResult.CardAccountId,
                    OtherRequestData= DataFactory.CreateFakeData(),
                });

            parallelTaskList.Add(UpdateCardStatusTask);

            var resultList1 = await Task.WhenAll(parallelTaskList);

            var res = resultList1.First(c => c.ActivityName == nameof(AccountOrchestrationActivities.RegisterCard)).Data as RegisterCardResponse;

            responseContext.Data.CardID = res.Data.CardID;
            responseContext.Data.OtherData = DataFactory.CreateFakeData();

           return responseContext;
        }

    }
}
