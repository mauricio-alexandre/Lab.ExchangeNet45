using System.Threading.Tasks;
using MediatR.Pipeline;

namespace Lab.ExchangeNet45.WebApi.Utils.MediatorSimpleInjector
{
    public class GenericRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        public Task Process(TRequest request, TResponse response)
        {
            // all done
            return Task.FromResult(0);
        }
    }
}