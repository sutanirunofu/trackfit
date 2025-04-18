using Newtonsoft.Json;

namespace Fitness.Diet;

public class Diet
{
    public Guid Id { get; set; }
    
    public DietType.DietType Type { get; set; }
    
    [JsonIgnore]
    public User.User User { get; set; }
    public Guid UserId { get; set; }
    public Product.Product Product { get; set; }
    public float Weight { get; set; }
    public DateTime CreationDate { get; set; }
}