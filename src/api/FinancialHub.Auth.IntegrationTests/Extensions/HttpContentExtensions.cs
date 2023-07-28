using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace FinancialHub.Auth.IntegrationTests.Extensions
{
    public static class HttpContentExtensions
    {
        public static HttpContent ToHttpContent<T>(this T content)
        {
            return new StringContent(
                content: JsonSerializer.Serialize(content), 
                encoding: Encoding.UTF8, 
                mediaType: MediaTypeNames.Application.Json
            );
        }
    }
}
