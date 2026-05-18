using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class HabitTargetUnitCannotBeEmptyRule : IBusinessRule
{
    private readonly string _unit;

    internal HabitTargetUnitCannotBeEmptyRule(string unit)
    {
        _unit = unit;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_unit);

    public string Message => "Habit target unit cannot be empty.";
}
