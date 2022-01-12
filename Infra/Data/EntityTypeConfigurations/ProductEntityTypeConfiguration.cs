using IWantApp.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWantApp.Infra.Data.EntityTypeConfigurations;

public class ProductEntityTypeConfiguration : GenericEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Entity Mapping
        builder.ToTable("tbl_products");

        // Property Mapping
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(p => p.Description).HasColumnName("description").HasMaxLength(400).IsRequired();
        builder.Property(p => p.HasStock).HasColumnName("has_stock").IsRequired();
        builder.Property(p => p.Active).HasColumnName("has_active").IsRequired();
        builder.Property(p => p.CategoryId).HasColumnName("category_id").IsRequired();

        // Relationship Mapping
        builder.HasOne<Category>(p => p.Category)
            .WithOne(c => c.Product)
            .HasForeignKey<Product>(p => p.CategoryId);

        base.Configure(builder);
    }
}
