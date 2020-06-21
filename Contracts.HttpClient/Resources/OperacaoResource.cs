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

        public Task<IEnumerable<OperacaoQueryModel>> GetAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}", UriKind.Relative);

            return _httpClient.GetRequestResponseAsync<IEnumerable<OperacaoQueryModel>>(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadAsCsvFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/csv-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadAsExcelFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/excel-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }


        public Task<IEnumerable<OperacaoStandardGroupingQueryModel>> GroupByStandardAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping", UriKind.Relative);

            return _httpClient.GetRequestResponseAsync<IEnumerable<OperacaoStandardGroupingQueryModel>>(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByStandardAsCsvFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/csv-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByStandardAsExcelFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/excel-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }


        public Task<IEnumerable<OperacaoAtivoGroupingQueryModel>> GroupByAtivoAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/ativo", UriKind.Relative);

            return _httpClient.GetRequestResponseAsync<IEnumerable<OperacaoAtivoGroupingQueryModel>>(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByAtivoAsCsvFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/ativo/csv-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByAtivoAsExcelFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/ativo/excel-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }


        public Task<IEnumerable<OperacaoTipoGroupingQueryModel>> GroupByTipoAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/tipo", UriKind.Relative);

            return _httpClient.GetRequestResponseAsync<IEnumerable<OperacaoTipoGroupingQueryModel>>(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByTipoAsCsvFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/tipo/csv-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByTipoAsExcelFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/tipo/excel-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }


        public Task<IEnumerable<OperacaoContaGroupingQueryModel>> GroupByContaAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/conta", UriKind.Relative);

            return _httpClient.GetRequestResponseAsync<IEnumerable<OperacaoContaGroupingQueryModel>>(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByContaAsCsvFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/conta/csv-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

        public Task<byte[]> DownloadGroupingByContaAsExcelFileAsync(CancellationToken cancellationToken = default)
        {
            var relativeUri = new Uri($"{RoutePrefix}/grouping/conta/excel-file", UriKind.Relative);

            return _httpClient.DownloadByteArray(relativeUri, cancellationToken);
        }

    }
}
