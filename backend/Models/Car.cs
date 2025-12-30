namespace backend.Models;

using System.ComponentModel.DataAnnotations;

public class Car
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
    
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }
    
    [StringLength(50)]
    public string Color { get; set; } = string.Empty;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
