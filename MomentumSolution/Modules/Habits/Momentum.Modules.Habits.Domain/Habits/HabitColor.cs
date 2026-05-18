using System.Text.RegularExpressions;
using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public partial class HabitColor : ValueObject
{
    public string Value { get; }

    private HabitColor(string value)
    {
        if (!HexColorRegex().IsMatch(value))
        {
            throw new BusinessRuleValidationException(new HabitColorMustBeHexRule());
        }

        Value = value.ToUpperInvariant();
    }

    public static HabitColor FromHex(string value) => new(value.Trim());

    [GeneratedRegex("^#[0-9A-Fa-f]{6}$")]
    private static partial Regex HexColorRegex();

    private class HabitColorMustBeHexRule : IBusinessRule
    {
        public bool IsBroken() => true;

        public string Message => "Habit color must be a 6-digit hex color.";
    }
}
