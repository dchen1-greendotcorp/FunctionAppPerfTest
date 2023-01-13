using AutoMapper;
using FunctionAppPerfTest.Models;
using Gd.Cbs.Mask.Common;
using MediatR;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
//using RegisterCardRequest = CoreBanking.Domain.Microservices.Card.Request.RegisterCardRequest;

namespace FunctionAppPerfTest.Activities
{
    public class AccountOrchestrationActivities
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountOrchestrationActivities(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [FunctionName(nameof(AddCardAccount))]
        public async Task<ActivityOrSubOrchResponse> AddCardAccount([ActivityTrigger] IDurableActivityContext context)
        {
            var activityData = context.GetInput<CreateAccountRequest>();

            var createCardAccountRequest = _mapper.Map<CreateAccountRequest, CreateCardAccountRequest>(activityData);

            var createCardAccountResponse = await _mediator.Send(createCardAccountRequest);

            AddCardAccountActivityData data = _mapper.Map<CreateCardAccountResponse, AddCardAccountActivityData>(createCardAccountResponse);

            var response = new ActivityOrSubOrchResponse();
            response.ActivityName = nameof(AccountOrchestrationActivities.AddCardAccount);
            response.Data = data;

            return response;

        }

        [FunctionName(nameof(GetCardPan))]
        public async Task<ClearTextPan> GetCardPan([ActivityTrigger] IDurableActivityContext context)
        {
            var getPanRequest = context.GetInput<GetClearTextPanRequest>();
            var getPanResult = await _mediator.Send(getPanRequest);
            return getPanResult.Data;
        }

        [FunctionName(nameof(RegisterCard))]
        public async Task<ActivityOrSubOrchResponse> RegisterCard([ActivityTrigger] IDurableActivityContext context)
        {
            var activityData = context.GetInput<RegisterCardActivityData>();

            var registerCard = _mapper.Map<RegisterCardActivityData, RegisterCardRequest>(activityData);

            var registerCardResponse = await _mediator.Send(registerCard);

            var response = new ActivityOrSubOrchResponse();
            response.ActivityName = nameof(AccountOrchestrationActivities.RegisterCard);
            response.Data = registerCardResponse;

            return response;
        }

        [FunctionName(nameof(UpdateCardRegistrationStatus))]
        public async Task<ActivityOrSubOrchResponse> UpdateCardRegistrationStatus([ActivityTrigger] IDurableActivityContext context)
        {
            var updateCardRegistrationStatusRequest = context.GetInput<UpdateCardRegisterStatusRequest>();
            var data = await _mediator.Send(updateCardRegistrationStatusRequest);

            var response = new ActivityOrSubOrchResponse();
            response.ActivityName = nameof(AccountOrchestrationActivities.UpdateCardRegistrationStatus);
            //response.Data = data;

            return response;
        }

        [FunctionName(nameof(UpdateCardStatus))]
        public async Task<ActivityOrSubOrchResponse> UpdateCardStatus([ActivityTrigger] IDurableActivityContext context)
        {
            var updateCardStatusRequest = context.GetInput<UpdateCardStatusRequest>();
            var data = await _mediator.Send(updateCardStatusRequest);

            var response = new ActivityOrSubOrchResponse();

            response.ActivityName = nameof(AccountOrchestrationActivities.UpdateCardStatus);
            //response.Data = data;
            return response;
        }
    }
}
