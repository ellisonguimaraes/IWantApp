using Flunt.Notifications;
using IWantApp.Domain.Products;
using IWantApp.Infra.Data.EntityTypeConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Não funciona devido ao GenericEntityTypeConfiguration
        //modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        //modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.Ignore<Notification>();

        new ProductEntityTypeConfiguration().Configure(modelBuilder.Entity<Product>());
        new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
    }
}
