using System.Collections.Generic;
using MediatR;

namespace Lab.ExchangeNet45.Contracts.Operacao.Commands
{
    public class AddOperacoesBatchCommand : List<AddOperacaoCommand>, IRequest
    {
    }
}
