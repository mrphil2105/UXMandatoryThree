using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

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
    }
}
