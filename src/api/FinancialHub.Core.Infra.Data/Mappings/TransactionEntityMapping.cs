using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Core.Infra.Data.Mappings
{
    internal class TransactionEntityMapping : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.Id).IsUnique(true);

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(500);
            builder.Property(t => t.Amount)
                .HasColumnType("money")
                .IsRequired();
            builder.Property(t => t.TargetDate)
                .HasColumnName("target_date")
                .IsRequired();
            builder.Property(t => t.FinishDate)
                .HasColumnName("finish_date");

            builder.Property(t => t.BalanceId)
                .HasColumnName("balance_id")
                .IsRequired();
            builder.HasOne(x => x.Balance)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.BalanceId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();
            builder.Navigation(t => t.Balance).AutoInclude();

            builder.Property(t => t.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CategoryId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();
            builder.Navigation(t => t.Category).AutoInclude();

            builder.Property(t => t.IsActive)
                .HasColumnName("active")
                .IsRequired();
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .IsRequired();
            builder.Property(t => t.Type)
                .HasColumnName("type")
                .IsRequired();

            builder.ToTable("transactions");
        }
    }
}
