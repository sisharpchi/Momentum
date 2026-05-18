using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.HabitGroups.Events;
using Momentum.Modules.Habits.Domain.HabitGroups.Rules;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Domain.HabitGroups;

public class HabitGroup : Entity, IAggregateRoot
{
    public HabitGroupId Id { get; private set; }

    public UserId UserId { get; private set; }

    public string Name => _name;

    public HabitIcon Icon => _icon;

    public HabitColor Color => _color;

    public int SortOrder => _sortOrder;

    public bool IsArchived => _isArchived;

    private string _name;

    private HabitIcon _icon;

    private HabitColor _color;

    private int _sortOrder;

    private bool _isArchived;

    private HabitGroup()
    {
        // Only for EF.
        Id = null!;
        UserId = null!;
        _name = string.Empty;
        _icon = null!;
        _color = null!;
    }

    public static HabitGroup Create(UserId userId, string name, HabitIcon icon, HabitColor color, int sortOrder = 0)
    {
        return new HabitGroup(userId, name, icon, color, sortOrder);
    }

    private HabitGroup(UserId userId, string name, HabitIcon icon, HabitColor color, int sortOrder)
    {
        CheckRule(new HabitGroupNameCannotBeEmptyRule(name));

        Id = new HabitGroupId(Guid.NewGuid());
        UserId = userId;
        _name = name.Trim();
        _icon = icon;
        _color = color;
        _sortOrder = sortOrder;
        _isArchived = false;

        AddDomainEvent(new HabitGroupCreatedDomainEvent(Id, UserId));
    }

    public void Rename(string name)
    {
        CheckRule(new ArchivedHabitGroupCannotBeChangedRule(_isArchived));
        CheckRule(new HabitGroupNameCannotBeEmptyRule(name));

        _name = name.Trim();
    }

    public void ChangeAppearance(HabitIcon icon, HabitColor color)
    {
        CheckRule(new ArchivedHabitGroupCannotBeChangedRule(_isArchived));

        _icon = icon;
        _color = color;
    }

    public void Reorder(int sortOrder)
    {
        CheckRule(new ArchivedHabitGroupCannotBeChangedRule(_isArchived));

        _sortOrder = sortOrder;
    }

    public void Archive()
    {
        if (_isArchived)
        {
            return;
        }

        _isArchived = true;
        AddDomainEvent(new HabitGroupArchivedDomainEvent(Id, UserId));
    }
}
