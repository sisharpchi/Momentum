using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits.Events;
using Momentum.Modules.Habits.Domain.Habits.Rules;

namespace Momentum.Modules.Habits.Domain.Habits;

public class Habit : Entity, IAggregateRoot
{
    public HabitId Id { get; private set; }

    public UserId UserId { get; private set; }

    public HabitGroupId GroupId { get; private set; }

    public string Title => _title;

    public string? Description => _description;

    public HabitIcon Icon => _icon;

    public HabitColor Color => _color;

    public HabitType Type => _type;

    public HabitStatus Status => _status;

    public HabitTarget? Target => _target;

    public HabitSchedule CurrentSchedule => _scheduleVersions.Last().Schedule;

    public IReadOnlyList<HabitScheduleVersion> ScheduleVersions => _scheduleVersions.AsReadOnly();

    public int SortOrder => _sortOrder;

    private string _title;

    private string? _description;

    private HabitIcon _icon;

    private HabitColor _color;

    private HabitType _type;

    private HabitStatus _status;

    private HabitTarget? _target;

    private int _sortOrder;

    private readonly List<HabitScheduleVersion> _scheduleVersions;

    private Habit()
    {
        // Only for EF.
        Id = null!;
        UserId = null!;
        GroupId = null!;
        _title = string.Empty;
        _icon = null!;
        _color = null!;
        _type = null!;
        _status = null!;
        _scheduleVersions = [];
    }

    public static Habit Create(
        UserId userId,
        HabitGroupId groupId,
        string title,
        string? description,
        HabitIcon icon,
        HabitColor color,
        HabitType type,
        HabitSchedule schedule,
        HabitTarget? target,
        int sortOrder = 0)
    {
        return new Habit(userId, groupId, title, description, icon, color, type, schedule, target, sortOrder);
    }

    private Habit(
        UserId userId,
        HabitGroupId groupId,
        string title,
        string? description,
        HabitIcon icon,
        HabitColor color,
        HabitType type,
        HabitSchedule schedule,
        HabitTarget? target,
        int sortOrder)
    {
        CheckRule(new HabitTitleCannotBeEmptyRule(title));

        Id = new HabitId(Guid.NewGuid());
        UserId = userId;
        GroupId = groupId;
        _title = title.Trim();
        _description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        _icon = icon;
        _color = color;
        _type = type;
        _target = target;
        _status = HabitStatus.Active;
        _sortOrder = sortOrder;
        _scheduleVersions = [HabitScheduleVersion.Start(schedule)];

        AddDomainEvent(new HabitCreatedDomainEvent(Id, UserId, GroupId));
    }

    public void ChangeDetails(
        string title,
        string? description,
        HabitIcon icon,
        HabitColor color,
        HabitTarget? target)
    {
        CheckRule(new ArchivedHabitCannotBeChangedRule(_status));
        CheckRule(new HabitTitleCannotBeEmptyRule(title));

        _title = title.Trim();
        _description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        _icon = icon;
        _color = color;
        _target = target;
    }

    public void AssignGroup(HabitGroupId groupId)
    {
        CheckRule(new ArchivedHabitCannotBeChangedRule(_status));

        GroupId = groupId;
    }

    public void ChangeSchedule(HabitSchedule schedule)
    {
        CheckRule(new ArchivedHabitCannotBeChangedRule(_status));

        var previousVersion = _scheduleVersions.Last();
        _scheduleVersions[^1] = previousVersion.CloseOn(schedule.StartDate.AddDays(-1));
        _scheduleVersions.Add(HabitScheduleVersion.Start(schedule));

        AddDomainEvent(new HabitScheduleChangedDomainEvent(Id, UserId, schedule.StartDate));
    }

    public void Pause()
    {
        CheckRule(new ArchivedHabitCannotBeChangedRule(_status));

        _status = HabitStatus.Paused;
        AddDomainEvent(new HabitStatusChangedDomainEvent(Id, UserId, _status));
    }

    public void Activate()
    {
        CheckRule(new ArchivedHabitCannotBeChangedRule(_status));

        _status = HabitStatus.Active;
        AddDomainEvent(new HabitStatusChangedDomainEvent(Id, UserId, _status));
    }

    public void Archive()
    {
        if (_status == HabitStatus.Archived)
        {
            return;
        }

        _status = HabitStatus.Archived;
        AddDomainEvent(new HabitArchivedDomainEvent(Id, UserId));
    }

    public void Reorder(int sortOrder)
    {
        CheckRule(new ArchivedHabitCannotBeChangedRule(_status));

        _sortOrder = sortOrder;
    }
}
