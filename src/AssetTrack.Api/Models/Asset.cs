using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTrack.Api.Models;

public class Asset
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AssetTag { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(120)]
    public string AssignedTo { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Location { get; set; } = string.Empty;

    public DateOnly? PurchaseDate { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PurchasePrice { get; set; }

    public AssetStatus Status { get; set; } = AssetStatus.Available;

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
