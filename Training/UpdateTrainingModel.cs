using System.ComponentModel.DataAnnotations;

namespace Fitness.Training;

public class UpdateTrainingModel
{
    [Length(1, 50, ErrorMessage = "Название тренировки должно быть от 1 до 50 символов")]
    public string? Title { get; set; }
    
    [Length(50, 5000, ErrorMessage = "Описание тренировки должно быть от 50 до 5000 символов")]
    public string? Body { get; set; }
    
    public string? PreviewUrl { get; set; }
    
    public string? VideoUrl { get; set; }
}