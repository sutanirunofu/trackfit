namespace Fitness.Goal;

public class Goal
{
    public Guid Id { get; set; }

    public GoalType.GoalType Type { get; set; }

    public int Weight { get; set; }
}