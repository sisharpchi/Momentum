using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class HabitTitleCannotBeEmptyRule : IBusinessRule
{
    private readonly string _title;

    internal HabitTitleCannotBeEmptyRule(string title)
    {
        _title = title;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_title);

    public string Message => "Habit title cannot be empty.";
}
