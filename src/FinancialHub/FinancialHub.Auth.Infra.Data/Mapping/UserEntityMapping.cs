using FinancialHub.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Auth.Infra.Data.Mapping
{
    internal class UserEntityMapping : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(t => t.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(t => t.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(300);

            builder.Property(t => t.BirthDate)
                .HasColumnName("birth_name");

            builder.Property(t => t.Email)
                .HasColumnName("email")
                .HasMaxLength(300)
                .IsRequired();

            builder.ToTable("users");
        }
    }
}
