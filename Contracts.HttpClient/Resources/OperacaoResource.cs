namespace Lab.ExchangeNet45.Contracts.HttpClient.Resources
{
    public class OperacaoResource
    {
        private const string RoutePrefix = "api/operacoes";

        private readonly ExchangeHttpClient _httpClient;

        public OperacaoResource(ExchangeHttpClient httpClient)
        {
            _httpClient = httpClient;
        }


    }
}
