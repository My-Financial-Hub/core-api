using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialHub.Infra.Data.Mappings
{
    internal class CategoryEntityMapping : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(t => t.IsActive)
                .HasColumnName("active");

            builder.ToTable("categories");
        }
    }
}
