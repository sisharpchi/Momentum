using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.Habits.Rules;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitTarget : ValueObject
{
    public decimal Value { get; }

    public string Unit { get; }

    private HabitTarget(decimal value, string unit)
    {
        CheckRule(new HabitTargetMustBePositiveRule(value));
        CheckRule(new HabitTargetUnitCannotBeEmptyRule(unit));

        Value = value;
        Unit = unit.Trim();
    }

    public static HabitTarget Count(decimal value, string unit) => new(value, unit);

    public static HabitTarget Time(decimal minutes) => new(minutes, "min");
}
