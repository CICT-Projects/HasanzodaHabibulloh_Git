using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// Моторные масла и жидкости
/// </summary>
public class Oil : BasePart
{
    [Required]
    [StringLength(50)]
    public string Viscosity { get; set; } = string.Empty; // Например: 5W-30, 10W-40

    [Required]
    [StringLength(100)]
    public string OilType { get; set; } = string.Empty; // Например: Синтетическое, Полусинтетическое
}
