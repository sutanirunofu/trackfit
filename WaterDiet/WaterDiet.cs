using Newtonsoft.Json;

namespace Fitness.WaterDiet;

public class WaterDiet
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public User.User User { get; set; }

    public decimal Count { get; set; }

    public DateTime CreationDate { get; set; }
}