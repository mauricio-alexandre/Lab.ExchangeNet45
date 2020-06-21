using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lab.ExchangeNet45.Contracts.Operacao.Queries;

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

        public Task<IEnumerable<OperacaoQueryModel>> Get(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}", UriKind.Relative);

            return _httpClient.GetRequestResponseAsync<IEnumerable<OperacaoQueryModel>>(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadCsvFile(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/csv-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadExcelFile(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/excel-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }
    }
}
