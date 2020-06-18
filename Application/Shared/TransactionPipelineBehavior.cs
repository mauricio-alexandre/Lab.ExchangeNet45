using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHibernate;

namespace Lab.ExchangeNet45.Application.Shared
{
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ISession _session;

        public TransactionPipelineBehavior(ISession session)
        {
            _session = session;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                try
                {
                    TResponse response = await next();

                    transaction.Commit();

                    return response;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}
