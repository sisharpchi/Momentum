using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.HabitGroups.Events;
using Momentum.Modules.Habits.Domain.Habits;
using Momentum.Modules.Habits.UnitTests.SeedWork;

namespace Momentum.Modules.Habits.UnitTests.HabitGroups;

public class HabitGroupTests : TestBase
{
    [Fact]
    public void CreateGroupStoresAppearanceAndRaisesDomainEvent()
    {
        var group = HabitGroup.Create(
            new UserId(Guid.NewGuid()),
            "Health",
            HabitIcon.Emoji("heart"),
            HabitColor.FromHex("#3B82F6"),
            10);

        Assert.Equal("Health", group.Name);
        Assert.Equal(10, group.SortOrder);
        Assert.False(group.IsArchived);
        var domainEvent = AssertPublishedDomainEvent<HabitGroupCreatedDomainEvent>(group);
        Assert.Equal(group.Id, domainEvent.HabitGroupId);
        Assert.Equal(group.UserId, domainEvent.UserId);
    }

    [Fact]
    public void RenameGroupRequiresName()
    {
        var group = HabitGroup.Create(
            new UserId(Guid.NewGuid()),
            "Health",
            HabitIcon.Emoji("heart"),
            HabitColor.FromHex("#3B82F6"),
            10);

        var exception = AssertBrokenRule(() => group.Rename(" "));

        Assert.Equal("Habit group name cannot be empty.", exception.Message);
    }

    [Fact]
    public void ChangeAppearanceAndReorderUpdateGroup()
    {
        var group = CreateGroup();

        group.ChangeAppearance(HabitIcon.Text("H"), HabitColor.FromHex("#10B981"));
        group.Reorder(20);

        Assert.Equal("Text", group.Icon.Type);
        Assert.Equal("H", group.Icon.Value);
        Assert.Equal("#10B981", group.Color.Value);
        Assert.Equal(20, group.SortOrder);
    }

    [Fact]
    public void ArchivePublishesArchivedEventAndIsIdempotent()
    {
        var group = CreateGroup();

        group.Archive();
        group.Archive();

        Assert.True(group.IsArchived);
        Assert.Single(group.DomainEvents.OfType<HabitGroupArchivedDomainEvent>());
    }

    [Fact]
    public void ArchivedGroupCannotBeRenamed()
    {
        var group = CreateGroup();
        group.Archive();

        var exception = AssertBrokenRule(() => group.Rename("Work"));

        Assert.Equal("Archived habit group cannot be changed.", exception.Message);
    }

    private static HabitGroup CreateGroup()
    {
        return HabitGroup.Create(
            new UserId(Guid.NewGuid()),
            "Health",
            HabitIcon.Emoji("heart"),
            HabitColor.FromHex("#3B82F6"),
            10);
    }
}
