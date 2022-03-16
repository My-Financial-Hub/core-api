using FinancialHub.Infra.Data.Contexts;
using FinancialHub.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialHub.IntegrationTests.Base
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient client;
        protected readonly FinancialHubContext database;

        public BaseControllerTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinancialHubContext>();
            optionsBuilder.UseInMemoryDatabase("test");
            this.database = new FinancialHubContext(optionsBuilder.Options); 

            var api = new WebApplicationFactory<Startup>().WithWebHostBuilder(
                builder =>
                {
                    builder.ConfigureServices(services => {
                        services.Remove(
                            new ServiceDescriptor(
                                typeof(FinancialHubContext), typeof(FinancialHubContext)
                            )
                        );

                        services.Configure<FinancialHubContext>(ctx =>
                        {
                            ctx = this.database;
                        });
                    });
                }
            );

            using (var scope = api.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubContext>();
                context.Database.EnsureCreated();
            }

            this.client = api.CreateClient();
        }

        protected static HttpContent CreateContent<T>(T content)
        {
            return new StringContent(JsonSerializer.Serialize(content));
        }

        protected static async Task<T?> ReadContentAsync<T>(HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(stream,
                new JsonSerializerOptions() { 
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        protected async Task<Y?> PostAsync<T, Y>(string endpoint, Y body)
        {
            var contentBody = CreateContent(body);
            var response = await this.client.PostAsync(endpoint, contentBody);

            return await ReadContentAsync<Y>(response.Content);
        }

        protected async Task<Y?> PutAsync<T, Y>(string endpoint, Y body)
        {
            var contentBody = CreateContent(body);
            var response = await this.client.PutAsync(endpoint, contentBody);

            return await ReadContentAsync<Y>(response.Content);
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
