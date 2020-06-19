using System.Collections.Generic;
using MediatR;

namespace Lab.ExchangeNet45.Contracts.Operacao.Queries
{
    public class GetOperacoesQuery : IRequest<IEnumerable<OperacaoQueryModel>>
    {
    }
}
