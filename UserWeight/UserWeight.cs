using Newtonsoft.Json;

namespace Fitness.UserWeight;

public class UserWeight
{
    public Guid Id { get; set; }
    
    [JsonIgnore]
    public User.User User { get; set; }
    public Guid UserId { get; set; }
    public int Weight { get; set; }
    public DateTime CreationDate { get; set; }
}