using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using MediatR;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Lab.ExchangeNet45.Application.Operacao.QuerySide
{
    using Operacao = CommandSide.DomainModels.Operacao;

    public class GetOperacoesQueryHandler : IRequestHandler<GetOperacoesQuery, IEnumerable<OperacaoQueryModel>>
    {
        private readonly ISession _session;

        public GetOperacoesQueryHandler(ISession session)
        {
            _session = session;
        }

        private static IProjection ProjectionOperacaoQueryModel()
        {
            OperacaoQueryModel queryModel = null;

            return Projections
                .ProjectionList()
                .Add(Projections.Group<Operacao>(operacao => operacao.Id).WithAlias(() => queryModel.Id))
                .Add(Projections.Group<Operacao>(operacao => operacao.Data).WithAlias(() => queryModel.Data))
                .Add(Projections.Group<Operacao>(operacao => operacao.Tipo.Value).WithAlias(() => queryModel.TipoOperacao))
                .Add(Projections.Group<Operacao>(operacao => operacao.Ativo).WithAlias(() => queryModel.Ativo))
                .Add(Projections.Group<Operacao>(operacao => operacao.Quantidade).WithAlias(() => queryModel.Quantidade))
                .Add(Projections.Group<Operacao>(operacao => operacao.Preco).WithAlias(() => queryModel.Preco))
                .Add(Projections.Group<Operacao>(operacao => operacao.Conta).WithAlias(() => queryModel.Conta))
                ;
        }

        public Task<IEnumerable<OperacaoQueryModel>> Handle(GetOperacoesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoQueryModel> operacoes = _session
                .QueryOver<Operacao>()
                .Select(ProjectionOperacaoQueryModel())
                .TransformUsing(Transformers.AliasToBean<OperacaoQueryModel>())
                .List<OperacaoQueryModel>();

            return Task.FromResult(operacoes);
        }
    }
}
