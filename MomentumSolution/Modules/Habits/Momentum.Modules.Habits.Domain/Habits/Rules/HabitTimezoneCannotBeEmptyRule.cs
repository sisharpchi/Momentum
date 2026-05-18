using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class HabitTimezoneCannotBeEmptyRule : IBusinessRule
{
    private readonly string _timezone;

    internal HabitTimezoneCannotBeEmptyRule(string timezone)
    {
        _timezone = timezone;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_timezone);

    public string Message => "Habit schedule timezone cannot be empty.";
}
