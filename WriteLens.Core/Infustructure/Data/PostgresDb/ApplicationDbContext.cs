using Microsoft.EntityFrameworkCore;
using Npgsql;
using WriteLens.Core.Infrastructure.Data.PostgresDb.Entities;

namespace WriteLens.Core.Infrastructure.Data.PostgresDb;

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
    public DbSet<DocumentEntity> Documents { get; set; }
}