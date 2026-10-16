using System.ComponentModel.DataAnnotations;
using AssetTrack.Api.Models;

namespace AssetTrack.Api.DTOs;

public class AssetCreateRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string AssetTag { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(120)]
    public string AssignedTo { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Location { get; set; } = string.Empty;

    public DateOnly? PurchaseDate { get; set; }

    [Range(0, 999999999)]
    public decimal PurchasePrice { get; set; }

    public AssetStatus Status { get; set; } = AssetStatus.Available;

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;
}
