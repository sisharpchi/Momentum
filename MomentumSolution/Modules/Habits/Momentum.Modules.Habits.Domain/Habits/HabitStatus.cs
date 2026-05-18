using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitStatus : ValueObject
{
    public static HabitStatus Active => new(nameof(Active));

    public static HabitStatus Paused => new(nameof(Paused));

    public static HabitStatus Archived => new(nameof(Archived));

    public string Value { get; }

    private HabitStatus(string value)
    {
        Value = value;
    }
}
