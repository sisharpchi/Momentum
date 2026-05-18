using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class HabitTargetMustBePositiveRule : IBusinessRule
{
    private readonly decimal _value;

    internal HabitTargetMustBePositiveRule(decimal value)
    {
        _value = value;
    }

    public bool IsBroken() => _value <= 0;

    public string Message => "Habit target value must be greater than zero.";
}
