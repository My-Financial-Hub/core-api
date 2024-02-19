using System.Net.Http;
using System.Text.Json;

namespace FinancialHub.Core.IntegrationTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T?> ReadContentAsync<T>(this HttpResponseMessage response)
        {
            try
            {
                var stream = await response.Content.ReadAsStreamAsync();

                return JsonSerializer.Deserialize<T>(stream,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    }
                );
            }
            catch (Exception e)
            {
                var json = await response.Content.ReadAsStringAsync();
                throw new Exception($"Not able to Read the content:\n{json}",e);
            }
        }
    }
}
