﻿using System.Collections.Generic;
using MediatR;

namespace Lab.ExchangeNet45.Contracts.Operacao.Queries
{
    public class GroupOperacoesByTipoQuery : IRequest<IEnumerable<OperacaoTipoGroupingQueryModel>>
    {
    }
}
