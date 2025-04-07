using System.ComponentModel.DataAnnotations;

namespace Fitness.Auth;

public class LoginModel
{
    [Required(ErrorMessage = "Никнейм обязательный")]
    [StringLength(100, ErrorMessage = "Некорректные данные")]
    public required string Username { get; set; }
    
    [Required(ErrorMessage = "Пароль обязательный")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Некорректные данные")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "Некорректные данные")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}