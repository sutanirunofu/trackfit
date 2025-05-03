namespace Fitness.Training;

public class Training
{
    public Guid Id { get; set; }
    public User.User Author { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string PreviewUrl { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
}