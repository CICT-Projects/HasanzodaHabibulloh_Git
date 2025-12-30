using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// Шины для автомобилей
/// </summary>
public class Tire : BasePart
{
    [Required]
    [Range(1, 100, ErrorMessage = "Диаметр должен быть между 1 и 100")]
    public int DiameterInches { get; set; }

    [Required]
    [StringLength(100)]
    public string ProfileDescription { get; set; } = string.Empty;
}
