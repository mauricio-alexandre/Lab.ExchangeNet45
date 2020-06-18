using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NLog;

namespace Lab.ExchangeNet45.Application.Shared
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Type requestType = typeof(TRequest);
            Type responseType = typeof(TResponse);
            try
            {
                Logger.Info($"Handling {requestType.Name}");

                TResponse response = await next();

                string responseLogging = responseType.IsClass ? $" {responseType.Name}" : "";

                Logger.Info($"Handled {requestType.Name}{responseLogging}");

                return response;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, $"Exception when handling {requestType.Name}: {exception}");
                throw;
            }
        }
    }
}
