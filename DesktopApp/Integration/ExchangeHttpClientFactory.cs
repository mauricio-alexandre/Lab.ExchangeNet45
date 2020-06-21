using System;
using System.Configuration;
using Lab.ExchangeNet45.Contracts.HttpClient;

namespace Lab.ExchangeNet45.DesktopApp.Integration
{
    public class ExchangeHttpClientFactory
    {
        public ExchangeHttpClient CreateFromAppSettings()
        {
            string baseUriString = ConfigurationManager.AppSettings["ExchangeService.BaseUri"];
            return new ExchangeHttpClient(new Uri(baseUriString, UriKind.Absolute));
        }
    }
}
