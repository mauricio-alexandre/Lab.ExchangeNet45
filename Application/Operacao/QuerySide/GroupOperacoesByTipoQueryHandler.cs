using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;
using MediatR;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;

namespace Lab.ExchangeNet45.Application.Operacao.QuerySide
{
    using Operacao = CommandSide.DomainModels.Operacao;

    public class GroupOperacoesByTipoQueryHandler : 
        IRequestHandler<GroupOperacoesByTipoQuery, IEnumerable<OperacaoTipoGroupingQueryModel>>
    {
        private readonly ISession _session;

        public GroupOperacoesByTipoQueryHandler(ISession session)
        {
            _session = session;
        }

        private static IProjection ProjectionOperacaoTipoGroupingQueryModel()
        {
            OperacaoTipoGroupingQueryModel queryModel = null;

            IProjection projectionQuantidade = Projections.Property<Operacao>(operacao => operacao.Quantidade);

            IProjection projectionPreco = Projections.Property<Operacao>(operacao => operacao.Preco);

            IProjection projectionQuantidadeVezesPreco = Projections.SqlFunction
            (
                function: new VarArgsSQLFunction("(", "*", ")"),
                type: NHibernateUtil.Double,
                projectionQuantidade, projectionPreco
            );

            IProjection projectionSumOfQuantidadeVezesPreco = Projections.Sum(projectionQuantidadeVezesPreco);

            IProjection projectionSumOfQuantidade = Projections.Sum(projectionQuantidade);

            IProjection projectionPrecoMedio = Projections.SqlFunction
            (
                function: new VarArgsSQLFunction("(", "/", ")"),
                type: NHibernateUtil.Double,
                projectionSumOfQuantidadeVezesPreco, projectionSumOfQuantidade
            );

            return Projections
                    .ProjectionList()
                    .Add(Projections.Group<Operacao>(operacao => operacao.Tipo.Value).WithAlias(() => queryModel.TipoOperacao))
                    .Add(projectionSumOfQuantidade.WithAlias(() => queryModel.SomaDasQuantidades))
                    .Add(projectionPrecoMedio.WithAlias(() => queryModel.PrecoMedio))
                ;
        }

        public Task<IEnumerable<OperacaoTipoGroupingQueryModel>> Handle(GroupOperacoesByTipoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoTipoGroupingQueryModel> operacoes = _session
                .QueryOver<Operacao>()
                .Select(ProjectionOperacaoTipoGroupingQueryModel())
                .TransformUsing(Transformers.AliasToBean<OperacaoTipoGroupingQueryModel>())
                .List<OperacaoTipoGroupingQueryModel>();

            return Task.FromResult(operacoes);
        }
    }
}
