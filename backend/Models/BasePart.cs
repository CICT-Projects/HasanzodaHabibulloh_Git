using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>
/// Абстрактный базовый класс для всех типов запчастей
/// </summary>
public abstract class BasePart
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Article { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Цена должна быть положительным числом")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
