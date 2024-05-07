using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FinancialHub.Core.Infra.Extensions;
using FinancialHub.Core.Infra.Data.Extensions.Configurations;
using FinancialHub.Core.Infra.Caching.Extensions.Configurations;

namespace FinancialHub.Core.Infra.IntegrationTests.Setup
{
    public class FinancialHubInfraFixture : IEnumerable, IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly IServiceCollection services;
        private readonly IServiceProvider serviceProvider;

        public FinancialHubInfraFixture()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            services = new ServiceCollection();
            services
                .AddCoreInfra()
                .AddCaching(configuration)
                .AddRepositories(configuration)
                .AddLogging();

            serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return serviceProvider.GetServices<T>().First();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerator GetEnumerator()
        {
            yield return this;
        }
    }
}
