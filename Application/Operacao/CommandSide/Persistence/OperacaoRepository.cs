using System.Collections.Generic;
using Lab.ExchangeNet45.Application.Operacao.CommandSide.DomainModels;
using NHibernate;
using NHibernate.Criterion;

namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.Persistence
{
    using Operacao = DomainModels.Operacao;

    public class OperacaoRepository : IOperacaoRepository
    {
        private readonly ISession _session;

        public OperacaoRepository(ISession session)
        {
            _session = session;
        }

        public Operacao FindById(long id)
        {
            Operacao operacao = _session
                .QueryOver<Operacao>()
                .Where(Restrictions.IdEq(id))
                .SingleOrDefault();

            if (operacao == null) throw new KeyNotFoundException($"Operação '{id}' não encontrada.");

            return operacao;
        }

        public void Add(Operacao operacao) => _session.Save(operacao);
    }
}
