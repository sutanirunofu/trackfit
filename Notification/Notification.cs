namespace Fitness.Notification;

public class Notification
{
    public Guid Id { get; set; }
    public User.User User { get; set; }
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreationDate { get; set; }
}