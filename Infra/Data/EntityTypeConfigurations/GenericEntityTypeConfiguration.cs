using IWantApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWantApp.Infra.Data.EntityTypeConfigurations;

public abstract class GenericEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Entity Mapping
        builder.HasKey(g => g.Id);

        // Property Mapping
        builder.Property(g => g.Id).HasColumnName("id").IsRequired();
        builder.Property(g => g.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(g => g.CreatedOn).HasColumnName("created_on").IsRequired();
        builder.Property(g => g.EditedBy).HasColumnName("edited_by");
        builder.Property(g => g.EditedOn).HasColumnName("edited_on");
    }
}
