using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Infra.Data.Mappings
{
    internal class TransactionEntityMapping : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> table)
        {
            table.HasKey(t => t.Id);
            table.HasIndex(t => t.Id).IsUnique(true);

            table.HasOne(x => x.Balance)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.BalanceId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired(true);
            table.Navigation(t => t.Balance).AutoInclude();

            table.HasOne(x => x.Category)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CategoryId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired(true);
            table.Navigation(t => t.Category).AutoInclude();
        }
    }
}
