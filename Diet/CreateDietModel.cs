using System.ComponentModel.DataAnnotations;

namespace Fitness.Diet;

public class CreateDietModel
{
    [Required(ErrorMessage = "Тип приема пищи обязательный")]
    public int Type { get; set; }
    
    [Required(ErrorMessage = "ID продукта обязательный")]
    public Guid ProductId { get; set; }
    
    [Required(ErrorMessage = "Вес продукта обязательный")]
    [Range(1, 100_000, ErrorMessage = "Вес продукта должен быть от 1 до 100000 грамм")]
    public float Weight { get; set; }
}