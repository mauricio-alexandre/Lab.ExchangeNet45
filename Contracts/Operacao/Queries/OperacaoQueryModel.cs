using System;

namespace Lab.ExchangeNet45.Contracts.Operacao.Queries
{
    public class OperacaoQueryModel
    {
        public long Id { get; set; }

        public DateTime Data { get; set; }

        public char TipoOperacao { get; set; }

        public string Ativo { get; set; }

        public int Quantidade { get; set; }

        public double Preco { get; set; }

        public int Conta { get; set; }
    }
}
