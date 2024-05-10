using Bogus.DataSets;
using FinancialHub.Common.Entities;
using FinancialHub.Core.Infra.Data.Extensions.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Infra.IntegrationTests.Setup
{
    public class FinancialHubInfraDataSetup : FinancialHubSetup
    {
        private readonly FinancialHubContext database;

        public FinancialHubInfraDataSetup() : base()
        {
            services.AddRepositories(configuration);

            serviceProvider = services.BuildServiceProvider();
            
            this.database   = serviceProvider.GetService<FinancialHubContext>()!;
        }

        public void CreateDatabase()
        {
            this.database.Database.EnsureCreated();
        }

        public void DropDatabase()
        {
            this.database.Database.EnsureDeleted();
        }

        public async Task<T> AddData<T>(T data) where T : BaseEntity
        {
            var context = this.serviceProvider.GetService<FinancialHubContext>()!;
            var entry = await context.Set<T>().AddAsync(data);
            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();
            return entry.Entity;
        }

        public async Task<T?> GetDataById<T>(Guid id) where T : BaseEntity
        {
            var context = this.serviceProvider.GetService<FinancialHubContext>()!;
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
