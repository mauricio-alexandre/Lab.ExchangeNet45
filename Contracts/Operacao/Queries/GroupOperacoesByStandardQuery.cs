using System.Collections.Generic;
using MediatR;

namespace Lab.ExchangeNet45.Contracts.Operacao.Queries
{
    public class GroupOperacoesByStandardQuery : IRequest<IEnumerable<OperacaoStandardGroupingQueryModel>>
    {
    }
}
