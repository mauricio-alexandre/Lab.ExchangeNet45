using System.Threading;
using System.Threading.Tasks;
using Lab.ExchangeNet45.Application.Operacao.CommandSide.DomainModels;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using MediatR;

namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.CommandHandlers
{
    using Operacao = DomainModels.Operacao;

    public class AddOperacaoCommandHandler : AsyncRequestHandler<AddOperacaoCommand>
    {
        private readonly IOperacaoRepository _operacaoRepository;

        public AddOperacaoCommandHandler(IOperacaoRepository operacaoRepository)
        {
            _operacaoRepository = operacaoRepository;
        }

        protected override Task Handle(AddOperacaoCommand command, CancellationToken cancellationToken)
        {
            var tipoOperacao = OperacaoTipo.Parse(command.TipoOperacao);

            var operacao = new Operacao(command.Id, command.Data, tipoOperacao, command.Ativo, command.Quantidade, command.Preco, command.Conta);

            _operacaoRepository.Add(operacao);

            return Task.FromResult(0);
        }
    }
}
