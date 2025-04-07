using System.ComponentModel.DataAnnotations;

namespace Fitness.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class ValidBirthDateAttribute : ValidationAttribute
{
    public int MinimumAge { get; set; } = 13;
    public int MaximumAge { get; set; } = 120;

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            
            if (birthDate.Date > today.AddYears(-age)) 
                age--;

            if (birthDate > today)
                return new ValidationResult("День рождения не может быть в будущем");
            
            if (age < MinimumAge)
                return new ValidationResult($"Возраст должен быть минимум {MinimumAge} лет");
                
            if (age > MaximumAge)
                return new ValidationResult($"Возраст не может быть больше {MaximumAge} лет");
        }

        return ValidationResult.Success;
    }
}