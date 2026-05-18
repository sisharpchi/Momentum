using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Events;

public class HabitScheduleChangedDomainEvent : DomainEventBase
{
    public HabitScheduleChangedDomainEvent(HabitId habitId, UserId userId, DateOnly effectiveFrom)
    {
        HabitId = habitId;
        UserId = userId;
        EffectiveFrom = effectiveFrom;
    }

    public HabitId HabitId { get; }

    public UserId UserId { get; }

    public DateOnly EffectiveFrom { get; }
}
