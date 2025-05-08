using Newtonsoft.Json;

namespace Fitness.Sleep;

public class Sleep
{
    public Guid Id { get; set; }
    public User.User User { get; set; }
    public int Hours { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    
    [JsonIgnore]
    public Guid UserId { get; set; }
}