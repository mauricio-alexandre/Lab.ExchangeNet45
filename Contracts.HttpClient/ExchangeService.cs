using Lab.ExchangeNet45.Contracts.HttpClient.Resources;

namespace Lab.ExchangeNet45.Contracts.HttpClient
{
    public class ExchangeService
    {
        public ExchangeService(OperacaoResource operacoes)
        {
            Operacoes = operacoes;
        }

        public OperacaoResource Operacoes { get; }
    }
}
