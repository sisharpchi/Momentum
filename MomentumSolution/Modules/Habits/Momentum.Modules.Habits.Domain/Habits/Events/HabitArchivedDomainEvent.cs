using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Events;

public class HabitArchivedDomainEvent : DomainEventBase
{
    public HabitArchivedDomainEvent(HabitId habitId, UserId userId)
    {
        HabitId = habitId;
        UserId = userId;
    }

    public HabitId HabitId { get; }

    public UserId UserId { get; }
}
