using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lab.ExchangeNet45.Contracts.HttpClient
{
    using HttpClient = System.Net.Http.HttpClient;

    public class ExchangeHttpClient : HttpClient
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public ExchangeHttpClient(Uri serviceUri)
        {
            BaseAddress = serviceUri;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        public async Task<TResponse> GetRequestResponseAsync<TResponse>(Uri requestUri)
        {
            HttpRequestMessage getRequest = CreateGetRequest(requestUri);

            HttpResponseMessage response = await SendAsync(getRequest);

            await AssertSuccessResponse(response);

            TResponse responseObject = await CreateObjectFromJsonContent<TResponse>(response.Content);

            return responseObject;
        }

        private static async Task AssertSuccessResponse(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException exception)
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new ExchangeHttpRequestException(response.StatusCode, content, exception.Message, exception);
            }
        }

        private async Task<T> CreateObjectFromJsonContent<T>(HttpContent content)
        {
            string responseContent = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent, _jsonSerializerSettings);
        }

        private static HttpRequestMessage CreateRequest(HttpMethod requestMethod, Uri requestUri)
        {
            var request = new HttpRequestMessage(requestMethod, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        private static HttpRequestMessage CreateGetRequest(Uri requestUri)
        {
            return CreateRequest(HttpMethod.Get, requestUri);
        }
    }
}
