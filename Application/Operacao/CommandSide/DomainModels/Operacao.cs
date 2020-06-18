using System;

namespace Lab.ExchangeNet45.Application.Operacao.CommandSide.DomainModels
{
    public class Operacao
    {
        public Operacao(long id, DateTime data, OperacaoTipo tipo, string ativo, int quantidade, double preco, int conta)
        {
            if (id < 1) throw new InvalidOperationException("Id inválido.");
            if (string.IsNullOrWhiteSpace(ativo)) throw new ArgumentNullException(nameof(ativo), "Informe um ativo.");

            Id = id;
            Data = data;
            Tipo = tipo;
            Ativo = ativo;
            Quantidade = quantidade;
            Preco = preco;
            Conta = conta;
        }

        public long Id { get; private set; }

        public DateTime Data { get; private set; }

        public OperacaoTipo Tipo { get; private set; }

        public string Ativo { get; private set; }

        public int Quantidade { get; private set; }

        public double Preco { get; private set; }

        public int Conta { get; private set; }

        #region Equality Member (Id)

        private Operacao() { }

        protected bool Equals(Operacao other) => Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Operacao)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        #endregion
    }
}
