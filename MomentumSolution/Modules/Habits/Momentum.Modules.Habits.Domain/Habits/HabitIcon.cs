using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitIcon : ValueObject
{
    public string Type { get; }

    public string Value { get; }

    private HabitIcon()
    {
        Type = string.Empty;
        Value = string.Empty;
    }

    private HabitIcon(string type, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleValidationException(new HabitIconValueCannotBeEmptyRule());
        }

        Type = type;
        Value = value.Trim();
    }

    public static HabitIcon Emoji(string value) => new(nameof(Emoji), value);

    public static HabitIcon Text(string value) => new(nameof(Text), value);

    public static HabitIcon Image(string imageUrl) => new(nameof(Image), imageUrl);

    private class HabitIconValueCannotBeEmptyRule : IBusinessRule
    {
        public bool IsBroken() => true;

        public string Message => "Habit icon value cannot be empty.";
    }
}
