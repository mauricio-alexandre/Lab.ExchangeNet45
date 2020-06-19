namespace Lab.ExchangeNet45.Contracts.Operacao.Queries
{
    public class OperacaoStandardGroupingQueryModel
    {
        public char TipoOperacao { get; set; }

        public string Ativo { get; set; }

        public int Conta { get; set; }

        public int SomaDasQuantidades { get; set; }

        public double PrecoMedio { get; set; }
    }
}
