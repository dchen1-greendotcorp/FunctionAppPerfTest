using DurableTask.Core.Exceptions;
using FunctionAppPerfTest.Factories;
using FunctionAppPerfTest.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Handlers
{
    public class ReplaceCardRequestHandler : IRequestHandler<ReplaceCardRequest, ReplaceCardResponse>
    {
        private readonly int _millSeconds;

        public ReplaceCardRequestHandler(IConfiguration configuration)
        {
            _millSeconds = int.Parse(configuration["AciOrderCardApiMiliSecond:ReplaceCard"]);
        }

        public async Task<ReplaceCardResponse> Handle(ReplaceCardRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new TaskFailureException(HttpStatusCode.BadRequest.ToString(), nameof(ReplaceCardRequest));
            try
            {
                await Task.Delay(_millSeconds);

                ReplaceCardResponse response = new ReplaceCardResponse();
                response.Data = new ReplaceCardExternalResponse() { Message = $"good-{DataFactory.CreateUtcData()}" };
                return response!;
            }
            catch (Exception e) when (e is not TaskFailureException)
            {
                throw new TaskFailureException($"{HttpStatusCode.InternalServerError} {e.Message}", e);
            }
        }
    }
}
