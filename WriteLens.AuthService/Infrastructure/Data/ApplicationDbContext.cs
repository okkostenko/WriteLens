using Microsoft.EntityFrameworkCore;
using Npgsql;
using WriteLens.Auth.Infrastructure.Data.Entities;

namespace WriteLens.Auth.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }

    public DbSet<UserEntity> Users { get; set; }
}