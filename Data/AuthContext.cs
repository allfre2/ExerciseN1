using AuthService.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class AuthContext : IdentityDbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<RefreshToken>().HasIndex(t => t.Value).IsUnique();
        builder.Entity<RefreshToken>().HasIndex(t => t.UserName);
        builder.Entity<ApiKey>().HasIndex(k => k.Key).IsUnique();

        base.OnModelCreating(builder);
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
}
