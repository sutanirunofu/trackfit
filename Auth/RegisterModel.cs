using System.ComponentModel.DataAnnotations;
using Fitness.User;
using Fitness.Validators;

namespace Fitness.Auth;

public class RegisterModel
{
    [Required(ErrorMessage = "Никнейм обязательный")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Никнейм должен быть от 4 до 100 символов")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Пароль обязательный")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 100 символов")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Пароль должен содержать заглавные, строчные буквы, цифры и спецсимволы")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Пол обязательный")]
    public required UserSex Sex { get; set; }

    [Required(ErrorMessage = "Дата рождения обязательна")]
    [ValidBirthDate(MinimumAge = 16, MaximumAge = 120, ErrorMessage = "Возраст может быть от 16 до 120 лет")]
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    [Required(ErrorMessage = "Рост обязательный")]
    [Range(10, 300, ErrorMessage = "Рост может быть от 10 до 300 сантиметров")]
    public required int Height { get; set; }

    [Required(ErrorMessage = "Вес обязательный")]
    [Range(10, 500, ErrorMessage = "Вес может быть от 10 до 500 килограммов")]
    public required int Weight { get; set; }

    // Goal

    [Required(ErrorMessage = "Тип цели обязателен")]
    public required string GoalType { get; set; }

    [Required(ErrorMessage = "Цель веса обязательна")]
    [Range(10, 500, ErrorMessage = "Вес может быть от 10 до 500 килограммов")]
    public required int GoalWeight { get; set; }
}