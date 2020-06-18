using FluentNHibernate.Mapping;

namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.Persistence
{
    using Operacao = DomainModels.Operacao;

    public class OperacaoMap : ClassMap<Operacao>
    {
        public OperacaoMap()
        {
            Table("operacoes");

            Not.LazyLoad();

            Id(operacao => operacao.Id).Column("id").GeneratedBy.Assigned();

            Map(operacao => operacao.Data).Column("dateTime").Not.Nullable();

            Component(operacao => operacao.Tipo, operacaoTipo =>
            {
                operacaoTipo.Map(tipo => tipo.Value).Column("tipoOperacao").Not.Nullable();
            });

            Map(operacao => operacao.Ativo).Column("ativo").Not.Nullable();

            Map(operacao => operacao.Quantidade).Column("quantidade").Not.Nullable();

            Map(operacao => operacao.Preco).Column("preco").Not.Nullable();

            Map(operacao => operacao.Conta).Column("conta").Not.Nullable();
        }
    }
}
