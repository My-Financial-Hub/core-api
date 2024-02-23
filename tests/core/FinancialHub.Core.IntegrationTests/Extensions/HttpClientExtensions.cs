using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FinancialHub.Core.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpContent CreateContent<T>(T content)
        {
            var serializedContent = 
                JsonSerializer.Serialize(
                    content, 
                    new JsonSerializerOptions() { 
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    }
               );

            return new StringContent(serializedContent, Encoding.UTF8, "application/json");
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string requestUri, T? content)
        {
            var json = CreateContent(content);
            return await client.PostAsync(requestUri, json);
        }

        public static async Task<HttpResponseMessage> PutAsync<T>(this HttpClient client, string requestUri, T? content)
        {
            var json = CreateContent(content);
            return await client.PutAsync(requestUri, json);
        }
    }
}
