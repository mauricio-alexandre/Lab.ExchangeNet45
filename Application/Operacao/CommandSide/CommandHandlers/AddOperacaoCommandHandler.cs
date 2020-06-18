using System.Threading;
using System.Threading.Tasks;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using MediatR;

namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.CommandHandlers
{
    public class AddOperacaoCommandHandler : AsyncRequestHandler<AddOperacaoCommand>
    {
        protected override Task Handle(AddOperacaoCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
