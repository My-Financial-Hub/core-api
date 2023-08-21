using System.Diagnostics.CodeAnalysis;

namespace FinancialHub.Core.Infra.Data.Contexts
{
    public class FinancialHubContext : DbContext
    {
        public FinancialHubContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialHubContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<BalanceEntity> Balances { get; set; }
    }
}
