namespace Fitness.Diet;

public class Diet
{
    public Guid Id { get; set; }
    public User.User User { get; set; }
    public Product.Product Product { get; set; }
    public float Weight { get; set; }
    public DateTime CreationDate { get; set; }
}