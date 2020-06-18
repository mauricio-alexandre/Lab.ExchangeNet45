using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace Lab.ExchangeNet45.WebApi.Utils.MediatorSimpleInjector
{
    public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            // Starting Up
            return Task.FromResult(0);
        }
    }
}