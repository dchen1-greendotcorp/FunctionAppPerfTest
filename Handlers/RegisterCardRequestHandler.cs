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
    public class RegisterCardRequestHandler : IRequestHandler<RegisterCardRequest, RegisterCardResponse>
    {
        private readonly int _millSeconds;

        public RegisterCardRequestHandler(IConfiguration configuration)
        {
            _millSeconds = int.Parse(configuration["RegisterCardRequestDelay"]);
        }

        public async Task<RegisterCardResponse> Handle(RegisterCardRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new TaskFailureException(HttpStatusCode.BadRequest.ToString(), nameof(RegisterCardRequest));
            try
            {
                await Task.Delay(_millSeconds);

                RegisterCardResponse response = new RegisterCardResponse();
                return response!;
            }
            catch (Exception e) when (e is not TaskFailureException)
            {
                throw new TaskFailureException($"{HttpStatusCode.InternalServerError} {e.Message}", e);
            }
        }
    }
}
