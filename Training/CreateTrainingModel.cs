using System.ComponentModel.DataAnnotations;

namespace Fitness.Training;

public class CreateTrainingModel
{
    [Required(ErrorMessage = "Название тренировки обязательно")]
    [Length(2, 50, ErrorMessage = "Название тренировки должно быть от 2 до 50 символов")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Описание тренировки обязательно")]
    [Length(50, 5000, ErrorMessage = "Описание тренировки должно быть от 50 до 5000 символов")]
    public string Body { get; set; }
    
    [Required(ErrorMessage = "Превью тренировки обязательно")]
    public string PreviewUrl { get; set; }
    
    public string? VideoUrl { get; set; }
}