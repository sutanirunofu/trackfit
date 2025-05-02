using System.ComponentModel.DataAnnotations;

namespace Fitness.Recipe;

public class AddRecipeModel
{
    [Required(ErrorMessage = "Название рецепта обязательно")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Описание рецепта обязательно")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Изображение рецепта обязатеьно")]
    public string ImageUrl { get; set; }
}