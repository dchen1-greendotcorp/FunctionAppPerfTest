using DurableTask.Core.Exceptions;
using FunctionAppPerfTest.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Handlers
{
    public class UpdateCardStatusRequestHandler : IRequestHandler<UpdateCardStatusRequest, UpdateCardStatusResponse>
    {
        private readonly int _millSeconds;

        public UpdateCardStatusRequestHandler(IConfiguration configuration)
        {
            _millSeconds = int.Parse(configuration["AciCreateAccountApiMiliSecond:UpdateCardStatus"]);
        }

        public async Task<UpdateCardStatusResponse> Handle(UpdateCardStatusRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new TaskFailureException(HttpStatusCode.BadRequest.ToString(), nameof(UpdateCardStatusRequest));
            try
            {
                await Task.Delay(_millSeconds);

                UpdateCardStatusResponse response = new UpdateCardStatusResponse();
                
                return response!;
            }
            catch (Exception e) when (e is not TaskFailureException)
            {
                throw new TaskFailureException($"{HttpStatusCode.InternalServerError} {e.Message}", e);
            }
        }
    }
}
