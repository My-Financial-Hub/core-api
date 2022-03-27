using NUnit.Framework;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FinancialHub.IntegrationTests.Setup;
using System.Text;
using System;

namespace FinancialHub.IntegrationTests.Base
{
    public abstract class BaseControllerTests
    {
        protected readonly FinancialHubApiFixture fixture;
        protected HttpClient client => fixture.Client;

        public BaseControllerTests()
        {
            this.fixture = new FinancialHubApiFixture();
        }

        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.fixture.ClearData();
        }

        protected static HttpContent CreateContent<T>(T content)
        {
            return new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8,"application/json");
        }

        protected static async Task<T?> ReadContentAsync<T>(HttpContent content)
        {
            try
            {
                var stream = await content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<T>(stream,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (Exception e)
            {
                var json = await content.ReadAsStringAsync();
                Assert.Fail($"Not able to Read the content:\n{json}\n Exception :\n{e}");
                throw;
            }
        }

        protected async Task<T?> PostAsync<T>(string endpoint, T body)
        {
            var contentBody = CreateContent(body);
            var response = await this.client.PostAsync(endpoint, contentBody);

            return await ReadContentAsync<T>(response.Content);
        }

        protected async Task<T?> PutAsync<T>(string endpoint, T body)
        {
            var contentBody = CreateContent(body);
            var response = await this.client.PutAsync(endpoint, contentBody);

            return await ReadContentAsync<T>(response.Content);
        }

        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await this.client.GetAsync(endpoint);

            return await ReadContentAsync<T>(response.Content);
        }

        protected async Task<T?> DeleteAsync<T>(string endpoint)
        {
            var response = await this.client.DeleteAsync(endpoint);

            return await ReadContentAsync<T>(response.Content);
        }
    }
}
