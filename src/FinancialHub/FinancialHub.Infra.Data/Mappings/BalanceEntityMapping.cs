using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Infra.Data.Mappings
{
    internal class BalanceEntityMapping : IEntityTypeConfiguration<BalanceEntity>
    {
        public void Configure(EntityTypeBuilder<BalanceEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.Id).IsUnique(true);

            builder.HasOne(x => x.Account)
                    .WithMany(x => x.Balances)
                    .HasForeignKey(x => x.AccountId)
                    .HasPrincipalKey(x => x.Id)
                    .IsRequired(true);
            builder.Navigation(t => t.Account).AutoInclude();
        }
    }
}
