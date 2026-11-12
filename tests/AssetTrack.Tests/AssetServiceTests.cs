using AssetTrack.Api.Data;
using AssetTrack.Api.DTOs;
using AssetTrack.Api.Models;
using AssetTrack.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace AssetTrack.Tests;

public class AssetServiceTests
{
    private static AssetDbContext
        CreateDatabase()
    {
        DbContextOptions<AssetDbContext>
            options =
                new DbContextOptionsBuilder<
                    AssetDbContext
                >()
                .UseInMemoryDatabase(
                    Guid.NewGuid()
                        .ToString()
                )
                .Options;

        return new AssetDbContext(
            options
        );
    }


    [Fact]
    public async Task CreateAsset_ShouldSaveAsset()
    {
        await using AssetDbContext db =
            CreateDatabase();

        AssetService service =
            new(db);

        AssetCreateRequest request =
            new()
            {
                Name =
                    "Test Laptop",

                AssetTag =
                    "TEST-001",

                Category =
                    "Laptop",

                Location =
                    "IT Office",

                PurchasePrice =
                    3000,

                Status =
                    AssetStatus.Available
            };

        Asset result =
            await service.CreateAsync(
                request
            );

        Assert.NotEqual(
            0,
            result.Id
        );

        Assert.Equal(
            "TEST-001",
            result.AssetTag
        );

        Assert.Single(
            db.Assets
        );
    }


    [Fact]
    public async Task DuplicateAssetTag_ShouldFail()
    {
        await using AssetDbContext db =
            CreateDatabase();

        AssetService service =
            new(db);

        AssetCreateRequest request =
            new()
            {
                Name =
                    "Laptop",

                AssetTag =
                    "IT-001",

                Category =
                    "Laptop"
            };

        await service.CreateAsync(
            request
        );

        await Assert.ThrowsAsync<
            InvalidOperationException
        >(
            () =>
                service.CreateAsync(
                    request
                )
        );
    }


    [Fact]
    public async Task DeleteAsset_ShouldRemoveAsset()
    {
        await using AssetDbContext db =
            CreateDatabase();

        AssetService service =
            new(db);

        Asset asset =
            await service.CreateAsync(
                new AssetCreateRequest
                {
                    Name =
                        "Test Monitor",

                    AssetTag =
                        "MON-001",

                    Category =
                        "Monitor"
                }
            );

        bool deleted =
            await service.DeleteAsync(
                asset.Id
            );

        Assert.True(
            deleted
        );

        Assert.Empty(
            db.Assets
        );
    }


    [Fact]
    public async Task UpdateStatus_ShouldChangeStatus()
    {
        await using AssetDbContext db =
            CreateDatabase();

        AssetService service =
            new(db);

        Asset asset =
            await service.CreateAsync(
                new AssetCreateRequest
                {
                    Name =
                        "Office Laptop",

                    AssetTag =
                        "LAP-100",

                    Category =
                        "Laptop",

                    Status =
                        AssetStatus.Available
                }
            );

        Asset? updated =
            await service
                .UpdateStatusAsync(
                    asset.Id,
                    AssetStatus.Assigned
                );

        Assert.NotNull(
            updated
        );

        Assert.Equal(
            AssetStatus.Assigned,
            updated!.Status
        );
    }
}
