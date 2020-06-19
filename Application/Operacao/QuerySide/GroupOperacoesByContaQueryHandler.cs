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

    public class GroupOperacoesByContaQueryHandler : 
        IRequestHandler<GroupOperacoesByContaQuery, IEnumerable<OperacaoContaGroupingQueryModel>>
    {
        private readonly ISession _session;

        public GroupOperacoesByContaQueryHandler(ISession session)
        {
            _session = session;
        }

        private static IProjection ProjectionOperacaoContaGroupingQueryModel()
        {
            OperacaoContaGroupingQueryModel queryModel = null;

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
                    .Add(Projections.Group<Operacao>(operacao => operacao.Conta).WithAlias(() => queryModel.Conta))
                    .Add(projectionSumOfQuantidade.WithAlias(() => queryModel.SomaDasQuantidades))
                    .Add(projectionPrecoMedio.WithAlias(() => queryModel.PrecoMedio))
                ;
        }

        public Task<IEnumerable<OperacaoContaGroupingQueryModel>> Handle(GroupOperacoesByContaQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoContaGroupingQueryModel> operacoes = _session
                .QueryOver<Operacao>()
                .Select(ProjectionOperacaoContaGroupingQueryModel())
                .TransformUsing(Transformers.AliasToBean<OperacaoContaGroupingQueryModel>())
                .List<OperacaoContaGroupingQueryModel>();

            return Task.FromResult(operacoes);
        }
    }
}
