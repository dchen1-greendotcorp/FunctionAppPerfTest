using DurableTask.Core.Exceptions;
using FunctionAppPerfTest.Factories;
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
    public class GetCardClearTextCardPanHandler : IRequestHandler<GetClearTextPanRequest, GetClearTextPanResponse>
    {
        private readonly int _millSeconds;

        public GetCardClearTextCardPanHandler(IConfiguration configuration)
        {
            _millSeconds = int.Parse(configuration["AciCreateAccountApiMiliSecond:GetCardPan"]);
        }
        

        public async Task<GetClearTextPanResponse> Handle(GetClearTextPanRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new TaskFailureException(HttpStatusCode.BadRequest.ToString(), nameof(GetClearTextPanRequest));
            try
            {
                var response = new GetClearTextPanResponse();
                response.Data = new ClearTextPan() { Pan = $"Pan-{DataFactory.CreateUtcData()}" };
                await Task.Delay(_millSeconds);
                return response!;
            }
            catch (Exception e) when (e is not TaskFailureException)
            {
                throw new TaskFailureException($"{HttpStatusCode.InternalServerError} {e.Message}", e);
            }
        }
    }
}
