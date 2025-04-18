using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Fitness.Validators;

namespace Fitness.User;

public class UpdateUserModel
{
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Никнейм должен быть от 4 до 100 символов")]
    public string? Username { get; set; }
    
    [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public string? FirstName { get; set; }

    public UserSex? Sex { get; set; }

    [ValidBirthDate(MinimumAge = 16, MaximumAge = 120, ErrorMessage = "Возраст может быть от 16 до 120 лет")]
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }

    [Range(10, 300, ErrorMessage = "Рост может быть от 10 до 300 сантиметров")]
    public int? Height { get; set; }

    [Range(10, 500, ErrorMessage = "Вес может быть от 10 до 500 килограммов")]
    public int? Weight { get; set; }
    
    // Goal

    public string? GoalType { get; set; }
    
    [Range(10, 500, ErrorMessage = "Вес может быть от 10 до 500 килограммов")]
    public int? GoalWeight { get; set; }

    public string? Avatar { get; set; }
}