using Microsoft.EntityFrameworkCore;
using Momentum.Modules.Habits.Application.Configuration.Queries;
using Momentum.Modules.Habits.Domain.HabitGroups;

namespace Momentum.Modules.Habits.Application.HabitGroups.GetHabitGroups;

internal class GetHabitGroupsQueryHandler : IQueryHandler<GetHabitGroupsQuery, IReadOnlyList<HabitGroupDto>>
{
    private readonly DbContext _context;

    public GetHabitGroupsQueryHandler(DbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<HabitGroupDto>> Handle(GetHabitGroupsQuery query, CancellationToken cancellationToken)
    {
        var groups = await _context.Set<HabitGroup>()
            .Where(group => group.UserId.Value == query.UserId)
            .ToListAsync(cancellationToken);

        return groups
            .Where(group => !group.IsArchived)
            .OrderBy(group => group.SortOrder)
            .Select(group => new HabitGroupDto
            {
                Id = group.Id.Value,
                UserId = group.UserId.Value,
                Name = group.Name,
                IconType = group.Icon.Type,
                IconValue = group.Icon.Value,
                Color = group.Color.Value,
                SortOrder = group.SortOrder,
                IsArchived = group.IsArchived,
            })
            .ToList();
    }
}
