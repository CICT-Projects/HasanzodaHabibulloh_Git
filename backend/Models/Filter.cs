using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// Фильтры воздушные, масляные и салонные
/// </summary>
public class Filter : BasePart
{
    [Required]
    [StringLength(100)]
    public string FilterType { get; set; } = string.Empty; // Например: Воздушный, Масляный, Салонный

    [Required]
    [StringLength(200)]
    public string ApprovedSize { get; set; } = string.Empty; // Размер, совместимость
}
