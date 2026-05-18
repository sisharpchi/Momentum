using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitScheduleFrequency : ValueObject
{
    public static HabitScheduleFrequency Daily => new(nameof(Daily));

    public static HabitScheduleFrequency Weekly => new(nameof(Weekly));

    public static HabitScheduleFrequency CustomWeekly => new(nameof(CustomWeekly));

    public string Value { get; }

    private HabitScheduleFrequency(string value)
    {
        Value = value;
    }
}
