using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.HabitGroups.Rules;

internal class HabitGroupNameCannotBeEmptyRule : IBusinessRule
{
    private readonly string _name;

    internal HabitGroupNameCannotBeEmptyRule(string name)
    {
        _name = name;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_name);

    public string Message => "Habit group name cannot be empty.";
}
