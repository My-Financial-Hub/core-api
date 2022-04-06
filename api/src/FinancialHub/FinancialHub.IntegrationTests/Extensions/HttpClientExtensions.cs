using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialHub.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpContent CreateContent<T>(T content)
        {
            return new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string requestUri, T? content)
        {
            return await client.PostAsync(requestUri, CreateContent(content));
        }

        public static async Task<HttpResponseMessage> PutAsync<T>(this HttpClient client, string requestUri, T? content)
        {
            return await client.PutAsync(requestUri, CreateContent(content));
        }
    }
}
