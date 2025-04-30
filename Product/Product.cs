using System.ComponentModel.DataAnnotations;

namespace Fitness.Product;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Calories { get; set; }      // Каллории на 100 грамм
    public float Proteins { get; set; }      // Белки на 100 грамм
    public float Fats { get; set; }          // Жиры на 100 грамм
    public float Carbohydrates { get; set; } // Углеводы на 100 грамм
    public DateTime CreationDate { get; set; }
}