using System.ComponentModel.DataAnnotations;

namespace Fitness.Product;

public class Product
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Название продукта обязательно")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Каллории продукта обязательные")]
    public float Calories { get; set; }      // Каллории на 100 грамм
    
    [Required(ErrorMessage = "Белки продукта обязательные")]
    public float Proteins { get; set; }      // Белки на 100 грамм
    
    [Required(ErrorMessage = "Жиры продукта обязательные")]
    public float Fats { get; set; }          // Жиры на 100 грамм
    
    [Required(ErrorMessage = "Углеводы продукта обязательные")]
    public float Carbohydrates { get; set; } // Углеводы на 100 грамм
    
    public DateTime CreationDate { get; set; }
}