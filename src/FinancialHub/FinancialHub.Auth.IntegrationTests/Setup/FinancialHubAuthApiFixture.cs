using System.Collections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using FinancialHub.Auth.WebApi;

namespace FinancialHub.Auth.IntegrationTests.Setup
{
    public partial class FinancialHubAuthApiFixture : IEnumerable, IDisposable
    {
        public HttpClient Client { get; protected set; }
        public WebApplicationFactory<Program> Api { get; protected set; }

        public FinancialHubAuthApiFixture()
        {
            this.Api = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder.UseEnvironment("Testing");
                    }
                );

            this.Client = this.Api.CreateClient();
        }

        public void Dispose()
        {
            this.Api.Dispose();
            this.Client.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerator GetEnumerator()
        {
            yield return this;
        }
    }
}
