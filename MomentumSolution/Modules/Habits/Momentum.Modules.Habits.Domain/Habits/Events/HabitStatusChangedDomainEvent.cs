using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Events;

public class HabitStatusChangedDomainEvent : DomainEventBase
{
    public HabitStatusChangedDomainEvent(HabitId habitId, UserId userId, HabitStatus status)
    {
        HabitId = habitId;
        UserId = userId;
        Status = status;
    }

    public HabitId HabitId { get; }

    public UserId UserId { get; }

    public HabitStatus Status { get; }
}
