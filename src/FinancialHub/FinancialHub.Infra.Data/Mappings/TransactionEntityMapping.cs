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

            table.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(500);
            table.Property(t => t.Amount)
                .HasColumnType("money")
                .IsRequired();
            table.Property(t => t.TargetDate)
                .HasColumnName("target_date")
                .IsRequired();
            table.Property(t => t.FinishDate)
                .HasColumnName("finish_date");

            table.Property(t => t.BalanceId)
                .HasColumnName("balance_id")
                .IsRequired(true);
            table.HasOne(x => x.Balance)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.BalanceId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired(true);
            table.Navigation(t => t.Balance).AutoInclude();

            table.Property(t => t.CategoryId)
                .HasColumnName("category_id")
                .IsRequired(true);
            table.HasOne(x => x.Category)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CategoryId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired(true);
            table.Navigation(t => t.Category).AutoInclude();

            table.Property(t => t.IsActive)
                .HasColumnName("active")
                .IsRequired(true);
            table.Property(t => t.Status)
                .HasColumnName("status")
                .IsRequired(true);
            table.Property(t => t.Type)
                .HasColumnName("type")
                .IsRequired(true);

            table.ToTable("transactions");
        }
    }
}
