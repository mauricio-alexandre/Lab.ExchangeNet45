using System;
using System.Configuration;

namespace Lab.ExchangeNet45.OperacaoLoader
{
    public class ExchangeServiceConfiguration
    {
        public ExchangeServiceConfiguration(string baseUri, string operacoesRelativeUri, string operacoesBatchRelativeUri)
        {
            if (string.IsNullOrWhiteSpace(baseUri)) throw new ArgumentNullException(nameof(baseUri));
            if (string.IsNullOrWhiteSpace(operacoesRelativeUri)) throw new ArgumentNullException(nameof(operacoesRelativeUri));
            if (string.IsNullOrWhiteSpace(operacoesBatchRelativeUri)) throw new ArgumentNullException(nameof(operacoesBatchRelativeUri));

            BaseUri = new Uri(baseUri, UriKind.Absolute);
            OperacoesUri = new Uri(BaseUri, operacoesRelativeUri);
            OperacoesBatchUri = new Uri(BaseUri, operacoesBatchRelativeUri);
        }

        public static ExchangeServiceConfiguration FromAppSettings()
        {
            string baseUri = ConfigurationManager.AppSettings["ExchangeService.BaseUri"];
            string operacoesRelativeUri = ConfigurationManager.AppSettings["ExchangeService.OperacoesRelativeUri"];
            string operacoesBatchRelativeUri = ConfigurationManager.AppSettings["ExchangeService.OperacoesBatchRelativeUri"];
            return new ExchangeServiceConfiguration(baseUri, operacoesRelativeUri, operacoesBatchRelativeUri);
        }

        public Uri BaseUri { get; }

        public Uri OperacoesUri { get; }

        public Uri OperacoesBatchUri { get; }
    }
}
