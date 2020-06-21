using System;
using System.Net;
using System.Net.Http;

namespace Lab.ExchangeNet45.Contracts.HttpClient
{
    public class ExchangeHttpRequestException : HttpRequestException
    {
        public ExchangeHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ExchangeHttpRequestException(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        public ExchangeHttpRequestException(HttpStatusCode statusCode, string content, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
            Content = content;
        }

        public HttpStatusCode StatusCode { get; }

        public string Content { get; }
    }
}
