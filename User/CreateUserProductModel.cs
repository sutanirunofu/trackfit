using System.ComponentModel.DataAnnotations;

namespace Fitness.User;

public class CreateUserProductModel
{
    [Required(ErrorMessage = "Название продукта обязательно")]
    public string Name { get; set; }
    
    [Range(0, 100_000, ErrorMessage = "Калории продукта обязательны")]
    public float? Calories { get; set; }      // Каллории на 100 грамм
    
    [Range(0, 100_000, ErrorMessage = "Белки продукта обязательные")]
    public float? Proteins { get; set; }      // Белки на 100 грамм
    
    [Range(0, 100_000, ErrorMessage = "Жиры продукта обязательные")]
    public float? Fats { get; set; }          // Жиры на 100 грамм
    
    [Range(0, 100_000, ErrorMessage = "Углеводы продукта обязательные")]
    public float? Carbohydrates { get; set; } // Углеводы на 100 грамм
}