using Newtonsoft.Json;

namespace Fitness.User;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }
    
    [JsonIgnore]
    public string Password { get; set; }
    
    public string FirstName { get; set; }

    public UserSex Sex { get; set; }

    public DateTime Birthday { get; set; }

    public int Height { get; set; }

    public List<UserWeight.UserWeight> Weights { get; set; }

    public Goal.Goal Goal { get; set; }
    
    public Guid GoalId { get; set; }

    public string? Avatar { get; set; }
    
    public List<Diet.Diet> Diets { get; set; }

    public List<WaterDiet.WaterDiet> WaterDiets { get; set; }

    public List<Product.Product> Products { get; set; }

    public DateTime RegistrationDate { get; set; }
    
    public DateTime ModificationDate { get; set; }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - Birthday.Year;
            
            if (Birthday.Date > today.AddYears(-age)) 
                age--;

            return age;
        }
    }
    
    public List<Achievement.Achievement> Achievements { get; set; }

    [JsonIgnore]
    public List<Sleep.Sleep> Sleeps { get; set; }
}