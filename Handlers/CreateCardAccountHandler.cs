using DurableTask.Core.Exceptions;
using FunctionAppPerfTest.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Handlers
{
    public class CreateCardAccountHandler : IRequestHandler<CreateCardAccountRequest, CreateCardAccountResponse>
    {
        private readonly int _millSeconds;

        public CreateCardAccountHandler(IConfiguration configuration)
        {
            _millSeconds = int.Parse(configuration["CreateCardAccountDelay"]);
        }

        public async Task<CreateCardAccountResponse> Handle(CreateCardAccountRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new TaskFailureException(HttpStatusCode.BadRequest.ToString(), nameof(CreateCardAccountRequest));
            try
            {
                await Task.Delay(_millSeconds);
                
                CreateCardAccountResponse response = new CreateCardAccountResponse();
                return response!;
            }
            catch (Exception e) when (e is not TaskFailureException)
            {
                throw new TaskFailureException($"{HttpStatusCode.InternalServerError} {e.Message}", e);
            }
        }
    }
}
