using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Habits.Application.Configuration.Commands;
using Momentum.Modules.Habits.Application.Habits;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Application.HabitGroups.CreateHabitGroup;

public class CreateHabitGroupCommandHandler : ICommandHandler<CreateHabitGroupCommand, Guid>
{
    private readonly IMainRepository _repository;

    public CreateHabitGroupCommandHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateHabitGroupCommand command, CancellationToken cancellationToken)
    {
        var group = HabitGroup.Create(
            new UserId(command.UserId),
            command.Name,
            HabitIconFactory.Create(command.IconType, command.IconValue),
            HabitColor.FromHex(command.Color),
            command.SortOrder);

        await _repository.Set<HabitGroup>().AddAsync(group, cancellationToken);

        return group.Id.Value;
    }
}
