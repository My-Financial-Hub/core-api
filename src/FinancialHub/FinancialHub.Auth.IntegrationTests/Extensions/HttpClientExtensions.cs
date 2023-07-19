using FinancialHub.Auth.IntegrationTests.Extensions.Utils;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net.Http.Headers;

namespace FinancialHub.Auth.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        private static void AddHeaders(this HttpRequestHeaders headers, Dictionary<string, string> headerData)
        {
            foreach (var header in headerData)
            {
                if (!headers.TryGetValues(header.Key, out _))
                {
                    headers.Add(header.Key, header.Value);
                }
            }
        }

        private static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpClientExtensionsParameters parameters)
        {
            var message = new HttpRequestMessage(parameters.Method, parameters.Url);

            message.Headers.AddHeaders(parameters.Headers);

            return await httpClient.SendAsync(message);
        }

        private static async Task<HttpResponseMessage> SendAsync<T>(this HttpClient httpClient, HttpClientExtensionsParameters<T> parameters)
        {
            var message = new HttpRequestMessage(parameters.Method, parameters.Url);

            if(parameters.Body != null)
            {
                message.Content = parameters.Body.ToHttpContent();
            }

            message.Headers.AddHeaders(parameters.Headers);

            return await httpClient.SendAsync(message);
        }

        public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string url, string token)
        {
            var message = new HttpClientExtensionsParameters()
            {
                Url = url,
                Method = HttpMethod.Get,
                Headers = new Dictionary<string, string>()
                {
                    { "Authorization", $"Bearer {token}" }
                }
            };

            return await httpClient.SendAsync(message);
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, string url, T body, string token)
        {
            var message = new HttpClientExtensionsParameters<T>()
            {
                Url = url,
                Method = HttpMethod.Post,
                Headers = new Dictionary<string, string>()
                {
                    { "Authorization", $"Bearer {token}" }
                },
                Body = body,
            };

            return await httpClient.SendAsync(message);
        }

        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient httpClient, string url, T body, string token)
        {
            var message = new HttpClientExtensionsParameters<T>()
            {
                Url = url,
                Method = HttpMethod.Patch,
                Headers = new Dictionary<string, string>()
                {
                    { "Authorization", $"Bearer {token}" }
                },
                Body = body,
            };

            return await httpClient.SendAsync(message);
        }
    }
}
