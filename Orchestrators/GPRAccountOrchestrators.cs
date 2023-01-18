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
using Newtonsoft.Json;

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

            var result=resultList.First(c => c.ActivityName == nameof(AccountOrchestrationActivities.AddCardAccount));
            var json=JsonConvert.SerializeObject(result.Data);

            AddCardAccountActivityData createCardAccountResult = JsonConvert.DeserializeObject<AddCardAccountActivityData>(json);//result.Data as AddCardAccountActivityData;

            var getCardPanActivityResult = await context.CallActivityAsync<ClearTextPan>(nameof(AccountOrchestrationActivities.GetCardPan),
                    new GetClearTextPanRequest
                    {
                        OtherData = DataFactory.CreateFakeCharacterData(2000),
                        RequestId = requestContext.RequestId
                    });

            //parallelTaskList include RegisterCard,UpdateCardRegistrationStatus,UpdateCardStatus
            var parallelTaskList = new List<Task<ActivityOrSubOrchResponse>>();

            var registerCardTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(nameof(AccountOrchestrationActivities.RegisterCard),
                new RegisterCardActivityData { OtherData = DataFactory.CreateFakeCharacterData(4000), CreateAccountRequest = requestContext, Pan= getCardPanActivityResult.Pan });

            parallelTaskList.Add(registerCardTask);


            var UpdateCardRegistrationTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(nameof(AccountOrchestrationActivities.UpdateCardRegistrationStatus),
                new UpdateCardRegisterStatusRequest
                {
                    PlasticId = $"PlasticId-{createCardAccountResult.CardAccountId}",
                    OtherRequestData = DataFactory.CreateFakeCharacterData(2000),
                });

            parallelTaskList.Add(UpdateCardRegistrationTask);


            var UpdateCardStatusTask = context.CallActivityAsync<ActivityOrSubOrchResponse>(nameof(AccountOrchestrationActivities.UpdateCardStatus),
                new UpdateCardStatusRequest
                {
                    PlasticId = $"PlasticId-{createCardAccountResult.CardAccountId}",
                    OtherRequestData= DataFactory.CreateFakeCharacterData(2000),
                });

            parallelTaskList.Add(UpdateCardStatusTask);

            var resultList1 = await Task.WhenAll(parallelTaskList);

            var res = resultList1.First(c => c.ActivityName == nameof(AccountOrchestrationActivities.RegisterCard));//.Data as RegisterCardResponse;
            json = JsonConvert.SerializeObject(res.Data);

            RegisterCardResponse resp = JsonConvert.DeserializeObject<RegisterCardResponse>(json);

            responseContext.Data.CardID = resp.Data.CardID;
            responseContext.Data.OtherData = DataFactory.CreateFakeCharacterData(4000);

           return responseContext;
        }

    }
}
