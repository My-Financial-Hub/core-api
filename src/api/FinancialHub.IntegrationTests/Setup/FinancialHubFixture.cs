using System.Net.Http;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.WebApi;
using FinancialHub.Core.Infra.Data.Contexts;

namespace FinancialHub.IntegrationTests.Setup
{
    public class FinancialHubApiFixture : IEnumerable, IDisposable
    {
        public HttpClient Client { get; protected set; }
        public WebApplicationFactory<Startup> Api { get; protected set; }

        public FinancialHubApiFixture()
        {
            this.Api = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            services.AddTestDbContext<FinancialHubContext>();
                        });
                    }
                );

            this.Client = this.Api.CreateClient();
        }

        public T[] AddData<T>(params T[] data)
            where T : BaseEntity
        {
            using (var scope = this.Api.Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubContext>();
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
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubContext>();
                return context.Set<T>().ToArray();
            }
        }

        public void CreateDatabase()
        {
            using(var scope = this.Api.Services.CreateScope())
            {
                //TODO: run migrations
                var db = scope.ServiceProvider.GetRequiredService<FinancialHubContext>();
                db.Database.EnsureCreated();
            }
        }

        public void ClearData()
        {
            using (var scope = this.Api.Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubContext>();
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
