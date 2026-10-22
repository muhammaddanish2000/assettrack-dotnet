using AssetTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTrack.Api.Data;

public class AssetDbContext : DbContext
{
    public AssetDbContext(
        DbContextOptions<AssetDbContext> options
    ) : base(options)
    {
    }

    public DbSet<Asset> Assets => Set<Asset>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>()
            .HasIndex(asset => asset.AssetTag)
            .IsUnique();

        modelBuilder.Entity<Asset>()
            .Property(asset => asset.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Asset>()
            .Property(asset => asset.PurchasePrice)
            .HasPrecision(12, 2);
    }
}
