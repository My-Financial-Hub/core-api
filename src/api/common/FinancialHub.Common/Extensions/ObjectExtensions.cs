using System.Text.Json;

namespace FinancialHub.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(
                    obj,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    }
               );
        }

        public static T? FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                }
            );
        }
    }
}
