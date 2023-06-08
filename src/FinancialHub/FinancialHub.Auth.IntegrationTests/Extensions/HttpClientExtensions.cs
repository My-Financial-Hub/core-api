using System.Text;
using System.Text.Json;

namespace FinancialHub.Auth.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpContent ToHttpContent<T>(this T content)
        {
            return new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        }
    }
}
