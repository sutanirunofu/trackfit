using Newtonsoft.Json;

namespace Fitness.Achievement;

public class Achievement
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }

    [JsonIgnore]
    public List<User.User> Users { get; set; }
}