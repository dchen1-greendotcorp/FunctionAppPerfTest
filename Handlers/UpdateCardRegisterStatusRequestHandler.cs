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
    public class UpdateCardRegisterStatusRequestHandler : IRequestHandler<UpdateCardRegisterStatusRequest, UpdateCardRegisterStatusResponse>
    {
        private readonly int _millSeconds;

        public UpdateCardRegisterStatusRequestHandler(IConfiguration configuration)
        {
            _millSeconds = int.Parse(configuration["AciCreateAccountApiMiliSecond:UpdateCardRegistrationStatus"]);
        }

        public async Task<UpdateCardRegisterStatusResponse> Handle(UpdateCardRegisterStatusRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new TaskFailureException(HttpStatusCode.BadRequest.ToString(), nameof(UpdateCardRegisterStatusRequest));
            try
            {
                await Task.Delay(_millSeconds);

                UpdateCardRegisterStatusResponse response = new UpdateCardRegisterStatusResponse();
                response.Data = new UpdateCardRegisterStatusResponseData()
                {
                   PlasticId = request.PlasticId,
                   OtherData = request.OtherRequestData
                };
                return response!;
            }
            catch (Exception e) when (e is not TaskFailureException)
            {
                throw new TaskFailureException($"{HttpStatusCode.InternalServerError} {e.Message}", e);
            }
        }
    }
}
