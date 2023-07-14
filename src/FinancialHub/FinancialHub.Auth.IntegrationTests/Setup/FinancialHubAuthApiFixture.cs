using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Domain.Entities;
using System.Collections;
using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Auth.WebApi;
using Microsoft.AspNetCore.Hosting;

namespace FinancialHub.Auth.IntegrationTests.Setup
{
    public class FinancialHubAuthApiFixture : IEnumerable, IDisposable
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

        public T[] AddData<T>(params T[] data)
            where T : BaseEntity
        {
            using (var scope = this.Api.Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
                context.Set<T>().AddRange(data);
                context.SaveChanges();

                context.ChangeTracker.Clear();

                var res = context.Set<T>().ToArray();
                return res.Where(x => data.Any(y => y.Id == x.Id)).ToArray();
            }
        }

        public IEnumerable<T> GetData<T>()
            where T : BaseEntity
        {
            using (var scope = this.Api.Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
                return context.Set<T>().ToArray();
            }
        }

        public void CreateDatabase()
        {
            using(var scope = this.Api.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
                db.Database.EnsureCreated();
            }
        }

        public void ClearData()
        {
            using (var scope = this.Api.Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
                context.Database.EnsureDeleted();
            }
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
