using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Common.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Auth.IntegrationTests.Setup
{
    public partial class FinancialHubAuthApiFixture
    {
        public T[] AddData<T>(params T[] data)
            where T : BaseEntity
        {
            using var scope = this.Api.Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
            context.Set<T>().AddRange(data);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var res = context.Set<T>().ToArray();
            return res.Where(x => data.Any(y => y.Id == x.Id)).ToArray();
        }

        public IEnumerable<T> GetData<T>()
            where T : BaseEntity
        {
            using var scope = this.Api.Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
            return context.Set<T>().ToArray();
        }

        public void CreateDatabase()
        {
            using var scope = this.Api.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
            db.Database.EnsureCreated();
        }

        public void ClearData()
        {
            using var scope = this.Api.Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FinancialHubAuthContext>();
            context.Database.EnsureDeleted();
        }
    }
}
