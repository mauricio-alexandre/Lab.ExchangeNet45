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

    public class GroupOperacoesByStandardQueryHandler : 
        IRequestHandler<GroupOperacoesByStandardQuery, IEnumerable<OperacaoStandardGroupingQueryModel>>
    {
        private readonly ISession _session;

        public GroupOperacoesByStandardQueryHandler(ISession session)
        {
            _session = session;
        }

        private static IProjection ProjectionOperacaoStandardGroupingQueryModel()
        {
            OperacaoStandardGroupingQueryModel queryModel = null;

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
                    .Add(Projections.Group<Operacao>(operacao => operacao.Ativo).WithAlias(() => queryModel.Ativo))
                    .Add(Projections.Group<Operacao>(operacao => operacao.Conta).WithAlias(() => queryModel.Conta))
                    .Add(projectionSumOfQuantidade.WithAlias(() => queryModel.SomaDasQuantidades))
                    .Add(projectionPrecoMedio.WithAlias(() => queryModel.PrecoMedio))
                ;
        }

        public Task<IEnumerable<OperacaoStandardGroupingQueryModel>> Handle(GroupOperacoesByStandardQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OperacaoStandardGroupingQueryModel> operacoes = _session
                .QueryOver<Operacao>()
                .Select(ProjectionOperacaoStandardGroupingQueryModel())
                .TransformUsing(Transformers.AliasToBean<OperacaoStandardGroupingQueryModel>())
                .List<OperacaoStandardGroupingQueryModel>();

            return Task.FromResult(operacoes);
        }
    }
}
