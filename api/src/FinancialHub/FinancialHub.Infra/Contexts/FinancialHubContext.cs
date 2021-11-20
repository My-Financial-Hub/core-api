using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FinancialHub.Infra.Contexts
{
    public class FinancialHubContext : DbContext
    {
        public FinancialHubContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionEntity>(table =>
            {
                table.HasOne(x => x.Account)
                    .WithMany(x => x.Transactions)
                    .HasForeignKey(x => x.AccountId)
                    .IsRequired(true);

                table.HasOne(x => x.Category)
                    .WithMany(x => x.Transactions)
                    .HasForeignKey(x => x.CategoryId)
                    .IsRequired(true);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
