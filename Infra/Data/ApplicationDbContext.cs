using Flunt.Notifications;
using IWantApp.Domain.Products;
using IWantApp.Infra.Data.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Não funciona devido ao GenericEntityTypeConfiguration
        //modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        //modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());

        new ProductEntityTypeConfiguration().Configure(modelBuilder.Entity<Product>());
        new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
    }
}
