using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Lab.ExchangeNet45.WebApi.Utils.MediatorSimpleInjector
{
    public class GenericPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Handling Request
            var response = await next();
            // Finished Request
            return response;
        }
    }
}
