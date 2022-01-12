using IWantApp.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWantApp.Infra.Data.EntityTypeConfigurations;

public class CategoryEntityTypeConfiguration : GenericEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Entity Mapping
        builder.ToTable("tbl_categories");

        // Property Mapping
        builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(c => c.Active).HasColumnName("has_active").IsRequired();

        base.Configure(builder);
    }
}
