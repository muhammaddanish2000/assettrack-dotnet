using System.Text;
using AssetTrack.Api.Data;
using AssetTrack.Api.DTOs;
using AssetTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTrack.Api.Services;

public class AssetService
{
    private readonly AssetDbContext _db;

    public AssetService(AssetDbContext db)
    {
        _db = db;
    }

    public async Task<List<Asset>> GetAssetsAsync(
        string? search,
        string? category,
        AssetStatus? status
    )
    {
        IQueryable<Asset> query =
            _db.Assets.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string keyword = search.Trim();

            query = query.Where(asset =>
                asset.Name.Contains(keyword) ||
                asset.AssetTag.Contains(keyword) ||
                asset.Category.Contains(keyword) ||
                asset.AssignedTo.Contains(keyword) ||
                asset.Location.Contains(keyword)
            );
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(asset =>
                asset.Category == category
            );
        }

        if (status.HasValue)
        {
            query = query.Where(asset =>
                asset.Status == status.Value
            );
        }

        return await query
            .OrderByDescending(asset => asset.UpdatedAt)
            .ToListAsync();
    }

    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _db.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(asset =>
                asset.Id == id
            );
    }

    public async Task<Asset> CreateAsync(
        AssetCreateRequest request
    )
    {
        string assetTag =
            request.AssetTag.Trim().ToUpperInvariant();

        bool exists =
            await _db.Assets.AnyAsync(asset =>
                asset.AssetTag == assetTag
            );

        if (exists)
        {
            throw new InvalidOperationException(
                "Asset tag already exists."
            );
        }

        DateTime now = DateTime.UtcNow;

        Asset asset = new()
        {
            Name = request.Name.Trim(),
            AssetTag = assetTag,
            Category = request.Category.Trim(),
            AssignedTo = request.AssignedTo.Trim(),
            Location = request.Location.Trim(),
            PurchaseDate = request.PurchaseDate,
            PurchasePrice = request.PurchasePrice,
            Status = request.Status,
            Notes = request.Notes.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Assets.Add(asset);

        await _db.SaveChangesAsync();

        return asset;
    }

    public async Task<Asset?> UpdateAsync(
        int id,
        AssetUpdateRequest request
    )
    {
        Asset? asset =
            await _db.Assets.FindAsync(id);

        if (asset is null)
        {
            return null;
        }

        string assetTag =
            request.AssetTag.Trim().ToUpperInvariant();

        bool duplicate =
            await _db.Assets.AnyAsync(other =>
                other.AssetTag == assetTag &&
                other.Id != id
            );

        if (duplicate)
        {
            throw new InvalidOperationException(
                "Asset tag already exists."
            );
        }

        asset.Name =
            request.Name.Trim();

        asset.AssetTag =
            assetTag;

        asset.Category =
            request.Category.Trim();

        asset.AssignedTo =
            request.AssignedTo.Trim();

        asset.Location =
            request.Location.Trim();

        asset.PurchaseDate =
            request.PurchaseDate;

        asset.PurchasePrice =
            request.PurchasePrice;

        asset.Status =
            request.Status;

        asset.Notes =
            request.Notes.Trim();

        asset.UpdatedAt =
            DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return asset;
    }

    public async Task<Asset?> UpdateStatusAsync(
        int id,
        AssetStatus status
    )
    {
        Asset? asset =
            await _db.Assets.FindAsync(id);

        if (asset is null)
        {
            return null;
        }

        asset.Status = status;
        asset.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return asset;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Asset? asset =
            await _db.Assets.FindAsync(id);

        if (asset is null)
        {
            return false;
        }

        _db.Assets.Remove(asset);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<object> GetSummaryAsync()
    {
        int total =
            await _db.Assets.CountAsync();

        int available =
            await _db.Assets.CountAsync(asset =>
                asset.Status ==
                AssetStatus.Available
            );

        int assigned =
            await _db.Assets.CountAsync(asset =>
                asset.Status ==
                AssetStatus.Assigned
            );

        int maintenance =
            await _db.Assets.CountAsync(asset =>
                asset.Status ==
                AssetStatus.Maintenance
            );

        int retired =
            await _db.Assets.CountAsync(asset =>
                asset.Status ==
                AssetStatus.Retired
            );

        decimal totalValue =
            await _db.Assets
                .SumAsync(asset =>
                    asset.PurchasePrice
                );

        int activeAssets =
            available +
            assigned +
            maintenance;

        double utilisationRate = 0;

        if (activeAssets > 0)
        {
            utilisationRate =
                (double)assigned /
                activeAssets *
                100;
        }

        return new
        {
            totalAssets = total,

            available,

            assigned,

            maintenance,

            retired,

            totalAssetValue =
                Math.Round(totalValue, 2),

            utilisationRate =
                Math.Round(
                    utilisationRate,
                    2
                )
        };
    }

    public async Task<object> GetCategoryBreakdownAsync()
    {
        var categories =
            await _db.Assets
                .AsNoTracking()
                .GroupBy(asset =>
                    asset.Category
                )
                .Select(group => new
                {
                    category = group.Key,

                    total =
                        group.Count(),

                    totalValue =
                        group.Sum(asset =>
                            asset.PurchasePrice
                        )
                })
                .OrderByDescending(item =>
                    item.total
                )
                .ToListAsync();

        return categories;
    }

    public async Task<byte[]> ExportCsvAsync()
    {
        List<Asset> assets =
            await _db.Assets
                .AsNoTracking()
                .OrderBy(asset =>
                    asset.AssetTag
                )
                .ToListAsync();

        StringBuilder csv = new();

        csv.AppendLine(
            "Id,AssetTag,Name,Category,Status,AssignedTo,Location,PurchaseDate,PurchasePrice"
        );

        foreach (Asset asset in assets)
        {
            csv.AppendLine(
                string.Join(
                    ",",
                    asset.Id,
                    Csv(asset.AssetTag),
                    Csv(asset.Name),
                    Csv(asset.Category),
                    Csv(asset.Status.ToString()),
                    Csv(asset.AssignedTo),
                    Csv(asset.Location),
                    Csv(
                        asset.PurchaseDate?
                            .ToString("yyyy-MM-dd")
                        ?? ""
                    ),
                    asset.PurchasePrice
                        .ToString("0.00")
                )
            );
        }

        return Encoding.UTF8.GetBytes(
            csv.ToString()
        );
    }

    private static string Csv(string value)
    {
        string escaped =
            value.Replace("\"", "\"\"");

        return $"\"{escaped}\"";
    }
}
