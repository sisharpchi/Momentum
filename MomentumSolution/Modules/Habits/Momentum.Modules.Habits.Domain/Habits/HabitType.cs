using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitType : ValueObject
{
    public static HabitType Good => new(nameof(Good));

    public static HabitType Bad => new(nameof(Bad));

    public static HabitType Time => new(nameof(Time));

    public static HabitType Count => new(nameof(Count));

    public static HabitType Health => new(nameof(Health));

    public string Value { get; }

    private HabitType(string value)
    {
        Value = value;
    }
}
