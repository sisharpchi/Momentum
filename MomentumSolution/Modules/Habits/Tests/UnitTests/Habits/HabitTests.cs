using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits;
using Momentum.Modules.Habits.Domain.Habits.Events;
using Momentum.Modules.Habits.UnitTests.SeedWork;

namespace Momentum.Modules.Habits.UnitTests.Habits;

public class HabitTests : TestBase
{
    [Fact]
    public void CreateDailyCountHabitCreatesActiveHabitAndDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var groupId = new HabitGroupId(Guid.NewGuid());

        var habit = Habit.Create(
            userId,
            groupId,
            "Drink 2L water",
            "Stay hydrated throughout the day",
            HabitIcon.Emoji("water"),
            HabitColor.FromHex("#3B82F6"),
            HabitType.Count,
            HabitSchedule.Daily(new DateOnly(2026, 5, 18), "Asia/Tashkent"),
            HabitTarget.Count(2000, "ml"));

        Assert.Equal(HabitStatus.Active, habit.Status);
        Assert.Equal("Drink 2L water", habit.Title);
        Assert.Equal(HabitType.Count, habit.Type);
        Assert.Equal(HabitScheduleFrequency.Daily, habit.CurrentSchedule.Frequency);
        Assert.Single(habit.ScheduleVersions);
        var domainEvent = AssertPublishedDomainEvent<HabitCreatedDomainEvent>(habit);
        Assert.Equal(habit.Id, domainEvent.HabitId);
        Assert.Equal(userId, domainEvent.UserId);
        Assert.Equal(groupId, domainEvent.GroupId);
    }

    [Fact]
    public void CreateHabitRequiresTitle()
    {
        var exception = AssertBrokenRule(() => CreateHabit(title: " "));

        Assert.Equal("Habit title cannot be empty.", exception.Message);
    }

    [Fact]
    public void ChangeDetailsUpdatesTrimmedValues()
    {
        var habit = CreateHabit();

        habit.ChangeDetails(
            "  Read 30 pages  ",
            "  before sleep  ",
            HabitIcon.Emoji("book"),
            HabitColor.FromHex("#10B981"),
            HabitTarget.Count(30, "pages"));

        Assert.Equal("Read 30 pages", habit.Title);
        Assert.Equal("before sleep", habit.Description);
        Assert.Equal("#10B981", habit.Color.Value);
        Assert.Equal(30, habit.Target!.Value);
    }

    [Fact]
    public void CustomWeeklyScheduleRequiresAtLeastOneDay()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() =>
            HabitSchedule.CustomWeekly(new DateOnly(2026, 5, 18), "Asia/Tashkent", []));

        Assert.Equal("Custom weekly habit schedule must contain at least one day.", exception.Message);
    }

    [Fact]
    public void ScheduleRequiresTimezone()
    {
        var exception = AssertBrokenRule(() =>
            HabitSchedule.Daily(new DateOnly(2026, 5, 18), " "));

        Assert.Equal("Habit schedule timezone cannot be empty.", exception.Message);
    }

    [Fact]
    public void ScheduleEndDateCannotBeBeforeStartDate()
    {
        var exception = AssertBrokenRule(() =>
            HabitSchedule.Daily(
                new DateOnly(2026, 5, 18),
                "Asia/Tashkent",
                new DateOnly(2026, 5, 17)));

        Assert.Equal("Habit schedule end date cannot be before start date.", exception.Message);
    }

    [Fact]
    public void ChangeScheduleAddsNewVersionFromEffectiveDate()
    {
        var habit = Habit.Create(
            new UserId(Guid.NewGuid()),
            new HabitGroupId(Guid.NewGuid()),
            "Run 3 km",
            null,
            HabitIcon.Emoji("run"),
            HabitColor.FromHex("#14B8A6"),
            HabitType.Count,
            HabitSchedule.CustomWeekly(
                new DateOnly(2026, 5, 18),
                "Asia/Tashkent",
                [DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday]),
            HabitTarget.Count(3, "km"));

        habit.ChangeSchedule(HabitSchedule.Daily(new DateOnly(2026, 6, 1), "Asia/Tashkent"));

        Assert.Equal(HabitScheduleFrequency.Daily, habit.CurrentSchedule.Frequency);
        Assert.Equal(2, habit.ScheduleVersions.Count);
        Assert.Equal(new DateOnly(2026, 5, 31), habit.ScheduleVersions[0].EffectiveTo);
        var domainEvent = AssertPublishedDomainEvent<HabitScheduleChangedDomainEvent>(habit);
        Assert.Equal(habit.Id, domainEvent.HabitId);
        Assert.Equal(new DateOnly(2026, 6, 1), domainEvent.EffectiveFrom);
    }

    [Fact]
    public void PauseAndActivatePublishStatusChangedEvents()
    {
        var habit = CreateHabit();

        habit.Pause();
        habit.Activate();

        Assert.Equal(HabitStatus.Active, habit.Status);
        Assert.Equal(2, habit.DomainEvents.OfType<HabitStatusChangedDomainEvent>().Count());
    }

    [Fact]
    public void ArchivePublishesArchivedEventAndIsIdempotent()
    {
        var habit = CreateHabit();

        habit.Archive();
        habit.Archive();

        Assert.Equal(HabitStatus.Archived, habit.Status);
        Assert.Single(habit.DomainEvents.OfType<HabitArchivedDomainEvent>());
    }

    [Fact]
    public void ArchivedHabitCannotBePaused()
    {
        var habit = CreateHabit();

        habit.Archive();

        var exception = AssertBrokenRule(habit.Pause);

        Assert.Equal("Archived habit cannot be changed.", exception.Message);
    }

    [Fact]
    public void TargetValueMustBePositive()
    {
        var exception = AssertBrokenRule(() => HabitTarget.Count(0, "pages"));

        Assert.Equal("Habit target value must be greater than zero.", exception.Message);
    }

    [Fact]
    public void TargetUnitCannotBeEmpty()
    {
        var exception = AssertBrokenRule(() => HabitTarget.Count(1, " "));

        Assert.Equal("Habit target unit cannot be empty.", exception.Message);
    }

    [Fact]
    public void ColorMustBeHex()
    {
        var exception = AssertBrokenRule(() => HabitColor.FromHex("blue"));

        Assert.Equal("Habit color must be a 6-digit hex color.", exception.Message);
    }

    [Fact]
    public void IconValueCannotBeEmpty()
    {
        var exception = AssertBrokenRule(() => HabitIcon.Emoji(" "));

        Assert.Equal("Habit icon value cannot be empty.", exception.Message);
    }

    private static Habit CreateHabit(string title = "Read 20 pages")
    {
        return Habit.Create(
            new UserId(Guid.NewGuid()),
            new HabitGroupId(Guid.NewGuid()),
            title,
            null,
            HabitIcon.Emoji("book"),
            HabitColor.FromHex("#A855F7"),
            HabitType.Count,
            HabitSchedule.Daily(new DateOnly(2026, 5, 18), "Asia/Tashkent"),
            HabitTarget.Count(20, "pages"));
    }
}
