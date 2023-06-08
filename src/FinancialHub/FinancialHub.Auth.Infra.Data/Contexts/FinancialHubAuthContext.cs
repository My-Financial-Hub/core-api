using FinancialHub.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialHub.Auth.Infra.Data.Contexts
{
    public class FinancialHubAuthContext : DbContext
    {
        public FinancialHubAuthContext(DbContextOptions options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialHubAuthContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
