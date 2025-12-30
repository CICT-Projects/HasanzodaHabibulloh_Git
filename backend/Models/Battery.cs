using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// Аккумуляторы для автомобилей
/// </summary>
public class Battery : BasePart
{
    [Required]
    [Range(1, 100, ErrorMessage = "Напряжение должно быть между 1 и 100V")]
    public int VoltageV { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "Емкость должна быть между 1 и 10000 Ач")]
    public int CapacityAh { get; set; }
}
