using System.Net.Http.Headers;

namespace FinancialHub.Auth.IntegrationTests.Extensions.Utils
{
    public class HttpClientExtensionsParameters<T> : HttpClientExtensionsParameters
    {
        public T Body { init; get; }
    }

    public class HttpClientExtensionsParameters
    {
        public string Url { init; get; }
        public HttpMethod Method { init; get; }
        public Dictionary<string, string> Headers { init; get; }
    }
}
