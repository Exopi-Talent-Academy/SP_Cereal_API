using Cereal_API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Cereal_API.API.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public const string ConnectionPath = "Data Source=database.db";

    public DbSet<CerealData> Cereals => Set<CerealData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CerealData>().Property(c => c.Manufacturer).HasConversion<string>();
        modelBuilder.Entity<CerealData>().Property(c => c.Type).HasConversion<string>();
    }
}
