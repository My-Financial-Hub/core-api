using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace FinancialHub.Core.Domain.Tests.Setup
{
    public abstract class FinancialHubSetup
    {
        protected readonly IConfiguration configuration;
        protected readonly IServiceCollection services;
        protected IServiceProvider serviceProvider;

        protected FinancialHubSetup()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services = new ServiceCollection();
        }

        public T GetService<T>() where T: notnull
        {
            return this.serviceProvider.GetRequiredService<T>();
        }

        public abstract void TearUp();
        public abstract void TearDown();
    }
}
