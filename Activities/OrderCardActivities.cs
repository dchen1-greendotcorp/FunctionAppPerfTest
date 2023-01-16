using AutoMapper;
using FunctionAppPerfTest.Models;
using MediatR;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Activities
{
    public class OrderCardActivities
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderCardActivities(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [FunctionName(nameof(ReplaceCard))]
        public async Task<ReplaceCardResponse> ReplaceCard([ActivityTrigger] IDurableActivityContext context)
        {
            var activityData = context.GetInput<ReplaceCardRequest>();
            return await _mediator.Send(activityData);
        }
    }
}
