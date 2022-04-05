using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FinancialHub.WebApi;
using FinancialHub.Infra.Data.Contexts;
using FinancialHub.Domain.Entities;
using System.Collections;
using Microsoft.Data.Sqlite;

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
                            var dbContextOptions = services.SingleOrDefault(
                                d => d.ServiceType == typeof(DbContextOptions<FinancialHubContext>)
                            );
                            services.Remove(dbContextOptions!);

                            services.AddDbContext<FinancialHubContext>(options =>
                            {
                                options.UseInMemoryDatabase("123");
                                options.EnableSensitiveDataLogging(true);
                                //TODO: remove it
                                //https://stackoverflow.com/questions/482827/database-data-needed-in-integration-tests-created-by-api-calls-or-using-importe
                                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            });
                        });
                    }
                );

            this.Client = this.Api.CreateClient();
        }

        public void AddData<T>(params T[] data)
            where T : BaseEntity
        {
            using (var scope = this.Api.Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinancialHubContext>();
                context.Set<T>().AddRange(data);
                context.SaveChanges();
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
