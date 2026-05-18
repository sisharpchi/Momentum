using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Domain.HabitGroups.Events;

public class HabitGroupArchivedDomainEvent : DomainEventBase
{
    public HabitGroupArchivedDomainEvent(HabitGroupId habitGroupId, UserId userId)
    {
        HabitGroupId = habitGroupId;
        UserId = userId;
    }

    public HabitGroupId HabitGroupId { get; }

    public UserId UserId { get; }
}
