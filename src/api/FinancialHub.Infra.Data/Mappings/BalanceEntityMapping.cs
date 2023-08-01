using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Infra.Data.Mappings
{
    internal class BalanceEntityMapping : IEntityTypeConfiguration<BalanceEntity>
    {
        public void Configure(EntityTypeBuilder<BalanceEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.Id).IsUnique(true);

            builder.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(200);
            builder.Property(t => t.Currency)
                .HasColumnName("currency")
                .HasMaxLength(50);
            builder.Property(t => t.Amount)
                .HasColumnType("money")
                .IsRequired();
            builder.Property(t => t.IsActive)
                .HasColumnName("active")
                .IsRequired();

            builder.Property(t => t.AccountId)
                .HasColumnName("account_id")
                .IsRequired();
            builder.HasOne(x => x.Account)
                .WithMany(x => x.Balances)
                .HasForeignKey(x => x.AccountId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();
            builder.Navigation(t => t.Account).AutoInclude();

            builder.ToTable("balances");
        }
    }
}
