using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Auth.Infra.Data.Mapping
{
    internal class UserCredentialEntityMapping : IEntityTypeConfiguration<CredentialEntity>
    {
        public void Configure(EntityTypeBuilder<CredentialEntity> builder)
        {
            builder.Property(x => x.Login)
                .HasColumnName("login")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasColumnName("password")
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();
            builder.HasOne(x => x.User);

            builder.ToTable("user_credential");
        }
    }
}
