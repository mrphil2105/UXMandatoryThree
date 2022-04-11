using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ShopItem> ShopItems { get; set; } = null!;

    public DbSet<ShopCategory> ShopCategories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // User

        builder.Entity<AppUser>()
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<AppUser>()
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        // Shop

        builder.Entity<ShopItem>()
            .Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<ShopItem>()
            .HasIndex(i => i.Name)
            .IsUnique();

        builder.Entity<ShopCategory>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<ShopCategory>()
            .HasIndex(c => c.Name)
            .IsUnique();
    }
}
